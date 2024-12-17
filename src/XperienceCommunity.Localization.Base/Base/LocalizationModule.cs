using CMS;
using CMS.Base;
using CMS.Core;
using CMS.DataEngine;
using Microsoft.Extensions.DependencyInjection;
using XperienceCommunity.Localization;


[assembly: RegisterModule(typeof(LocalizationModule))]

namespace XperienceCommunity.Localization;

/// <summary>
/// Manages administration features and integration.
/// </summary>
internal class LocalizationModule : Module
{
    private LocalizationModuleInstaller? installer = null!;

    public LocalizationModule() : base(nameof(LocalizationModule)) { }

    protected override void OnInit(ModuleInitParameters parameters)
    {
        base.OnInit(parameters);

        var services = parameters.Services;

        installer = services.GetService<LocalizationModuleInstaller>();

        ApplicationEvents.Initialized.Execute += InitializeModule;
    }

    private void InitializeModule(object? sender, EventArgs e) =>
        installer?.Install();
}

