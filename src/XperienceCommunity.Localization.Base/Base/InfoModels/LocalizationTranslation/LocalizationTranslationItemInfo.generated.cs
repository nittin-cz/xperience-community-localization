using System.Data;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

using CMS;
using CMS.ContentEngine;
using CMS.DataEngine;
using CMS.Helpers;

using XperienceCommunity.Localization;

[assembly: RegisterObjectType(typeof(LocalizationTranslationItemInfo), LocalizationTranslationItemInfo.OBJECT_TYPE)]

namespace XperienceCommunity.Localization
{
    /// <summary>
    /// Data container class for <see cref="LocalizationTranslationItemInfo"/>.
    /// </summary>
    [Serializable]
    public partial class LocalizationTranslationItemInfo : AbstractInfo<LocalizationTranslationItemInfo, IInfoProvider<LocalizationTranslationItemInfo>>, IInfoWithId, IInfoWithGuid
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "nittinlocalization.localizationtranslationitem";


        /// <summary>
        /// Type information.
        /// </summary>
        public static readonly ObjectTypeInfo TYPEINFO = new(typeof(IInfoProvider<LocalizationTranslationItemInfo>), OBJECT_TYPE, "NittinLocalization.LocalizationTranslationItem", nameof(LocalizationTranslationItemID), null, nameof(LocalizationTranslationItemGuid), null, null, null, null, null)
        {
            TouchCacheDependencies = true,
            DependsOn = new List<ObjectDependency>()
            {
                new(nameof(LocalizationTranslationItemID), LocalizationKeyInfo.OBJECT_TYPE, ObjectDependencyEnum.Required),
                new(nameof(LocalizationTranslationItemContentLanguageId), ContentLanguageInfo.OBJECT_TYPE, ObjectDependencyEnum.Required),
            },
            ContinuousIntegrationSettings =
            {
                Enabled = true
            },
        };



        /// <summary>
        /// Localization translation ID.
        /// </summary>
        [DatabaseField]
        public virtual int LocalizationTranslationItemID
        {
            get => ValidationHelper.GetInteger(GetValue(nameof(LocalizationTranslationItemID)), 0);
            set => SetValue(nameof(LocalizationTranslationItemID), value);
        }


        /// <summary>
        /// Localization translation guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid LocalizationTranslationItemGuid
        {
            get => ValidationHelper.GetGuid(GetValue(nameof(LocalizationTranslationItemGuid)), Guid.Empty);
            set => SetValue(nameof(LocalizationTranslationItemGuid), value);
        }


        /// <summary>
        /// Localization key.
        /// </summary>
        [DatabaseField]
        public virtual int LocalizationTranslationItemLocalizationKeyItemId
        {
            get => ValidationHelper.GetInteger(GetValue(nameof(LocalizationTranslationItemLocalizationKeyItemId)), 0);
            set => SetValue(nameof(LocalizationTranslationItemLocalizationKeyItemId), value);
        }


        /// <summary>
        /// Language.
        /// </summary>
        [DatabaseField]
        public virtual int LocalizationTranslationItemContentLanguageId
        {
            get => ValidationHelper.GetInteger(GetValue(nameof(LocalizationTranslationItemContentLanguageId)), 0);
            set => SetValue(nameof(LocalizationTranslationItemContentLanguageId), value);
        }


        /// <summary>
        /// Translation text.
        /// </summary>
        [DatabaseField]
        public virtual string LocalizationTranslationItemText
        {
            get => ValidationHelper.GetString(GetValue(nameof(LocalizationTranslationItemText)), String.Empty);
            set => SetValue(nameof(LocalizationTranslationItemText), value, String.Empty);
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
        /// Creates an empty instance of the <see cref="LocalizationTranslationItemInfo"/> class.
        /// </summary>
        public LocalizationTranslationItemInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="LocalizationTranslationItemInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public LocalizationTranslationItemInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}
