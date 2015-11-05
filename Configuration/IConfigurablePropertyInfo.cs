using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ModularFramework.Configuration
{
    public interface IConfigurablePropertyInfo
    {
        string Name { get; }
        PropertyInfo PropertyInfo { get; }
        Type ElementType { get; }
    }
}
