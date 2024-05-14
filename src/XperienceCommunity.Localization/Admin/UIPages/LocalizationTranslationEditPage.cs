using CMS.DataEngine;

using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;

using XperienceCommunity.Localization.Admin.UIPages;

using IFormItemCollectionProvider = Kentico.Xperience.Admin.Base.Forms.Internal.IFormItemCollectionProvider;

[assembly: UIPage(
    parentType: typeof(LocalizationTranslationListingPage),
    slug: PageParameterConstants.PARAMETERIZED_SLUG,
    uiPageType: typeof(LocalizationTranslationEditPage),
    name: "Edit translation key",
    templateName: TemplateNames.EDIT,
    order: UIPageOrder.NoOrder)]

namespace XperienceCommunity.Localization.Admin.UIPages;

internal class LocalizationTranslationEditPage : LocalizationTranslationEditPageBase
{
    private readonly IPageUrlGenerator pageUrlGenerator;
    private readonly IInfoProvider<LocalizationTranslationItemInfo> localizationTranslationInfoProvider;

    [PageParameter(typeof(IntPageModelBinder))]
    public int KeyIdentifier { get; set; }

    private LocalizationTranslationConfigurationModel? model = null;

    public LocalizationTranslationEditPage(
        IFormItemCollectionProvider formItemCollectionProvider,
        IFormDataBinder formDataBinder,
        IPageUrlGenerator pageUrlGenerator,
        IInfoProvider<LocalizationTranslationItemInfo> localizationTranslationInfoProvider
    )
        : base(formItemCollectionProvider, formDataBinder, localizationTranslationInfoProvider)
    {
        this.pageUrlGenerator = pageUrlGenerator;
        this.localizationTranslationInfoProvider = localizationTranslationInfoProvider;
    }

    protected override LocalizationTranslationConfigurationModel Model
    {
        get
        {
            model ??= GetConfigurationModel();
            return model;
        }
    }

    public override Task ConfigurePage()
    {
        PageConfiguration.Headline = LocalizationService.GetString("Edit translation");
        return base.ConfigurePage();
    }

    private LocalizationTranslationConfigurationModel GetConfigurationModel()
    {
        var infoModel = localizationTranslationInfoProvider.Get().WithID(KeyIdentifier).FirstOrDefault() ??
            throw new InvalidOperationException("Specified key does not exist");

        return new LocalizationTranslationConfigurationModel(infoModel,
            infoModel.LocalizationTranslationItemContentLanguageId.ToString(),
            infoModel.LocalizationTranslationItemLocalizationKeyItemId.ToString());
    }

    protected override Task<ICommandResponse> ProcessFormData(LocalizationTranslationConfigurationModel model, ICollection<IFormItem> formItems)
    {
        var result = ValidateAndProcess(model, updateExisting: true);

        if (result.LocalizationModificationResultState == LocalizationModificationResultState.Success)
        {
            var successResponse = NavigateTo(pageUrlGenerator.GenerateUrl<LocalizationTranslationListingPage>())
                .AddSuccessMessage("Translation record edited");

            return Task.FromResult<ICommandResponse>(successResponse);
        }

        var errorResponse = ResponseFrom(new FormSubmissionResult(FormSubmissionStatus.ValidationFailure))
            .AddErrorMessage(result.Message);

        return Task.FromResult<ICommandResponse>(errorResponse);
    }
}
