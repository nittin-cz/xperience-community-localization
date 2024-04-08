using Microsoft.Extensions.Localization;

namespace Nittin.Xperience.Localization;

public interface IKenticoStringLocalizer : IStringLocalizer
{
    IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures);
}

