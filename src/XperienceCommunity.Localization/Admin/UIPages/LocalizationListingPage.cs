using Kentico.Xperience.Admin.Base;

using XperienceCommunity.Localization.Admin.UIPages;

[assembly: UIPage(
    parentType: typeof(LocalizationApplicationPage),
    slug: "localizations",
    uiPageType: typeof(LocalizationListingPage),
    name: "Localizations",
    templateName: TemplateNames.LISTING,
    order: UIPageOrder.NoOrder)]

namespace XperienceCommunity.Localization.Admin.UIPages;

public class LocalizationListingPage : ListingPage
{
    private string? _searchTerm;

    protected override string ObjectType => LocalizationKeyInfo.OBJECT_TYPE;

    public override Task ConfigurePage()
    {
        PageConfiguration.ColumnConfigurations
            .AddColumn(nameof(LocalizationKeyInfo.LocalizationKeyItemId), "ID")
            .AddColumn(nameof(LocalizationKeyInfo.LocalizationKeyItemName), "Name", searchable: true)
            .AddColumn(nameof(LocalizationKeyInfo.LocalizationKeyItemDescription), "Description", searchable: true);

        PageConfiguration.QueryModifiers.AddModifier((query, settings) =>
        {
            if (string.IsNullOrEmpty(_searchTerm))
            {
                return query;
            }

            // LEFT JOIN translations and filter across all fields in a single SQL query.
            // DISTINCT avoids duplicate keys when multiple translations match.
            query
                .Source(s => s.LeftJoin(
                    "NittinLocalization_LocalizationTranslationItem",
                    "LocalizationKeyItemId",
                    "LocalizationTranslationItemLocalizationKeyItemId"))
                .Where(w => w
                    .WhereContains(nameof(LocalizationKeyInfo.LocalizationKeyItemName), _searchTerm)
                    .Or()
                    .WhereContains(nameof(LocalizationKeyInfo.LocalizationKeyItemDescription), _searchTerm)
                    .Or()
                    .WhereContains(nameof(LocalizationTranslationItemInfo.LocalizationTranslationItemText), _searchTerm))
                .Distinct();

            return query;
        });

        PageConfiguration.HeaderActions.AddLink<LocalizationCreatePage>("Create");
        PageConfiguration.AddEditRowAction<LocalizationEditPage>();
        PageConfiguration.TableActions.AddDeleteAction("Delete");

        return base.ConfigurePage();
    }

    protected override Task<LoadDataResult> LoadData(LoadDataSettings settings, CancellationToken cancellationToken)
    {
        _searchTerm = settings.SearchTerm;

        // Clear the search term to prevent the built-in searchable column filter from
        // AND-ing with our custom filter (which would exclude translation-only matches).
        if (!string.IsNullOrEmpty(_searchTerm))
        {
            settings = new LoadDataSettings(
                settings.PageSize,
                settings.SelectedPage,
                settings.SortBy,
                settings.SortType,
                string.Empty,
                settings.FilterWhereCondition
            );
        }

        return base.LoadData(settings, cancellationToken);
    }

    [PageCommand]
    public override Task<ICommandResponse<RowActionResult>> Delete(int id) => base.Delete(id);
}
