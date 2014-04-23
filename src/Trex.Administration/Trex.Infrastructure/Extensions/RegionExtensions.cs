using System.Collections.Generic;
using Microsoft.Practices.Prism.Regions;

namespace Trex.Infrastructure.Extensions
{
    public static class RegionExtensions
    {
        public static void RemoveAll(this IRegion region)
        {
            var views = new List<object>();
            views.AddRange(region.Views);
            foreach (var view in views)
            {
                region.Remove(view);
            }
        }

        public static void AddAndActivateIfNotExists(this IRegion region, object view)
        {
            if (!region.Views.Contains(view))
            {
                region.Add(view);
                region.Activate(view);
            }
        }
    }
}