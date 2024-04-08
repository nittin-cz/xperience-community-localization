using CMS.Core;
using Kentico.Xperience.Admin.Base.Forms;

namespace Nittin.Xperience.Localization.Admin.UIPages;

public class LocalizationKeyLanguageValidationRuleProperties : ValidationRuleProperties
{
    public override string GetDescriptionText(ILocalizationService localizationService)
        => "Ensures that the combination of localization key and language is unique.";
    public override string ErrorMessage => "A record with the same Localization Key and Language already exists.";

}
