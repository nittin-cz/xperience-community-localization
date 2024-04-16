using CMS.DataEngine;
using Kentico.Xperience.Admin.Base.Forms;
using Nittin.Xperience.Localization.Admin.UIPages;

[assembly:
    RegisterFormValidationRule("Nittin.Xperience.Validation.LocalizationKeyLanguage",
        typeof(LocalizationKeyLanguageValidationRule), "Localization key and language uniqueness",
        "Ensures that the combination of localization key and language is unique. Usable only on create form.")]

namespace Nittin.Xperience.Localization.Admin.UIPages;

public class
    LocalizationKeyLanguageValidationRule : ValidationRule<LocalizationKeyLanguageValidationRuleProperties, int?>
{
    private readonly IInfoProvider<LocalizationTranslationItemInfo> localizationTranslationInfoProvider;
    public LocalizationKeyLanguageValidationRule(IInfoProvider<LocalizationTranslationItemInfo> localizationTranslationInfoProvider) => this.localizationTranslationInfoProvider = localizationTranslationInfoProvider;

    public override Task<ValidationResult> Validate(int? value, IFormFieldValueProvider formFieldValueProvider)
    {
        bool keyIdHasValue = formFieldValueProvider.TryGet(nameof(LocalizationTranslationItemInfo.LocalizationTranslationItemLocalizationKeyItemId), out int localizationKey);
        bool languageHasValue = formFieldValueProvider.TryGet(nameof(LocalizationTranslationItemInfo.LocalizationTranslationItemContentLanguageId), out int language);

        if (keyIdHasValue && languageHasValue)
        {
            bool pairExists = localizationTranslationInfoProvider.Get()
                .WhereEquals(nameof(LocalizationTranslationItemInfo.LocalizationTranslationItemLocalizationKeyItemId), localizationKey)
                .WhereEquals(nameof(LocalizationTranslationItemInfo.LocalizationTranslationItemContentLanguageId), language)
                .TopN(1).Count() > 0;

            if (pairExists)
            {
                return ValidationResult.FailResult("A record with the same Localization Key and Language already exists.");
            }
        }

        return ValidationResult.SuccessResult();
    }
}
