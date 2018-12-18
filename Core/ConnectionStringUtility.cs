using System;
using System.Collections.Generic;
using System.Linq;
using Core.Extensions;
using Core.Interfaces;
using Core.Models;

namespace Core
{
    public class ConnectionStringUtility<T> : IConnectionStringUtility<T> where T : new()
    {
        private const char Delimiter = ';';

        private const char EqualityChar = '=';

        private readonly List<IPropertyConverter> _converters = new List<IPropertyConverter>();

        /// <summary>
        /// Static instance
        /// </summary>
        public static IConnectionStringUtility<T> New() => new ConnectionStringUtility<T>();

        /// <summary>
        /// Parses a connection string into an instance
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public IObjectWrapper<T> Parse(string str)
        {
            // Create instance
            var instance = new ObjectWrapper<T>(Activator.CreateInstance<T>(), _converters);

            // Get values splitted by delimiter
            var values = str.Split(Delimiter);

            // Separate by '='
            values = values.Select(x => x.Split(EqualityChar))
                .Select(x => new[] {x.FirstOrDefault(), x.LastOrDefault()})
                .Select(x => x.Where(y => y != null).ToList())
                // If values length is not even then something is wrong
                .Select(x => x.Count % 2 != 0 ? throw new Exception("ConnectionString is not valid") : x)
                .SelectMany(x => x)
                .ToArray();

            // Populate the resulting object
            Enumerable.Range(0, values.Length / 2)
                .Select(i => new KeyValuePair<string, string>(values[i * 2], values[i * 2 + 1]))
                .ForEach(x => instance.Set(x.Key, x.Value));

            // Return the instance
            return instance;
        }

        /// <summary>
        /// Converts the instance to connection string
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public string InstanceToString(IObjectWrapper<T> instance)
        {
            return string.Join(Delimiter, instance.Flatten().Select(x => $"{x.Key}={x.Value}"));
        }

        /// <summary>
        /// Adds a type converter
        /// </summary>
        /// <param name="converter"></param>
        /// <typeparam name="TProperty"></typeparam>
        /// <returns></returns>
        public IConnectionStringUtility<T> AddTypeConverter<TProperty>(IPropertyConverter converter)
            where TProperty : struct
        {
            _converters.Add(converter);

            return this;
        }
    }
}