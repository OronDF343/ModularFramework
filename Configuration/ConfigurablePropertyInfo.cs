using System;
using System.Reflection;

namespace ModularFramework.Configuration
{
    public class ConfigurablePropertyInfo
    {
        public ConfigurablePropertyInfo(PropertyInfo pi, ConfigurablePropertyAttribute cpa)
        {
            Name = cpa.Name;
            PropertyInfo = pi;
            ElementType = pi.DeclaringType;
        }

        public bool IsDefined(object elem)
        {
            return ElementType.IsInstanceOfType(elem) && elem.GetType().GetProperty(PropertyInfo.Name) != null;
        }

        public string Name { get; }
        public PropertyInfo PropertyInfo { get; }
        public Type ElementType { get; }

        public override bool Equals(object obj)
        {
            var info = obj as ConfigurablePropertyInfo;
            if (info != null)
                return Name.Equals(info.Name) && ElementType == info.ElementType;
            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
