using System;
using System.Reflection;

namespace ModularFramework.Configuration
{
    public class ConfigurablePropertyInfo<TElement> : IConfigurablePropertyInfo
    {
        public ConfigurablePropertyInfo(PropertyInfo pi, ConfigurablePropertyAttribute cpa)
        {
            Name = cpa.Name;
            PropertyInfo = pi;
        }

        public string Name { get; }
        public PropertyInfo PropertyInfo { get; }
        public Type ElementType => typeof(TElement);

        public override bool Equals(object obj)
        {
            var info = obj as IConfigurablePropertyInfo;
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
