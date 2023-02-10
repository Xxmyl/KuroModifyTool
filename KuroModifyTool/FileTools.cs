using KuroModifyTool.KuroTable;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Input;

namespace KuroModifyTool
{
    internal class FileTools
    {
        public static string LogPath;
        public static StreamWriter SW;
        public static byte[] FileToBuffer(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();

            return buffer;
        }

        public static void BufferToFile(string path, byte[] buffer)
        {
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            fs.Write(buffer, 0, buffer.Length);
            fs.Flush();
            fs.Close();
        }

        public static void PackTbl(string srcp, string tarp)
        {
            Process p = new Process();
            //设置要启动的应用程序
            p.StartInfo.FileName = "cmd.exe";
            //是否使用操作系统shell启动
            p.StartInfo.UseShellExecute = false;
            // 接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardInput = true;
            //输出信息
            p.StartInfo.RedirectStandardOutput = true;
            // 输出错误
            p.StartInfo.RedirectStandardError = true;
            //不显示程序窗口
            p.StartInfo.CreateNoWindow = true;
            //启动程序
            p.StartInfo.Verb = "RunAs";

            p.Start();
            string strInput = "\"" + Environment.CurrentDirectory + "\\EnComp.exe\" -ec \"" + srcp + "\" \"" + tarp + "\"";
            //sb.Append(strInput + "\n");
            p.StandardInput.WriteLine(strInput);
            p.StandardInput.WriteLine("exit");
            p.StandardInput.AutoFlush = true;
            Thread tr = new Thread(new ParameterizedThreadStart(outputT));
            tr.Start(p);
            Thread tr1 = new Thread(new ParameterizedThreadStart(outputT));
            tr1.Start(p);
            p.WaitForExit();
            p.Close();

            p = null;
        }

        private static void outputT(object o)
        {
            Process p = (Process)o;
            string outPut = p.StandardOutput.ReadToEnd();
            outPut = null;
        }

        private void errorT(object o)
        {
            Process p = (Process)o;
            string error = p.StandardError.ReadToEnd();
            error = null;
        }

        public static void WriteLog(string text)
        {
            if (LogPath == null || text == null)
            {
                return;
            }

            if (SW == null)
            {
                SW = new StreamWriter(LogPath);
            }

            SW.Write(text);
        }

        public static void CloseLog()
        {
            if (SW != null)
            {
                SW.Close();
            }
        }

        public static List<OtherDesc> GetEffectList(string path)
        {
            StreamReader sr = new StreamReader(path);
            List<OtherDesc> effList = new List<OtherDesc>();

            while (!sr.EndOfStream)
            {
                string text = sr.ReadLine();

                if (text == "")
                {
                    continue;
                }

                string[] data = text.Split('￥');
                OtherDesc desc = new OtherDesc();
                desc.ID = data[0];
                desc.Description = data[1];

                if (data.Length > 2)
                {
                    desc.Param1 = data[2];
                }
                if (data.Length > 3)
                {
                    desc.Param2 = data[3];
                }
                if (data.Length > 4)
                {
                    desc.Param3 = data[4];
                }

                effList.Add(desc);
            }
            sr.Close();

            return effList;
        }

        public static List<OtherDic> GetSkillDic(string path)
        {
            StreamReader sr = new StreamReader(path);
            List<OtherDic> skillDic = new List<OtherDic>();

            while (!sr.EndOfStream)
            {
                string text = sr.ReadLine();

                if (text == "")
                {
                    continue;
                }

                string[] data = text.Split(':');

                OtherDic dic = new OtherDic();
                dic.ID = data[0];
                dic.Description = data[1];

                skillDic.Add(dic);
            }
            sr.Close();

            return skillDic;
        }

        public static Dictionary<byte, string> GetCurrencyDic(string path)
        {
            StreamReader sr = new StreamReader(path);
            Dictionary<byte, string> dic = new Dictionary<byte, string>();

            while (!sr.EndOfStream)
            {
                string text = sr.ReadLine();

                if (text == "")
                {
                    continue;
                }

                string[] data = text.Split(':');

                dic.Add(byte.Parse(data[0]), data[1]);
            }
            sr.Close();

            return dic;
        }

        public static T JsonLoad<T>(string path)
        {
            StreamReader sr = new StreamReader(path);
            T t = JsonConvert.DeserializeObject<T>(sr.ReadToEnd());

            sr.Close();
            return t;
        }

        public static void JsonSave(object obj, string path)
        {
            StreamWriter sw = new StreamWriter(path, false);
            string json = JsonConvert.SerializeObject(obj);

            sw.Write(json);
            sw.Close();
        }

        public static bool initConfig(Action action)
        {
            if (!File.Exists(StaticField.ConfigPath))
            {
                action.Invoke();

                if (StaticField.GamePath != null && StaticField.GamePath != "" 
                    && Directory.Exists(StaticField.GamePath) 
                    && File.Exists(StaticField.GamePath + "\\ed9.exe"))
                {
                    FileStream fs = new FileStream(StaticField.ConfigPath, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(StaticField.GamePath);
                    sw.Flush();
                    sw.Close();
                    fs.Close();

                    StaticField.CLEFiles = new List<CLEFile>();
                    JsonSave(StaticField.CLEFiles, StaticField.CLEFList);

                    BakcutFile("t_item.tbl");
                    BakcutFile("t_skill.tbl");
                    BakcutFile("t_shard_skill.tbl");
                    BakcutFile("t_hollowcore.tbl");
                    BakcutFile("t_artsdriver.tbl"); 
                    BakcutFile("t_voice.tbl");

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                StreamReader sr = new StreamReader(StaticField.ConfigPath);

                StaticField.GamePath = sr.ReadLine();
                sr.Close();

                StaticField.CLEFiles = JsonLoad<List<CLEFile>>(".\\KuroList\\CLEFileList.json");
                return true;
            }
        }

        /// <summary> 
        /// 繁体转换为简体
        /// </summary> 
        /// <param name="str">繁体字</param> 
        /// <returns>简体字</returns> 
        public static string GetSimplified(string str)
        {
            string r = string.Empty;
            r = ChineseConverter.Convert(str, ChineseConversionDirection.TraditionalToSimplified);
            return r;
        }

        public static string GetTraditional(string str)
        {
            string r = string.Empty;
            r = ChineseConverter.Convert(str, ChineseConversionDirection.SimplifiedToTraditional);
            return r;
        }

        public static void BakcutFile(string name)
        {
            if(!File.Exists(".\\bak\\" + name))
            {
                if(File.Exists(StaticField.TBLPath + name))
                {
                    File.Copy(StaticField.TBLPath + name, ".\\bak\\" + name);
                }
                else
                {
                    File.Copy(StaticField.TBLPath1 + name, ".\\bak\\" + name);
                }
            }
        }

        public static void PlayOpus(string name)
        {
            Process.Start(StaticField.OpusPath + name + ".opus");
        }

        public static void OutPutOpus(string pn, System.Collections.IList list)
        {
            pn = ".\\Opus\\" + pn + "\\";
            if (!Directory.Exists(pn))
            {
                Directory.CreateDirectory(pn);
            }

            foreach(object o in list)
            {
                string n = o as string;
                if (!File.Exists(pn + n + ".opus"))
                {
                    File.Copy(StaticField.OpusPath + n + ".opus", pn + n + ".opus");
                }
            }

            Process.Start(pn);
        }
    }
}
