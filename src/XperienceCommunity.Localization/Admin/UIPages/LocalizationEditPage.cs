﻿using CMS.DataEngine;

using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;

using IFormItemCollectionProvider = Kentico.Xperience.Admin.Base.Forms.Internal.IFormItemCollectionProvider;

using XperienceCommunity.Localization.Admin.UIPages;
using CMS.ContentEngine;
using XperienceCommunity.Localization.Base;

[assembly: UIPage(
    parentType: typeof(LocalizationListingPage),
    slug: PageParameterConstants.PARAMETERIZED_SLUG,
    uiPageType: typeof(LocalizationEditPage),
    name: "Edit localization",
    templateName: TemplateNames.EDIT,
    order: 1)]

namespace XperienceCommunity.Localization.Admin.UIPages;

internal class LocalizationEditPage(
    IFormItemCollectionProvider formItemCollectionProvider,
    IFormDataBinder formDataBinder,
    IPageLinkGenerator pageLinkGenerator,
    IInfoProvider<ContentLanguageInfo> contentLanguageInfoProvider,
    IInfoProvider<LocalizationKeyInfo> localizationKeyInfoProvider,
    IInfoProvider<LocalizationTranslationItemInfo> localizationTranslationItemInfoProvider) : LocalizationEditPageBase(formItemCollectionProvider,
            formDataBinder,
            localizationKeyInfoProvider,
            localizationTranslationItemInfoProvider)
{
    private readonly IInfoProvider<LocalizationKeyInfo> localizationKeyInfoProvider = localizationKeyInfoProvider;
    private readonly IInfoProvider<ContentLanguageInfo> contentLanguageInfoProvider = contentLanguageInfoProvider;
    private readonly IInfoProvider<LocalizationTranslationItemInfo> localizationTranslationItemInfoProvider = localizationTranslationItemInfoProvider;
    private LocalizationConfigurationModel? model = null;

    [PageParameter(typeof(IntPageModelBinder))]
    public int KeyIdentifier { get; set; }

    protected override LocalizationConfigurationModel Model
    {
        get
        {
            model ??= new LocalizationConfigurationModel(
                localizationKeyInfoProvider.Get().WithID(KeyIdentifier).FirstOrDefault() ?? throw new InvalidOperationException("Specified key does not exist"),
                localizationTranslationItemInfoProvider.Get().GetEnumerableTypedResult(),
                contentLanguageInfoProvider.Get().GetEnumerableTypedResult()
            );

            return model;
        }
    }

    public override Task ConfigurePage()
    {
        PageConfiguration.Headline = LocalizationService.GetString("Edit localization key");
        return base.ConfigurePage();
    }

    protected override Task<ICommandResponse> ProcessFormData(LocalizationConfigurationModel model, ICollection<IFormItem> formItems)
    {
        var result = ValidateAndProcess(model, updateExisting: true);

        if (result.LocalizationModificationResultState == LocalizationModificationResultState.Success)
        {
            var successResponse = NavigateTo(pageLinkGenerator.GetPath<LocalizationListingPage>())
                .AddSuccessMessage("Localization key edited");

            return Task.FromResult<ICommandResponse>(successResponse);
        }

        var errorResponse = ResponseFrom(new FormSubmissionResult(FormSubmissionStatus.ValidationFailure))
            .AddErrorMessage(result.Message);

        return Task.FromResult<ICommandResponse>(errorResponse);
    }
}
