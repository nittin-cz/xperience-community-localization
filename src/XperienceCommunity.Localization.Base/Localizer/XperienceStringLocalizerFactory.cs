using CMS.ContentEngine;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Websites.Routing;
using Microsoft.Extensions.Localization;

namespace XperienceCommunity.Localizer.Internal
{
    internal class XperienceStringLocalizerFactory(IStringLocalizerFactory baseStringLocalizerFactory,
        IProgressiveCache progressiveCache,
        IWebsiteChannelContext websiteChannelContext,
        IInfoProvider<ContentLanguageInfo> contentLanguageInfoProvider) : IStringLocalizerFactory
    {
        private readonly IStringLocalizerFactory baseStringLocalizerFactory = baseStringLocalizerFactory;
        private readonly IProgressiveCache progressiveCache = progressiveCache;
        private readonly IWebsiteChannelContext websiteChannelContext = websiteChannelContext;
        private readonly IInfoProvider<ContentLanguageInfo> contentLanguageInfoProvider = contentLanguageInfoProvider;

        public IStringLocalizer Create(Type resourceSource)
        {
            var baseLocalizer = baseStringLocalizerFactory.Create(resourceSource);
            return progressiveCache.Load(cs => new XperienceStringLocalizer(baseLocalizer, progressiveCache, websiteChannelContext, contentLanguageInfoProvider), new CacheSettings(1440, "GetStringLocalizer", baseLocalizer.GetType().FullName));
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            var baseLocalizer = baseStringLocalizerFactory.Create(baseName, location);
            return progressiveCache.Load(cs => new XperienceStringLocalizer(baseLocalizer, progressiveCache, websiteChannelContext, contentLanguageInfoProvider), new CacheSettings(1440, "GetStringLocalizer", baseLocalizer.GetType().FullName));
        }
    }
}
