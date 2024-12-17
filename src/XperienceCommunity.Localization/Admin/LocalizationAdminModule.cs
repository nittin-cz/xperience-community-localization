using CMS;
using CMS.Core;
using Kentico.Xperience.Admin.Base;
using XperienceCommunity.Localization.Admin;

[assembly: RegisterModule(typeof(LocalizationAdminModule))]

namespace XperienceCommunity.Localization.Admin;

/// <summary>
/// Manages administration features and integration.
/// </summary>
internal class LocalizationAdminModule : AdminModule
{
    public LocalizationAdminModule() : base(nameof(LocalizationAdminModule)) { }

    protected override void OnInit(ModuleInitParameters parameters)
    {
        base.OnInit(parameters);

        RegisterClientModule("nittin", "xperience-community-localization");
    }

}

