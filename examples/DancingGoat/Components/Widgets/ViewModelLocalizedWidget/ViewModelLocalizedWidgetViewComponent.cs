using Microsoft.AspNetCore.Mvc;

using Kentico.PageBuilder.Web.Mvc;

using XperienceCommunity.Localization;

using DancingGoat.Widgets;

[assembly: RegisterWidget(
    identifier: ViewModelLocalizedWidgetViewComponent.IDENTIFIER,
    viewComponentType: typeof(ViewModelLocalizedWidgetViewComponent),
    name: "String Localized widget",
    IconClass = "icon-box")]

namespace DancingGoat.Widgets;

/// <summary>
/// Controller for localized widget.
/// </summary>
public class ViewModelLocalizedWidgetViewComponent : ViewComponent
{
    /// <summary>
    /// Widget identifier.
    /// </summary>
    public const string IDENTIFIER = "DancingGoat.LandingPage.ViewModelLocalizedWidget";

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
}
