using System;
using System.Collections.Generic;

namespace KuroModifyTool.KuroTable
{
    internal abstract class TBLCommon
    {
        public string Flag;

        public uint SHLength;

        public SubHeader[] Nodes;

        public bool IsDecrypt;

        public byte[] ReadHeader(string filename, ref int i)
        {
            byte[] buffer = FileTools.FileToBuffer(StaticField.TBLPath + filename);
            
            Flag = new string(StaticField.MyBS.DeSerialization(typeof(char[]), buffer, ref i, new BinStreamAttr() { Length = 4 }));

            if (!Flag.Equals("#TBL"))
            {
                IsDecrypt = false;
                return null;
            }

            SHLength = StaticField.MyBS.DeSerialization(typeof(uint), buffer, ref i);

            Nodes = new SubHeader[SHLength];

            for (int j = 0; j < SHLength; j++)
            {
                Nodes[j] = StaticField.MyBS.DeSerialization(typeof(SubHeader), buffer, ref i);
            }

            IsDecrypt = true;

            return buffer;
        }

        public string SetValue(string o, string n)
        {
            if (n == null || n == "")
            {
                return o;
            }

            return n;
        }
        public void SetValue(ref byte o, string n)
        {
            if (n == null || n == "")
            {
                return;
            }

            o = byte.Parse(n);
        }
        public void SetValue(ref short o, string n)
        {
            if (n == null || n == "")
            {
                return;
            }

            o = short.Parse(n);
        }
        public void SetValue(ref ushort o, string n)
        {
            if (n == null || n == "")
            {
                return;
            }

            o = ushort.Parse(n);
        }
        public void SetValue(ref int o, string n)
        {
            if (n == null || n == "")
            {
                return;
            }

            o = int.Parse(n);
        }
        public void SetValue(ref uint o, string n)
        {
            if (n == null || n == "")
            {
                return;
            }

            o = uint.Parse(n);
        }
        public void SetValue(ref ulong o, string n)
        {
            if (n == null || n == "")
            {
                return;
            }

            o = ulong.Parse(n);
        }
        public void SetValue(ref float o, string n)
        {
            if (n == null || n == "")
            {
                return;
            }

            o = float.Parse(n);
        }

        public abstract void Load();

        public abstract void Save();

        public abstract void DataToUI(MainWindow mw, MainFunc mf, int i);

        public abstract void UIToData(MainWindow mw, MainFunc mf, int i);
    }

    public class SubHeader
    {
        [FieldIndexAttr(Index = 0)]
        [BinStreamAttr(Length = 64)]
        public char[] Name;

        [FieldIndexAttr(Index = 1)]
        public uint Unknown;

        [FieldIndexAttr(Index = 2)]
        public uint DataOffset;

        [FieldIndexAttr(Index = 3)]
        public uint DataLength;

        [FieldIndexAttr(Index = 4)]
        public uint NodeCount;
    }

    public class Effect
    {
        [FieldIndexAttr(Index = 0)]
        public uint ID;

        [FieldIndexAttr(Index = 1)]
        public uint Param1;

        [FieldIndexAttr(Index = 2)]
        public uint Param2;

        [FieldIndexAttr(Index = 3)]
        public uint Param3;
    }

    public class HCEffect
    {
        [FieldIndexAttr(Index = 0)]
        public uint ID;

        [FieldIndexAttr(Index = 1)]
        public uint Param1;

        [FieldIndexAttr(Index = 2)]
        public uint Param2;

        [FieldIndexAttr(Index = 3)]
        public uint Param3;

        [FieldIndexAttr(Index = 4)]
        public uint Param4;

        [FieldIndexAttr(Index = 5)]
        public uint Param5;

        [FieldIndexAttr(Index = 6)]
        public uint Param6;
    }

    public class TextData
    {
        public int StartIndex;

        public int EndIndex;

        public List<ulong> Offsets;

        public List<string> Texts;

        public TextData(int sinx, int einx)
        {
            StartIndex = sinx;
            EndIndex = einx;
        }

        public static int GetTextStartOff(SubHeader[] nodes, string name)
        {
            SubHeader node = Array.Find<SubHeader>(nodes, n => new string(n.Name).StartsWith(name));
            return (int)(node.DataOffset + node.DataLength * node.NodeCount);
        }
    }

    public class OtherDesc
    {
        public string ID;

        public string Description;

        public string Param1;

        public string Param2;

        public string Param3;

        public override string ToString()
        {
            return Description;
        }
    }

    public class OtherDic
    {
        public string ID;

        public string Description;

        public override string ToString()
        {
            return Description;
        }
    }
}
