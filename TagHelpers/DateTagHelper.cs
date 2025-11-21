using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HMS.TagHelpers
{
    [HtmlTargetElement("date")]  // This helper works on <date> tags
    public class DateTagHelper : TagHelper
    {
        public string Format { get; set; } = "dd-MM-yyyy"; // Default format

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";  // Replace <date> with <span>
            string today = DateTime.Now.ToString(Format);
            output.Content.SetContent(today);
        }
    }
}
