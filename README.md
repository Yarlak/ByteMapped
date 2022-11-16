# ByteMapped

This is a framework meant to convert between a byte array and an instance of a primitive or custom class in as low overhead as possible. The byte array is simply an ordered set of bytes that directly convert to the fields of a class. The order of the array is based on the line number of each field attributed with [ByteMapped](). If the order of the fields on the class were to change any previously made byte arrays would be unreadable.

* [WritingMapFunctions]() contains functions that convert a an instance of a class to a byte array
* [ReadingMapFunctions]() contains functions that convert a byte array to an instance of a class
* [ByteMapped]() is the attribute needed on each field of a class that you would like converted to/from a byte array
* [ByteMapper]() contains the methods needed to convert to a byte array (**toBytes()**) and from a byte array (fromBytes<T>())

By default the [ByteMapper]() is capable of converting the following types to/from byte arrays:
* short
* int
* float
* char
* string
* bool
* Enum
* Lists
* Dictionaries

Both **toBytes()**

The byte arrays that 

