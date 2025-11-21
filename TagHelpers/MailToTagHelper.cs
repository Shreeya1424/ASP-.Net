using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HMS.TagHelpers
{
    [HtmlTargetElement("mail")]
    public class MailToTagHelper : TagHelper
    {
        public string Address { get; set; }
        public string Text { get; set; } = "Contact Us";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a"; // must be <a>
            output.Attributes.SetAttribute("href", "mailto:" + Address);
            output.Content.SetContent(Text);
        }
    }
}
