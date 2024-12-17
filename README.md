# XperienceCommunity.Localization

[![CI: Build and Test](https://github.com/nittin-cz/xperience-community-localization/actions/workflows/ci.yml/badge.svg)](https://github.com/nittin-cz/xperience-community-localization/actions/workflows/ci.yml) Localization [![NuGet Package](https://img.shields.io/nuget/v/XperienceCommunity.Localization.svg)](https://www.nuget.org/packages/XperienceCommunity.Localization)

## Description

This project enables creating and using localizations and translations in Xperience by Kentico project.
Create translations in Xperience admin UI or programatically and use in your pages.

## Screenshots

![Administration localization edit form](/images/xperience-administration-edit-localization-key.png)
![Administration key listing page](/images/xperience-administration-key-listing.png)

## Library Version Matrix

| Xperience Version | Library Version |
| ----------------- | --------------- |
| >= 30.0.0         | >= 2.0.0        |
| >= 29.6.0         | >= 1.4.0        |
| >= 29.2.0         | >= 1.2.0        |
| >= 28.4.3         | 1.0.0           |


### Dependencies

- [ASP.NET Core 8.0](https://dotnet.microsoft.com/en-us/download)
- [Xperience by Kentico](https://docs.kentico.com/changelog)

## Package Installation

Add the package to your application using the .NET CLI

```powershell
dotnet add package XperienceCommunity.Localization
```

You can optionally also install the XperienceCommunity.Localization.Base to any non-admin projects if you need.

## Note on Version 2.0.0

In Version 2.0.0, we integrated our localization strings with the default Microsoft `IStringLocalizer` and `IHtmlLocalizer`.  This works in tandem with any .resx you may also leverage.

Becuase of this, we removed the `IKenticoHtmlLocalizer` and `IKenticoStringLocalizer` as they are no longer needed.  Please update any code needed and inject the standard `IStringLocalizer` and `IHtmlLocalizer`

This also means that you can use Localization keys in Attributes for any custom forms.

## Quick Start

1. Add this library to the application services.

   ```csharp
   // Program.cs
    builder.Services.AddXperienceCommunityLocalization();
   ```
2. Open the Localization application added by this library in the Xperience's Administration.
3. Press Create to add a localization.
4. Fill out the key and the description of the localized content.
5. Add translations for the desired content languages. ![Administration localization edit form](/images/xperience-administration-edit-localization-key.png)
6. Display the results on your site with a `ViewComponent`.

**Version 2.0.0 or above**
```csharp
    
    // ViewModelLocalizedWidgetViewComponent.cs
    private readonly IKenticoStringLocalizer localizer;

    public ViewModelLocalizedWidgetViewComponent(IStringLocalizer<OptionalResxClass> localizer)
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

**Version 1.4.0 or below**
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
**Version 2.0.0 or above**

 ```html

@inject IStringLocalizer<OptionalResxClass> stringLocalizer
@inject IHtmlLocalizer<OptionalResxClass> htmlLocalizer

<div>
    <h1>@stringLocalizer["Title"]</h1>
    @htmlLocalizer["Content"]
</div>

```

**Version 1.4.0 or below**
```html

@using XperienceCommunity.Localization

@inject IKenticoHtmlLocalizer localizer

<div>
    <h1>@localizer["Title"]</h1>
    <p>@localizer["Content"]</p>
</div>

```

![Administration html localizer example](/images/example-localization-html-localized-widget.png)

## Customization 2.0.0 or above
Version 2.0.0 integrates with the default IStringLocalizer and IHtmlLocalizer, you can optionally define a .resx file and class and use either the standard Resource File keys, or localizations created in the admin (admin takes priority).

If a key exists in the language of the current culture, the translation is returned.  If a key exists in a fallback or default culture, but not the language currently requested, it will fall back to the fallback/default language.

If the key doesn't exist in either the .resx nor the Localization application, it will return the key you entered.

Additionally, you can add these helpful extensions to have a fallback values:

```csharp
namespace Microsoft.Extensions.Localization
{
    public static class IStringLocalizerExtensions
    {
        /// <summary>
        /// Returns the GetString result, or the default value if not found or is empty
        /// </summary>
        /// <param name="stringLocalizer">The String Localizer</param>
        /// <param name="name">The Key Name</param>
        /// <param name="defaultValue">The Default Value</param>
        /// <returns>The value</returns>
        public static string GetStringOrDefault<T>(this IStringLocalizer<T> stringLocalizer, string name, string defaultValue)
        {
            var value = stringLocalizer.GetString(name);
            if(value == null || value.ResourceNotFound || string.IsNullOrWhiteSpace(value.Value) || value.Value.Equals(name))
            {
                return defaultValue;
            }
            return value.Value;
        }
    }
}

// Usage: @StringLocalizer.GetStringOrDefault("user.greeting", "Hello!");
```

```csharp

using Microsoft.AspNetCore.Html;

namespace Microsoft.AspNetCore.Mvc.Localization
{
    public static class IHtmlLocalizerExtensions
    {
        /// <summary>
        /// Returns the GetString result, or the default value if not found or is empty
        /// </summary>
        /// <param name="stringLocalizer">The String Localizer</param>
        /// <param name="name">The Key Name</param>
        /// <param name="defaultValue">The Default Value</param>
        /// <returns>The value</returns>
        public static HtmlString GetHtmlStringOrDefault<T>(this IHtmlLocalizer<T> htmlLocalizer, string name, HtmlString defaultValue)
        {
            var value = htmlLocalizer.GetHtml(name);
            if(value == null || value.IsResourceNotFound || string.IsNullOrWhiteSpace(value.Value) || value.Value.Equals(name))
            {
                return defaultValue;
            }
            return new HtmlString(value.Value);
        }
    }
}

// Usage: @HtmlLocalizer.GetHtmlStringOrDefault("user.greeting", new HtmlString("<p>Hello!</p>"));

```


## Customization 1.4.0 or below

Administration does not allow for storing empty string values as the translations. By default, if a specified key in a specified language does not exist the name of the key is returned

You can override this functionality by specifying your own implmentation of `IKenticoHtmlLocalizer` and `IKenticoStringLocalizer`. Default implementations are the `KenticoHtmlLocalizer` and the `KenticoStringLocalizer`.

This can be useful if you want to display a value in one language and display nothing in a different language.
To achieve this, you can inherit the `KenticoHtmlLocalizer` or the `KenticoStringLocalizer`.

```csharp
public class ExampleHtmlLocalizer : KenticoHtmlLocalizer
{
    public override string? GetStringByName(string name)
    {
        string culture = CultureInfo.CurrentCulture.ToString();

        // return empty string instead of null.
        return localizationService.GetValueByNameAndCulture(name, culture) ?? string.Empty;
    }
}
```

Similarly implement the `IKenticoStringLocalizer`, or use the default `KenticoStringLocalizer`

```csharp
// In your Program.cs
// ... Other registrations
builder.Services.AddXperienceCommunityLocalization<ExampleHtmlLocalizer, KenticoStringLocalizer>();
```

## Contributing

Instructions and technical details for contributing to **this** project can be found in [Contributing Setup](./docs/Contributing-Setup.md).

## License

Distributed under the MIT License. See [`LICENSE.md`](./LICENSE.md) for more information.

## Support

This project has **Limited support**.
