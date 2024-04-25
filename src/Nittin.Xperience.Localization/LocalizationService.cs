using System.Data;

using CMS.ContentEngine;
using CMS.DataEngine;

namespace Nittin.Xperience.Localization;

public class LocalizationService : ILocalizationService
{
    private readonly IInfoProvider<LocalizationKeyInfo> localizationKeyInfoProvider;
    private readonly IInfoProvider<LocalizationTranslationItemInfo> localizationTranslationInfoProvider;
    private readonly IInfoProvider<ContentLanguageInfo> contentLanguageInfoProvider;

    public LocalizationService(IInfoProvider<LocalizationKeyInfo> localizationKeyInfoProvider,
        IInfoProvider<LocalizationTranslationItemInfo> localizationTranslationInfoProvider, IInfoProvider<ContentLanguageInfo> contentLanguageInfoProvider)
    {
        this.localizationKeyInfoProvider = localizationKeyInfoProvider;
        this.localizationTranslationInfoProvider = localizationTranslationInfoProvider;
        this.contentLanguageInfoProvider = contentLanguageInfoProvider;
    }

    public List<LocalizationKeyInfo> GetItems()
    {
        var allItems = localizationKeyInfoProvider.Get().ToList();
        return allItems;
    }

    public string? GetValueByNameAndCulture(string name, string culture)
    {
        var language = contentLanguageInfoProvider.Get().WhereEquals(nameof(ContentLanguageInfo.ContentLanguageCultureFormat), culture).First();
        return GetValueByNameAndLanguage(name, language);
    }

    public IEnumerable<ContentLanguageInfo?> GetAllCultureDisplayNames() => contentLanguageInfoProvider.Get().ToList();

    public string? GetValueByNameAndLanguage(string name, ContentLanguageInfo language)
    {
        var result = localizationTranslationInfoProvider.Get()
            .Source(s => s.Join<LocalizationKeyInfo>(nameof(LocalizationTranslationItemInfo.LocalizationTranslationItemLocalizationKeyItemId), nameof(LocalizationKeyInfo.LocalizationKeyItemId)))
            .WhereEquals(nameof(LocalizationKeyInfo.LocalizationKeyItemName), name)
            .WhereEquals(nameof(LocalizationTranslationItemInfo.LocalizationTranslationItemContentLanguageId), language.ContentLanguageID)
            .TopN(1)
            .FirstOrDefault();

        return result?.LocalizationTranslationItemText;
    }

    public Dictionary<string, string> GetAllValuesForCulture(string culture)
    {
        var language = contentLanguageInfoProvider.Get(culture);
        return GetAllValuesForLanguage(language);
    }

    public Dictionary<string, string> GetAllValuesForLanguage(ContentLanguageInfo language)
    {
        var dataSet = localizationTranslationInfoProvider.Get()
            .WhereEquals(nameof(LocalizationTranslationItemInfo.LocalizationTranslationItemContentLanguageId), language.ContentLanguageID)
            .Source(s => s.Join<LocalizationKeyInfo>(nameof(LocalizationTranslationItemInfo.LocalizationTranslationItemLocalizationKeyItemId), nameof(LocalizationKeyInfo.LocalizationKeyItemId)))
            .Result;

        return dataSet?.Tables[0].Rows.Cast<DataRow>()
            .Select(r => new
            {
                Key = r.Field<string>(nameof(LocalizationKeyInfo.LocalizationKeyItemName)),
                TranslationText = r.Field<string>(nameof(LocalizationTranslationItemInfo.LocalizationTranslationItemText))
            })
            .Where(i => i.Key != null)
            .GroupBy(i => i.Key!, i => i.TranslationText)
            .ToDictionary(g => g.Key, g => g.First() ?? string.Empty) ?? [];
    }
}


