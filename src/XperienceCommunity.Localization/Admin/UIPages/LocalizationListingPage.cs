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
    protected override string ObjectType => LocalizationKeyInfo.OBJECT_TYPE;

    public override Task ConfigurePage()
    {
        PageConfiguration.ColumnConfigurations
            .AddColumn(nameof(LocalizationKeyInfo.LocalizationKeyItemId), "ID")
            .AddColumn(nameof(LocalizationKeyInfo.LocalizationKeyItemName), "Name")
            .AddColumn(nameof(LocalizationKeyInfo.LocalizationKeyItemDescription), "Description");

        PageConfiguration.HeaderActions.AddLink<LocalizationCreatePage>("Create");
        PageConfiguration.AddEditRowAction<LocalizationEditPage>();
        PageConfiguration.TableActions.AddDeleteAction("Delete");

        return base.ConfigurePage();
    }

    [PageCommand]
    public override Task<ICommandResponse<RowActionResult>> Delete(int id) => base.Delete(id);
}
