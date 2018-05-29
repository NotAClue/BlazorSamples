using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.RenderTree;
using PortalSample.Shared;

namespace PortalSample.Client.Pages
{
    public class GridView : BlazorComponent
    {
        private Dictionary<String, Type> Columns;
        private string ObjectType;

        #region dependancy injection
        [Inject]
        private HttpClient Http { get; set; }
        #endregion

        #region parameters
        [Parameter]
        private string cssClass { get; set; }

        [Parameter]
        private string RequestUri { get; set; };
        #endregion

        #region properties
        public IEnumerable<object> Items { get; set; }

        public string Title { get; set; } = "Hello World - Class Only";
        #endregion

        protected override void OnInit()
        {
            base.OnInit();

            if (Items.Any())
            {
            //    var properties = typeof(object).GetProperties();
            //    Columns = new Dictionary<string, Type>();

                //    foreach (PropertyInfo p in properties)
                //    {
                //        Columns.Add(p.Name, p.PropertyType);
                //    }
            }
        }

        protected override async Task OnInitAsync()
        {
            Items = await Http.GetJsonAsync<WeatherForecast[]>(RequestUri);
            ObjectType = Items.First().GetType().ToString();
        }


        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(1, "div");
            builder.AddAttribute(2, "class", "test");
            if(!String.IsNullOrEmpty(ObjectType))
                builder.AddContent(3, ObjectType);
            else
                builder.AddContent(3, Title);

            builder.CloseElement();
        }
    }
}