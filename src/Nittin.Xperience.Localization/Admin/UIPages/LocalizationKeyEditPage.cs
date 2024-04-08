using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;
using Nittin.Xperience.Localization.Admin.UIPages;

[assembly: UIPage(typeof(LocalizationKeyListingPage), ":", typeof(LocalizationKeyEditPage), "Edit localization key",
    TemplateNames.EDIT, 1)]

namespace Nittin.Xperience.Localization.Admin.UIPages;

public class LocalizationKeyEditPage : InfoEditPage<LocalizationKeyInfo>
{
    public LocalizationKeyEditPage(IFormComponentMapper formComponentMapper, IFormDataBinder formDataBinder) : base(
        formComponentMapper, formDataBinder)
    {
    }

    [PageParameter(typeof(IntPageModelBinder))]
    public override int ObjectId { get; set; }

    public override Task ConfigurePage()
    {
        PageConfiguration.Headline = LocalizationService.GetString("Edit localization key");
        PageConfiguration.UIFormName = "LocalizationKeyEdit";
        return base.ConfigurePage();
    }
}
