using CMS.ContentEngine;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Websites.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using XperienceCommunity.Localization;
using XperienceCommunity.Localizer.Internal;

namespace XperienceCommunity.Localizer
{
    public static class XperienceLocalizerExtension
    {
        /// <summary>
        /// Adds Kentico Localization to the base StringLocalizerFactory, thus adding fallback to all IStringLocalizer / IHtmlLocalizer generated
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddXperienceCommunityLocalization(this IServiceCollection services, Action<LocalizationOptions>? localizationOptions = null)
        {
            // NITTIN
            if (localizationOptions != null)
            {
                services.AddLocalization(localizationOptions);
            }
            else
            {
                services.AddLocalization();
            }
            services
                .AddSingleton<LocalizationModuleInstaller>();

            services.AddSingleton<IStringLocalizerFactory>((provider) =>
            {
                // Have to manually build the "base" implementation so we can use it as a fall back
                var options = provider.GetRequiredService<IOptions<LocalizationOptions>>();
                var logger = provider.GetRequiredService<ILoggerFactory>();
                var baseStringLocalizerFactory = new ResourceManagerStringLocalizerFactory(options, logger);
                var websiteChannelContext = provider.GetRequiredService<IWebsiteChannelContext>();
                var progressiveCache = provider.GetRequiredService<IProgressiveCache>();
                var contentLanguageInfoProvider = provider.GetRequiredService<IInfoProvider<ContentLanguageInfo>>();
                return new XperienceStringLocalizerFactory(baseStringLocalizerFactory, progressiveCache, websiteChannelContext, contentLanguageInfoProvider);
            });
            return services;
        }


    }
}
