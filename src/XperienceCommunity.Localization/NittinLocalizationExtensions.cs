using XperienceCommunity.Localization;
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
        serviceCollection.AddLocalizationServicesInternal()
            .AddSingleton<IKenticoStringLocalizer, KenticoStringLocalizer>()
            .AddSingleton<IKenticoHtmlLocalizer, KenticoHtmlLocalizer>();

        return serviceCollection;
    }

    /// <summary>
    /// Adds Localization services and custom module to application.
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <typeparam name="TKenticoStringLocalizer">Overrides default <see cref="KenticoStringLocalizer"/> as the implementation of the <see cref="IKenticoStringLocalizer"/>.</typeparam>
    /// <typeparam name="TKenticoHtmlLocalizer">Overrides default <see cref="KenticoHtmlLocalizer"/> as the implementation of the <see cref="IKenticoHtmlLocalizer"/>.</typeparam>
    /// <returns></returns>
    public static IServiceCollection AddXperienceCommunityLocalization<TKenticoHtmlLocalizer, TKenticoStringLocalizer>(this IServiceCollection serviceCollection)
        where TKenticoHtmlLocalizer : KenticoHtmlLocalizer
        where TKenticoStringLocalizer : KenticoStringLocalizer
    {
        serviceCollection.AddLocalizationServicesInternal()
            .AddSingleton<IKenticoHtmlLocalizer, TKenticoHtmlLocalizer>()
            .AddSingleton<IKenticoStringLocalizer, TKenticoStringLocalizer>();

        return serviceCollection;
    }

    private static IServiceCollection AddLocalizationServicesInternal(this IServiceCollection services) =>
        services
            .AddSingleton<LocalizationModuleInstaller>()
            .AddSingleton<ILocalizationService, LocalizationService>();
}
