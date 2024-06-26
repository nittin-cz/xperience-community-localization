﻿using XperienceCommunity.Localization;
using XperienceCommunity.Localization.Admin;

namespace Microsoft.Extensions.DependencyInjection;

public static class NittinLocalizationStartupExtensions
{
    /// <summary>
    /// Adds Localization services and custom module to application.
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <returns></returns>
    public static IServiceCollection AddXperienceCommunityLocalization(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddLocalizationServicesInternal();

        return serviceCollection;
    }


    private static IServiceCollection AddLocalizationServicesInternal(this IServiceCollection services) =>
        services
            .AddSingleton<LocalizationModuleInstaller>()
            .AddSingleton<ILocalizationService, LocalizationService>()
            .AddSingleton<IKenticoStringLocalizer, KenticoStringLocalizer>()
            .AddSingleton<IKenticoHtmlLocalizer, KenticoHtmlLocalizer>();
}
