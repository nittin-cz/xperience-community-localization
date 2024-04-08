using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;
using Nittin.Xperience.Localization.Admin.UIPages;

[assembly: UIPage(typeof(LocalizationTranslationListingPage), ":", typeof(LocalizationTranslationEditPage), "Edit translation",
    TemplateNames.EDIT, 1)]

namespace Nittin.Xperience.Localization.Admin.UIPages;

public class LocalizationTranslationEditPage : InfoEditPage<LocalizationTranslationInfo>
{
    public LocalizationTranslationEditPage(IFormComponentMapper formComponentMapper, IFormDataBinder formDataBinder) : base(
        formComponentMapper, formDataBinder)
    {
    }

    [PageParameter(typeof(IntPageModelBinder))]
    public override int ObjectId { get; set; }

    public override Task ConfigurePage()
    {
        PageConfiguration.Headline = LocalizationService.GetString("Edit translation");
        PageConfiguration.UIFormName = "LocalizationTranslationEdit";
        return base.ConfigurePage();
    }
}
