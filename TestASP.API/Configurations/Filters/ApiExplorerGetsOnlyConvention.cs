using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc;

namespace TestASP.Configurations.Filters
{
    public class ApiExplorerGetsOnlyConvention : IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            action.ApiExplorer.IsVisible = action.Attributes.OfType<HttpGetAttribute>().Any();
        }
    }
}
