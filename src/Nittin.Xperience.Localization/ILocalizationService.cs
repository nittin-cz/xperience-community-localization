using CMS.ContentEngine;

namespace Nittin.Xperience.Localization;

public interface ILocalizationService
{
    IEnumerable<ContentLanguageInfo?> GetAllCultureDisplayNames();
    List<LocalizationKeyInfo> GetItems();
    string? GetValueByNameAndCulture(string name, string culture);
    string? GetValueByNameAndLanguage(string name, ContentLanguageInfo language);
    Dictionary<string, string> GetAllValuesForCulture(string culture);
    Dictionary<string, string> GetAllValuesForLanguage(ContentLanguageInfo language);
}
