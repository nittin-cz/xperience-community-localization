# XperienceCommunity.Localization

[![CI: Build and Test](https://github.com/nittin-cz/xperience-community-localization/actions/workflows/ci.yml/badge.svg)](https://github.com/nittin-cz/xperience-community-localization/actions/workflows/ci.yml) Localization [![NuGet Package](https://img.shields.io/nuget/v/XperienceCommunity.Localization.svg)](https://www.nuget.org/packages/XperienceCommunity.Localization)

## Description

This project enables creating and using localizations and translations in Xperience by Kentico project.
Create translations in Xperience admin UI or programatically and use in your pages.

## Screenshots

![Administration localization edit form](/images/xperience-administration-edit-localization-key.png)
![Administration translation edit form](/images/xperience-administration-edit-translation.png)
![Administration key listing page](/images/xperience-administration-key-listing.png)
![Administration translation listing page](/images/xperience-administration-translation-listing.png)

## Library Version Matrix

| Xperience Version | Library Version |
| ----------------- | --------------- |
| >= 28.4.3         | 1.0.0           |

### Dependencies

- [ASP.NET Core 6.0](https://dotnet.microsoft.com/en-us/download)
- [Xperience by Kentico](https://docs.kentico.com/changelog)

## Package Installation

Add the package to your application using the .NET CLI

```powershell
dotnet add package XperienceCommunity.Localization
```

## Quick Start

1. Add this library to the application services.

   ```csharp
   // Program.cs
    builder.Services.AddNittinLocalization();
   ```

2. Create a localization key in Xperience's Administration within the Localization application added by this library.
![Administration localization edit form](/images/xperience-administration-edit-localization-key.png)
3. Create a translation for this key in Xperience's Administration within the Localization application added by this library.
![Administration translation edit form](/images/xperience-administration-edit-translation.png)
1. Display the results on your site with a `ViewComponent`.

```csharp
    
    // ViewModelLocalizedWidgetViewComponent.cs
    private readonly IKenticoStringLocalizer localizer;

    public ViewModelLocalizedWidgetViewComponent(IKenticoStringLocalizer localizer)
        => this.localizer = localizer;

    public IViewComponentResult Invoke()
    {
        var model = new ViewModelLocalizedWidgetViewModel
        {
            Title = localizer["Title"],
            Content = localizer["Content"]
        };

        return View("~/Components/Widgets/ViewModelLocalizedWidget/_ViewModelLocalizedWidget.cshtml", model);
    }

```

![Administration string localizer example](/images/example-localization-string-localized-widget.png)

5. Or display the results on your site with a Razor View 👍
```html

@using XperienceCommunity.Localization

@inject IKenticoHtmlLocalizer localizer

<div>
    <h1>@localizer["Title"]</h1>
    <p>@localizer["Content"]</p>
</div>

```

![Administration html localizer example](/images/example-localization-html-localized-widget.png)

## Contributing

Instructions and technical details for contributing to **this** project can be found in [Contributing Setup](./docs/Contributing-Setup.md).

## License

Distributed under the MIT License. See [`LICENSE.md`](./LICENSE.md) for more information.

## Support

This project has **Limited support**.
