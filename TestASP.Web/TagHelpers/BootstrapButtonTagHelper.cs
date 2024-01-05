﻿using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TestASP.Web.TagHelpers;

[HtmlTargetElement("BootstrapButton", TagStructure = TagStructure.NormalOrSelfClosing) ]
[HtmlTargetElement("bootstrap-button", TagStructure = TagStructure.NormalOrSelfClosing)]
public class BootstrapButtonTagHelper : TagHelper
{
    public BootstrapButtonTagHelper()
    {

    }
    
    [HtmlAttributeName("button-type")]
    public BoostrapButtonType ButtonType { get; set; } = BoostrapButtonType.Default;
    [HtmlAttributeName("size")]
    public BoostrapButtonSize ButtonSize { get; set; } = BoostrapButtonSize.Medium;
    [HtmlAttributeName("color")]
    public BoostrapColor ButtonColor { get; set; } = BoostrapColor.Primary;
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "button";
        output.AddClass("btn",HtmlEncoder.Default);
        string buttonType = ButtonType == BoostrapButtonType.Outline ? "-outline" : "";
        output.AddClass($"btn{buttonType}-{ButtonColor.ToString().ToLower()}",HtmlEncoder.Default);

        switch (ButtonSize)
        {
            case BoostrapButtonSize.Small:
                output.AddClass($"btn-sm",HtmlEncoder.Default);
                break;
            case BoostrapButtonSize.Medium:
                break;
            case BoostrapButtonSize.Large:            
                output.AddClass($"btn-lg",HtmlEncoder.Default);
                break;
        }
        if(!context.AllAttributes.ContainsName("type"))
        {
            output.Attributes.SetAttribute("type", "button");
        }
        // output.CopyExistingHtmlAttribute("type",context);
        // output.CopyExistingHtmlAttribute("context",context);
        // output.CopyExistingHtmlAttribute("onclick",context);    
        output.CopyAllAttributeFrom(context);
        base.Process(context, output);
    }
}

public enum BoostrapColor
{
    Primary,
    Secondary,
    Success,
    Danger,
    Warning,
    Info,
    Light,
    Dark
}

public enum BoostrapButtonType
{
    Default,
    Outline
}

public enum BoostrapButtonSize
{
    Small,
    Medium,
    Large
}
