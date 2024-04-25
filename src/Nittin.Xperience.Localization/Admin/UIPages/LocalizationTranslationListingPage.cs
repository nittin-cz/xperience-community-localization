using CMS.ContentEngine;

using Kentico.Xperience.Admin.Base;

using Nittin.Xperience.Localization.Admin.UIPages;

[assembly: UIPage(
    parentType: typeof(LocalizationApplication),
    slug: "localization-translations",
    uiPageType: typeof(LocalizationTranslationListingPage),
    name: "Translations",
    templateName: TemplateNames.LISTING,
    order: UIPageOrder.NoOrder)]

namespace Nittin.Xperience.Localization.Admin.UIPages;

public class LocalizationTranslationListingPage : ListingPage
{
    public LocalizationTranslationListingPage()
    {
    }

    protected override string ObjectType => LocalizationTranslationItemInfo.OBJECT_TYPE;

    public override Task ConfigurePage()
    {
        PageConfiguration.ColumnConfigurations
            .AddColumn(nameof(LocalizationTranslationItemInfo.LocalizationTranslationItemID), "ID", defaultSortDirection: SortTypeEnum.Asc, sortable: true)
            .AddColumn(nameof(LocalizationKeyInfo.LocalizationKeyItemName), "LocalizationKey Name", sortable: true)
            .AddColumn(nameof(ContentLanguageInfo.ContentLanguageDisplayName), "Language", sortable: true)
            .AddColumn(nameof(LocalizationTranslationItemInfo.LocalizationTranslationItemText), "Translation", sortable: true);

        PageConfiguration.HeaderActions.AddLink<LocalizationTranslationCreatePage>("Create");
        PageConfiguration.AddEditRowAction<LocalizationTranslationEditPage>();
        PageConfiguration.TableActions.AddDeleteAction("Delete");

        PageConfiguration.QueryModifiers.Add(new QueryModifier((query, settings) =>
        {
            query.Source(s => s
                .LeftJoin<LocalizationKeyInfo>(
                    $"{LocalizationTranslationItemInfo.OBJECT_TYPE.Replace('.', '_')}.{nameof(LocalizationTranslationItemInfo.LocalizationTranslationItemLocalizationKeyItemId)}",
                    nameof(LocalizationKeyInfo.LocalizationKeyItemId))
                .LeftJoin<ContentLanguageInfo>(
                    $"{LocalizationTranslationItemInfo.OBJECT_TYPE.Replace('.', '_')}.{nameof(LocalizationTranslationItemInfo.LocalizationTranslationItemContentLanguageId)}",
                    nameof(ContentLanguageInfo.ContentLanguageID))
            );
            return query;
        }));

        return base.ConfigurePage();
    }

    [PageCommand]
    public override Task<ICommandResponse<RowActionResult>> Delete(int id) => base.Delete(id);
}

