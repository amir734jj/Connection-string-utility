using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IObjectWrapper<T>
    {
        T Value { get; }
        
        Dictionary<string, string> CustomProperties { get; }

        IObjectWrapper<T> Delete(string key);

        IObjectWrapper<T> Delete(Expression<Func<T, object>> propSelector);
        
        IObjectWrapper<T> Set(string key, string value);

        IObjectWrapper<T> Set(Expression<Func<T, object>> propSelector, string value);

        Dictionary<string, string> Flatten();
    }
}