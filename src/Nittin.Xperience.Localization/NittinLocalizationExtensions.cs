using Nittin.Xperience.Localization;
using Nittin.Xperience.Localization.Admin;

namespace Microsoft.Extensions.DependencyInjection;

public static class NittinLocalizationStartupExtensions
{
    /// <summary>
    /// Adds Lucene services and custom module to application with customized options provided by the <see cref="ILuceneBuilder"/>
    /// in the <paramref name="configure" /> action.
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IServiceCollection AddNittinLocalization(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddLocalizationServicesInternal();

        //var builder = new LuceneBuilder(serviceCollection);

        //configure(builder);

        //if (builder.IncludeDefaultStrategy)
        //{
        //    serviceCollection.AddTransient<DefaultLuceneIndexingStrategy>();
        //    builder.RegisterStrategy<DefaultLuceneIndexingStrategy>("Default");
        //}

        return serviceCollection;
    }


    private static IServiceCollection AddLocalizationServicesInternal(this IServiceCollection services) =>
        services
            .AddSingleton<LocalizationModuleInstaller>()
            .AddSingleton<LocalizationService, LocalizationService>();
}
