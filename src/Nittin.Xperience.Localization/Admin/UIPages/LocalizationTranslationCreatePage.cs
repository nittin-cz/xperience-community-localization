using CMS.ContentEngine;
using CMS.DataEngine;
using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;
using Nittin.Xperience.Localization.Admin.UIPages;
using IFormItemCollectionProvider = Kentico.Xperience.Admin.Base.Forms.Internal.IFormItemCollectionProvider;

[assembly: UIPage(
    parentType: typeof(LocalizationTranslationListingPage),
    slug: "create",
    uiPageType: typeof(LocalizationTranslationCreatePage),
    name: "Create a translation",
    templateName: TemplateNames.EDIT,
    order: UIPageOrder.NoOrder)]

namespace Nittin.Xperience.Localization.Admin.UIPages;

internal class LocalizationTranslationCreatePage : ModelEditPage<LocalizationTranslationConfigurationModel>
{
    private readonly IPageUrlGenerator pageUrlGenerator;
    private readonly IInfoProvider<LocalizationKeyInfo> localizationKeyInfoProvider;
    private readonly IInfoProvider<ContentLanguageInfo> contentLanguageInfoProvider;

    private LocalizationTranslationConfigurationModel? model = null;

    public LocalizationTranslationCreatePage(
        IFormItemCollectionProvider formItemCollectionProvider,
        IFormDataBinder formDataBinder,
        IPageUrlGenerator pageUrlGenerator,
        IInfoProvider<LocalizationKeyInfo> localizationKeyInfoProvider,
        IInfoProvider<ContentLanguageInfo> contentLanguageInfoProvider) : base(formItemCollectionProvider, formDataBinder)
    {
        this.pageUrlGenerator = pageUrlGenerator;
        this.localizationKeyInfoProvider = localizationKeyInfoProvider;
        this.contentLanguageInfoProvider = contentLanguageInfoProvider;
    }

    protected override LocalizationTranslationConfigurationModel Model
    {
        get
        {
            model ??= new();

            return model;
        }
    }

    protected override Task<ICommandResponse> ProcessFormData(LocalizationTranslationConfigurationModel model, ICollection<IFormItem> formItems)
    {
        var result = ValidateAndProcess(model);

        if (result == IndexModificationResult.Success)
        {
            var successResponse = NavigateTo(pageUrlGenerator.GenerateUrl<LocalizationTranslationListingPage>())
                .AddSuccessMessage("Translation record created");

            return Task.FromResult<ICommandResponse>(successResponse);
        }

        var errorResponse = ResponseFrom(new FormSubmissionResult(FormSubmissionStatus.ValidationFailure))
            .AddErrorMessage("Could not create Translation.");

        return Task.FromResult<ICommandResponse>(errorResponse);
    }

    protected IndexModificationResult ValidateAndProcess(LocalizationTranslationConfigurationModel configuration)
    {
        var localizationKey = localizationKeyInfoProvider
            .Get()
            .WhereEquals(nameof(LocalizationKeyInfo.LocalizationKeyItemName), configuration.LocalizationKeyName)
            .FirstOrDefault();

        var language = contentLanguageInfoProvider
            .Get()
            .WhereEquals(nameof(ContentLanguageInfo.ContentLanguageDisplayName), configuration.LanguageName)
            .FirstOrDefault();

        if (localizationKey == default || language == default)
        {
            return IndexModificationResult.Failure;
        }

        var localizationTranslationInfo = new LocalizationTranslationItemInfo
        {
            LocalizationTranslationItemLocalizationKeyItemId = localizationKey.LocalizationKeyItemId,
            LocalizationTranslationItemContentLanguageId = language.ContentLanguageID,
            LocalizationTranslationItemText = configuration.TranslationText
        };

        localizationTranslationInfo.Insert();

        return IndexModificationResult.Success;
    }
}
