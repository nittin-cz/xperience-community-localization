using CMS.ContentEngine;

namespace XperienceCommunity.Localization.Admin;

public class LocalizationTranslationModel
{
    public int Id { get; set; }
    public string LanguageId { get; set; } = string.Empty;
    public string LanguageName { get; set; } = string.Empty;
    public string TranslationText { get; set; } = string.Empty;

    public LocalizationTranslationModel()
    {
    }

    public LocalizationTranslationModel(LocalizationTranslationItemInfo translationInfo, IEnumerable<ContentLanguageInfo> languages)
    {
        Id = translationInfo.LocalizationTranslationItemID;
        LanguageId = translationInfo.LocalizationTranslationItemContentLanguageId.ToString();
        LanguageName = languages.First(x => x.ContentLanguageID == translationInfo.LocalizationTranslationItemContentLanguageId).ContentLanguageDisplayName;
        TranslationText = translationInfo.LocalizationTranslationItemText;
    }

    public static LocalizationTranslationModel New(ContentLanguageInfo language)
        => new()
        {
            LanguageId = language.ContentLanguageID.ToString(),
            LanguageName = language.ContentLanguageDisplayName,
        };
}
