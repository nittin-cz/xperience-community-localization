using Kentico.Xperience.Admin.Base.FormAnnotations;
using Nittin.Xperience.Localization.Admin.Providers;

namespace Nittin.Xperience.Localization.Admin;

public class LocalizationTranslationConfigurationModel
{
    public int Id { get; set; }

    [DropDownComponent(Label = "Translation key", DataProviderType = typeof(TranslationKeyOptionsProvider), Order = 1)]
    public string LocalizationKey { get; set; } = "";

    [DropDownComponent(Label = "Localized language", DataProviderType = typeof(LanguageOptionsProvider), Order = 2)]
    public string LanguageName { get; set; } = "";

    [TextAreaComponent(Label = "Translation text", Order = 3)]
    public string TranslationText { get; set; } = "";

    public LocalizationTranslationConfigurationModel() { }

    public LocalizationTranslationConfigurationModel(
        LocalizationTranslationInfo localizationTranslation,
        string languageName,
        string translationKey
    )
    {
        LanguageName = languageName;
        LocalizationKey = translationKey;
        TranslationText = localizationTranslation.LocalizationTranslationText;
    }
}
