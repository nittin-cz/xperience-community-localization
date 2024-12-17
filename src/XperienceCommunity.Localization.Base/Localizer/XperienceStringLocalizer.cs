using CMS.Helpers;
using CMS.Websites.Routing;
using Microsoft.Extensions.Localization;
using System.Data;

namespace XperienceCommunity.Localizer.Internal
{
    /// <summary>
    /// Creates a new String Localizer.
    /// </summary>
    /// <param name="localizer">The <see cref="IStringLocalizer"/> to read strings from.</param>
    /// <param name="progressiveCache"></param>
    /// <param name="websiteChannelContext"></param>
    internal class XperienceStringLocalizer<T>(IStringLocalizer<T> localizer,
        IProgressiveCache progressiveCache,
        IWebsiteChannelContext websiteChannelContext) : XperienceStringLocalizerBase(progressiveCache, websiteChannelContext), IStringLocalizer<T>
    {
        private readonly IStringLocalizer<T> localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));

        public virtual IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var strings = new List<LocalizedString>();
            strings.AddRange(localizer.GetAllStrings(includeParentCultures));
            // add custom strings
            strings.AddRange(XperienceResourceStrings.Select(x => new LocalizedString(x.Key, x.Value, false)));
            return strings;
        }

        LocalizedString IStringLocalizer.this[string name]
        {
            get
            {
                if (name == null)
                {
                    throw new ArgumentNullException(nameof(name));
                }
                var result = localizer[name];
                if (result != null && result.ResourceNotFound)
                {
                    result = LocalizeWithKentico(name, result.SearchedLocation ?? "");
                }

                return result ?? new LocalizedString(name, string.Empty);
            }
        }

        LocalizedString IStringLocalizer.this[string name, params object[] arguments]
        {
            get
            {
                if (name == null)
                {
                    throw new ArgumentNullException(nameof(name));
                }
                var result = localizer[name];
                if (result != null && result.ResourceNotFound)
                {
                    result = LocalizeWithKentico(name, result.SearchedLocation ?? string.Empty, arguments);
                }

                return result ?? new LocalizedString(name, string.Empty);
            }
        }

    }

    /// <summary>
    /// Creates a new String Localizer
    /// </summary>
    /// <param name="localizer">The <see cref="IStringLocalizer"/> to read strings from.</param>
    /// <param name="progressiveCache"></param>
    /// <param name="websiteChannelContext"></param>
    public class XperienceStringLocalizer(IStringLocalizer localizer,
        IProgressiveCache progressiveCache,
        IWebsiteChannelContext websiteChannelContext) : XperienceStringLocalizerBase(progressiveCache, websiteChannelContext), IStringLocalizer
    {
        private readonly IStringLocalizer localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));


        /// <inheritdoc />
        public virtual IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var strings = new List<LocalizedString>();
            strings.AddRange(localizer.GetAllStrings(includeParentCultures));
            // add custom strings
            strings.AddRange(XperienceResourceStrings.Select(x => new LocalizedString(x.Key, x.Value, false)));
            return strings;
        }

        LocalizedString IStringLocalizer.this[string name]
        {
            get
            {
                if (name == null)
                {
                    throw new ArgumentNullException(nameof(name));
                }
                var result = localizer[name];
                if (result.ResourceNotFound)
                {
                    result = LocalizeWithKentico(name);
                }

                return result;
            }
        }

        LocalizedString IStringLocalizer.this[string name, params object[] arguments]
        {
            get
            {
                if (name == null)
                {
                    throw new ArgumentNullException(nameof(name));
                }
                var result = localizer[name];
                if (result.ResourceNotFound)
                {
                    result = LocalizeWithKentico(name, arguments);
                }

                return result;
            }
        }

    }
}
