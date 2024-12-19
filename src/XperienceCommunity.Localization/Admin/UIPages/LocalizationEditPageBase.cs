using CMS.DataEngine;

using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;
using XperienceCommunity.Localization.Base;
using IFormItemCollectionProvider = Kentico.Xperience.Admin.Base.Forms.Internal.IFormItemCollectionProvider;

namespace XperienceCommunity.Localization.Admin.UIPages;

internal abstract class LocalizationEditPageBase(
    IFormItemCollectionProvider formItemCollectionProvider,
    IFormDataBinder formDataBinder,
    IInfoProvider<LocalizationKeyInfo> localizationKeyInfoProvider,
    IInfoProvider<LocalizationTranslationItemInfo> localizationTranslationInfoProvider
    ) : ModelEditPage<LocalizationConfigurationModel>(formItemCollectionProvider, formDataBinder)
{
    protected LocalizationModificationResult ValidateAndProcess(LocalizationConfigurationModel configuration, bool updateExisting = false)
    {
        var localizationKeyInfo = new LocalizationKeyInfo();

        if (updateExisting)
        {
            localizationKeyInfo = localizationKeyInfoProvider.Get().WithID(configuration.KeyId).FirstOrDefault();

            if (localizationKeyInfo == null)
            {
                string keyDoesNotExistErrorMessage = "Specified key does not exist";

                return new LocalizationModificationResult(LocalizationModificationResultState.Failure, keyDoesNotExistErrorMessage);
            }
        }

        if (localizationKeyInfoProvider.Get()
            .WhereEquals(nameof(LocalizationKeyInfo.LocalizationKeyItemName), configuration.KeyName)
            .WhereNotEquals(nameof(LocalizationKeyInfo.LocalizationKeyItemId), configuration.KeyId)
            .Any()
        )
        {
            string invalidKeyLanguageCombinationErrorMessage = "A record with the same Localization Key already exists.";

            return new LocalizationModificationResult(LocalizationModificationResultState.Failure,
                invalidKeyLanguageCombinationErrorMessage);
        }

        localizationKeyInfo.LocalizationKeyItemDescription = configuration.Description;
        localizationKeyInfo.LocalizationKeyItemName = configuration.KeyName;

        if (updateExisting)
        {
            localizationKeyInfo.Update();
        }
        else
        {
            localizationKeyInfo.Insert();
            configuration.KeyId = localizationKeyInfo.LocalizationKeyItemId;
        }

        foreach (var translation in configuration.Translations)
        {
            ValidateAndProcessTranslation(translation, configuration);
        }

        return new(LocalizationModificationResultState.Success);
    }

    private void ValidateAndProcessTranslation(LocalizationTranslationModel translation, LocalizationConfigurationModel localization)
    {
        var localizationTranslationInfo = localizationTranslationInfoProvider.Get(translation.Id);

        if (localizationTranslationInfo is not null)
        {
            if (string.IsNullOrEmpty(translation.TranslationText))
            {
                localizationTranslationInfoProvider.Delete(localizationTranslationInfo);

                return;
            }
            else
            {
                localizationTranslationInfo.LocalizationTranslationItemText = translation.TranslationText;
                localizationTranslationInfo.Update();

                return;
            }
        }
        else
        {
            localizationTranslationInfo = new LocalizationTranslationItemInfo();

            int languageId = int.Parse(translation.LanguageId);
            int keyId = localization.KeyId;

            localizationTranslationInfo.LocalizationTranslationItemContentLanguageId = languageId;
            localizationTranslationInfo.LocalizationTranslationItemLocalizationKeyItemId = keyId;
            localizationTranslationInfo.LocalizationTranslationItemText = translation.TranslationText;

            localizationTranslationInfoProvider.Set(localizationTranslationInfo);
        }
    }
}
