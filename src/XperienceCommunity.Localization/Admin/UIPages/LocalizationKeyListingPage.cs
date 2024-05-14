using Kentico.Xperience.Admin.Base;

using XperienceCommunity.Localization.Admin.UIPages;

[assembly: UIPage(
    parentType: typeof(LocalizationApplication),
    slug: "localization-keys",
    uiPageType: typeof(LocalizationKeyListingPage),
    name: "Localization keys",
    templateName: TemplateNames.LISTING,
    order: UIPageOrder.NoOrder)]

namespace XperienceCommunity.Localization.Admin.UIPages;

public class LocalizationKeyListingPage : ListingPage
{
    protected override string ObjectType => LocalizationKeyInfo.OBJECT_TYPE;

    public override Task ConfigurePage()
    {
        PageConfiguration.ColumnConfigurations
            .AddColumn(nameof(LocalizationKeyInfo.LocalizationKeyItemId), "ID")
        .AddColumn(nameof(LocalizationKeyInfo.LocalizationKeyItemName), "Name");

        PageConfiguration.HeaderActions.AddLink<LocalizationKeyCreatePage>("Create");
        PageConfiguration.AddEditRowAction<LocalizationKeyEditPage>();
        PageConfiguration.TableActions.AddDeleteAction("Delete");

        return base.ConfigurePage();
    }

    [PageCommand]
    public override Task<ICommandResponse<RowActionResult>> Delete(int id) => base.Delete(id);
}

