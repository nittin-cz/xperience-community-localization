using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;
using Nittin.Xperience.Localization.Admin.UIPages;
using IFormItemCollectionProvider = Kentico.Xperience.Admin.Base.Forms.Internal.IFormItemCollectionProvider;

[assembly: UIPage(
    parentType: typeof(LocalizationKeyListingPage),
    slug: "create",
    uiPageType: typeof(LocalizationKeyCreatePage),
    name: "Create a localization key",
    templateName: TemplateNames.EDIT,
    order: UIPageOrder.NoOrder)]

namespace Nittin.Xperience.Localization.Admin.UIPages;

internal class LocalizationKeyCreatePage : ModelEditPage<LocalizationKeyConfigurationModel>
{
    private readonly IPageUrlGenerator pageUrlGenerator;
    private LocalizationKeyConfigurationModel? model = null;

    public LocalizationKeyCreatePage(
        IFormItemCollectionProvider formItemCollectionProvider,
        IFormDataBinder formDataBinder,
        IPageUrlGenerator pageUrlGenerator) : base(formItemCollectionProvider, formDataBinder) => this.pageUrlGenerator = pageUrlGenerator;

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

        if (result == IndexModificationResult.Success)
        {
            var successResponse = NavigateTo(pageUrlGenerator.GenerateUrl<LocalizationKeyListingPage>())
                .AddSuccessMessage("Localization key created");

            return Task.FromResult<ICommandResponse>(successResponse);
        }

        var errorResponse = ResponseFrom(new FormSubmissionResult(FormSubmissionStatus.ValidationFailure))
            .AddErrorMessage("Could not create Localization key.");

        return Task.FromResult<ICommandResponse>(errorResponse);
    }

    protected IndexModificationResult ValidateAndProcess(LocalizationKeyConfigurationModel configuration)
    {
        var localizationKeyInfo = new LocalizationKeyInfo
        {
            LocalizationKeyItemName = configuration.Key,
            LocalizationKeyItemDescription = configuration.Description
        };

        localizationKeyInfo.Insert();

        return IndexModificationResult.Success;
    }
}
