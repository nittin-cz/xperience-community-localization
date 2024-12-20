using CMS.ContentEngine;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Websites.Routing;
using Microsoft.Extensions.Localization;
using System.Data;
using XperienceCommunity.Localization;

namespace XperienceCommunity.Localizer.Internal
{
    internal class XperienceStringLocalizerBase(IProgressiveCache progressiveCache,
        IWebsiteChannelContext websiteChannelContext,
        IInfoProvider<ContentLanguageInfo> contentLanguageInfoProvider)
    {
        private const string WebsiteToLanguageInfoQuery = $@"select WebsiteChannelID, ContentLanguageName, ContentLanguageCultureFormat from CMS_ContentLanguage
                inner join CMS_WebsiteChannel on WebsiteChannelPrimaryContentLanguageID = ContentLanguageID
                union all 
                Select top 1 -1 as WebsiteChannelID, ContentLanguageName, ContentLanguageCultureFormat from CMS_SettingsKey 
                inner join CMS_ContentLanguage on ContentLanguageCultureFormat = KeyValue or KeyValue like ContentLanguageName+'%'
                where KeyName = 'CMSDefaultCultureCode'";
        private const string LocalizationWithLanguagePriorityQuery = @"select LocalizationKeyItemName as StringKey, LocalizationTranslationItemText as TranslationText from (
select ROW_NUMBER() over (partition by LocalizationKeyItemName {0}) as priority, LocalizationKeyItemName, LocalizationTranslationItemText from NittinLocalization_LocalizationTranslationItem
left join NittinLocalization_LocalizationKeyItem on LocalizationTranslationItemLocalizationKeyItemId = LocalizationKeyItemId
left join CMS_ContentLanguage on ContentLanguageID = LocalizationTranslationItemContentLanguageId
where ContentLanguageCultureFormat in ('{1}')
) combined where priority = 1";

        public static string CurrentCulture => System.Globalization.CultureInfo.CurrentCulture.Name;

        internal Dictionary<string, string> XperienceResourceStrings => GetDictionary(CurrentCulture);

        // The formatting rules on this repo are pretty messy...
        internal Dictionary<int, LanguageSummary> WebsiteIdToLanguageInfoDictionary() =>
            progressiveCache.Load(cs =>
            {
                if (cs.Cached)
                {
                    cs.CacheDependency = CacheHelper.GetCacheDependency([
                        $"{SettingsKeyInfo.OBJECT_TYPE}|byname|CMSDefaultCultureCode",
                        $"{ContentLanguageInfo.OBJECT_TYPE}|all"
                    ]);
                }

                string query = WebsiteToLanguageInfoQuery;
                return ConnectionHelper.ExecuteQuery(query, [], QueryTypeEnum.SQLQuery).Tables[0].Rows.Cast<DataRow>()
                .ToDictionary(key => (int)key["WebsiteChannelID"], value => new LanguageSummary((string)value["ContentLanguageName"], (string)value["ContentLanguageCultureFormat"]));
            }, new CacheSettings(1440, "Localization_WebsiteIdToLanguageInfoDictionary"));

        internal record LanguageSummary(string LanguageName, string CultureName);

        internal string SiteVisitorDefaultCulture
        {
            get
            {
                var dictionary = WebsiteIdToLanguageInfoDictionary();
                if (dictionary.TryGetValue(websiteChannelContext.WebsiteChannelID, out var summary))
                {
                    return summary.CultureName;
                }
                return dictionary.TryGetValue(-1, out var defaultSummary) ? defaultSummary.CultureName : "en-US";
            }
        }

        internal string CMSDefaultCulture => WebsiteIdToLanguageInfoDictionary().TryGetValue(-1, out var defaultSummary) ? defaultSummary.CultureName : "en-US";

