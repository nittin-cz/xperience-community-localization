using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.Admin.Base.Forms;

using XperienceCommunity.Localization.Admin.Components;

[assembly: RegisterFormComponent(
    identifier: LocalizationConfigurationComponent.IDENTIFIER,
    componentType: typeof(LocalizationConfigurationComponent),
    name: "Localization Configuration")]

namespace XperienceCommunity.Localization.Admin.Components;

#pragma warning disable S2094 // intentionally empty class
public class LocalizationConfigurationComponentProperties : FormComponentProperties
{
}
#pragma warning restore

public class LocalizationConfigurationComponentClientProperties : FormComponentClientProperties<IEnumerable<LocalizationTranslationModel>>
{

}

public sealed class LocalizationConfigurationComponentAttribute : FormComponentAttribute
{
}

[ComponentAttribute(typeof(LocalizationConfigurationComponentAttribute))]
public class LocalizationConfigurationComponent : FormComponent<LocalizationConfigurationComponentProperties, LocalizationConfigurationComponentClientProperties, IEnumerable<LocalizationTranslationModel>>
{
    public const string IDENTIFIER = "nittin.xperience-community-localization.localization-configuration";

    internal List<LocalizationTranslationModel>? Value { get; set; }

    public override string ClientComponentName => "@nittin/xperience-community-localization/LocalizationConfiguration";

    public override IEnumerable<LocalizationTranslationModel> GetValue() => Value ?? [];
    public override void SetValue(IEnumerable<LocalizationTranslationModel> value) => Value = value.ToList();

    protected override async Task ConfigureClientProperties(LocalizationConfigurationComponentClientProperties properties)
    {
        properties.Value = Value ?? [];

        await base.ConfigureClientProperties(properties);
    }
}
