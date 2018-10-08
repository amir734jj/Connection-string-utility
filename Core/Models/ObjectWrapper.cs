using System;
using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;

namespace Core.Models
{
    public class ObjectWrapper<T> : IObjectWrapper<T>
    {
        /// <summary>
        /// Object type
        /// </summary>
        private readonly Type _type;

        private readonly List<IPropertyConverter> _converters;

        /// <summary>
        /// Original object properties
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Custom properties
        /// </summary>
        public Dictionary<string, string> CustomProperties { get; } = new Dictionary<string, string>();

        /// <inheritdoc />
        /// <summary>
        /// Primary constructor
        /// </summary>
        /// <param name="value"></param>
        private ObjectWrapper(T value) : this(value,
            new List<IPropertyConverter>())
        {
            
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value"></param>
        /// <param name="converters"></param>
        public ObjectWrapper(T value, List<IPropertyConverter> converters)
        {
            Value = value;
            _type = typeof(T);
            _converters = converters;
        }

        /// <summary>
        /// Sets the property
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IObjectWrapper<T> Set(string key, string value)
        {
            var property = _type.GetProperty(key);

            // If property is native
            if (property != null && property.GetSetMethod() != null)
            {
                // Try to convert the type from string to destination type
                var convertedValue = SafelyParse(property.PropertyType, value);
                
                // Call the setter
                property.SetValue(Value, convertedValue);
            }
            else
            {
                // Save as custom property
                CustomProperties[key] = value;
            }

            // Return for chaining
            return this;
        }

        /// <summary>
        /// Flattens the object into dictionary of string to string
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> Flatten()
        {
            return CustomProperties
                .Concat(_type.GetProperties()
                    .Select(x => new KeyValuePair<string, string>(x.Name, SafelyToString(x.PropertyType, x.GetValue(Value)))))
                .ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Safely parses string into a type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        private object SafelyParse(Type type, string str)
        {
            var converter = _converters.FirstOrDefault(x => x.CanConvert(type));

            return converter != null ? converter.Parse(str) : Convert.ChangeType(str, type);
        }
        
        /// <summary>
        /// Safely parses string into a type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string SafelyToString(Type type, object value)
        {
            var converter = _converters.FirstOrDefault(x => x.CanConvert(_type));

            return converter != null ? converter.ToString(value) : value.ToString();
        }
        
        /// <summary>
        /// Implicit cast value to object wrapper
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator ObjectWrapper<T>(T value)
        {
            return new ObjectWrapper<T>(value);
        }
        
        /// <summary>
        /// Implicit cast object wrapper to value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator T(ObjectWrapper<T> value)
        {
            return value.Value;
        }
    }
}