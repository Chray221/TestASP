using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;

namespace TestASP.Web.Services;

public class CustomHtmlGenerator: DefaultHtmlGenerator
{
    public const string BootStrapInvalidError = "is-invalid";
    public const string BootStrapValidError = "is-valid";
    public const string BootStrapFormWasValidated = "was-validated";

    public CustomHtmlGenerator(IAntiforgery antiforgery, IOptions<MvcViewOptions> optionsAccessor,
        IModelMetadataProvider metadataProvider, IUrlHelperFactory urlHelperFactory, HtmlEncoder htmlEncoder,
        ValidationHtmlAttributeProvider validationAttributeProvider) : 
        base(antiforgery, optionsAccessor, metadataProvider, urlHelperFactory, htmlEncoder, validationAttributeProvider)
    {
    }

    protected override TagBuilder GenerateInput(ViewContext viewContext, InputType inputType, ModelExplorer modelExplorer, string expression,
        object value, bool useViewData, bool isChecked, bool setId, bool isExplicitValue, string format,
        IDictionary<string, object> htmlAttributes)
    {
        var tagBuilder = base.GenerateInput(viewContext, inputType, modelExplorer, expression, value, useViewData, isChecked, setId, isExplicitValue, format, htmlAttributes);
        FixValidationCssClassNames(tagBuilder);

        return tagBuilder;
    }
    
    public override TagBuilder GenerateTextArea(ViewContext viewContext, ModelExplorer modelExplorer, string expression, int rows,
        int columns, object htmlAttributes)
    {
        var tagBuilder = base.GenerateTextArea(viewContext, modelExplorer, expression, rows, columns, htmlAttributes);
        FixValidationCssClassNames(tagBuilder);

        return tagBuilder;
    }
    public override TagBuilder GenerateCheckBox(ViewContext viewContext, ModelExplorer modelExplorer, string expression, bool? isChecked, object htmlAttributes)
    {
        var tagBuilder = base.GenerateCheckBox(viewContext, modelExplorer, expression, isChecked, htmlAttributes);
        FixValidationCssClassNames(tagBuilder);

        return tagBuilder;
    }

    public override TagBuilder GenerateRadioButton(ViewContext viewContext, ModelExplorer modelExplorer, string expression, object value, bool? isChecked, object htmlAttributes)
    {        
        var tagBuilder = base.GenerateRadioButton(viewContext, modelExplorer, expression, value, isChecked, htmlAttributes);
        FixValidationCssClassNames(tagBuilder);

        return tagBuilder;
    }

    public override TagBuilder GenerateForm(ViewContext viewContext, string actionName, string controllerName, object routeValues, string method, object htmlAttributes)
    {
        var tagBuilder = base.GenerateForm(viewContext, actionName, controllerName, routeValues, method, htmlAttributes);   
        tagBuilder.FormWasValidated();
        return tagBuilder;
    }

    protected override TagBuilder GenerateFormCore(ViewContext viewContext, string action, string method, object htmlAttributes)
    {
        var tagBuilder = base.GenerateFormCore(viewContext, action, method, htmlAttributes);
        if(viewContext.ModelState.ValidationState == ModelValidationState.Invalid)
        {
            tagBuilder.FormWasValidated();
        }
        return tagBuilder;
    }

    public override TagBuilder GeneratePageForm(ViewContext viewContext, string pageName, string pageHandler, object routeValues, string fragment, string method, object htmlAttributes)
    {
        var tagBuilder = base.GeneratePageForm(viewContext, pageName, pageHandler, routeValues, fragment, method, htmlAttributes);
        tagBuilder.FormWasValidated();
        
        return tagBuilder;
    }

    private void FixValidationCssClassNames(TagBuilder tagBuilder)
    {
        tagBuilder.ReplaceCssClass(HtmlHelper.ValidationInputCssClassName, BootStrapInvalidError);
        tagBuilder.ReplaceCssClass(HtmlHelper.ValidationInputValidCssClassName, BootStrapValidError);
        tagBuilder.SetRequired();
    }
}

public static class TagBuilderHelpers
{
    public static void ReplaceCssClass(this TagBuilder tagBuilder, string old, string val)
    {
        if (!tagBuilder.Attributes.TryGetValue("class", out string? str)) return;
        tagBuilder.Attributes["class"] = str.Replace(old, val);
    }

    public static void SetRequired(this TagBuilder tagBuilder)
    {
        if (!tagBuilder.Attributes.TryGetValue("class", out string str)) return;
        if(str.Contains(HtmlHelper.ValidationInputCssClassName) || 
           str.Contains("is-invalid") &&
           !tagBuilder.Attributes.ContainsKey("required"))
        {
            tagBuilder.Attributes.Add("required","");
        }
    }

    public static void FormWasValidated(this TagBuilder tagBuilder)
    {
        tagBuilder.Attributes.TryGetValue("class",out string? attClass);
        if(!(attClass ?? "").Contains(CustomHtmlGenerator.BootStrapFormWasValidated))
        {
            tagBuilder.Attributes["class"] = (attClass ?? "") + $" {CustomHtmlGenerator.BootStrapFormWasValidated}";
        }
    }
}