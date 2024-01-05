using System.Text;
using System.Linq;
using System.Text.Encodings.Web;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TestASP.Web.Services;
using System.Reflection;
using System.ComponentModel;

namespace TestASP.Web.TagHelpers;

[HtmlTargetElement("input-text", Attributes = "asp-for", TagStructure = TagStructure.WithoutEndTag)]
[HtmlTargetElement("InputText", Attributes = "asp-for", TagStructure = TagStructure.WithoutEndTag)]
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

[HtmlTargetElement("BootstrapInput", TagStructure = TagStructure.Unspecified)]
[HtmlTargetElement("bootstrap-input", TagStructure = TagStructure.Unspecified)]
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
    const string @class = "form-control";
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var childContext = output.GetChildContentAsync().Result;
        var content = childContext.GetContent();


        var propName = For.Name;
        // var modelExProp = For.ModelExplorer.Container.Properties.Single(x => x.Metadata.PropertyName.Equals(propName));
        var modelExProp = For.ModelExplorer;
        var propValue = modelExProp.Model;
        var propEditFormatString = modelExProp.Metadata.EditFormatString;

        
        TagBuilder input = null;
        switch(Type.ToLower())
        {
            case "text":
                input = _generator.GenerateTextBox(ViewContext, For.ModelExplorer,
                            propName, For.ModelExplorer.Model, propEditFormatString, new { @class});
                break;
            case "password":
                input = GeneratePassword(propName, propValue);
                break;
            case "textarea":
                input = _generator.GenerateTextArea(ViewContext, For.ModelExplorer,
            propName, /*rows*/1, /*columns*/1 , new { @class});
                break;
            case "select":
                input = await GenerateSelect(context,output,propName,propValue);
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
        if(!string.IsNullOrEmpty(CssClass))
        {
            input.AddCssClass(CssClass);
        }
        AppendCommonAttributeFromContext(input,context);
        AppendCommonAttributeFromContext(validation,context);        

        input.AppendExistingHtmlAttribute("onchange",output);
        input.CopyAllAttributeFrom(output);
        
        output.TagName = null;
        if(IsShowLabel)
        {
            input.Attributes.TryGetValue("id",out string? inputId);
            var labelName = propName?.Split(".")?.Last() ?? propName;
            var label = _generator.GenerateLabel(ViewContext, For.ModelExplorer,
                                labelName, labelName, new { @class = "control-label", @for = inputId ?? propName});
            AppendCommonAttributeFromContext(label,context);
            output.Content.Append(label);
        }
        
        output.Content.Append(input);
        output.Content.Append(validation);
        // output.Content.SetHtmlContent("<input></input>");
        // base.Process(context, output);
        await base.ProcessAsync(context, output);
    }

    private TagBuilder GeneratePassword(string? propName, object? propValue)
    {
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

        return parent;
    }

    private async Task<TagBuilder> GenerateSelect(TagHelperContext context, TagHelperOutput output, string? propName, object? propValue)
    {
        bool isAllowMultple = context.AllAttributes.ContainsName("multiple");
        var selectOptions = new List<SelectListItem>();
        if(!output.Content.IsEmptyOrWhiteSpace)
        {
            string selectContent = "<select>"+ (await output.GetChildContentAsync()).GetContent()+"</select>";

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(selectContent);
            foreach(XmlNode node in xmlDocument.FirstChild.ChildNodes)
            {
                if(node.Name == "option")
                {
                    if(node.Attributes.GetNamedItem("value") is XmlNode valueAttributeNode && 
                    node.FirstChild is XmlNode optionName &&
                    optionName.NodeType == XmlNodeType.Text )
                    {
                        selectOptions.Add(new SelectListItem(optionName.Value, valueAttributeNode.Value));
                    }
                }
            }
        }
        else if(For.ModelExplorer.ModelType.IsEnum)
        {
            System.Type enumType = For.ModelExplorer.ModelType;
            System.Type enumUnderlyingType = System.Enum.GetUnderlyingType(enumType);
            System.Array enumValues = System.Enum.GetValues(enumType);

            for (int i=0; i < enumValues.Length; i++)
            {
                // Retrieve the value of the ith enum item.
                object? value = enumValues.GetValue(i);
                string? valueStr = value?.ToString();
                if(value?.GetType()
                        .GetCustomAttributes(typeof(DescriptionAttribute),false)
                        .FirstOrDefault() is DescriptionAttribute valueDescription)
                {
                    value = valueDescription.Description;
                }

                // Convert the value to its underlying type (int, byte, long, ...)
                object? underlyingValue = System.Convert.ChangeType(value, enumUnderlyingType);

                selectOptions.Add(new SelectListItem(valueStr, underlyingValue?.ToString()));
            }
        }
        
        return _generator.GenerateSelect(ViewContext, For.ModelExplorer, propValue?.ToString(),propName,
                selectOptions, isAllowMultple , new { @class});
    }

    private TagBuilder GenerateTextAre(TagHelperContext context, string? propName, object? propValue)
    {
        int row = 1;
        int col = 1;
        if(context.AllAttributes.TryGetAttribute("rows", out TagHelperAttribute rowAttr) &&
           int.TryParse(rowAttr.Value.ToString(), out int rowValue))
        {
            row = rowValue;
        }
        if(context.AllAttributes.TryGetAttribute("cols", out TagHelperAttribute colAttr) &&
           int.TryParse(colAttr.Value.ToString(), out int colValue))
        {
            col = colValue;
        }
        return _generator.GenerateTextArea(ViewContext, For.ModelExplorer,
            propName, row, col , new { @class});
    }
    // public override void Process(TagHelperContext context, TagHelperOutput output)
    // {
    //     base.ProcessAsync(context, output).Wait();
    //     base.Process(context, output);
    // }

    public void AppendCommonAttributeFromContext(TagBuilder tag, TagHelperContext context)
    {
        tag.AppendExistingHtmlAttribute("hidden",context);
        tag.AppendExistingHtmlAttribute("disabled",context);
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
    
    public static void Append(this IHtmlContentBuilder content, IHtmlContent tag)
    {
        using (var writer = new System.IO.StringWriter())
        {        
            tag.WriteTo(writer, HtmlEncoder.Default);
            content.AppendHtml(writer.ToString());            
        }
    }
    
    public static string GetStringValue(this IHtmlContent contextAttribute)
    {
        using (var writer = new System.IO.StringWriter())
        {        
            contextAttribute.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }

    public static void CopyExistingHtmlAttribute(this TagHelperOutput output, string attribute, TagHelperContext context)
    {
        if(context.AllAttributes.ContainsName(attribute))
        {
            output.CopyHtmlAttribute(attribute,context);
        }
    }
    
    public static void AppendExistingHtmlAttribute(this TagBuilder tag, string attribute, TagHelperContext context)
    {        
        if(context.AllAttributes.TryGetAttribute(attribute,out TagHelperAttribute contextAttribute))
        {            
            bool hasTagAttre = tag.Attributes.TryGetValue(attribute,out string? tagAttr);
            if(hasTagAttre)
            {
                tagAttr = contextAttribute.Value.ToString();
            }
            else 
            {
                tagAttr = tagAttr + " " + contextAttribute.Value.ToString();
            }
            tag.Attributes[attribute] = tagAttr;
        }
    }
    
    public static void AppendExistingHtmlAttribute(this TagBuilder tag, string attribute, TagHelperOutput context)
    {        
        if(context.Attributes.TryGetAttribute(attribute,out TagHelperAttribute contextAttribute))
        {            
            bool hasTagAttre = tag.Attributes.TryGetValue(attribute,out string? tagAttr);
            if(hasTagAttre)
            {
                tagAttr = contextAttribute.Value.ToString();
            }
            else 
            {
                tagAttr = tagAttr + " " + contextAttribute.Value.ToString();
            }
            tag.Attributes[attribute] = tagAttr;
        }
    }
    
    public static void CopyAllAttributeFrom(this TagBuilder tag, TagHelperOutput output)
    {        
        foreach(var attribute in output.Attributes)
        {
            tag.AppendExistingHtmlAttribute(attribute.Name,output);
        }
    }
    
    public static void CopyAllAttributeFrom(this TagHelperOutput output, TagHelperContext context)
    {        
        foreach(var attribute in context.AllAttributes)
        {
            output.CopyExistingHtmlAttribute(attribute.Name,context);
        }
    }
}