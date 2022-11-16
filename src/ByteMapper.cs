using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;

public static class ByteMapper
{

	public static int floatMult = 100;

    public static Dictionary<Type, ReadBytes> readers = new Dictionary<Type, ReadBytes>()
    {
        {typeof(short), ReadingMapFunctions.bytesToInt16 },
		{typeof(int), ReadingMapFunctions.bytesToInt32 },
        {typeof(float), ReadingMapFunctions.bytesToFloat32 },
        {typeof(char), ReadingMapFunctions.byteToChar },
        {typeof(string), ReadingMapFunctions.bytesToString },
		{typeof(bool), ReadingMapFunctions.bytesToBool }
    };

	public static Dictionary<Type, WriteBytes> writers = new Dictionary<Type, WriteBytes>()
	{
		{typeof(short), WritingMapFunctions.bytesFromShort },
		{typeof(int), WritingMapFunctions.bytesFromInt32 },
		{typeof(float), WritingMapFunctions.bytesFromFloat32 },
		{typeof(char), WritingMapFunctions.bytesFromChar },
        {typeof(string), WritingMapFunctions.bytesFromString },
		{typeof(bool), WritingMapFunctions.bytesFromBool}
    };

	public static Dictionary<Type, List<MappableProperty>> mappableProperties = new Dictionary<Type, List<MappableProperty>>();

	public static byte[] toBytes(object obj)
    {
		Type type = obj.GetType();

		if (writers.ContainsKey(type))
		{
			return writers[type](obj).ToArray();
		}

		List<byte> bytes = new List<byte>();

		if (type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>)))
		{
			IList tempList = (IList)obj;

			bytes.AddRange(toBytes(tempList.Count));

			Type tempType = type.GetGenericArguments()[0];

			for (int j = 0; j < tempList.Count; j++)
			{
				bytes.AddRange(toBytes(tempList[j]));
			}
        }else if (type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>)))
        {
			IDictionary tempDict = (IDictionary)obj;

			bytes.AddRange(toBytes(tempDict.Keys.Count));

			foreach(object key in tempDict.Keys)
            {
				bytes.AddRange(toBytes(key));
				bytes.AddRange(toBytes(tempDict[key]));
            }
        }
		else if (type.IsEnum)
        {
			return writers[typeof(int)](obj).ToArray();
        }
        else
        {
			checkProperties(type);

			List<MappableProperty> props = mappableProperties[type];

			for(int i = 0; i < props.Count; i++)
            {
				PropertyInfo prop = props[i].propertyInfo;
				Type propType = prop.PropertyType;

				object temp = prop.GetValue(obj);

				bytes.AddRange(toBytes(temp));
			}
		}		

		return bytes.ToArray();
    }

	public static T fromBytes<T>(byte[] bytes, ref int pointer)
    {
		Type type = typeof(T);

		if (readers.ContainsKey(type))
        {
			return (T) readers[type](bytes, ref pointer);
		}

		object obj = Activator.CreateInstance(type);

		if (type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>)))
		{
			int count = fromBytes<int>(bytes, ref pointer);

			IList tempList = (IList)obj;

			Type tempType = type.GetGenericArguments()[0];

			for (int j = 0; j < count; j++)
			{
				object result = abstractImplementFromBytes(tempType, bytes, ref pointer);
				tempList.Add(result);
			}
        }
		else if (type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>)))
		{
			IDictionary tempDict = (IDictionary)obj;

			Type keyType = type.GetGenericArguments()[0];
			Type valType = type.GetGenericArguments()[1];

			int count = fromBytes<int>(bytes, ref pointer);

			for(int i = 0; i < count; i++)
			{
				object key = abstractImplementFromBytes(keyType, bytes, ref pointer);
				object val = abstractImplementFromBytes(valType, bytes, ref pointer);

				tempDict[key] = val;
			}
		}
		else if (type.IsEnum)
        {
			return (T)readers[typeof(int)](bytes, ref pointer);
        }
		else
        {
			checkProperties(type);

			List<MappableProperty> props = mappableProperties[type];

			for (int i = 0; i < props.Count; i++)
			{
				PropertyInfo prop = props[i].propertyInfo;
				Type propType = prop.PropertyType;

				object result = abstractImplementFromBytes(propType, bytes, ref pointer);

				prop.SetValue(obj, result);
			}
		}

		return (T) obj;
	}

	private static object abstractImplementFromBytes(Type type, byte[] bytes, ref int pointer)
    {
		MethodInfo method = typeof(ByteMapper).GetMethod("fromBytes").MakeGenericMethod(new Type[] { type });

		int tempPointer = pointer;

		object[] args = new object[] { bytes, tempPointer};

		object temp = method.Invoke(null, args);

		pointer = (int) args[1];

		return temp;
	}


    public static void checkProperties(Type type)
    {
		if (type == null || mappableProperties.ContainsKey(type))
        {
			return;
        }

		List<MappableProperty> properties = new List<MappableProperty>();

		foreach (PropertyInfo props in type.GetProperties())
		{
			if (Attribute.IsDefined(props, typeof(ByteMapped)))
			{
				ByteMapped byteMapped = props.GetCustomAttribute(typeof(ByteMapped)) as ByteMapped;

				int order = byteMapped.order;

				MappableProperty temp = new MappableProperty(order, props);

				properties.Add(temp);
			}
		}

		properties.Sort();

		mappableProperties[type] = properties;

	}

}
