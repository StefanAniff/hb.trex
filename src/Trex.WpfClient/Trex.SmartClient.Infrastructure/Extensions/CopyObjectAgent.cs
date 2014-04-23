using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.SmartClient.Infrastructure.Extensions
{
    public static class CopyObjectAgent
    {
        public static T Copy<T>(this T obj)
        {
            var newObject = Activator.CreateInstance<T>();
            var props = obj.GetType().GetProperties();

            foreach (var info in props)
            {
                if (info.GetSetMethod() != null)
                    newObject.GetType().GetProperty(info.Name).SetValue(newObject, info.GetValue(obj, null), null);
            }
            return newObject;

        }

        public static void CopyFromObject<T>(this T obj, T objectToCopy)
        {
            var props = objectToCopy.GetType().GetProperties();
            foreach (var info in props)
            {
                if (info.GetSetMethod() != null)
                    obj.GetType().GetProperty(info.Name).SetValue(obj, info.GetValue(objectToCopy, null), null);
            }
        }
    }

}
