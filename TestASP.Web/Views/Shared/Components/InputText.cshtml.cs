// using System.Linq.Expressions;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.RazorPages;

// namespace TestASP.Web.Views.Components;

// public class InputTextModel<TModel,TResult> : ViewComponent
// {
//     public Expression<Func<TModel, TResult>> Expression { get; set; }
//     public string Placeholder { get; set; }
//     public object LabelHtmlAttributes { get; set; }
//     public object InputHtmlAttributes { get; set; }
//     public InputTextModel()
//     {
//     }

//     public async Task<IViewComponentResult> InvokeAsync(
//         Expression<Func<TModel, TResult>> expression, 
//         string? placeholder,
//         object labelHtmlAttributes,
//         object inputHtmlAttributes)
//     {
//         return View(this);
//     }
// }

