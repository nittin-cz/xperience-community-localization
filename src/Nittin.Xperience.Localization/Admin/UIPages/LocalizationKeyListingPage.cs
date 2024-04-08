using Kentico.Xperience.Admin.Base;
using Nittin.Xperience.Localization.Admin.UIPages;

[assembly: UIPage(typeof(LocalizationApplication), "localization-keys", typeof(LocalizationKeyListingPage), "Localization keys",
    TemplateNames.LISTING, 20)]

namespace Nittin.Xperience.Localization.Admin.UIPages;

public class LocalizationKeyListingPage : ListingPage
{
    protected override string ObjectType => LocalizationKeyInfo.OBJECT_TYPE;

    public override Task ConfigurePage()
    {
        PageConfiguration.ColumnConfigurations
            .AddColumn(nameof(LocalizationKeyInfo.LocalizationKeyID), "ID")
        .AddColumn(nameof(LocalizationKeyInfo.LocalizationKeyName), "Name");

        PageConfiguration.HeaderActions.AddLink<LocalizationKeyCreatePage>("Create");
        PageConfiguration.AddEditRowAction<LocalizationKeyEditPage>();
        PageConfiguration.TableActions.AddDeleteAction("Delete");

        return base.ConfigurePage();
    }

    [PageCommand]
    public override Task<ICommandResponse<RowActionResult>> Delete(int id) => base.Delete(id);
}

