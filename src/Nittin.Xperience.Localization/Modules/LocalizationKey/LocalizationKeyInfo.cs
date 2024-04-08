//using CMS;
using CMS.DataEngine;

namespace Nittin.Xperience.Localization;

public partial class LocalizationKeyInfo
{
    /// <summary>
    /// Type information.
    /// </summary>
    public static readonly ObjectTypeInfo TYPEINFO = new(typeof(IInfoProvider<LocalizationKeyInfo>), OBJECT_TYPE, "NittinLocalization.LocalizationKey", "LocalizationKeyID", null, "LocalizationKeyGuid", "LocalizationKeyName", "LocalizationKeyName", null, null, null)
    {
        TouchCacheDependencies = true,
        ContinuousIntegrationSettings =
        {
            Enabled = true
        },
    };
}
