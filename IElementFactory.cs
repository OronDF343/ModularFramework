using System;
using System.Collections.Generic;
using System.Reflection;
using ModularFramework.Attributes;

namespace ModularFramework
{
    public interface IElementFactory
    {
        ModuleDefinitionAttribute ModuleInfo { get; }
        Type ElementInterfaceType { get; }
        Type AttributeType { get; }
        void LoadFile(string fileName, ErrorCallback errorCallback);
        void LoadAssembly(Assembly a, ErrorCallback errorCallback);
        void LoadFolder(string folder, Func<string, bool> fileSelector, ErrorCallback errorCallback);
        TInterface GetElement<TInterface>(Func<Type, bool> selector = null)
            where TInterface : class;
        IEnumerable<TInterface> GetElements<TInterface>(ErrorCallback errorCallback)
            where TInterface : class;
        IEnumerable<TInterface> GetElements<TInterface>(Func<Type, bool> selector = null, ErrorCallback errorCallback = null)
            where TInterface : class;
    }
}
