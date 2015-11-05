using System;

namespace ModularFramework.Configuration
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = true)]
    public class ConfigurableElementAttribute : Attribute
    {

    }
}
