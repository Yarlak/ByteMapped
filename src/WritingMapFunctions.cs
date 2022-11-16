using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;

public static class WritingMapFunctions
{

	public static List<byte> bytesFromBool(object obj)
    {
		bool temp = (bool)obj;

		short num = 0;

		if (temp)
        {
			num = 1;
        }

		return bytesFromShort(num);
    }



	public static List<byte> bytesFromFloat32(object obj)
	{
		float num = (float)obj;

		return BitConverter.GetBytes(Convert.ToInt32(num * ByteMapper.floatMult)).ToList();
	}

	public static List<byte> bytesFromShort(object obj)
	{
		short num = (short)obj;
		return BitConverter.GetBytes(Convert.ToInt16(num)).ToList();
	}

	public static List<byte> bytesFromInt32(object obj)
    {
		int num = (int)obj;
		return BitConverter.GetBytes(Convert.ToInt32(num)).ToList();
    }

	public static List<byte> bytesFromString(object obj)
    {
		string temp = (string)obj;

		List<byte> bytes = new List<byte>();

		bytes.AddRange(bytesFromInt32(temp.Length));

		foreach(char character in temp.ToCharArray())
        {
			bytes.AddRange(bytesFromChar(character));
        }

		return bytes;
    }

	public static List<byte> bytesFromChar(object obj)
    {
		char temp = (char)obj;

		return new List<byte>() { (byte)temp};
    }

}
