using Microsoft.Extensions.Localization;

namespace Nittin.Xperience.Localization;

public class KenticoStringLocalizerFactory : IStringLocalizerFactory
{
    private readonly LocalizationService localizationService;

    public KenticoStringLocalizerFactory(LocalizationService localizationService) => this.localizationService = localizationService;

    public IStringLocalizer Create(Type resourceSource) => new KenticoStringLocalizer(localizationService);

    public IStringLocalizer Create(string baseName, string location) => new KenticoStringLocalizer(localizationService);
}
