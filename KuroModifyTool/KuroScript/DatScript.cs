using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuroModifyTool.KuroScript
{
    internal class DatScript
    {
        public string Flag;

        public uint StartOff;

        public uint FunctionCount;

        public uint VariableOff;

        public uint VariableInCount;

        public uint VariableOutCount;

        public Function[] Functions;

        public class Function
        {
            [FieldIndexAttr(Index = 0)]
            public uint Start;

            [FieldIndexAttr(Index = 1)]
            public byte VarIn;

            [FieldIndexAttr(Index = 2)]
            public byte Unknown1;

            [FieldIndexAttr(Index = 3)]
            public byte Unknown2;

            [FieldIndexAttr(Index = 4)]
            public byte VarOut;

            [FieldIndexAttr(Index = 5)]
            public uint OutOff;

            [FieldIndexAttr(Index = 6)]
            public uint InOff;

            [FieldIndexAttr(Index = 7)]
            public uint StructCount;

            [FieldIndexAttr(Index = 8)]
            public uint StructOff;

            [FieldIndexAttr(Index = 9)]
            public uint Hash;

            [FieldIndexAttr(Index = 10)]
            public uint NameOff;

            public uint[] InArgs { get; set; }

            public uint[] OutArgs { get; set; }

            public FuncStruct[] Structs { get; set; }

            public string Name { get; set; }

            public InStruction[] InStructions { get; set; }
        }

        public class FuncStruct
        {
            [FieldIndexAttr(Index = 0)]
            public uint CharID;

            [FieldIndexAttr(Index = 1)]
            public ushort Unknown1;

            [FieldIndexAttr(Index = 2)]
            public ushort Unknown2;

            [FieldIndexAttr(Index = 3)]
            public uint UnknownOff;

            public uint[] UnknownArr { get; set; }
        }

        public class InStruction
        {
            public byte Code;
            public object[] Operands;
        }

        private readonly string filename = "battle\\btlsys.dat";

        public bool IsDecrypt;

        public void Load()
        {
            int i = 0;
            byte[] buffer = FileTools.FileToBuffer(StaticField.ScriptPath + filename);
            Flag = new string(StaticField.MyBS.DeSerialization(typeof(char[]), buffer, ref i, new BinStreamAttr() { Length = 4 }));
            if (!Flag.Equals("#scp"))
            {
                IsDecrypt = false;
                return;
            }

            StartOff = StaticField.MyBS.DeSerialization(typeof(uint), buffer, ref i);
            FunctionCount = StaticField.MyBS.DeSerialization(typeof(uint), buffer, ref i);
            VariableOff = StaticField.MyBS.DeSerialization(typeof(uint), buffer, ref i);
            VariableInCount = StaticField.MyBS.DeSerialization(typeof(uint), buffer, ref i);
            VariableOutCount = StaticField.MyBS.DeSerialization(typeof(uint), buffer, ref i);

            Functions = StaticField.MyBS.GetArrayData(typeof(Function), FunctionCount, buffer, ref i);
            
            foreach(Function f in Functions)
            {
                int j = (int)f.OutOff;
                int k = (int)f.InOff;
                int l = (int)f.StructOff;
                f.OutArgs = StaticField.MyBS.GetArrayData(typeof(uint), f.VarOut, buffer, ref j);
                f.InArgs = StaticField.MyBS.GetArrayData(typeof(uint), f.VarIn, buffer, ref k);
                f.Structs = StaticField.MyBS.GetArrayData(typeof(FuncStruct), f.StructCount, buffer, ref l);

                foreach(FuncStruct fstu in f.Structs)
                {
                    int m = (int)fstu.UnknownOff;
                    uint len = (uint)(fstu.Unknown2 * 2); 
                    fstu.UnknownArr = StaticField.MyBS.GetArrayData(typeof(uint), len, buffer, ref m);
                }

                int n = (int)StaticField.MyBS.Remove2MSB(f.NameOff);

                f.Name = StaticField.MyBS.DeSerialization(typeof(string), buffer, ref n);

                List<InStruction> inStus = new List<InStruction>();
                int o = (int)f.Start;
                while (true)
                {
                    inStus.Add(GetInStruction(buffer, ref o));

                    if (inStus.Last().Code == 13)
                    {
                        break;
                    }
                }

                f.InStructions = inStus.ToArray();
                inStus.Clear();
            }
            DebugLog();
        }

        public void DebugLog()
        {
            FileTools.LogPath = ".\\log.txt";

            for (int i = 0; i < Functions.Length; i++)
            {
                FileTools.WriteLog(Functions[i].Name);
                //FileTools.WriteLog(":");



                /*for (int j = 0; j < Skills[i].Effects.Length; j++)
                {
                    if (Skills[i].Effects[j].ID != 0)
                    {
                        FileTools.WriteLog("(");
                        FileTools.WriteLog(Skills[i].Effects[j].ID.ToString());
                        FileTools.WriteLog(",");
                        FileTools.WriteLog(Skills[i].Effects[j].Param1.ToString());
                        FileTools.WriteLog(",");
                        FileTools.WriteLog(Skills[i].Effects[j].Param2.ToString());
                        FileTools.WriteLog(",");
                        FileTools.WriteLog(Skills[i].Effects[j].Param3.ToString());
                        FileTools.WriteLog(")");
                    }
                }*/
                FileTools.WriteLog("\n");
            }

            FileTools.CloseLog();
        }

        private InStruction GetInStruction(byte[] buffer, ref int i)
        {
            InStruction inStu = new InStruction();
            inStu.Code = StaticField.MyBS.DeSerialization(typeof(byte), buffer, ref i);

            switch (inStu.Code)
            {
                case 0:
                    inStu.Operands = new object[2];
                    inStu.Operands[0] = StaticField.MyBS.DeSerialization(typeof(byte), buffer, ref i);
                    inStu.Operands[1] = BitConverter.ToUInt32(buffer, i);
                    i += (byte)inStu.Operands[0];
                    break;
                case 1:
                case 9:
                case 10:
                case 39:
                    inStu.Operands = new object[1];
                    inStu.Operands[0] = StaticField.MyBS.DeSerialization(typeof(byte), buffer, ref i);
                    break;
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                    inStu.Operands = new object[1];
                    inStu.Operands[0] = StaticField.MyBS.DeSerialization(typeof(int), buffer, ref i);
                    break;
                case 11:
                case 14:
                case 15:
                case 16:
                case 37:
                case 40:
                    inStu.Operands = new object[1];
                    inStu.Operands[0] = StaticField.MyBS.DeSerialization(typeof(uint), buffer, ref i);
                    break;
                case 12:
                case 38:
                    inStu.Operands = new object[1];
                    inStu.Operands[0] = StaticField.MyBS.DeSerialization(typeof(ushort), buffer, ref i);
                    break;
                case 34:
                case 35:
                    inStu.Operands = new object[3];
                    inStu.Operands[0] = StaticField.MyBS.DeSerialization(typeof(uint), buffer, ref i);
                    inStu.Operands[1] = StaticField.MyBS.DeSerialization(typeof(uint), buffer, ref i);
                    inStu.Operands[2] = StaticField.MyBS.DeSerialization(typeof(byte), buffer, ref i);
                    break;
                case 36:
                    inStu.Operands = new object[3];
                    inStu.Operands[0] = StaticField.MyBS.DeSerialization(typeof(byte), buffer, ref i);
                    inStu.Operands[1] = StaticField.MyBS.DeSerialization(typeof(byte), buffer, ref i);
                    inStu.Operands[2] = StaticField.MyBS.DeSerialization(typeof(byte), buffer, ref i);
                    break;
            }

            return inStu;
        }
    }
}
