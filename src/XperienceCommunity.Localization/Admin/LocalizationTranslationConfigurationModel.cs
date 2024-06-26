﻿using Kentico.Xperience.Admin.Base.FormAnnotations;
using XperienceCommunity.Localization.Admin.Providers;

namespace XperienceCommunity.Localization.Admin;

public class LocalizationTranslationConfigurationModel
{
    public int Id { get; set; }

    [DropDownComponent(Label = "Translation key", DataProviderType = typeof(TranslationKeyOptionsProvider), Order = 1)]
    public string LocalizationKeyId { get; set; } = "";

    [DropDownComponent(Label = "Localized language", DataProviderType = typeof(LanguageOptionsProvider), Order = 2)]
    public string LanguageId { get; set; } = "";

    [TextAreaComponent(Label = "Translation text", Order = 3)]
    public string TranslationText { get; set; } = "";

    public LocalizationTranslationConfigurationModel() { }

    public LocalizationTranslationConfigurationModel(
        LocalizationTranslationItemInfo localizationTranslation,
        string languageId,
        string localizationKeyId
    )
    {
        Id = localizationTranslation.LocalizationTranslationItemID;
        LanguageId = languageId;
        LocalizationKeyId = localizationKeyId;
        TranslationText = localizationTranslation.LocalizationTranslationItemText;
    }
}
