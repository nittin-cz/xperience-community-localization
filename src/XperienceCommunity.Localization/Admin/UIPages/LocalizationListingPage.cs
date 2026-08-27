using CMS.ContentEngine;
using CMS.DataEngine;

using Kentico.Xperience.Admin.Base;

using XperienceCommunity.Localization.Admin.Components;
using XperienceCommunity.Localization.Admin.Filters;
using XperienceCommunity.Localization.Admin.UIPages;

using LoadDataSettings = Kentico.Xperience.Admin.Base.LoadDataSettings;

[assembly: UIPage(
    parentType: typeof(LocalizationApplicationPage),
    slug: "localizations",
    uiPageType: typeof(LocalizationListingPage),
    name: "Localizations",
    templateName: TemplateNames.LISTING,
    order: UIPageOrder.NoOrder)]

namespace XperienceCommunity.Localization.Admin.UIPages;

public class LocalizationListingPage(
    IInfoProvider<ContentLanguageInfo> contentLanguageInfoProvider,
    IInfoProvider<LocalizationTranslationItemInfo> localizationTranslationItemInfoProvider) : ListingPage
{
    private string? _searchTerm;
    private List<ContentLanguageInfo>? _languages;
    private Dictionary<int, HashSet<int>>? _translatedLanguageIdsByKeyId;

    protected override string ObjectType => LocalizationKeyInfo.OBJECT_TYPE;

    public override Task ConfigurePage()
    {
        PageConfiguration.ColumnConfigurations
            .AddColumn(nameof(LocalizationKeyInfo.LocalizationKeyItemId), "ID")
            .AddColumn(nameof(LocalizationKeyInfo.LocalizationKeyItemName), "Name", searchable: true)
            .AddColumn(nameof(LocalizationKeyInfo.LocalizationKeyItemDescription), "Description", searchable: true)
            .AddComponentColumn(
                "TranslationStatus",
                "@nittin/xperience-community-localization/TranslationStatus",
                "Translations",
                modelRetriever: (_, rowData) => GetTranslationStatus((int)rowData[nameof(LocalizationKeyInfo.LocalizationKeyItemId)]),
                loadedExternally: true,
                sortable: false);

        PageConfiguration.FilterConfiguration.FormModel = new LocalizationListingFilterModel();

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

        // Loaded once per request (not per row) and read from the "Translations" column's
        // modelRetriever, which cannot itself run async DB queries per row.
        _languages = contentLanguageInfoProvider.Get().GetEnumerableTypedResult().ToList();

        _translatedLanguageIdsByKeyId = localizationTranslationItemInfoProvider.Get()
            .WhereNotEmpty(nameof(LocalizationTranslationItemInfo.LocalizationTranslationItemText))
            .Columns(
                nameof(LocalizationTranslationItemInfo.LocalizationTranslationItemLocalizationKeyItemId),
                nameof(LocalizationTranslationItemInfo.LocalizationTranslationItemContentLanguageId))
            .GetEnumerableTypedResult()
            .GroupBy(translation => translation.LocalizationTranslationItemLocalizationKeyItemId)
            .ToDictionary(
                group => group.Key,
                group => group.Select(translation => translation.LocalizationTranslationItemContentLanguageId).ToHashSet());

        return base.LoadData(settings, cancellationToken);
    }

    private TranslationStatusCellModel GetTranslationStatus(int keyId)
    {
        var languages = _languages ?? [];
        var translatedLanguageIds = _translatedLanguageIdsByKeyId?.GetValueOrDefault(keyId) ?? [];

        var missingLanguageNames = languages
            .Where(language => !translatedLanguageIds.Contains(language.ContentLanguageID))
            .Select(language => language.ContentLanguageDisplayName)
            .ToList();

        string status;
        if (missingLanguageNames.Count == 0)
        {
            status = "complete";
        }
        else if (translatedLanguageIds.Count == 0)
        {
            status = "none";
        }
        else
        {
            status = "partial";
        }

        return new TranslationStatusCellModel
        {
            Status = status,
            MissingLanguageNames = missingLanguageNames
        };
    }

    [PageCommand]
    public override Task<ICommandResponse<RowActionResult>> Delete(int id) => base.Delete(id);
}
