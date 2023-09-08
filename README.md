# ExtendedThreading

A super small library for providing strong typed Ids (as opposed to using primitives).

The benefit of this is simple: You don't run the risk of accidentally using the wrong type of id. (e.g. sending a UserId into a query for products)

This works through the use of an abstract base class (`ExtendedThreading<TExtendedThreading, TPrimitiveId>`) which is inherited to gain the id functionality.

This project is inspired by [Andrew Lock's StronglyTypedId](https://github.com/andrewlock/StronglyTypedId).
However I needed support for .Net 5 and thus this project was born. It has since evolved to .Net 7.

# Installation

I recommend using the NuGet package: [ExtendedThreading](https://www.nuget.org/packages/ExtendedThreading) however feel free to clone the source instead if that suits your needs better.

# Usage

Specify your class like this:

```
[TypeConverter(typeof(StrongTypedValueTypeConverter<UserId, Guid>))]
[ExtendedThreadingJsonConverter<UserId, Guid>]
public class UserId: ExtendedThreading<UserId, Guid>
{
	public UserId(Guid primitiveId) : base(primitiveId)
	{
	}
}
```

This specifies that the class `UserId` is in fact a `Guid` and can be used in place of a `Guid`.
And that's basically all there is to it, now you just use `UserId` in place of `Guid` where you're dealing with an User's Id.

You can omit the `JsonConverter` if you don't use json serialization as well as the `TypeConverter` if you're not using WebAPI or MVC.

Furthermore there are a couple of base classes available to you:
- `StrongTypedValue` for anything that's not an id, this supports `string` as a primitive value.
- `ExtendedThreading` for anything that IS an id, this only supports `struct` types as primitives (therefore no `strings`). 
  - Adds the static `Parse(string)` and `TryParse(string, out TExtendedThreading)` methods.
- `StrongTypedGuid` a further specialization of `ExtendedThreading`.
  - Adds the static `New()` method for instantiating new ids with random values as well as the static `Empty` property.

# Compatibility

## Dapper.DDD.Repository

This can work without any extensions, however it's a bit simpler to use via the package [ExtendedThreading.Dapper.DDD.Repository](https://www.nuget.org/packages/ExtendedThreading.Dapper.DDD.Repository/).

## Entity Framework

This is supported through the package [ExtendedThreading.EntityFrameworkCore](https://www.nuget.org/packages/ExtendedThreading.EntityFrameworkCore).

## WebAPI

This is supported through the use of the built-in `JsonConverter` and `TypeConverter`.

## MVC

This is supported through the use of the built-in `TypeConverter`.

## NewtonSoft.Json

This is supported through the package [ExtendedThreading.NewtonSoft](https://www.nuget.org/packages/ExtendedThreading.NewtonSoft).

## Swagger

This is supported through the package [ExtendedThreading.Swagger](https://www.nuget.org/packages/ExtendedThreading.Swagger).