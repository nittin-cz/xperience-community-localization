using CMS.DataEngine;
using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;
using Nittin.Xperience.Localization.Admin.UIPages;
using IFormItemCollectionProvider = Kentico.Xperience.Admin.Base.Forms.Internal.IFormItemCollectionProvider;

[assembly: UIPage(
    parentType: typeof(LocalizationKeyListingPage),
    slug: PageParameterConstants.PARAMETERIZED_SLUG,
    uiPageType: typeof(LocalizationKeyEditPage),
    name: "Edit localization key",
    templateName: TemplateNames.EDIT,
    order: 1)]

namespace Nittin.Xperience.Localization.Admin.UIPages;

internal class LocalizationKeyEditPage : ModelEditPage<LocalizationKeyConfigurationModel>
{
    private readonly IPageUrlGenerator pageUrlGenerator;
    private readonly IInfoProvider<LocalizationKeyInfo> localizationKeyInfoProvider;
    private LocalizationKeyConfigurationModel? model = null;

    [PageParameter(typeof(IntPageModelBinder))]
    public int KeyIdentifier { get; set; }

    public LocalizationKeyEditPage(
        IFormItemCollectionProvider formItemCollectionProvider,
        IFormDataBinder formDataBinder,
        IPageUrlGenerator pageUrlGenerator,
        IInfoProvider<LocalizationKeyInfo> localizationKeyInfoProvider) : base(formItemCollectionProvider, formDataBinder)
    {
        this.pageUrlGenerator = pageUrlGenerator;
        this.localizationKeyInfoProvider = localizationKeyInfoProvider;
    }

    protected override LocalizationKeyConfigurationModel Model
    {
        get
        {
            model ??= new LocalizationKeyConfigurationModel(
                localizationKeyInfoProvider.Get().WithID(KeyIdentifier).FirstOrDefault() ??
                    throw new InvalidOperationException("Specified key does not exist")
                );

            return model;
        }
    }

    public override Task ConfigurePage()
    {
        PageConfiguration.Headline = LocalizationService.GetString("Edit localization key");
        return base.ConfigurePage();
    }

    protected override Task<ICommandResponse> ProcessFormData(LocalizationKeyConfigurationModel model, ICollection<IFormItem> formItems)
    {
        var result = ValidateAndProcess(model);

        if (result == IndexModificationResult.Success)
        {
            var successResponse = NavigateTo(pageUrlGenerator.GenerateUrl<LocalizationKeyListingPage>())
                .AddSuccessMessage("Localization key edited");

            return Task.FromResult<ICommandResponse>(successResponse);
        }

        var errorResponse = ResponseFrom(new FormSubmissionResult(FormSubmissionStatus.ValidationFailure))
            .AddErrorMessage("Could not edit Localization key.");

        return Task.FromResult<ICommandResponse>(errorResponse);
    }

    protected IndexModificationResult ValidateAndProcess(LocalizationKeyConfigurationModel configuration)
    {
        var localizationKeyInfo = localizationKeyInfoProvider.Get().WithID(KeyIdentifier).FirstOrDefault() ??
            throw new InvalidOperationException("Specified key does not exist");

        localizationKeyInfo.LocalizationKeyItemDescription = configuration.Description;
        localizationKeyInfo.LocalizationKeyItemName = configuration.Key;

        localizationKeyInfo.Update();

        return IndexModificationResult.Success;
    }
}
