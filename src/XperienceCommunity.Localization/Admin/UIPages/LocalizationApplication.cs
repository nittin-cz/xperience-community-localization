using CMS.Membership;

using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.UIPages;

using XperienceCommunity.Localization.Admin.UIPages;

[assembly:
    UIApplication(LocalizationApplication.IDENTIFIER, typeof(LocalizationApplication),
        "nittin-xperience-localization", "Localization", BaseApplicationCategories.CONFIGURATION, Icons.Earth,
        TemplateNames.SECTION_LAYOUT)]

namespace XperienceCommunity.Localization.Admin.UIPages;

[UIPermission(SystemPermissions.VIEW)]
public class LocalizationApplication : ApplicationPage
{
    public const string IDENTIFIER = "XperienceCommunity.Localization.LocalizationApplication";
}
