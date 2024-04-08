using Microsoft.Extensions.Localization;
using System.Globalization;

namespace Nittin.Xperience.Localization;

public class KenticoStringLocalizer : IKenticoStringLocalizer
{
    private readonly LocalizationService localizationService;

    public KenticoStringLocalizer(LocalizationService localizationService) => this.localizationService = localizationService;

    public LocalizedString this[string name] => new(name, GetStringByName(name) ?? name, resourceNotFound: GetStringByName(name) == null);

    public LocalizedString this[string name, params object[] arguments] => new(name, string.Format(GetStringByName(name) ?? name, arguments), resourceNotFound: GetStringByName(name) == null);

    private string? GetStringByName(string name)
    {
        string culture = CultureInfo.CurrentCulture.ToString();
        return localizationService.GetValueByNameAndCulture(name, culture);
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        string culture = CultureInfo.CurrentCulture.ToString();
        var allStrings = localizationService.GetAllValuesForCulture(culture).Select(v => new LocalizedString(v.Key, v.Value, resourceNotFound: false));
        return allStrings;
    }


}

