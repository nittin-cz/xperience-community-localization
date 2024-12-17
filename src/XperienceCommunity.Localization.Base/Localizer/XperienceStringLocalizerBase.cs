using AngleSharp.Common;
using CMS.ContentEngine;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Websites.Routing;
using Microsoft.Extensions.Localization;
using System.Data;
using XperienceCommunity.Localization;

namespace XperienceCommunity.Localizer.Internal
{
    public class XperienceStringLocalizerBase(IProgressiveCache progressiveCache,
        IWebsiteChannelContext websiteChannelContext)
    {
        private readonly IProgressiveCache progressiveCache = progressiveCache;
        private readonly IWebsiteChannelContext websiteChannelContext = websiteChannelContext;

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

                string query = $@"select WebsiteChannelID, ContentLanguageName, ContentLanguageCultureFormat from CMS_ContentLanguage
                inner join CMS_WebsiteChannel on WebsiteChannelPrimaryContentLanguageID = ContentLanguageID
                union all 
                Select top 1 -1 as WebsiteChannelID, ContentLanguageName, ContentLanguageCultureFormat from CMS_SettingsKey 
                inner join CMS_ContentLanguage on ContentLanguageCultureFormat = KeyValue or KeyValue like ContentLanguageName+'%'
                where KeyName = 'CMSDefaultCultureCode'";
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
                else
                {
                    return dictionary.TryGetValue(-1, out var defaultSummary) ? defaultSummary.CultureName : "en-US";
                }
            }
        }

        internal string CMSDefaultCulture => WebsiteIdToLanguageInfoDictionary().TryGetValue(-1, out var defaultSummary) ? defaultSummary.CultureName : "en-US";

        private Dictionary<string, string> GetDictionary(string cultureName)
        {
            // Now load up dictionary
#pragma warning disable IDE0022 // Use expression body for method
            return progressiveCache.Load(cs =>
            {
                if (cs.Cached)
                {
                    cs.CacheDependency = CacheHelper.GetCacheDependency(new string[]
                    {
                        $"{LocalizationKeyInfo.OBJECT_TYPE}|all",
                        $"{LocalizationTranslationItemInfo.OBJECT_TYPE}|all",
                    });
                }
                // TODO: Eventually tie in the language fallback settings for the site, right now only does Culture -> Site Default Culture -> CMS Default Culture.  Should take the current language culture and get it's fallback tree.
                var results = ConnectionHelper.ExecuteQuery(
                    $@"select LocalizationKeyItemName as StringKey, LocalizationTranslationItemText as TranslationText from (
select ROW_NUMBER() over (partition by LocalizationKeyItemName order by case when ContentLanguageCultureFormat = @CultureName then 0 else case when ContentLanguageCultureFormat = @SiteDefaultCulture then 1 else 2 end end) as priority, LocalizationKeyItemName, LocalizationTranslationItemText from NittinLocalization_LocalizationTranslationItem
left join NittinLocalization_LocalizationKeyItem on LocalizationTranslationItemLocalizationKeyItemId = LocalizationKeyItemId
left join CMS_ContentLanguage on ContentLanguageID = LocalizationTranslationItemContentLanguageId
where ContentLanguageCultureFormat in (@CultureName, @SiteDefaultCulture, @CMSDefaultCulture)
) combined where priority = 1"
                    , new QueryDataParameters() { { "@CultureName", cultureName }, { "@SiteDefaultCulture", SiteVisitorDefaultCulture }, { "@CMSDefaultCulture", CMSDefaultCulture } }, QueryTypeEnum.SQLQuery);
                return results.Tables[0].Rows.Cast<DataRow>()
                    .Select(x => new Tuple<string, string>(ValidationHelper.GetString(x["StringKey"], "").ToLowerInvariant(), ValidationHelper.GetString(x["TranslationText"], "")))
                    .GroupBy(x => x.Item1)
                    .ToDictionary(key => key.Key, value => value.First().Item2);
            }, new CacheSettings(1440, "LocalizedStringDictionary", cultureName, SiteVisitorDefaultCulture, CMSDefaultCulture));
#pragma warning restore IDE0022 // Use expression body for method
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
