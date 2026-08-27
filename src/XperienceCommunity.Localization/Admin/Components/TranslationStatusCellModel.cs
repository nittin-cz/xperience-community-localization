namespace XperienceCommunity.Localization.Admin.Components;

public sealed class TranslationStatusCellModel
{
    public string Status { get; set; } = "none";

    public IEnumerable<string> MissingLanguageNames { get; set; } = [];

    public IEnumerable<TranslationPreviewModel> Translations { get; set; } = [];
}

public sealed class TranslationPreviewModel
{
    public string LanguageName { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;
}
