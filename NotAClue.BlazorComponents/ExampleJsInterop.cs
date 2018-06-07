using System;
using Microsoft.AspNetCore.Blazor.Browser.Interop;

namespace NotAClue.BlazorComponents
{
    public class ExampleJsInterop
    {
        public static string Prompt(string message)
        {
            return RegisteredFunction.Invoke<string>(
                "NotAClue.BlazorComponents.ExampleJsInterop.Prompt",
                message);
        }
    }
}
