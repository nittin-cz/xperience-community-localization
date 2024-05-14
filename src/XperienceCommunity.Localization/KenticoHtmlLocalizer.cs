using System.Globalization;

using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;

namespace XperienceCommunity.Localization;

public class KenticoHtmlLocalizer : IKenticoHtmlLocalizer
{
    private readonly ILocalizationService localizationService;

    public KenticoHtmlLocalizer(ILocalizationService localizationService) => this.localizationService = localizationService;

    public LocalizedHtmlString this[string name] => new(name, GetStringByName(name) ?? name, isResourceNotFound: GetStringByName(name) == null);

    public LocalizedHtmlString this[string name, params object[] arguments] => new(name, string.Format(GetStringByName(name) ?? name, arguments), isResourceNotFound: GetStringByName(name) == null);

    private string? GetStringByName(string name)
    {
        string culture = CultureInfo.CurrentCulture.ToString();
        return localizationService.GetValueByNameAndCulture(name, culture);
    }

    public LocalizedString GetString(string name)
        => new(name, GetStringByName(name) ?? name, resourceNotFound: GetStringByName(name) == null);

    public LocalizedString GetString(string name, params object[] parameters)
        => GetString(name);

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        string culture = CultureInfo.CurrentCulture.ToString();
        var allStrings = localizationService.GetAllValuesForCulture(culture).Select(v => new LocalizedString(v.Key, v.Value, resourceNotFound: false));
        return allStrings;
    }
}
