using System.Data;
using CMS.ContentEngine;
using CMS.DataEngine;

namespace Nittin.Xperience.Localization;

public class LocalizationService
{
    private readonly IInfoProvider<LocalizationKeyInfo> localizationKeyInfoProvider;
    private readonly IInfoProvider<LocalizationTranslationInfo> localizationTranslationInfoProvider;
    private readonly IInfoProvider<ContentLanguageInfo> contentLanguageInfoProvider;

    public LocalizationService(IInfoProvider<LocalizationKeyInfo> localizationKeyInfoProvider,
        IInfoProvider<LocalizationTranslationInfo> localizationTranslationInfoProvider, IInfoProvider<ContentLanguageInfo> contentLanguageInfoProvider)
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
        var language = contentLanguageInfoProvider.Get(culture);
        return GetValueByNameAndLanguage(name, language);
    }

    public IEnumerable<ContentLanguageInfo?> GetAllCultureDisplayNames() => contentLanguageInfoProvider.Get().ToList();

    public string? GetValueByNameAndLanguage(string name, ContentLanguageInfo language)
    {
        var result = localizationTranslationInfoProvider.Get()
            .Source(s => s.Join<LocalizationKeyInfo>(nameof(LocalizationTranslationInfo.LocalizationTranslationLocalizationKeyId), nameof(LocalizationKeyInfo.LocalizationKeyId)))
            .WhereEquals(nameof(LocalizationKeyInfo.LocalizationKeyName), name)
            .WhereEquals(nameof(LocalizationTranslationInfo.LocalizationTranslationContentLanguageId), language.ContentLanguageID)
            .TopN(1)
            .FirstOrDefault();

        return result?.LocalizationTranslationText;
    }

    public Dictionary<string, string> GetAllValuesForCulture(string culture)
    {
        var language = contentLanguageInfoProvider.Get(culture);
        return GetAllValuesForLanguage(language);
    }

    public Dictionary<string, string> GetAllValuesForLanguage(ContentLanguageInfo language)
    {
        var dataSet = localizationTranslationInfoProvider.Get()
            .WhereEquals(nameof(LocalizationTranslationInfo.LocalizationTranslationContentLanguageId), language.ContentLanguageID)
            .Source(s => s.Join<LocalizationKeyInfo>(nameof(LocalizationTranslationInfo.LocalizationTranslationLocalizationKeyId), nameof(LocalizationKeyInfo.LocalizationKeyId)))
            .Result;

        return dataSet?.Tables[0].Rows.Cast<DataRow>()
            .Select(r => new
            {
                Key = r.Field<string>(nameof(LocalizationKeyInfo.LocalizationKeyName)),
                TranslationText = r.Field<string>(nameof(LocalizationTranslationInfo.LocalizationTranslationText))
            })
            .Where(i => i.Key != null)
            .GroupBy(i => i.Key!, i => i.TranslationText)
            .ToDictionary(g => g.Key, g => g.First() ?? string.Empty) ?? new Dictionary<string, string>();
    }

}


