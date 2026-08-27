using CMS.ContentEngine;
using CMS.DataEngine;

using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace XperienceCommunity.Localization.Admin.Filters;

public sealed class TranslationStatusDropDownOptionsProvider(IInfoProvider<ContentLanguageInfo> contentLanguageInfoProvider) : IDropDownOptionsProvider
{
    public Task<IEnumerable<DropDownOptionItem>> GetOptionItems()
    {
        var languages = contentLanguageInfoProvider.Get()
            .GetEnumerableTypedResult()
            .OrderBy(language => language.ContentLanguageDisplayName)
            .ToList();

        var options = new List<DropDownOptionItem>();

        foreach (var language in languages)
        {
            options.Add(new DropDownOptionItem
            {
                Value = $"translated:{language.ContentLanguageID}",
                Text = $"Translated – {language.ContentLanguageDisplayName}"
            });
        }

        foreach (var language in languages)
        {
            options.Add(new DropDownOptionItem
            {
                Value = $"missing:{language.ContentLanguageID}",
                Text = $"Missing translation – {language.ContentLanguageDisplayName}"
            });
        }

        return Task.FromResult<IEnumerable<DropDownOptionItem>>(options);
    }
}
