using CMS.DataEngine;

namespace Nittin.Xperience.Localization;

public partial class LocalizationTranslationInfo : AbstractInfo<LocalizationTranslationInfo, IInfoProvider<LocalizationTranslationInfo>>, IInfoWithId, IInfoWithGuid
{
    /// <summary>
    /// Type information.
    /// </summary>
    public static readonly ObjectTypeInfo TYPEINFO = new(typeof(IInfoProvider<LocalizationTranslationInfo>), OBJECT_TYPE, "nittinlocalization.LocalizationTranslation", "LocalizationTranslationID", null, "LocalizationTranslationGuid", null, null, null, null, null)
    {
        TouchCacheDependencies = true,
        DependsOn = new List<ObjectDependency>()
            {
                new("LocalizationKey", "nittinlocalization.localizationkey", ObjectDependencyEnum.Required),
                new("Language", "cms.contentlanguage", ObjectDependencyEnum.Required),
            },
        ContinuousIntegrationSettings =
        {
            Enabled = true
        },
    };
}
