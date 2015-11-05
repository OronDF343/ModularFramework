using System;
using System.ComponentModel;
using System.Reflection;

namespace ModularFramework.Configuration
{
    public interface IConfigurablePropertyInfo : INotifyPropertyChanged
    {
        string Name { get; }
        PropertyInfo PropertyInfo { get; }
        Type ElementType { get; }
        object BoundObject { get; set; }
        object Value { get; set; }
        void SetWaitForInstance(IElementFactory source);
    }
}
