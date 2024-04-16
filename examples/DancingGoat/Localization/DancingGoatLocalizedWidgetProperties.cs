using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace DancingGoat.Localization;

public class DancingGoatLocalizedWidgetProperties : IWidgetProperties
{
    [TextInputComponent(Label = "Title", Order = 1)]
    public string Title { get; set; } = "";

    [TextInputComponent(Label = "Content", Order = 2)]
    public string Content { get; set; } = "";
}
