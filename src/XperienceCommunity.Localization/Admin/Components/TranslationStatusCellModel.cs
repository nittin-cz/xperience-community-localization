namespace XperienceCommunity.Localization.Admin.Components;

public sealed class TranslationStatusCellModel
{
    public string Status { get; set; } = "none";

    public IEnumerable<string> MissingLanguageNames { get; set; } = [];
}
