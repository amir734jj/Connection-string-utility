using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface IObjectWrapper<out T>
    {
        T Value { get; }
        
        Dictionary<string, string> CustomProperties { get; }

        IObjectWrapper<T> Set(string key, string value);

        Dictionary<string, string> Flatten();
    }
}