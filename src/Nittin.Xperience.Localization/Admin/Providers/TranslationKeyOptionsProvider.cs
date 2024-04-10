using CMS.DataEngine;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Nittin.Xperience.Localization.Admin.Providers;

internal class TranslationKeyOptionsProvider : IDropDownOptionsProvider
{
    private readonly IInfoProvider<LocalizationKeyInfo> localizationKeyInfoProvider;

    public TranslationKeyOptionsProvider(IInfoProvider<LocalizationKeyInfo> localizationKeyInfoProvider) => this.localizationKeyInfoProvider = localizationKeyInfoProvider;

    public async Task<IEnumerable<DropDownOptionItem>> GetOptionItems() =>
        (await localizationKeyInfoProvider
            .Get()
            .GetEnumerableTypedResultAsync())
        .Select(x => new DropDownOptionItem()
        {
            Value = x.LocalizationKeyName,
            Text = x.LocalizationKeyName
        });
}
