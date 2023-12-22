using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TestASP.Web.TagHelpers;

[HtmlTargetElement("input-text", Attributes = "asp-for", TagStructure = TagStructure.WithoutEndTag)]
public class InputTextTagHelper : InputTagHelper
{
    public InputTextTagHelper(IHtmlGenerator generator) : base(generator)
    {
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        base.Process(context, output);
        // output.AddClass("form-control",HtmlEncoder.Default);
        output.Attributes.Add("class","form-control");
    }
}

// [HtmlTargetElement("customer-info-Tag-Helper")]  
// public class MyCustomTagHelper : TagHelper  
// {  
//     public string customerName { get; set; } 
//   public override void Process(TagHelperContext context, TagHelperOutput output)
//     {
//     output.TagName = "CustomerSectionTagHelper";
//     output.TagMode = TagMode.StartTagAndEndTag;  
    
//     var sb = new StringBuilder();  
//     sb.AppendFormat("Customer Name: {0}", this.customerName);  
//     output.PreContent.SetHtmlContent(sb.ToString());
//     }
// }