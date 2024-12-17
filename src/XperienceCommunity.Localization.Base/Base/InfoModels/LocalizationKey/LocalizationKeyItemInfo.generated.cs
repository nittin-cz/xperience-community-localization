using System;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using XperienceCommunity.Localization;

[assembly: RegisterObjectType(typeof(LocalizationKeyInfo), LocalizationKeyInfo.OBJECT_TYPE)]

namespace XperienceCommunity.Localization
{
    /// <summary>
    /// Data container class for <see cref="LocalizationKeyInfo"/>.
    /// </summary>
    [Serializable]
    public partial class LocalizationKeyInfo : AbstractInfo<LocalizationKeyInfo, IInfoProvider<LocalizationKeyInfo>>, IInfoWithId, IInfoWithName, IInfoWithGuid
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "nittinlocalization.localizationkeyitem";


        /// <summary>
        /// Type information.
        /// </summary>
        public static readonly ObjectTypeInfo TYPEINFO = new(typeof(IInfoProvider<LocalizationKeyInfo>), OBJECT_TYPE, "NittinLocalization.LocalizationKeyItem", nameof(LocalizationKeyItemId), null, nameof(LocalizationKeyItemGuid), nameof(LocalizationKeyItemName), nameof(LocalizationKeyItemName), null, null, null)
        {
            TouchCacheDependencies = true,
            ContinuousIntegrationSettings =
            {
                Enabled = true
            },
        };


        /// <summary>
        /// Localization key ID.
        /// </summary>
        [DatabaseField]
        public virtual int LocalizationKeyItemId
        {
            get => ValidationHelper.GetInteger(GetValue(nameof(LocalizationKeyItemId)), 0);
            set => SetValue(nameof(LocalizationKeyItemId), value);
        }


        /// <summary>
        /// Localization key guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid LocalizationKeyItemGuid
        {
            get => ValidationHelper.GetGuid(GetValue(nameof(LocalizationKeyItemGuid)), Guid.Empty);
            set => SetValue(nameof(LocalizationKeyItemGuid), value);
        }


        /// <summary>
        /// Localization key name.
        /// </summary>
        [DatabaseField]
        public virtual string LocalizationKeyItemName
        {
            get => ValidationHelper.GetString(GetValue(nameof(LocalizationKeyItemName)), String.Empty);
            set => SetValue(nameof(LocalizationKeyItemName), value);
        }


        /// <summary>
        /// Description.
        /// </summary>
        [DatabaseField]
        public virtual string LocalizationKeyItemDescription
        {
            get => ValidationHelper.GetString(GetValue(nameof(LocalizationKeyItemDescription)), String.Empty);
            set => SetValue(nameof(LocalizationKeyItemDescription), value, String.Empty);
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            Provider.Delete(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            Provider.Set(this);
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="LocalizationKeyInfo"/> class.
        /// </summary>
        public LocalizationKeyInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="LocalizationKeyInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public LocalizationKeyInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}
