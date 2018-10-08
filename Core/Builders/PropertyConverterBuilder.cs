using System;
using Core.Interfaces;
using Core.Models;
using static Core.Utilities.LambdaHelper;

namespace Core.Builders
{
    public class PropertyConverterBuilderInstance
    {
        /// <summary>
        /// Static instance
        /// </summary>
        public static PropertyConverterBuilder<TProperty> New<TProperty>() => new PropertyConverterBuilder<TProperty>();
    }
    
    public class PropertyConverterBuilder<T>
    {
        /// <summary>
        /// Can convert handler
        /// </summary>
        private Func<Type, bool> _canConvertHandler = _ => false;

        /// <summary>
        /// Parse handler
        /// </summary>
        private Func<string, object> _parseHandler = str => Convert.ChangeType(str, typeof(T));

        /// <summary>
        /// ToString handler
        /// </summary>
        private Func<object, string> _toStringHandler = @object => @object.ToString();
        
        /// <summary>
        /// Set the handler
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public PropertyConverterBuilder<T> SetCanConvert(Func<Type, bool> handler) => Run(() => _canConvertHandler = handler, this);
        
        /// <summary>
        /// Set the handler
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public PropertyConverterBuilder<T> SetParse(Func<string, object> handler) => Run(() => _parseHandler = handler, this);
        
        /// <summary>
        /// Set the handler
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public PropertyConverterBuilder<T> SetToString(Func<object, string> handler) => Run(() => _toStringHandler = handler, this);

        /// <summary>
        /// Builds the instance
        /// </summary>
        /// <returns></returns>
        public IPropertyConverter Build() => new PropertyConverter(_canConvertHandler, _parseHandler, _toStringHandler);
    }
}