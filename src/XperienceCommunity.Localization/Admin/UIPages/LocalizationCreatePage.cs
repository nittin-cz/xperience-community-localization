using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;

using IFormItemCollectionProvider = Kentico.Xperience.Admin.Base.Forms.Internal.IFormItemCollectionProvider;

using XperienceCommunity.Localization.Admin.UIPages;

using CMS.DataEngine;
using CMS.ContentEngine;
using XperienceCommunity.Localization.Base;

[assembly: UIPage(
    parentType: typeof(LocalizationListingPage),
    slug: "create",
    uiPageType: typeof(LocalizationCreatePage),
    name: "Create a localization",
    templateName: TemplateNames.EDIT,
    order: UIPageOrder.NoOrder)]

namespace XperienceCommunity.Localization.Admin.UIPages;

internal class LocalizationCreatePage(
    IFormItemCollectionProvider formItemCollectionProvider,
    IFormDataBinder formDataBinder,
    IPageLinkGenerator pageLinkGenerator,
    IInfoProvider<LocalizationKeyInfo> localizationKeyInfoProvider,
    IInfoProvider<ContentLanguageInfo> contentLanguageInfoProvider,
    IInfoProvider<LocalizationTranslationItemInfo> localizationTranslationItemInfo
    ) : LocalizationEditPageBase(formItemCollectionProvider,
        formDataBinder,
        localizationKeyInfoProvider,
        localizationTranslationItemInfo
    )
{
    private LocalizationConfigurationModel? model = null;

    protected override LocalizationConfigurationModel Model
    {
        get
        {
            model ??= new(contentLanguageInfoProvider.Get().GetEnumerableTypedResult());

            return model;
        }
    }

    protected override Task<ICommandResponse> ProcessFormData(LocalizationConfigurationModel model, ICollection<IFormItem> formItems)
    {
        var result = ValidateAndProcess(model);

        if (result.LocalizationModificationResultState == LocalizationModificationResultState.Success)
        {
            var successResponse = NavigateTo(pageLinkGenerator.GetPath<LocalizationListingPage>())
                .AddSuccessMessage("Localization key created");

            return Task.FromResult<ICommandResponse>(successResponse);
        }

        var errorResponse = ResponseFrom(new FormSubmissionResult(FormSubmissionStatus.ValidationFailure))
            .AddErrorMessage(result.Message);

        return Task.FromResult<ICommandResponse>(errorResponse);
    }
}
