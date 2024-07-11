using CMS.Membership;

using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.UIPages;

using XperienceCommunity.Localization.Admin.UIPages;

[assembly:
    UIApplication(
    identifier: LocalizationApplicationPage.IDENTIFIER,
    type: typeof(LocalizationApplicationPage),
    slug: "nittin-xperience-localization",
    name: "Localization",
    category: BaseApplicationCategories.CONFIGURATION,
    icon: Icons.Earth,
    templateName: TemplateNames.SECTION_LAYOUT)]

namespace XperienceCommunity.Localization.Admin.UIPages;

[UIPermission(SystemPermissions.VIEW)]
public class LocalizationApplicationPage : ApplicationPage
{
    public const string IDENTIFIER = "Nittin.Xperience.Community.Localization";
}
