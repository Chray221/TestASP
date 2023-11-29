using System;
namespace TestASP.Model
{
    public class RestResponse<T>
    {
        public T Result { get; set; }
        public bool IsSuccess { get; private set; }
        public CustomMessage Error { get; set; }
        public RestResponse(T result)
        {
            Result = result;
            IsSuccess = true;
        }
        public RestResponse(string title, string content, string button = "Okay", string icon = null, IconType icon_type = 0)
        {
            Result = default;
            IsSuccess = false;
            Error = new CustomMessage( title, content, button, icon , icon_type);
        }

        public RestResponse(CustomMessage error)
        {
            Result = default;
            IsSuccess = false;
            Error = error;
        }
    }

    public class CustomMessage
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public IconType IconType { get; set; }
        public string Content { get; set; }
        public string Button { get; set; }
        public CustomMessage(string title, string content, string button = "Okay", string icon = null, IconType icon_type = 0)
        {
            Title = title;
            Content = content;
            Button = button;
            Icon = icon;
            IconType = icon_type;
        }
    }

    public enum IconType
    {
        SVG,
        FontawesomeRegular,
        FontawesomeSolid,
        FontawesomeBrand,
        FontawesomeBrandSolid
    }
}
