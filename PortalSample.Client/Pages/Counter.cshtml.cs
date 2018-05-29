using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Components;

namespace PortalSample
{
    public class CounterComponent : BlazorComponent
    {
        public int CurrentCount { get; set; }

        protected override void OnInit()
        {
            CurrentCount = 0;
            base.OnInit();
        }

        public void IncrementCount()
        {
            CurrentCount++;
        }

        public void DecrementCount()
        {
            CurrentCount--;
        }
    }
}
