using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using System.Text;

public class TargetData
{
    public bool boolValue;
    public byte byteValue;
    public short shortValue;
    public ushort ushortValue;
    public int intValue;
    public uint uintValue;
    public long longValue;
    public ulong ulongValue;
    public float floatValue;
    public double doubleValue;
    public string strValue;

    public InnerTargetData innerData = new InnerTargetData();
}

public class InnerTargetData
{
    public bool boolValue;
    public byte byteValue;
    public short shortValue;
    public ushort ushortValue;
    public int intValue;
    public uint uintValue;
    public long longValue;
    public ulong ulongValue;
    public float floatValue;
    public double doubleValue;
    public string strValue;
}

unsafe public static class DataSerialize
{
    private static byte FalseByte = (byte)0;
    private static byte TrueByte = (byte)1;

     public static void WriteBool(Stream stream,ref int position,bool value)
    {
        stream.Seek(position, SeekOrigin.Begin);
        stream.WriteByte(value ? TrueByte : FalseByte);
        position += 1;
    }

    public static bool ReadBool(byte[] bytes,ref int start)
    {
        bool result;
        fixed(byte* point = &bytes[start])
        {
            result = (*point) == FalseByte ? false : true;
        }
        ++start;
        return result;
    }

    public static void WriteByte(Stream stream,ref int position,byte value)
    {
        stream.Seek(position, SeekOrigin.Begin);
        stream.WriteByte(value);
        position += 1;
    }

    public static byte ReadByte(byte[] bytes,ref int start)
    {
        byte result;
        fixed(byte* point = &bytes[start])
        {
            result = *point;
        }
        ++start;
        return result;
    }

    public static void WriteShort(Stream stream, ref int position, short value)
    {
        stream.Seek(position, SeekOrigin.Begin);

        byte[] bytes = BitConverter.GetBytes(value);
        stream.Write(bytes, 0, bytes.Length);
        position += sizeof(short);
    }

    public static short ReadShort(byte[] bytes,ref int start)
    {
        short result;
        fixed(byte* point = &bytes[start])
        {
            result = *((short*)point);
        }
        start += sizeof(short);
        return result;
    }

    public static void WriteUShort(Stream stream, ref int position, ushort value)
    {
        stream.Seek(position, SeekOrigin.Begin);

        byte[] bytes = BitConverter.GetBytes(value);
        stream.Write(bytes, 0, bytes.Length);
        position += sizeof(ushort);
    }

    public static ushort ReadUShort(byte[] bytes, ref int start)
    {
        ushort result;
        fixed (byte* point = &bytes[start])
        {
            result = *((ushort*)point);
        }
        start += sizeof(ushort);
        return result;
    }

    public static void WriteInt(Stream stream, ref int position, int value)
    {
        stream.Seek(position, SeekOrigin.Begin);

        byte[] bytes = BitConverter.GetBytes(value);
        stream.Write(bytes, 0, bytes.Length);
        position += sizeof(int);
    }

    public static int ReadInt(byte[] bytes, ref int start)
    {
        int result;
        fixed (byte* point = &bytes[start])
        {
            result = *((int*)point);
        }
        start += sizeof(int);
        return result;
    }

    public static void WriteUInt(Stream stream, ref int position, uint value)
    {
        stream.Seek(position, SeekOrigin.Begin);

        byte[] bytes = BitConverter.GetBytes(value);
        stream.Write(bytes, 0, bytes.Length);
        position += sizeof(uint);
    }

    public static uint ReadUInt(byte[] bytes, ref int start)
    {
        uint result;
        fixed (byte* point = &bytes[start])
        {
            result = *((uint*)point);
        }
        start += sizeof(uint);
        return result;
    }

    public static void WriteLong(Stream stream, ref int position, long value)
    {
        stream.Seek(position, SeekOrigin.Begin);

        byte[] bytes = BitConverter.GetBytes(value);
        stream.Write(bytes, 0, bytes.Length);
        position += sizeof(long);
    }

    public static long ReadLong(byte[] bytes, ref int start)
    {
        long result;
        fixed (byte* point = &bytes[start])
        {
            result = *((long*)point);
        }
        start += sizeof(long);
        return result;
    }

    public static void WriteULong(Stream stream, ref int position, ulong value)
    {
        stream.Seek(position, SeekOrigin.Begin);

        byte[] bytes = BitConverter.GetBytes(value);
        stream.Write(bytes, 0, bytes.Length);
        position += sizeof(ulong);
    }

    public static ulong ReadULong(byte[] bytes, ref int start)
    {
        ulong result;
        fixed (byte* point = &bytes[start])
        {
            result = *((ulong*)point);
        }
        start += sizeof(ulong);
        return result;
    }

