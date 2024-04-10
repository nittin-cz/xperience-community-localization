using System;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using Nittin.Xperience.Localization;

[assembly: RegisterObjectType(typeof(LocalizationKeyInfo), LocalizationKeyInfo.OBJECT_TYPE)]

namespace Nittin.Xperience.Localization
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
        public const string OBJECT_TYPE = "nittinlocalization.localizationkey";

        /// <summary>
        /// Localization key ID.
        /// </summary>
        [DatabaseField]
        public virtual int LocalizationKeyId
        {
            get => ValidationHelper.GetInteger(GetValue(nameof(LocalizationKeyId)), 0);
            set => SetValue(nameof(LocalizationKeyId), value);
        }


        /// <summary>
        /// Localization key guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid LocalizationKeyGuid
        {
            get => ValidationHelper.GetGuid(GetValue(nameof(LocalizationKeyGuid)), Guid.Empty);
            set => SetValue(nameof(LocalizationKeyGuid), value);
        }


        /// <summary>
        /// Localization key name.
        /// </summary>
        [DatabaseField]
        public virtual string LocalizationKeyName
        {
            get => ValidationHelper.GetString(GetValue(nameof(LocalizationKeyName)), String.Empty);
            set => SetValue(nameof(LocalizationKeyName), value);
        }


        /// <summary>
        /// Description.
        /// </summary>
        [DatabaseField]
        public virtual string LocalizationDescription
        {
            get => ValidationHelper.GetString(GetValue(nameof(LocalizationDescription)), String.Empty);
            set => SetValue(nameof(LocalizationDescription), value, String.Empty);
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
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected LocalizationKeyInfo(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
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
