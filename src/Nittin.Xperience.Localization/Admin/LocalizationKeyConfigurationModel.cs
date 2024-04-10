using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Nittin.Xperience.Localization.Admin;

public class LocalizationKeyConfigurationModel
{
    public int Id { get; set; }

    [TextInputComponent(Label = "Key", Order = 1)]
    [RequiredValidationRule]
    [MinLengthValidationRule(1)]
    public string Key { get; set; } = "";

    [TextAreaComponent(Label = "Description", Order = 2)]
    public string Description { get; set; } = "";

    public LocalizationKeyConfigurationModel() { }

    public LocalizationKeyConfigurationModel(LocalizationKeyInfo localizationKey)
    {
        Id = localizationKey.LocalizationKeyId;
        Key = localizationKey.LocalizationKeyName;
        Description = localizationKey.LocalizationDescription;
    }
}