        private Dictionary<string, string> GetDictionary(string cultureName)
        {
            // Now load up dictionary
            var langChain = GetLanguageChain(cultureName);
            if (langChain.Count == 0)
            {
                return [];
            }
            return progressiveCache.Load(cs =>
            {
                if (cs.Cached)
                {
                    cs.CacheDependency = CacheHelper.GetCacheDependency([
                        $"{LocalizationKeyInfo.OBJECT_TYPE}|all",
                        $"{LocalizationTranslationItemInfo.OBJECT_TYPE}|all"
                    ]);
                }
                // This logic will build out the priority listing for the language fallbacks, picking the most accurate match
                string orderBySql = "order by ";
                string ends = "";
                for (int i = 0; i < langChain.Count; i++)
                {
#pragma warning disable S1643 // Strings should not be concatenated using '+' in a loop
                    orderBySql += $" case when ContentLanguageCultureFormat = '{SqlHelper.EscapeQuotes(langChain[i])}' then {i} else ";
                    ends += " end ";
#pragma warning restore S1643 // Strings should not be concatenated using '+' in a loop
                }
                orderBySql += $" {langChain.Count} {ends}";

                var results = ConnectionHelper.ExecuteQuery(
                    string.Format(LocalizationWithLanguagePriorityQuery, orderBySql, string.Join("','", langChain.Select(x => SqlHelper.EscapeQuotes(x))))
                    , [], QueryTypeEnum.SQLQuery);
                return results.Tables[0].Rows.Cast<DataRow>()
                    .Select(x => new Tuple<string, string>(ValidationHelper.GetString(x["StringKey"], "").ToLowerInvariant(), ValidationHelper.GetString(x["TranslationText"], "")))
                    .GroupBy(x => x.Item1)
                    .ToDictionary(key => key.Key, value => value.First().Item2);
            }, new CacheSettings(1440, "LocalizedStringDictionary", cultureName, SiteVisitorDefaultCulture, CMSDefaultCulture));
        }

        private List<string> GetLanguageChain(string cultureName)
        {
            var chainByCultureName = progressiveCache.Load(cs =>
            {
                if (cs.Cached)
                {
                    cs.CacheDependency = CacheHelper.GetCacheDependency([
                        $"{SettingsKeyInfo.OBJECT_TYPE}|byname|CMSDefaultCultureCode",
                        $"{ContentLanguageInfo.OBJECT_TYPE}|all"]);
                }
                var allLanguages = contentLanguageInfoProvider.Get().GetEnumerableTypedResult();
                var languageById = allLanguages.ToDictionary(key => key.ContentLanguageID, value => value);
                var dictionary = new Dictionary<string, List<string>>();
                foreach (var language in allLanguages)
                {
                    var fallBackCultureChain = new List<string>
                    {
                        language.ContentLanguageCultureFormat
                    };
                    int fallbackLangId = language.ContentLanguageFallbackContentLanguageID;
                    while (fallbackLangId > 0)
                    {
                        var fallbackLang = languageById[fallbackLangId];
                        fallBackCultureChain.Add(fallbackLang.ContentLanguageCultureFormat);
                        fallbackLangId = fallbackLang.ContentLanguageFallbackContentLanguageID;
                    }
                    // Add in default if not in the mix.
                    if (!fallBackCultureChain.Contains(CMSDefaultCulture))
                    {
                        fallBackCultureChain.Add(CMSDefaultCulture);
                    }
                    dictionary.Add(language.ContentLanguageCultureFormat.ToLowerInvariant(), fallBackCultureChain);
                }
                return dictionary;
            }, new CacheSettings(1440, "GetFallbackCultures"));

            return chainByCultureName.TryGetValue(cultureName.ToLowerInvariant(), out var fallbackList) ? fallbackList : [SiteVisitorDefaultCulture, CMSDefaultCulture];
        }

        internal LocalizedString LocalizeWithKentico(string name, params object[] arguments)
        {
            string value = string.Empty;
            var dictionary = GetDictionary(CurrentCulture);
            string key = name.ToLower().Replace("{$", "").Replace("$}", "").Trim();

            if (dictionary.ContainsKey(key.ToLowerInvariant()))
            {
                value = dictionary[key.ToLowerInvariant()];
            }
            if (string.IsNullOrWhiteSpace(value))
            {
                // Fall back, if the string has {$ $} or is complex, try to run with this.
                value = progressiveCache.Load(cs =>
                {
                    if (cs.Cached)
                    {
                        cs.CacheDependency = CacheHelper.GetCacheDependency(
                        [
                            $"{LocalizationKeyInfo.OBJECT_TYPE}|all",
                            $"{LocalizationTranslationItemInfo.OBJECT_TYPE}|all",
                        ]);
                    }
                    return ResHelper.LocalizeString(name, CurrentCulture);
                }, new CacheSettings(30, "ResHelperLocalization", name, CurrentCulture, SiteVisitorDefaultCulture));
            }
            if (arguments.Length > 0 && !string.IsNullOrWhiteSpace(value) && value.Contains($"{{{arguments.Length - 1}}}", StringComparison.CurrentCulture))
            {
                value = string.Format(value, arguments);
            }
            return new LocalizedString(name, value, string.IsNullOrWhiteSpace(value) || name.Equals(value, StringComparison.OrdinalIgnoreCase));
        }
    }
}
