using CMS.ContentEngine;
using Kentico.Xperience.Admin.Base;
using Nittin.Xperience.Localization.Admin.UIPages;

[assembly: UIPage(typeof(LocalizationApplication), "localization-translations", typeof(LocalizationTranslationListingPage), "Translations",
    TemplateNames.LISTING, 20)]

namespace Nittin.Xperience.Localization.Admin.UIPages;

public class LocalizationTranslationListingPage : ListingPage
{
    public LocalizationTranslationListingPage()
    {
    }

    protected override string ObjectType => LocalizationTranslationInfo.OBJECT_TYPE;

    public override Task ConfigurePage()
    {
        PageConfiguration.ColumnConfigurations
            .AddColumn(nameof(LocalizationTranslationInfo.LocalizationTranslationID), "ID", defaultSortDirection: SortTypeEnum.Asc, sortable: true)
            //.AddColumn(nameof(LocalizationTranslationInfo.LocalizationKey), "Localization Key id", sortable: true)
            .AddColumn(nameof(LocalizationKeyInfo.LocalizationKeyName), "LocalizationKey Name", sortable: true)
            //.AddColumn(nameof(LocalizationTranslationInfo.Language), "Language id", sortable: true)
            .AddColumn(nameof(ContentLanguageInfo.ContentLanguageDisplayName), "Language", sortable: true)
            .AddColumn(nameof(LocalizationTranslationInfo.TranslationText), "Translation", sortable: true);

        PageConfiguration.HeaderActions.AddLink<LocalizationTranslationCreatePage>("Create");
        PageConfiguration.AddEditRowAction<LocalizationTranslationEditPage>();
        PageConfiguration.TableActions.AddDeleteAction("Delete");

        PageConfiguration.QueryModifiers.Add(new QueryModifier((query, settings) =>
        {
            query.Source(s => s
                .LeftJoin<LocalizationKeyInfo>("Nittinlocalization_LocalizationTranslation.LocalizationKey", "NittinLocalization_LocalizationKey.LocalizationKeyID")
                .LeftJoin<ContentLanguageInfo>("Nittinlocalization_LocalizationTranslation.Language", "CMS_ContentLanguage.ContentLanguageID")
            );
            return query;
        }));

        return base.ConfigurePage();
    }

    [PageCommand]
    public override Task<ICommandResponse<RowActionResult>> Delete(int id) => base.Delete(id);
}

