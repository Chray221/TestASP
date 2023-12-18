using System;
using Microsoft.AspNetCore.Components;

namespace TestASP.BlazorServer.Components
{
	public class BlazorComponentBase : ComponentBase
    {

        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object>? AdditionalAttributes { get; set; }

        public BlazorComponentBase()
		{
		}
	}
}

