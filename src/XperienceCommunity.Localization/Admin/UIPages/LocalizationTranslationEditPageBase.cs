using CMS.DataEngine;

using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;

using IFormItemCollectionProvider = Kentico.Xperience.Admin.Base.Forms.Internal.IFormItemCollectionProvider;

namespace XperienceCommunity.Localization.Admin.UIPages;

internal abstract class LocalizationTranslationEditPageBase : ModelEditPage<LocalizationTranslationConfigurationModel>
{
    private readonly IInfoProvider<LocalizationTranslationItemInfo> localizationTranslationInfoProvider;

    protected LocalizationTranslationEditPageBase(
        IFormItemCollectionProvider formItemCollectionProvider,
        IFormDataBinder formDataBinder,
        IInfoProvider<LocalizationTranslationItemInfo> localizationTranslationInfoProvider) : base(formItemCollectionProvider, formDataBinder)
            => this.localizationTranslationInfoProvider = localizationTranslationInfoProvider;

    protected LocalizationModificationResult ValidateAndProcess(LocalizationTranslationConfigurationModel configuration, bool updateExisting = false)
    {
        var localizationTranslationInfo = new LocalizationTranslationItemInfo();

        if (updateExisting)
        {
            string translationDoesNotExist = "Specified translation does not exist";

            localizationTranslationInfo = localizationTranslationInfoProvider.Get().WithID(configuration.Id).FirstOrDefault() ??
                throw new InvalidOperationException(translationDoesNotExist);
        }

        int languageId = int.Parse(configuration.LanguageId);
        int keyId = int.Parse(configuration.LocalizationKeyId);

        localizationTranslationInfo.LocalizationTranslationItemContentLanguageId = languageId;
        localizationTranslationInfo.LocalizationTranslationItemLocalizationKeyItemId = keyId;
        localizationTranslationInfo.LocalizationTranslationItemText = configuration.TranslationText;

        if (localizationTranslationInfoProvider.Get()
            .WhereEquals(nameof(LocalizationTranslationItemInfo.LocalizationTranslationItemContentLanguageId), languageId)
            .WhereEquals(nameof(LocalizationTranslationItemInfo.LocalizationTranslationItemLocalizationKeyItemId), keyId)
            .WhereNotEquals(nameof(LocalizationTranslationItemInfo.LocalizationTranslationItemID), configuration.Id)
            .Count() > 0
        )
        {
            string invalidKeyLanguageCombinationErrorMessage = "A record with the same Localization Key and Language already exists.";

            return new LocalizationModificationResult(LocalizationModificationResultState.Failure,
                invalidKeyLanguageCombinationErrorMessage);
        }

        if (updateExisting)
        {
            localizationTranslationInfo.Update();
        }
        else
        {
            localizationTranslationInfo.Insert();
        }

        return new(LocalizationModificationResultState.Success);
    }
}
