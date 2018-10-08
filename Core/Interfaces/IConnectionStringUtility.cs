namespace Core.Interfaces
{
    public interface IConnectionStringUtility<T> where T: new()
    {
        IObjectWrapper<T> Parse(string str);

        string InstanceToString(IObjectWrapper<T> instance);

        IConnectionStringUtility<T> AddTypeConverter<TProperty>(IPropertyConverter converter)
            where TProperty : struct;
    }
}