using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace LFeBlog.Infrastructure.Extensions
{
    public static class EnumerableExtensions
    {


        public static IEnumerable<ExpandoObject> ToDynamicIenumerable<TSourse>(this IEnumerable<TSourse> sourse,
            string fields = null)
        {
            if (sourse==null)
            {
                throw new ArgumentNullException(nameof(sourse));
            }
            
            var expandoObjectList=new List<ExpandoObject>();
            
            var propertyInfoList=new List<PropertyInfo>();


            if (string.IsNullOrWhiteSpace(fields))
            {
                var propertyInfos = typeof(TSourse).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                propertyInfoList.AddRange(propertyInfos);
            }
            else
            {
                var fieldsAfterSplit = fields.Split(',').ToList();

                foreach (var field in fieldsAfterSplit)
                {
                    var propertyName = field.Trim();
                    if (string.IsNullOrWhiteSpace(field))
                    {
                        continue;
                    }

                    var propertyInfo = typeof(TSourse).GetProperty(propertyName,
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo==null)
                    {
                        throw new Exception($"Property {propertyName} wasn't found on {typeof(TSourse)}");
                    }
                    
                    propertyInfoList.Add(propertyInfo);
                }
            }


            foreach (TSourse sourceObject in sourse)
            {
                var dataShapedObject=new ExpandoObject();

                foreach (var propertyInfo in propertyInfoList)
                {
                    var propertyValue = propertyInfo.GetValue(sourceObject);
                    
                    ((IDictionary<string,object>)dataShapedObject).Add(propertyInfo.Name,propertyValue);
                }
                
                expandoObjectList.Add(dataShapedObject);
                
            }


            return expandoObjectList;

        }
    }
}