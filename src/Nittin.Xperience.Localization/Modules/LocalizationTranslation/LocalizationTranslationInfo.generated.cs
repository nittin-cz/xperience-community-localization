using System;
using System.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using Nittin.Xperience.Localization;

[assembly: RegisterObjectType(typeof(LocalizationTranslationInfo), LocalizationTranslationInfo.OBJECT_TYPE)]

namespace Nittin.Xperience.Localization
{
    /// <summary>
    /// Data container class for <see cref="LocalizationTranslationInfo"/>.
    /// </summary>
    [Serializable]
    public partial class LocalizationTranslationInfo : AbstractInfo<LocalizationTranslationInfo, IInfoProvider<LocalizationTranslationInfo>>, IInfoWithId, IInfoWithGuid
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "nittinlocalization.localizationtranslation";


        /// <summary>
        /// Localization translation ID.
        /// </summary>
        [DatabaseField]
        public virtual int LocalizationTranslationID
        {
            get => ValidationHelper.GetInteger(GetValue(nameof(LocalizationTranslationID)), 0);
            set => SetValue(nameof(LocalizationTranslationID), value);
        }


        /// <summary>
        /// Localization translation guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid LocalizationTranslationGuid
        {
            get => ValidationHelper.GetGuid(GetValue(nameof(LocalizationTranslationGuid)), Guid.Empty);
            set => SetValue(nameof(LocalizationTranslationGuid), value);
        }


        /// <summary>
        /// Localization key.
        /// </summary>
        [DatabaseField]
        public virtual int LocalizationKey
        {
            get => ValidationHelper.GetInteger(GetValue(nameof(LocalizationKey)), 0);
            set => SetValue(nameof(LocalizationKey), value);
        }


        /// <summary>
        /// Language.
        /// </summary>
        [DatabaseField]
        public virtual int Language
        {
            get => ValidationHelper.GetInteger(GetValue(nameof(Language)), 0);
            set => SetValue(nameof(Language), value);
        }


        /// <summary>
        /// Translation text.
        /// </summary>
        [DatabaseField]
        public virtual string TranslationText
        {
            get => ValidationHelper.GetString(GetValue(nameof(TranslationText)), String.Empty);
            set => SetValue(nameof(TranslationText), value, String.Empty);
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
        protected LocalizationTranslationInfo(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="LocalizationTranslationInfo"/> class.
        /// </summary>
        public LocalizationTranslationInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="LocalizationTranslationInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public LocalizationTranslationInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}
