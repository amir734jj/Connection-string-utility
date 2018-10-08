using System;
using Core.Interfaces;

namespace Core.Models
{
    public class PropertyConverter : IPropertyConverter
    {
        private readonly Func<Type, bool> _canConvert;
        
        private readonly Func<string, object> _parse;
        
        private readonly Func<object, string> _toString;

        public PropertyConverter(Func<Type, bool> canConvert, Func<string, object> parse, Func<object, string> toString)
        {
            _canConvert = canConvert;
            _parse = parse;
            _toString = toString;
        }
        
        /// <summary>
        /// Call forwarding
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool CanConvert(Type type) => _canConvert(type);

        /// <summary>
        /// Call forwarding
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public object Parse(string str) => _parse(str);

        /// <summary>
        /// Call forwarding
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public string ToString(object source) => _toString(source);
    }
}