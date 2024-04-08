using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;
using Nittin.Xperience.Localization.Admin.UIPages;

[assembly: UIPage(typeof(LocalizationKeyListingPage), "create", typeof(LocalizationKeyCreatePage), "Create a localization key",
    TemplateNames.EDIT, 1)]

namespace Nittin.Xperience.Localization.Admin.UIPages;

public class LocalizationKeyCreatePage : CreatePage<LocalizationKeyInfo, LocalizationKeyListingPage>
{
    public LocalizationKeyCreatePage(IFormComponentMapper formComponentMapper, IFormDataBinder formDataBinder,
        IPageUrlGenerator pageUrlGenerator) : base(formComponentMapper, formDataBinder, pageUrlGenerator)
    {
    }

    public override Task ConfigurePage()
    {
        PageConfiguration.UIFormName = "LocalizationKeyEdit";

        return base.ConfigurePage();
    }

    protected override Task<ICommandResponse> GetSubmitSuccessResponse(LocalizationKeyInfo savedInfoObject, ICollection<IFormItem> items) => Task.FromResult(
            (ICommandResponse)NavigateTo(pageUrlGenerator.GenerateUrl<LocalizationKeyListingPage>())
                .AddSuccessMessage("Localization key created"));

    protected override Task SetFormData(LocalizationKeyInfo infoObject, IFormFieldValueProvider fieldValueProvider)
    {
        infoObject.LocalizationKeyGuid = Guid.NewGuid();
        return base.SetFormData(infoObject, fieldValueProvider);
    }
}
