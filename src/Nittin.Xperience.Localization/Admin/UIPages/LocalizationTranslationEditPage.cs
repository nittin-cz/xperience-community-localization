using CMS.ContentEngine;
using CMS.DataEngine;
using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;
using Nittin.Xperience.Localization.Admin.UIPages;
using IFormItemCollectionProvider = Kentico.Xperience.Admin.Base.Forms.Internal.IFormItemCollectionProvider;

[assembly: UIPage(
    parentType: typeof(LocalizationTranslationListingPage),
    slug: "create",
    uiPageType: typeof(LocalizationTranslationEditPage),
    name: "Create a localization key",
    templateName: TemplateNames.EDIT,
    order: 1)]

namespace Nittin.Xperience.Localization.Admin.UIPages;

internal class LocalizationTranslationEditPage : ModelEditPage<LocalizationTranslationConfigurationModel>
{
    private readonly IPageUrlGenerator pageUrlGenerator;
    private readonly IInfoProvider<LocalizationKeyInfo> localizationKeyInfoProvider;
    private readonly IInfoProvider<ContentLanguageInfo> contentLanguageInfoProvider;
    private readonly IInfoProvider<LocalizationTranslationInfo> localizationTranslationInfoProvider;

    [PageParameter(typeof(IntPageModelBinder))]
    public int KeyIdentifier { get; set; }

    private LocalizationTranslationConfigurationModel? model = null;

    public LocalizationTranslationEditPage(
        IFormItemCollectionProvider formItemCollectionProvider,
        IFormDataBinder formDataBinder,
        IPageUrlGenerator pageUrlGenerator,
        IInfoProvider<LocalizationKeyInfo> localizationKeyInfoProvider,
        IInfoProvider<ContentLanguageInfo> contentLanguageInfoProvider,
        IInfoProvider<LocalizationTranslationInfo> localizationTranslationInfoProvider) : base(formItemCollectionProvider, formDataBinder)
    {
        this.pageUrlGenerator = pageUrlGenerator;
        this.localizationKeyInfoProvider = localizationKeyInfoProvider;
        this.contentLanguageInfoProvider = contentLanguageInfoProvider;
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

        string languageName = contentLanguageInfoProvider
            .Get()
            .WithID(infoModel.LocalizationTranslationContentLanguageId)
            .FirstOrDefault()
            !.ContentLanguageDisplayName;

        string localizationKeyName = localizationKeyInfoProvider
            .Get()
            .WithID(infoModel.LocalizationTranslationLocalizationKeyId)
            .FirstOrDefault()
            !.LocalizationKeyName;

        return new LocalizationTranslationConfigurationModel(infoModel, languageName, localizationKeyName);
    }

    protected override Task<ICommandResponse> ProcessFormData(LocalizationTranslationConfigurationModel model, ICollection<IFormItem> formItems)
    {
        var result = ValidateAndProcess(model);

        if (result == IndexModificationResult.Success)
        {
            var successResponse = NavigateTo(pageUrlGenerator.GenerateUrl<LocalizationTranslationListingPage>())
                .AddSuccessMessage("Translation record edited");

            return Task.FromResult<ICommandResponse>(successResponse);
        }

        var errorResponse = ResponseFrom(new FormSubmissionResult(FormSubmissionStatus.ValidationFailure))
            .AddErrorMessage("Could not edit Translation.");

        return Task.FromResult<ICommandResponse>(errorResponse);
    }

    protected IndexModificationResult ValidateAndProcess(LocalizationTranslationConfigurationModel configuration)
    {
        var localizationTranslationInfo = localizationTranslationInfoProvider.Get().WithID(KeyIdentifier).FirstOrDefault() ??
            throw new InvalidOperationException("Specified translation does not exist");

        var localizationKey = localizationKeyInfoProvider
            .Get()
            .WhereEquals(nameof(LocalizationKeyInfo.LocalizationKeyId), configuration.LocalizationKey)
            .FirstOrDefault();

        var language = contentLanguageInfoProvider
            .Get()
            .WhereEquals(nameof(ContentLanguageInfo.ContentLanguageDisplayName), configuration.LanguageName)
            .FirstOrDefault();

        if (localizationKey == default || language == default)
        {
            return IndexModificationResult.Failure;
        }

        localizationTranslationInfo.LocalizationTranslationContentLanguageId = language.ContentLanguageID;
        localizationTranslationInfo.LocalizationTranslationLocalizationKeyId = localizationKey.LocalizationKeyId;
        localizationTranslationInfo.LocalizationTranslationText = configuration.TranslationText;

        localizationTranslationInfo.Update();

        return IndexModificationResult.Success;
    }
}
