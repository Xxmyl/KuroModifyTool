using KuroModifyTool.KuroTable;
using System.Collections.Generic;

namespace KuroModifyTool
{
    internal class StaticField
    {
        public static MyBinStream MyBS;

        public static string ConfigPath = ".\\Config\\config";

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
            }
        }
        public static string TBLPath;

        public static List<OtherDesc> EffectList;
        public static List<OtherDesc> RangeList;
        public static List<OtherDesc> HCEffectList;

        public static List<OtherDic> SkillDic;
    }
}
