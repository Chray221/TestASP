//using System;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc.ModelBinding;

//namespace TestASP.API.Helpers.CustomBinders
//{
//    // then use in controller
//    // [ModelBinder(typeof(SigninCustomBinder))] User user
//    public class SigninCustomBinder : IModelBinder
//    {
//        public SigninCustomBinder()
//        {
//        }

//        public async Task BindModelAsync(ModelBindingContext bindingContext)
//        {
//            await Task.Delay(10);
//            // use to 
//            //bindingContext.HttpContext.Request[""]
//        }
//    }

//    public class SigninCustomBinderProvider : IModelBinderProvider
//    {
//        public IModelBinder GetBinder(ModelBinderProviderContext context)
//        {
//            return new SigninCustomBinder();
//        }
//    }
//}
