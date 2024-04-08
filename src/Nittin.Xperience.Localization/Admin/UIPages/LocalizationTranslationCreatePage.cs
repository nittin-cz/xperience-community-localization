using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;
using Nittin.Xperience.Localization.Admin.UIPages;

[assembly: UIPage(typeof(LocalizationTranslationListingPage), "create", typeof(LocalizationTranslationCreatePage),
    "Create a localization key",
    TemplateNames.EDIT, 1)]

namespace Nittin.Xperience.Localization.Admin.UIPages;

public class
    LocalizationTranslationCreatePage : CreatePage<LocalizationTranslationInfo, LocalizationTranslationListingPage>
{
    public LocalizationTranslationCreatePage(IFormComponentMapper formComponentMapper, IFormDataBinder formDataBinder,
        IPageUrlGenerator pageUrlGenerator) : base(formComponentMapper, formDataBinder, pageUrlGenerator)
    {
    }
    public override Task ConfigurePage()
    {
        PageConfiguration.UIFormName = "LocalizationTranslationCreate";

        return base.ConfigurePage();
    }

    protected override Task<ICommandResponse> GetSubmitSuccessResponse(LocalizationTranslationInfo savedInfoObject,
        ICollection<IFormItem> items) => Task.FromResult(
        (ICommandResponse)NavigateTo(pageUrlGenerator.GenerateUrl<LocalizationTranslationListingPage>())
            .AddSuccessMessage("Localization key created"));

    protected override Task SetFormData(LocalizationTranslationInfo infoObject,
        IFormFieldValueProvider fieldValueProvider)
    {
        infoObject.LocalizationTranslationGuid = Guid.NewGuid();
        return base.SetFormData(infoObject, fieldValueProvider);
    }
}
