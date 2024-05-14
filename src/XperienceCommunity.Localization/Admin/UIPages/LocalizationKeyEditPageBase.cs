using CMS.DataEngine;

using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;

using IFormItemCollectionProvider = Kentico.Xperience.Admin.Base.Forms.Internal.IFormItemCollectionProvider;

namespace XperienceCommunity.Localization.Admin.UIPages;

internal abstract class LocalizationKeyEditPageBase : ModelEditPage<LocalizationKeyConfigurationModel>
{
    private readonly IInfoProvider<LocalizationKeyInfo> localizationKeyInfoProvider;

    protected LocalizationKeyEditPageBase(
        IFormItemCollectionProvider formItemCollectionProvider,
        IFormDataBinder formDataBinder,
        IInfoProvider<LocalizationKeyInfo> localizationKeyInfoProvider
    ) : base(formItemCollectionProvider, formDataBinder)
        => this.localizationKeyInfoProvider = localizationKeyInfoProvider;

    protected LocalizationModificationResult ValidateAndProcess(LocalizationKeyConfigurationModel configuration, bool updateExisting = false)
    {
        var localizationKeyInfo = new LocalizationKeyInfo();

        if (updateExisting)
        {
            localizationKeyInfo = localizationKeyInfoProvider.Get().WithID(configuration.Id).FirstOrDefault();

            if (localizationKeyInfo == null)
            {
                string keyDoesNotExistErrorMessage = "Specified key does not exist";

                return new LocalizationModificationResult(LocalizationModificationResultState.Failure, keyDoesNotExistErrorMessage);
            }
        }

        if (localizationKeyInfoProvider.Get()
            .WhereEquals(nameof(LocalizationKeyInfo.LocalizationKeyItemName), configuration.Key)
            .WhereNotEquals(nameof(LocalizationKeyInfo.LocalizationKeyItemId), configuration.Id)
            .Count() > 0
        )
        {
            string invalidKeyLanguageCombinationErrorMessage = "A record with the same Localization Key already exists.";

            return new LocalizationModificationResult(LocalizationModificationResultState.Failure,
                invalidKeyLanguageCombinationErrorMessage);
        }

        localizationKeyInfo.LocalizationKeyItemDescription = configuration.Description;
        localizationKeyInfo.LocalizationKeyItemName = configuration.Key;

        if (updateExisting)
        {
            localizationKeyInfo.Update();
        }
        else
        {
            localizationKeyInfo.Insert();
        }

        return new(LocalizationModificationResultState.Success);
    }
}
