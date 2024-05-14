using CMS.ContentEngine;
using CMS.DataEngine;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace XperienceCommunity.Localization.Admin.Providers;

internal class LanguageOptionsProvider : IDropDownOptionsProvider
{
    private readonly IInfoProvider<ContentLanguageInfo> contentLanguageInfoProvider;

    public LanguageOptionsProvider(IInfoProvider<ContentLanguageInfo> contentLanguageInfoProvider) => this.contentLanguageInfoProvider = contentLanguageInfoProvider;

    public async Task<IEnumerable<DropDownOptionItem>> GetOptionItems() =>
        (await contentLanguageInfoProvider
            .Get()
            .GetEnumerableTypedResultAsync())
        .Select(x => new DropDownOptionItem()
        {
            Value = x.ContentLanguageID.ToString(),
            Text = x.ContentLanguageDisplayName
        });
}
