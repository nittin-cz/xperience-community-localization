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

internal class LocalizationTranslationCreatePage : LocalizationTranslationEditPageBase
{
    private readonly IPageUrlGenerator pageUrlGenerator;

    private LocalizationTranslationConfigurationModel? model = null;

    public LocalizationTranslationCreatePage(
        IFormItemCollectionProvider formItemCollectionProvider,
        IFormDataBinder formDataBinder,
        IPageUrlGenerator pageUrlGenerator,
        IInfoProvider<LocalizationTranslationItemInfo> localizationTranslationInfoProvider
    ) : base(formItemCollectionProvider, formDataBinder, localizationTranslationInfoProvider)
        => this.pageUrlGenerator = pageUrlGenerator;

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

        if (result.LocalizationModificationResultState == LocalizationModificationResultState.Success)
        {
            var successResponse = NavigateTo(pageUrlGenerator.GenerateUrl<LocalizationTranslationListingPage>())
                .AddSuccessMessage("Translation record created");

            return Task.FromResult<ICommandResponse>(successResponse);
        }

        var errorResponse = ResponseFrom(new FormSubmissionResult(FormSubmissionStatus.ValidationFailure))
            .AddErrorMessage(result.Message);

        return Task.FromResult<ICommandResponse>(errorResponse);
    }
}
