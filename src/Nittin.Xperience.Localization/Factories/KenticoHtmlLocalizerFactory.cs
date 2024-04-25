using Microsoft.AspNetCore.Mvc.Localization;

namespace Nittin.Xperience.Localization.Factories;

public class KenticoHtmlLocalizerFactory : IHtmlLocalizerFactory
{
    private readonly LocalizationService localizationService;

    public KenticoHtmlLocalizerFactory(LocalizationService localizationService) => this.localizationService = localizationService;

    public IHtmlLocalizer Create(Type resourceSource) => new KenticoHtmlLocalizer(localizationService);

    public IHtmlLocalizer Create(string baseName, string location) => new KenticoHtmlLocalizer(localizationService);
}
