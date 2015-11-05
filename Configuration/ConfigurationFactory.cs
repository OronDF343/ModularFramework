using System;
using System.Collections.Generic;
using System.Linq;

namespace ModularFramework.Configuration
{
    public class ConfigurationFactory
    {
        /// <summary>
        /// Gets all the configurable properties of the specified element type.
        /// </summary>
        /// <param name="element">The element type. Must be a class with the <see cref="ConfigurableElementAttribute">ConfigurableElementAttribute</see> attribute.</typeparam>
        /// <param name="errorCallback">An <see cref="ErrorCallback">ErrorCallback</see>.</param>
        /// <returns>All the configurable properties found on the element.</returns>
        IEnumerable<ConfigurablePropertyInfo> GetProperties(Type element, ErrorCallback errorCallback)
        {
            if (!Attribute.IsDefined(element, typeof(ConfigurableElementAttribute)))
                throw new InvalidOperationException("This element is not configurable!");
            return from p in element.GetProperties()
                   where Attribute.IsDefined(p, typeof(ConfigurablePropertyAttribute))
                   let a = p.GetAttribute<ConfigurablePropertyAttribute>()
                   let r = TryUtils.TryGetResult(() => new ConfigurablePropertyInfo(p, a), errorCallback)
                   where r != default(ConfigurablePropertyInfo)
                   select r;
        }
    }
}
