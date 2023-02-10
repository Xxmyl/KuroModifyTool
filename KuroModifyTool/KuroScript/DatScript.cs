using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KuroModifyTool.KuroScript.DatScript;

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

        public ulong[] VariableIns;

        public ulong[] VariableOuts;

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
            public int CharID;

            [FieldIndexAttr(Index = 1)]
            public ushort Unknown1;

            [FieldIndexAttr(Index = 2)]
            public ushort Unknown2;

            [FieldIndexAttr(Index = 3)]
            public uint UnknownOff;

            public int[] UnknownArr { get; set; }
        }

        public class InStruction
        {
            [FieldIndexAttr(Index = 0)]
            public byte Code;
            [FieldIndexAttr(Index = 1)]
            public object[] Operands;

            public uint Offset;
        }

        public uint ExDataPos;

        public byte[] ExtraData;

        public Function func;
        public List<InStruction> instructions;
        public uint AddCount;
        private readonly string filename = "ani\\chr0002.dat";

        public bool IsDecrypt;

        public void Load()
        {
            int i = 0;
            byte[] buffer = FileTools.FileToBuffer(".\\Script\\" + filename);
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

            i = (int)VariableOff;
            VariableIns = StaticField.MyBS.GetArrayData(typeof(ulong), VariableInCount, buffer, ref i);
            VariableOuts = StaticField.MyBS.GetArrayData(typeof(ulong), VariableOutCount, buffer, ref i);

            foreach (Function f in Functions)
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

                int n = StaticField.MyBS.Remove2MSB((int)f.NameOff);

                f.Name = StaticField.MyBS.DeSerialization(typeof(string), buffer, ref n);

                List<InStruction> inStus = new List<InStruction>();
                int o = (int)f.Start;
                while (true)
                {
                    inStus.Add(GetInStruction(buffer, ref o));
                    int inx = Array.FindIndex(Functions, func => func.Start == o);
                    if (inx != -1 || o == 63255)
                        break;
                }

                f.InStructions = inStus.ToArray();
                inStus.Clear();

                if( i < o)
                {
                    i = o;
                }

                
            }
            ExDataPos = (uint)i;
            ExtraData = new byte[buffer.Length - i];
            Array.Copy(buffer, i, ExtraData, 0, ExtraData.Length);

            DebugLog(buffer);
        }

        public void Save()
        {
            List<byte> modify = new List<byte>();

            modify.AddRange(Encoding.UTF8.GetBytes(Flag));
            StaticField.MyBS.Serialization(StartOff, modify);
            StaticField.MyBS.Serialization(FunctionCount, modify);
            StaticField.MyBS.Serialization(VariableOff, modify);
            StaticField.MyBS.Serialization(VariableInCount, modify);
            StaticField.MyBS.Serialization(VariableOutCount, modify);

            StaticField.MyBS.Serialization(Functions, modify);

            List<byte> modify1 = new List<byte>();
            List<byte> modify2 = new List<byte>();
            List<byte> modify3 = new List<byte>();
            List<byte> modify4 = new List<byte>();
            List<byte> modify5 = new List<byte>();
            List<byte> modify6 = new List<byte>();
            StaticField.MyBS.Serialization(VariableIns, modify5);
            StaticField.MyBS.Serialization(VariableOuts, modify5);
            foreach (Function f in Functions)
            {
                StaticField.MyBS.Serialization(f.OutArgs, modify1);
                StaticField.MyBS.Serialization(f.InArgs, modify2);
                StaticField.MyBS.Serialization(f.Structs, modify3);

                foreach (FuncStruct fstu in f.Structs)
                {
                   StaticField.MyBS.Serialization(fstu.UnknownArr, modify4);
                }
            }

            Function[] functions = new Function[Functions.Length];

            Array.Copy(Functions, functions, Functions.Length);

            Array.Sort(functions, delegate(Function x, Function y)
            {
                return x.Start.CompareTo(y.Start);
            });

            foreach (Function f in functions)
            {
                StaticField.MyBS.Serialization(f.InStructions, modify6);
            }

            modify.AddRange(modify1);
            modify.AddRange(modify2);
            modify.AddRange(modify3);
            modify.AddRange(modify4);
            modify.AddRange(modify5);
            modify.AddRange(modify6);
            modify.AddRange(ExtraData);

            FileTools.BufferToFile(".\\" + filename, modify.ToArray());

        }

        public void AddFunction(string name, string args)
        {
            func = new Function();
            //func.Start = ExDataPos;
            func.VarIn = 0;
            func.Unknown1 = 0;
            func.Unknown2 = 0;
            func.VarOut = 0;
            //func.OutOff = Functions.Last().InOff;
            //func.InOff = Functions.Last().StructOff;
            func.StructCount = 0;
            //func.StructOff = VariableOff;
            func.Hash = StaticField.MyBS.ComputeCrc32(name);
            func.Name = name;
            instructions = new List<InStruction>();
            AddCount = 32;
            ExDataPos += 32;
        }

        public void AddInStruction(int code, string arg)
        {
            switch (code)
            {
                case 12:
                    instructions.Add(new InStruction()
                    {
                        Code = 0,
                        Operands = new object[] { (byte)4, StaticField.MyBS.Add2MSB(1, 1)},
                        Offset = ExDataPos
                    });
                    string[] args = arg.Split(',');
                    for(int i = args.Length; i > 1; i--)
                    {

                    }
                    break;
            }
        }

        public void DebugLog(byte[] buffer)
        {
            FileTools.LogPath = ".\\log1.txt";
            //List<string> Stack = new List<string>();
            Stack<string> stack = new Stack<string>();
            Dictionary<int, string> Result = new Dictionary<int, string>();
            string Temp = "";


            for (int i = 0; i < Functions.Length; i++)
            {
                /*if(Functions[i].Name == "sound_play_se")
                {
                    FileTools.WriteLog("");
                }*/
                stack.Clear();
                FileTools.WriteLog("Function " + Functions[i].Name + "(");
                if(Functions[i].Name == "PlayStartVoiceText")
                {
                    FileTools.WriteLog("*");
                }
                int linx = Functions[i].InArgs.Length - 1;
                for (int j = 0; j < Functions[i].InArgs.Length; j++)
                {
                    int arg = (int)Functions[i].InArgs[j];
                    FileTools.WriteLog(StaticField.MyBS.IdentifyType(arg));
                    FileTools.WriteLog(" arg_" + j.ToString());
                    stack.Push("arg_" + j.ToString());
                    

                    if (j != linx)
                    {
                        FileTools.WriteLog(", ");
                    }
                }
                FileTools.WriteLog(")");

                if(Functions[i].OutArgs.Length > 0)
                {
                    FileTools.WriteLog(" -> ");
                }

                linx = Functions[i].OutArgs.Length - 1;
                for (int j = 0; j < Functions[i].OutArgs.Length; j++)
                {
                    int arg = (int)Functions[i].OutArgs[j];
                    FileTools.WriteLog(StaticField.MyBS.IdentifyType(arg));

                    if (j != linx)
                    {
                        FileTools.WriteLog(", ");
                    }
                }
                FileTools.WriteLog(":\n");

                /*for (int j = 0; j < Functions[i].Structs.Length; j++)
                {
                    FuncStruct fstu = Functions[i].Structs[j];
                    FileTools.WriteLog("\tStruct " + j.ToString() + ":\n");
                    FileTools.WriteLog("\t\tCharID = " + fstu.CharID.ToString() + "\n");
                    FileTools.WriteLog("\t\tUnknown = " + fstu.Unknown1.ToString() + "\n");
                    FileTools.WriteLog("\t\tArray = [");
                    linx = fstu.UnknownArr.Length - 1;
                    for (int k = 0; k < fstu.UnknownArr.Length; k++)
                    {
                        FileTools.WriteLog(StaticField.MyBS.GetScriptValue(buffer, fstu.UnknownArr[k]));
                        if (k != linx)
                        {
                            FileTools.WriteLog(", ");
                        }
                    }
                    FileTools.WriteLog("]\n\n");
                }

                FileTools.WriteLog("\n");*/


                //Decompile
                
                List<string> Decompile = new List<string>();
                bool istjump = false;
                bool isfjump = false;
                uint ifjump = 0;
                uint jump = 0;
                StringBuilder sb = new StringBuilder();

                for (int j = 0; j < Functions[i].InStructions.Length; j++)
                {
                    InStruction ins = Functions[i].InStructions[j];
                    //FileTools.WriteLog("\t");

                    /*sb.Clear();
                    sb.Append(ins.Offset);
                    sb.Append(":");
                    sb.Append(StaticField.ScriptInSDic[ins.Code]);
                    //FileTools.WriteLog(StaticField.ScriptInSDic[ins.Code]);
                    if (ins.Operands == null)
                    {
                        sb.Append("\n");
                        FileTools.WriteLog(sb.ToString());
                        continue;
                    }

                    sb.Append("(");
                    //FileTools.WriteLog("(");
                    linx = ins.Operands.Length - 1;
                    /*for (int k = 0; k < ins.Operands.Length; k++)
                    {
                        //FileTools.WriteLog(StaticField.MyBS.GetScriptValue(buffer, ins.Operands[k]));
                        //FileTools.WriteLog(ins.Operands[k].ToString());
                        if(ins.Code == 0)
                        {
                            sb.Append(StaticField.MyBS.GetScriptValue(ExtraData, (int)ins.Operands[1], 63255));
                        }
                        else
                        {
                            sb.Append(ins.Operands[k].ToString());
                        }
                        
                        if (k != linx)
                        {
                            //FileTools.WriteLog(", ");
                            sb.Append(", ");
                        }
                    }
                    //FileTools.WriteLog(")");
                    sb.Append(")\n");
                    FileTools.WriteLog(sb.ToString());*/

                    if (ins.Code == 0)
                    {
                        stack.Push(StaticField.MyBS.IdentifyType((int)ins.Operands[1]) + ":" + StaticField.MyBS.GetScriptValue(ExtraData, (int)ins.Operands[1], 63255));
                        Temp = stack.Peek();
                    }
                    else if(ins.Code == 2)
                    {
                        int inx = Math.Abs((int)ins.Operands[0] / 4);
                        stack.Push(stack.ElementAt(inx - 1));
                        Temp = stack.Peek();
                    }
                    else if(ins.Code == 5)
                    {
                        int inx = Math.Abs((int)ins.Operands[0] / 4);
                        List<string> temp = new List<string>();
                        for(int k = 0; k < inx - 1; k++)
                        {
                            temp.Add(stack.Pop());
                        }
                        stack.Pop();
                        stack.Push(Temp);
                        foreach(string str in temp)
                        {
                            stack.Push(str);
                        }
                    }
                    else if (ins.Code == 9)
                    {
                        //stack.Push("RET_" + ins.Operands[0].ToString());
                        Temp = stack.Peek();
                    }
                    else if (ins.Code == 10)
                    {
                        stack.Push("RET(" + ins.Operands[0].ToString() + ", " + Temp + ")");
                    }
                    else if (ins.Code == 12)
                    {
                        sb.Clear();
                        Function func = Functions[(ushort)ins.Operands[0]];
                        //FileTools.WriteLog(func.Name);
                        sb.Append(func.Name);

                        //FileTools.WriteLog("(");
                        sb.Append("(");
                        linx = func.InArgs.Length - 1;
                        for (int k = 0; k < func.InArgs.Length; k++)
                        {
                            //FileTools.WriteLog(Decompile[Decompile.Count - k]);
                            sb.Append(stack.Pop());
                            if (k >= 0 && k < func.InArgs.Length - 1)
                            {
                                //FileTools.WriteLog(", ");
                                sb.Append(", ");
                            }
                        }
                        
                        //FileTools.WriteLog(")");
                        sb.Append(")");
                        stack.Pop();
                        stack.Pop();
                        stack.Push(sb.ToString());
                    }
                    else if (ins.Code >= 16 && ins.Code <= 30)
                    {
                        string l = stack.Pop();
                        string r = stack.Pop();
                        stack.Push(l + StaticField.ScriptInSDic[ins.Code] + r);
                    }
                    else if(ins.Code >= 31 && ins.Code <= 33)
                    {
                        string code = StaticField.ScriptInSDic[ins.Code] + "(" + stack.Pop() + ")";
                        stack.Push(code);
                    }
                    else if(ins.Code == 14)
                    {
                        sb.Clear();
                        if(istjump)
                        {
                            sb.Append("elif ");
                        }
                        else
                        {
                            sb.Append("if ");
                            istjump = true;
                        }

                        sb.Append(stack.Pop());
                        sb.Append(":");
                        stack.Push(sb.ToString());
                        ifjump = (uint)ins.Operands[0];
                        //FileTools.WriteLog(text);
                        //跳转地址
                    }
                    else if(ins.Code == 15)
                    {
                        sb.Clear();
                        if (isfjump)
                        {
                            sb.Append("elif not ");
                        }
                        else
                        {
                            sb.Append("if not ");
                            isfjump = true;
                        }

                        sb.Append(stack.Pop());
                        sb.Append(":");
                        stack.Push(sb.ToString());
                        ifjump = (uint)ins.Operands[0];
                        //FileTools.WriteLog(text);
                    }
                    /*else if(ins.Code == 11)
                    {
                        if
                    }*/
                    else if (ins.Code == 38)
                    {
                        continue;
                    }
                    else
                    {
                        sb.Clear();
                        sb.Append(StaticField.ScriptInSDic[ins.Code]);
                        //FileTools.WriteLog(StaticField.ScriptInSDic[ins.Code]);
                        if (ins.Operands == null)
                        {
                            continue;
                        }

                        sb.Append("(");
                        //FileTools.WriteLog("(");
                        linx = ins.Operands.Length - 1;
                        for (int k = 0; k < ins.Operands.Length; k++)
                        {
                            //FileTools.WriteLog(StaticField.MyBS.GetScriptValue(buffer, ins.Operands[k]));
                            //FileTools.WriteLog(ins.Operands[k].ToString());
                            sb.Append(ins.Operands[k].ToString());
                            if (k != linx)
                            {
                                //FileTools.WriteLog(", ");
                                sb.Append(", ");
                            }
                        }
                        //FileTools.WriteLog(")");
                        sb.Append(")");
                        stack.Push(sb.ToString());
                    }
                }

                List<string> funcc = stack.Reverse().ToList();
                foreach (string str in funcc)
                {
                    FileTools.WriteLog("\t");
                    FileTools.WriteLog(str);
                    FileTools.WriteLog("\n");
                }

                //FileTools.WriteLog("\n");
            }

            FileTools.CloseLog();
        }

        private void RemoveEnd(List<string> decompile, int len)
        {
            for (int k = 0; k < len; k++)
            {
                decompile.RemoveAt(decompile.Count - 1);
            }
        }

        private InStruction GetInStruction(byte[] buffer, ref int i)
        {
            InStruction inStu = new InStruction();
            inStu.Offset = (uint)i;
            inStu.Code = StaticField.MyBS.DeSerialization(typeof(byte), buffer, ref i);

            switch (inStu.Code)
            {
                case 0:
                    inStu.Operands = new object[2];
                    byte size = StaticField.MyBS.DeSerialization(typeof(byte), buffer, ref i);
                    inStu.Operands[0] = size;
                    //int value = BitConverter.ToInt32(buffer, i);
                    inStu.Operands[1] = BitConverter.ToInt32(buffer, i);
                    //inStu.Operands[2] = StaticField.MyBS.GetScriptValue(buffer, value);
                    i += size;
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
