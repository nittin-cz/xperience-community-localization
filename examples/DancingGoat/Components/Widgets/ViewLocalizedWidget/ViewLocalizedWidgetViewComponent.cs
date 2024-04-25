using Microsoft.AspNetCore.Mvc;

using Kentico.PageBuilder.Web.Mvc;

using DancingGoat.Widgets;

[assembly: RegisterWidget(
    identifier: ViewLocalizedWidgetViewComponent.IDENTIFIER,
    viewComponentType: typeof(ViewLocalizedWidgetViewComponent),
    name: "Html Localized widget",
    IconClass = "icon-box")]

namespace DancingGoat.Widgets;

/// <summary>
/// Controller for localized widget.
/// </summary>
public class ViewLocalizedWidgetViewComponent : ViewComponent
{
    /// <summary>
    /// Widget identifier.
    /// </summary>
    public const string IDENTIFIER = "DancingGoat.LandingPage.ViewLocalizedWidget";

    public IViewComponentResult Invoke()
        => View("~/Components/Widgets/ViewLocalizedWidget/_ViewLocalizedWidget.cshtml");
}
