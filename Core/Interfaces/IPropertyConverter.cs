using System;

namespace Core.Interfaces
{
    public interface IPropertyConverter
    {
        bool CanConvert(Type type);
        
        object Parse(string str);
        
        string ToString(object source);
    }
}