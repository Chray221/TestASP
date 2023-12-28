using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TestASP.Web.TagHelpers;

[HtmlTargetElement("input-text", Attributes = "asp-for", TagStructure = TagStructure.WithoutEndTag)]
// [HtmlTargetElement("InputText", Attributes = "asp-for", TagStructure = TagStructure.WithoutEndTag)]
public class InputTextTagHelper : InputTagHelper
{
    public InputTextTagHelper(IHtmlGenerator generator) : base(generator)
    {
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.Add("class","form-control");
        base.Process(context, output);
        // output.AddClass("form-control",HtmlEncoder.Default);
    }
}

[HtmlTargetElement("BootstrapInput", TagStructure = TagStructure.WithoutEndTag)]
[HtmlTargetElement("bootstrap-input", TagStructure = TagStructure.WithoutEndTag)]
public class BoostrapInputTagHelper: TagHelper
{
    [HtmlAttributeName("asp-for")]
    public ModelExpression For { get; set; }

    [HtmlAttributeName("show-label")]
    public bool IsShowLabel { get; set; } = true;

    [HtmlAttributeName("type")]
    public string Type { get; set; } = "text";

    [HtmlAttributeName("class")]
    public string CssClass { get; set; }

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; }

    private readonly IHtmlGenerator _generator;

    public BoostrapInputTagHelper(IHtmlGenerator htmlGenerator)
    {
        _generator = htmlGenerator;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = null;
        var propName = For.Name;
        var modelExProp = For.ModelExplorer.Container.Properties.Single(x => x.Metadata.PropertyName.Equals(propName));
        var propValue = modelExProp.Model;
        var propEditFormatString = modelExProp.Metadata.EditFormatString;

        var label = _generator.GenerateLabel(ViewContext, For.ModelExplorer,
            propName, propName, new { @class = "col-md-2 control-label", @type = "email" });
        
        TagBuilder input = null;
        string @class = "form-control";
        switch(Type.ToLower())
        {
            case "text":
                input = _generator.GenerateTextBox(ViewContext, For.ModelExplorer,
            propName, propValue, propEditFormatString, new { @class});
                break;
            case "password":
            /*
            <div class="input-group mb-3">
                <input asp-for="Password" class="form-control"/>
                <button type="button" class="input-group-text" id="basic-addon2"
                    onclick="togglePassword(this,'Password')">
                    <span class="fa-regular fa-eye"/>
                </button>                
            </div>
            */
                var parent = new TagBuilder("div");
                parent.AddCssClass("input-group");

                var password = _generator.GeneratePassword(ViewContext, For.ModelExplorer,
                                     propName, propValue, new { @class});
                                     
                password.Attributes.TryGetValue("id",out string? idVal);
                idVal = idVal ?? propName.Replace("[","_").Replace("]","_").Replace(".","_");
                string buttonStr = 
                @$"<button type=""button"" class=""input-group-text"" id=""basic-addon2""
                    onclick=""togglePassword(this,'{idVal}')"">
                    <span class=""fa-regular fa-eye""/>
                </button>";
                
                parent.InnerHtml.Append(password);
                parent.InnerHtml.AppendHtml(buttonStr);

                input = parent;
                
                // input = _generator.GeneratePassword(ViewContext, For.ModelExplorer,
                // propName, propValue, new { @class});
                break;
            case "textarea":
                input = _generator.GenerateTextArea(ViewContext, For.ModelExplorer,
            propName, /*rows*/1, /*columns*/1 , new { @class});
                break;
            // case "checkbox" when propValue is bool boolVal:
            //     input = _generator.GenerateCheckBox(ViewContext, For.ModelExplorer,
            // propName, boolVal, new { @class});
            //     break;
            // case "radio" when propValue is bool boolVal:
            //     input = _generator.GenerateRadioButton(ViewContext, For.ModelExplorer,
            // propName, boolVal, new { @class});
            //     break;
            default:
                input = _generator.GenerateTextBox(ViewContext, For.ModelExplorer,
            propName, propValue, propEditFormatString, new { @class});
                break;
        }

        var validation = _generator.GenerateValidationMessage(ViewContext, For.ModelExplorer, 
            propName, string.Empty, string.Empty, new { @class = "text-danger" });

        if(IsShowLabel)
        {
            // label.AddCssClass(CssClass);
            output.Content.Append(label);
        }
        if(!string.IsNullOrEmpty(CssClass))
        {
            input.AddCssClass(CssClass);
        }
        
        output.Content.Append(input);
        output.Content.Append(validation);
        // output.Content.SetHtmlContent("<input></input>");
        base.Process(context, output);
    }
}

public static class TagHelperExtension
{
    public static void AppendTo(this TagBuilder tag, TagHelperContent content)
    {
        using (var writer = new System.IO.StringWriter())
        {        
            tag.WriteTo(writer, HtmlEncoder.Default);
            content.Append(writer.ToString());
        }
    }
    
    public static void Append(this TagHelperContent content, TagBuilder tag )
    {
        using (var writer = new System.IO.StringWriter())
        {        
            tag.WriteTo(writer, HtmlEncoder.Default);
            content.AppendHtml(writer.ToString());
        }
    }
    
    public static void Append(this IHtmlContentBuilder content, TagBuilder tag )
    {
        using (var writer = new System.IO.StringWriter())
        {        
            tag.WriteTo(writer, HtmlEncoder.Default);
            content.AppendHtml(writer.ToString());            
        }
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