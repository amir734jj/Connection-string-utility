# Connection-string-utility

A simple utility that makes interactions with connection strings easier

Essentially an attempt to abstract out `Prop1=123;Prop2=456;Prop3=789` to an easier modifiable state and back to a connection string.

```csharp
// object or a class type
// if we use type instead then we can use lambda property info instead of string property name
var utility = ConnectionStringUtility<object>.New()
    // if we are using the typed utility, and if we encounter a property of type == Guid, then
    // library will use the provided concerter
    .AddTypeConverter<Guid>(PropertyConverterBuilderInstance.New<Guid>()
        .SetParse(x => Guid.Parse(x))
        .SetToString(x => x.ToString())
        .Build());

var result = utility.InstanceToString(
    utility.Parse("Prop1='123';Prop2='456';Prop3='789'",
          // sanitizers
          x => x.TrimStart('\''), x => x.TrimEnd('\''))
        .Set("Prop1", "XXX"),
    // toString formatter
    x => $"'{x}'"
);

// > Prop1='XXX';Prop2='456';Prop3='789'
```
