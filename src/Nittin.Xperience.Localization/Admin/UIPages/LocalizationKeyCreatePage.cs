using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;

using IFormItemCollectionProvider = Kentico.Xperience.Admin.Base.Forms.Internal.IFormItemCollectionProvider;

using Nittin.Xperience.Localization.Admin.UIPages;

using CMS.DataEngine;

[assembly: UIPage(
    parentType: typeof(LocalizationKeyListingPage),
    slug: "create",
    uiPageType: typeof(LocalizationKeyCreatePage),
    name: "Create a localization key",
    templateName: TemplateNames.EDIT,
    order: UIPageOrder.NoOrder)]

namespace Nittin.Xperience.Localization.Admin.UIPages;

internal class LocalizationKeyCreatePage : LocalizationKeyEditPageBase
{
    private readonly IPageUrlGenerator pageUrlGenerator;
    private LocalizationKeyConfigurationModel? model = null;

    public LocalizationKeyCreatePage(
        IFormItemCollectionProvider formItemCollectionProvider,
        IFormDataBinder formDataBinder,
        IPageUrlGenerator pageUrlGenerator,
        IInfoProvider<LocalizationKeyInfo> localizationKeyInfoProvider
    ) : base(formItemCollectionProvider, formDataBinder, localizationKeyInfoProvider)
        => this.pageUrlGenerator = pageUrlGenerator;

    protected override LocalizationKeyConfigurationModel Model
    {
        get
        {
            model ??= new();

            return model;
        }
    }

    protected override Task<ICommandResponse> ProcessFormData(LocalizationKeyConfigurationModel model, ICollection<IFormItem> formItems)
    {
        var result = ValidateAndProcess(model);

        if (result.LocalizationModificationResultState == LocalizationModificationResultState.Success)
        {
            var successResponse = NavigateTo(pageUrlGenerator.GenerateUrl<LocalizationKeyListingPage>())
                .AddSuccessMessage("Localization key created");

            return Task.FromResult<ICommandResponse>(successResponse);
        }

        var errorResponse = ResponseFrom(new FormSubmissionResult(FormSubmissionStatus.ValidationFailure))
            .AddErrorMessage(result.Message);

        return Task.FromResult<ICommandResponse>(errorResponse);
    }
}
