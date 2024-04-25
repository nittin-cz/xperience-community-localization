using CMS.Membership;

using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.UIPages;

using Nittin.Xperience.Localization.Admin.UIPages;

[assembly:
    UIApplication(LocalizationApplication.IDENTIFIER, typeof(LocalizationApplication),
        "nittin-xperience-localization", "Localization", BaseApplicationCategories.CONFIGURATION, Icons.Earth,
        TemplateNames.SECTION_LAYOUT)]

namespace Nittin.Xperience.Localization.Admin.UIPages;

[UIPermission(SystemPermissions.VIEW)]
public class LocalizationApplication : ApplicationPage
{
    public const string IDENTIFIER = "Nittin.Xperience.Localization.LocalizationApplication";
}
