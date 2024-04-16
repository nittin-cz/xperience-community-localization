using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

using Kentico.PageBuilder.Web.Mvc;

using DancingGoat.Localization;

[assembly: RegisterWidget(
    DancingGoatLocalizedWidgetViewComponent.IDENTIFIER,
    typeof(DancingGoatLocalizedWidgetViewComponent),
    "Localized widget",
    typeof(DancingGoatLocalizedWidgetProperties))
]

namespace DancingGoat.Localization;

/// <summary>
/// Controller for localized widget.
/// </summary>
public class DancingGoatLocalizedWidgetViewComponent : ViewComponent
{
    /// <summary>
    /// Widget identifier.
    /// </summary>
    public const string IDENTIFIER = "DancingGoat.Localization.LocalizedWidget";

    public ViewViewComponentResult Invoke(DancingGoatLocalizedWidgetProperties properties)
    {
        var model = new DancingGoatLocalizedWidgetViewModel();

        return View("~/Localization/_DancingGoatLocalizedWidget.cshtml", model);
    }
}
