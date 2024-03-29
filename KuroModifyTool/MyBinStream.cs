﻿using KuroModifyTool.KuroTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace KuroModifyTool
{
    internal class MyBinStream
    {
        public Dictionary<string, int> ArrayLenDic;

        public MyBinStream()
        {
            ArrayLenDic = new Dictionary<string, int>();
        }

        public static byte[] GetStringBuf(byte[] buf, int inx)
        {
            List<byte> str = new List<byte>();
            for (; inx < buf.Length; inx++)
            {
                str.Add(buf[inx]);

                if (buf[inx] == 0)
                {
                    break;
                }
            }

            return str.ToArray();
        }

        public dynamic DeSerialization(Type type, byte[] data, ref int i, BinStreamAttr attr = null)
        {
            dynamic a = null;
            if (type == typeof(string))
            {
                byte[] str = GetStringBuf(data, i);
                a = Encoding.UTF8.GetString(str).TrimEnd('\0');
                i += str.Length;
            }
            else if (type == typeof(char))
            {
                a = data[i];
                i += 1;
            }
            else if (type == typeof(byte))
            {
                a = data[i];
                i += 1;
            }
            else if (type == typeof(ushort))
            {
                a = BitConverter.ToUInt16(data, i);
                i += 2;
            }
            else if (type == typeof(short))
            {
                a = BitConverter.ToInt16(data, i);
                i += 2;
            }
            else if (type == typeof(uint))
            {
                a = BitConverter.ToUInt32(data, i);
                i += 4;
            }
            else if (type == typeof(int))
            {
                a = BitConverter.ToInt32(data, i);
                i += 4;
            }
            else if (type == typeof(ulong))
            {
                a = BitConverter.ToUInt64(data, i);
                i += 8;
            }
            else if (type == typeof(float))
            {
                a = BitConverter.ToSingle(data, i);
                i += 4;
            }
            else if (type.IsArray)
            {
                if (attr != null)
                {
                    int arrayLen = attr.Length;
                    Array arr = Array.CreateInstance(type.GetElementType(), arrayLen);

                    for (int j = 0; j < arrayLen; j++)
                    {
                        arr.SetValue(DeSerialization(type.GetElementType(), data, ref i), j);
                    }

                    a = arr;
                }
            }
            else if (type.IsClass)
            {
                a = type.Assembly.CreateInstance(type.FullName);

                FieldInfo[] fields = type.GetFields();
                Array.Sort(fields, new FieldsComparable());

                foreach (FieldInfo field in fields)
                {
                    BinStreamAttr bsAttr = field.GetCustomAttribute(typeof(BinStreamAttr), false) as BinStreamAttr;

                    object value = DeSerialization(field.FieldType, data, ref i, bsAttr);
                    field.SetValue(a, value);
                }
            }

            return a;
        }

        public void Serialization(object obj, List<byte> modify)
        {
            Type type = obj.GetType();
            if (type == typeof(string))
            {
                modify.AddRange(Encoding.UTF8.GetBytes((string)obj));
                modify.Add(0);
            }
            else if (type == typeof(char))
            {
                modify.AddRange(Encoding.ASCII.GetBytes(new char[] { (char)obj }));
            }
            else if (type == typeof(byte))
            {
                modify.Add((byte)obj);
            }
            else if (type == typeof(ushort))
            {
                modify.AddRange(BitConverter.GetBytes((ushort)obj));
            }
            else if (type == typeof(short))
            {
                modify.AddRange(BitConverter.GetBytes((short)obj));
            }
            else if (type == typeof(uint))
            {
                modify.AddRange(BitConverter.GetBytes((uint)obj));
            }
            else if (type == typeof(int))
            {
                modify.AddRange(BitConverter.GetBytes((int)obj));
            }
            else if (type == typeof(ulong))
            {
                modify.AddRange(BitConverter.GetBytes((ulong)obj));
            }
            else if (type == typeof(float))
            {
                modify.AddRange(BitConverter.GetBytes((float)obj));
            }
            else if (type.IsArray)
            {
                if (obj != null)
                {
                    Array array = (Array)obj;
                    for (int j = 0; j < array.Length; j++)
                    {
                        Serialization(array.GetValue(j), modify);
                    }
                }
            }
            else if (type.IsClass)
            {
                FieldInfo[] fields = type.GetFields();
                Array.Sort(fields, new FieldsComparable());

                foreach (FieldInfo field in fields)
                {
                    Serialization(field.GetValue(obj), modify);
                }
            }
        }

        public int GetDataLen(object obj)
        {
            Type type = obj.GetType();

            if (type == typeof(string))
            {
                return Encoding.UTF8.GetBytes((string)obj).Length + 1;
            }
            else if (type == typeof(byte))
            {
                return 1;
            }
            else if (type == typeof(ushort))
            {
                return 2;
            }
            else if (type == typeof(short))
            {
                return 2;
            }
            else if (type == typeof(uint))
            {
                return 4;
            }
            else if (type == typeof(int))
            {
                return 4;
            }
            else if (type == typeof(ulong))
            {
                return 8;
            }
            else if (type == typeof(float))
            {
                return 4;
            }

            return -1;
        }

        public dynamic GetNode(SubHeader[] nodes, Type type, byte[] buffer, ref int i)
        {
            if (!type.IsArray)
            {
                return null;
            }
            
            SubHeader node = Array.Find<SubHeader>(nodes, n => new string(n.Name).StartsWith(type.GetElementType().Name));
            i = (int)node.DataOffset;

            return GetArrayData(type.GetElementType(), node.NodeCount, buffer, ref i);
        }

        public dynamic GetArrayData(Type type, uint len, byte[] buffer, ref int i)
        {
            Array arr = Array.CreateInstance(type, len);

            for (int j = 0; j < len; j++)
            {
                arr.SetValue(DeSerialization(type, buffer, ref i), j);
            }

            return arr;
        }

        public ulong GetStringDiff(string o, string n)
        {
            return (ulong)(Encoding.UTF8.GetBytes(n).Length - Encoding.UTF8.GetBytes(o).Length);
        }

        public uint Remove2MSB(uint off)
        {
            uint a = off << 2;
            uint b = a >> 2;
            return b;
        }
    }

    public class FieldsComparable : IComparer<FieldInfo>
    {
        public int Compare(FieldInfo x, FieldInfo y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            FieldIndexAttr attrx = x.GetCustomAttribute(typeof(FieldIndexAttr), false) as FieldIndexAttr;
            FieldIndexAttr attry = y.GetCustomAttribute(typeof(FieldIndexAttr), false) as FieldIndexAttr;

            {
                if (attrx.Index > attry.Index) return 1;
                if (attrx.Index < attry.Index) return -1;
            }

            return 0;
        }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class BinStreamAttr : Attribute
    {
        public byte Id { get; set; }
        public int Length { get; set; }
        public string FieldName { get; set; }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class FieldIndexAttr : Attribute
    {
        public short Index { get; set; }
    }
}
