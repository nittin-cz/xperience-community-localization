using CMS.DataEngine;

using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;

using IFormItemCollectionProvider = Kentico.Xperience.Admin.Base.Forms.Internal.IFormItemCollectionProvider;

using XperienceCommunity.Localization.Admin.UIPages;

[assembly: UIPage(
    parentType: typeof(LocalizationKeyListingPage),
    slug: PageParameterConstants.PARAMETERIZED_SLUG,
    uiPageType: typeof(LocalizationKeyEditPage),
    name: "Edit localization key",
    templateName: TemplateNames.EDIT,
    order: 1)]

namespace XperienceCommunity.Localization.Admin.UIPages;

internal class LocalizationKeyEditPage : LocalizationKeyEditPageBase
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
        IInfoProvider<LocalizationKeyInfo> localizationKeyInfoProvider) : base(formItemCollectionProvider, formDataBinder, localizationKeyInfoProvider)
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
        var result = ValidateAndProcess(model, updateExisting: true);

        if (result.LocalizationModificationResultState == LocalizationModificationResultState.Success)
        {
            var successResponse = NavigateTo(pageUrlGenerator.GenerateUrl<LocalizationKeyListingPage>())
                .AddSuccessMessage("Localization key edited");

            return Task.FromResult<ICommandResponse>(successResponse);
        }

        var errorResponse = ResponseFrom(new FormSubmissionResult(FormSubmissionStatus.ValidationFailure))
            .AddErrorMessage(result.Message);

        return Task.FromResult<ICommandResponse>(errorResponse);
    }
}
