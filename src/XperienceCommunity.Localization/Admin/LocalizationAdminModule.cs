using CMS;
using CMS.Base;
using CMS.Core;
using Kentico.Xperience.Admin.Base;
using Microsoft.Extensions.DependencyInjection;
using XperienceCommunity.Localization.Admin;

[assembly: RegisterModule(typeof(LocalizationAdminModule))]

namespace XperienceCommunity.Localization.Admin;

/// <summary>
/// Manages administration features and integration.
/// </summary>
internal class LocalizationAdminModule : AdminModule
{
    private LocalizationModuleInstaller installer = null!;

    public LocalizationAdminModule() : base(nameof(LocalizationAdminModule)) { }

    protected override void OnInit(ModuleInitParameters parameters)
    {
        base.OnInit(parameters);

        // RegisterClientModule("nittin", "xperience-localization");

        var services = parameters.Services;

        installer = services.GetRequiredService<LocalizationModuleInstaller>();

        ApplicationEvents.Initialized.Execute += InitializeModule;
    }

    private void InitializeModule(object? sender, EventArgs e) =>
        installer.Install();
}

