using CMS.Helpers;
using CMS.Websites.Routing;
using Microsoft.Extensions.Localization;

namespace XperienceCommunity.Localizer.Internal
{
    internal class XperienceStringLocalizerFactory(IStringLocalizerFactory baseStringLocalizerFactory,
        IProgressiveCache progressiveCache,
        IWebsiteChannelContext websiteChannelContext) : IStringLocalizerFactory
    {
        private readonly IStringLocalizerFactory baseStringLocalizerFactory = baseStringLocalizerFactory;
        private readonly IProgressiveCache progressiveCache = progressiveCache;
        private readonly IWebsiteChannelContext websiteChannelContext = websiteChannelContext;

        public IStringLocalizer Create(Type resourceSource)
        {
            var baseLocalizer = baseStringLocalizerFactory.Create(resourceSource);
            return progressiveCache.Load(cs => new XperienceStringLocalizer(baseLocalizer, progressiveCache, websiteChannelContext), new CacheSettings(1440, "GetStringLocalizer", baseLocalizer.GetType().FullName));
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            var baseLocalizer = baseStringLocalizerFactory.Create(baseName, location);
            return progressiveCache.Load(cs => new XperienceStringLocalizer(baseLocalizer, progressiveCache, websiteChannelContext), new CacheSettings(1440, "GetStringLocalizer", baseLocalizer.GetType().FullName));
        }
    }
}
