using KuroModifyTool.KuroTable;
using System.Collections.Generic;

namespace KuroModifyTool
{
    internal class StaticField
    {
        public static MyBinStream MyBS;

        public static string ConfigPath = ".\\Config\\config";
        //public static string LocalTbl = ".\\Tbl\\";

        public static string gamePath;
        public static string GamePath
        {
            get
            {
                return gamePath;
            }
            set
            {
                gamePath = value;
                TBLPath = value + "\\tc\\f\\table\\";
                TBLPath1 = value + "\\f\\table\\";
                ScriptPath = value + "\\tc\\f\\script\\";
                OpusPath = value + "\\voice\\opus\\";
            }
        }
        public static string TBLPath;

        public static string TBLPath1;

        public static string ScriptPath;

        public static string OpusPath;

        public static string CLEFList = ".\\KuroList\\CLEFileList.json";

        public static List<OtherDesc> EffectList;
        public static List<OtherDesc> RangeList;
        public static List<OtherDesc> HCEffectList;

        public static List<OtherDic> SkillDic;
        public static Dictionary<byte, string> ScriptInSDic;

        public static List<CLEFile> CLEFiles;
        public static CLEFile CurrentCLEF;
    }
}
