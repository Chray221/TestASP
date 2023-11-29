using System;

namespace TestASP.Model
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public ErrorViewModel(string requestId)
        {
            RequestId = requestId;
        }
    }
}
