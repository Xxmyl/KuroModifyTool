using KuroModifyTool.KuroTable;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

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

                    BakcutFile("t_item.tbl");
                    BakcutFile("t_skill.tbl");
                    BakcutFile("t_shard_skill.tbl");
                    BakcutFile("t_hollowcore.tbl");
                    BakcutFile("t_artsdriver.tbl");

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
                File.Copy(StaticField.TBLPath + name, ".\\bak\\" + name);
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
