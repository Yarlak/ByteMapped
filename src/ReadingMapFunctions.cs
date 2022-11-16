using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;

public static class ReadingMapFunctions
{

	public static object bytesToFloat32(byte[] bytes, ref int pointer)
	{
		float temp = (float)BitConverter.ToInt32(bytes, pointer);
		pointer += 4;

		return temp / ByteMapper.floatMult;
	}

	public static object bytesToBool(byte[] bytes, ref int poiner)
    {
		short num = (short)bytesToInt16(bytes, ref poiner);

		if (num == 0)
        {
			return false;
        }

		return true;
    }

	public static object bytesToInt16(byte[] bytes, ref int pointer)
	{
		short temp = BitConverter.ToInt16(bytes, pointer);
		pointer += 2;

		return temp;
	}

	public static object bytesToInt32(byte[] bytes, ref int pointer)
    {
		int temp = BitConverter.ToInt32(bytes, pointer);
		pointer += 4;

		return temp;
    }

	public static object byteToChar(byte[] bytes, ref int pointer)
    {
		char character = (char)bytes[pointer];
		pointer += 1;

		return character;
    }

	public static object bytesToString(byte[] bytes, ref int pointer)
    {
		StringBuilder builder = new StringBuilder();

		int length = (int) bytesToInt32(bytes, ref pointer);

		for(int i = 0; i < length; i++)
        {
			char temp = (char)byteToChar(bytes, ref pointer);
			builder.Append(temp);
        }

		return builder.ToString();
    }
}
