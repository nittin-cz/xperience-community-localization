using System.Globalization;

using Microsoft.Extensions.Localization;

namespace XperienceCommunity.Localization;

public class KenticoStringLocalizer : IKenticoStringLocalizer
{
    private readonly ILocalizationService localizationService;

    public KenticoStringLocalizer(ILocalizationService localizationService) => this.localizationService = localizationService;

    public virtual LocalizedString this[string name] => new(name, GetStringByName(name) ?? name, resourceNotFound: GetStringByName(name) == null);

    public virtual LocalizedString this[string name, params object[] arguments] => new(name, string.Format(GetStringByName(name) ?? name, arguments), resourceNotFound: GetStringByName(name) == null);

    public virtual string? GetStringByName(string name)
    {
        string culture = CultureInfo.CurrentCulture.ToString();
        return localizationService.GetValueByNameAndCulture(name, culture);
    }

    public virtual IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        string culture = CultureInfo.CurrentCulture.ToString();
        var allStrings = localizationService.GetAllValuesForCulture(culture).Select(v => new LocalizedString(v.Key, v.Value, resourceNotFound: false));
        return allStrings;
    }
}
