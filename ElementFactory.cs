using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ModularFramework.Attributes;
using ModularFramework.Exceptions;

namespace ModularFramework
{
    public class ElementFactory<TElement, TAttribute> : IElementFactory
        where TAttribute : ElementAttribute
    {
        public ElementFactory(ModuleDefinitionAttribute info) { ModuleInfo = info; }

        public ModuleDefinitionAttribute ModuleInfo { get; }
        public Type ElementInterfaceType => typeof(TElement);
        public Type AttributeType => typeof(TAttribute);

        public void LoadFile(string fileName, ErrorCallback errorCallback)
        {
            LoadTypes(AssemblyLoader.GetTypes(fileName, errorCallback));
        }

        public void LoadAssembly(Assembly a, ErrorCallback errorCallback)
        {
            LoadTypes(AssemblyLoader.GetTypes(a, errorCallback));
        }

        public void LoadFolder(string folder, Func<string, bool> fileSelector, ErrorCallback errorCallback)
        {
            LoadTypes(AssemblyLoader.GetTypesFromFolder(folder, fileSelector, errorCallback));
        }

        private void LoadTypes(IEnumerable<Type> types)
        {
            var exp = from t in AssemblyLoader.FindByAttribute<Type, TAttribute>(types)
                      where typeof(TElement).IsAssignableFrom(t)
                      where !t.IsGenericTypeDefinition
                      select t;

            _elements.AddRange(exp);
        }

        private readonly List<Type> _elements = new List<Type>();

        private readonly Dictionary<Type, TElement> _cache = new Dictionary<Type, TElement>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        /// <exception cref="ElementNotFoundException"></exception>
        /// <exception cref="ElementCreationException"></exception>
        public TInterface GetElement<TInterface>(Func<Type, bool> selector = null)
            where TInterface : class //TElement
        {
            var elemType = typeof(TInterface);
            if (!typeof(TElement).IsAssignableFrom(elemType)) throw new InvalidOperationException(elemType.Name + " does not belong to this layer!");
            var entries = _elements.Where(e => elemType.IsAssignableFrom(e) && (selector == null || selector(e)));
            var etype = entries.FirstOrDefault();
            if (etype == default(Type)) throw new ElementNotFoundException(ModuleInfo.ModuleName, elemType);

            if (_cache.ContainsKey(etype))
            {
                try { return _cache[etype] as TInterface; }
                catch (Exception ex)
                {
                    throw new ElementCreationException(ModuleInfo.ModuleName, etype, ex);
                }
            }

            try
            {
                var o = TryUtils.TryCreateInstance<TElement>(etype);
                _cache.Add(etype, o);
                return o as TInterface;
            }
            catch (Exception ex)
            {
                throw new ElementCreationException(ModuleInfo.ModuleName, etype, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorCallback"></param>
        /// <returns></returns>
        /// <exception cref="ElementNotFoundException"></exception>
        /// <exception cref="ElementCreationException"></exception>
        public IEnumerable<TInterface> GetElements<TInterface>(ErrorCallback errorCallback)
            where TInterface : class //TElement
        {
            return GetElements<TInterface>(null, errorCallback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="errorCallback"></param>
        /// <returns></returns>
        /// <exception cref="ElementNotFoundException"></exception>
        /// <exception cref="ElementCreationException"></exception>
        public IEnumerable<TInterface> GetElements<TInterface>(Func<Type, bool> selector = null, ErrorCallback errorCallback = null)
            where TInterface : class //TElement
        {
            var elemType = typeof(TInterface);
            if (!typeof(TElement).IsAssignableFrom(elemType)) throw new InvalidOperationException(elemType.Name + " does not belong to this layer!");
            var entries = _elements.Where(e => elemType.IsAssignableFrom(e)
                                                 && (selector == null || selector(e))).ToList();
            if (entries.Count < 1) throw new ElementNotFoundException(ModuleInfo.ModuleName, elemType);
            foreach (var etype in entries)
            {
                if (_cache.ContainsKey(etype))
                {
                    TInterface t;
                    try { t = _cache[etype] as TInterface; }
                    catch (Exception ex)
                    {
                        var x = new ElementCreationException(ModuleInfo.ModuleName, etype, ex);
                        if (errorCallback == null) throw x;
                        errorCallback(x);
                        continue;
                    }
                    yield return t;
                    continue;
                }

                TElement o;
                try
                {
                    o = TryUtils.TryCreateInstance<TElement>(etype);
                    _cache.Add(etype, o);
                }
                catch (Exception ex)
                {
                    var x = new ElementCreationException(ModuleInfo.ModuleName, etype, ex);
                    if (errorCallback == null) throw x;
                    errorCallback(x);
                    continue;
                }
                yield return o as TInterface;
            }
        }
    }
}