    public static void WriteFloat(Stream stream, ref int position, float value)
    {
        stream.Seek(position, SeekOrigin.Begin);

        byte[] bytes = BitConverter.GetBytes(value);
        stream.Write(bytes, 0, bytes.Length);
        position += sizeof(float);
    }

    public static float ReadFloat(byte[] bytes, ref int start)
    {
        float result;
        fixed (byte* point = &bytes[start])
        {
            result = *((float*)point);
        }
        start += sizeof(float);
        return result;
    }

    public static void WriteDouble(Stream stream, ref int position, double value)
    {
        stream.Seek(position, SeekOrigin.Begin);

        byte[] bytes = BitConverter.GetBytes(value);
        stream.Write(bytes, 0, bytes.Length);
        position += sizeof(double);
    }

    public static double ReadDouble(byte[] bytes, ref int start)
    {
        double result;
        fixed (byte* point = &bytes[start])
        {
            result = *((double*)point);
        }
        start += sizeof(double);
        return result;
    }

    public static void WriteString(Stream stream, ref int position, string value)
    {
        if(value == null)
        {
            value = "";
        }
        stream.Seek(position, SeekOrigin.Begin);
        int strPosition = (int)stream.Length;
        byte[] bytes = BitConverter.GetBytes(strPosition);
        stream.Write(bytes, 0, bytes.Length);

        stream.Seek(0, SeekOrigin.End);
        bytes = Encoding.UTF8.GetBytes(value);
        byte[] lenBytes = BitConverter.GetBytes(bytes.Length);

        stream.Write(lenBytes,0,lenBytes.Length);
        stream.Write(bytes, 0, bytes.Length);

        position += sizeof(int);
    }

    public static string ReadString(byte[] bytes, ref int start)
    {
        int strPosition;
        fixed (byte* point = &bytes[start])
        {
            strPosition = *((int*)point);
        }

        int strLen;
        fixed (byte* point = &bytes[strPosition])
        {
            strLen = *((int*)point);
        }
        byte[] strBytes = new byte[strLen];
        Array.Copy(bytes, strPosition + sizeof(int), strBytes, 0, strLen);

        string result = Encoding.UTF8.GetString(strBytes);

        start += sizeof(int);
        return result;
    }
}


//public static class TargetDataSerialize
//{
//    public static void Serialize(Stream stream, TargetData targetData)
//    {
//        stream.WriteByte(targetData.boolValue);
//    }

//    public static TargetData Deserialize(byte[] bytes,int startIndex)
//    {

//    }
//}

public static class InnerTargetDataSerialize
{
    public static void Serialize(Stream stream, InnerTargetData targetData)
    {
        int position = 0;
        Serialize(stream, targetData, ref position);
    }

    public static void Serialize(Stream stream, InnerTargetData targetData,ref int position)
    {
        DataSerialize.WriteBool(stream, ref position, targetData.boolValue);
        DataSerialize.WriteByte(stream, ref position, targetData.byteValue);
        DataSerialize.WriteShort(stream, ref position, targetData.shortValue);
        DataSerialize.WriteUShort(stream, ref position, targetData.ushortValue);
        DataSerialize.WriteInt(stream, ref position, targetData.intValue);
        DataSerialize.WriteUInt(stream, ref position, targetData.uintValue);
        DataSerialize.WriteLong(stream, ref position, targetData.longValue);
        DataSerialize.WriteULong(stream, ref position, targetData.ulongValue);
        DataSerialize.WriteFloat(stream, ref position, targetData.floatValue);
        DataSerialize.WriteDouble(stream, ref position, targetData.doubleValue);
        DataSerialize.WriteString(stream, ref position, targetData.strValue);
    }

    public static InnerTargetData Deserialize(byte[] bytes)
    {
        int start = 0;
        return Deserialize(bytes, ref start);
    }
    public static InnerTargetData Deserialize(byte[] bytes, ref int start)
    {
        InnerTargetData innerData = new InnerTargetData();
        innerData.boolValue = DataSerialize.ReadBool(bytes, ref start);
        innerData.byteValue = DataSerialize.ReadByte(bytes, ref start);
        innerData.shortValue = DataSerialize.ReadShort(bytes, ref start);
        innerData.ushortValue = DataSerialize.ReadUShort(bytes, ref start);
        innerData.intValue = DataSerialize.ReadInt(bytes, ref start);
        innerData.uintValue = DataSerialize.ReadUInt(bytes, ref start);
        innerData.longValue = DataSerialize.ReadLong(bytes, ref start);
        innerData.ulongValue = DataSerialize.ReadULong(bytes, ref start);
        innerData.floatValue = DataSerialize.ReadFloat(bytes, ref start);
        innerData.doubleValue = DataSerialize.ReadDouble(bytes, ref start);
        innerData.strValue = DataSerialize.ReadString(bytes, ref start);

        return innerData;
    }
}


