using Kentico.Xperience.Admin.Base.Filters;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace XperienceCommunity.Localization.Admin.Filters;

public sealed class LocalizationListingFilterModel
{
    [DropDownComponent(
        Label = "Translation status",
        Placeholder = "All",
        DataProviderType = typeof(TranslationStatusDropDownOptionsProvider),
        Order = 1)]
    [FilterCondition(
        BuilderType = typeof(TranslationStatusWhereConditionBuilder),
        ColumnName = nameof(LocalizationKeyInfo.LocalizationKeyItemId))]
    public string TranslationStatus { get; set; } = string.Empty;
}
