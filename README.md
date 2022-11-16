# ByteMapped

This is a framework meant to convert between a byte array and an instance of a primitive or custom class in as low overhead as possible. The byte array is simply an ordered set of bytes that directly convert to the fields of a class. The order of the array is based on the line number of each field attributed with [ByteMapped](https://github.com/Yarlak/ByteMapped/blob/main/src/ByteMapped.cs). If the order of the fields on the class were to change any previously made byte arrays would be unreadable.

* [WritingMapFunctions](https://github.com/Yarlak/ByteMapped/blob/main/src/WritingMapFunctions.cs) contains functions that convert a an instance of a class to a byte array
* [ReadingMapFunctions](https://github.com/Yarlak/ByteMapped/blob/main/src/ReadingMapFunctions.cs) contains functions that convert a byte array to an instance of a class
* [ByteMapped](https://github.com/Yarlak/ByteMapped/blob/main/src/ByteMapped.cs) is the attribute needed on each field of a class that you would like converted to/from a byte array
* [ByteMapper](https://github.com/Yarlak/ByteMapped/blob/main/src/ByteMapper.cs) contains the methods needed to convert to a byte array (**toBytes()**) and from a byte array (**fromBytes<T>()**)

By default the [ByteMapper](https://github.com/Yarlak/ByteMapped/blob/main/src/ByteMapper.cs) is capable of converting the following types to/from byte arrays:
* short
* int
* float
* char
* string
* bool
* Enum
* Lists
* Dictionaries

Both **toBytes()** and **fromBytes<T>()** are recursive - so if you are converting an instance of a custom class it will continue to recurse until it comes upon instances of the previously mentioned classes above. This means you can convert a custom class to a byte array with no custom serialization code.

# How to Use

Decorate the fields of your custom class that you want included in your byte array with the [ByteMapped](https://github.com/Yarlak/ByteMapped/blob/main/src/ByteMapped.cs) attribute. See [MenuItem](https://github.com/Yarlak/ByteMapped/blob/main/demo/MenuItem.cs) and [Restaurant](https://github.com/Yarlak/ByteMapped/blob/main/demo/Restaurant.cs) for examples.

# Limitations

Null values are currently unsupported - if you have a field decorated with [ByteMapped](https://github.com/Yarlak/ByteMapped/blob/main/src/ByteMapped.cs) whose value is null - you will be unable to write to/from a byte array.