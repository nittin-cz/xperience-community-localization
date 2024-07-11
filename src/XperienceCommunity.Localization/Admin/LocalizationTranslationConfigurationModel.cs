using CMS.ContentEngine;
using Kentico.Forms.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using XperienceCommunity.Localization.Admin.Components;

namespace XperienceCommunity.Localization.Admin;

public class LocalizationConfigurationModel
{
    public int KeyId { get; set; }
     
    [TextInputComponent(Label = "Key", Order = 1)]
    [RequiredValidationRule]
    [MinLengthValidationRule(1)]
    public string KeyName { get; set; } = string.Empty;

    [TextAreaComponent(Label = "Description", Order = 2)]
    public string Description { get; set; } = string.Empty;

    [LocalizationConfigurationComponent(Label = "Translations", Order = 3)]
    public IEnumerable<LocalizationTranslationModel> Translations { get; set; } = [];

    public LocalizationConfigurationModel() { }

    public LocalizationConfigurationModel(IEnumerable<ContentLanguageInfo> allLanguages)
        => Translations = allLanguages.Select(LocalizationTranslationModel.New).ToList();

    public LocalizationConfigurationModel(LocalizationKeyInfo keyInfo,
        IEnumerable<LocalizationTranslationItemInfo> translations,
        IEnumerable<ContentLanguageInfo> allLanguages)
    {
        KeyId = keyInfo.LocalizationKeyItemId;
        KeyName = keyInfo.LocalizationKeyItemName;
        Description = keyInfo.LocalizationKeyItemDescription;

        var allTranslations = translations
            .Where(x => x.LocalizationTranslationItemLocalizationKeyItemId == keyInfo.LocalizationKeyItemId)
            .Select(x => new LocalizationTranslationModel(x, allLanguages))
            .ToList();

        var languagesWithoutExistingTranslations = allLanguages.Where(x => !allTranslations.Any(y => y.LanguageId == x.ContentLanguageID.ToString()));

        foreach (var language in languagesWithoutExistingTranslations)
        {
            allTranslations.Add(LocalizationTranslationModel.New(language));
        }

        Translations = allTranslations;
    }
}
