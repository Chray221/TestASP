namespace TestASP.Web.Models;

public class ErrorViewModel
{

    public string? RequestId { get; set; }
    public string? Message { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public ErrorViewModel() {}

    public ErrorViewModel(string requestId, string message)
    {
        RequestId = requestId;
        Message = message;
    }
}
