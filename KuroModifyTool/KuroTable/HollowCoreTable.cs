using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuroModifyTool.KuroTable
{
    internal class HollowCoreTable : TBLCommon
    {
        public HollowCoreBaseParam[] BaseParams;

        public class HollowCoreBaseParam
        {
            [FieldIndexAttr(Index = 0)]
            public uint ItemID;

            [FieldIndexAttr(Index = 1)]
            public uint Unknown1;

            [FieldIndexAttr(Index = 2)]
            public uint Unknown2;

            [FieldIndexAttr(Index = 3)]
            public byte Unknown3;

            [FieldIndexAttr(Index = 4)]
            public byte Unknown4;

            [FieldIndexAttr(Index = 5)]
            public short Empty1;

            [FieldIndexAttr(Index = 6)]
            public ulong TextOff1;

            [FieldIndexAttr(Index = 7)]
            public ulong Unknown6;

            [FieldIndexAttr(Index = 8)]
            public ulong TitleOff;

            [FieldIndexAttr(Index = 9)]
            public uint VoiceID;

            [FieldIndexAttr(Index = 10)]
            public float Unknown8;

            [FieldIndexAttr(Index = 11)]
            public float Unknown9;

            [FieldIndexAttr(Index = 12)]
            public float Unknown10;

            [FieldIndexAttr(Index = 13)]
            public uint Empty2;

            [FieldIndexAttr(Index = 14)]
            public float Unknown11;

            [FieldIndexAttr(Index = 15)]
            public ulong TextOff2;

            [FieldIndexAttr(Index = 16)]
            public ulong Unknown12;

            [FieldIndexAttr(Index = 17)]
            public ulong NameOff;
        }

        public HollowCoreLevelParam[] LevelParams;

        public class HollowCoreLevelParam
        {
            [FieldIndexAttr(Index = 0)]
            public uint ItemID;

            [FieldIndexAttr(Index = 1)]
            public uint Level;

            [FieldIndexAttr(Index = 2)]
            public uint Exp;

            [FieldIndexAttr(Index = 3)]
            public uint MagicAttack;

            [FieldIndexAttr(Index = 4)]
            public uint EP;

            [FieldIndexAttr(Index = 5)]
            [BinStreamAttr(Length = 7)]
            public HCEffect[] Effects;

            [FieldIndexAttr(Index = 6)]
            public ulong DescriptionOff;
        }

        public HollowCoreEffParam[] EffParams;

        public class HollowCoreEffParam
        {
            [FieldIndexAttr(Index = 0)]
            public uint ID;

            [FieldIndexAttr(Index = 1)]
            public uint Unknown1;

            [FieldIndexAttr(Index = 2)]
            public ulong Unknown2;

            [FieldIndexAttr(Index = 3)]
            public ulong EffNameOff;
        }

        public HollowCoreEffText[] EffTexts;

        public class HollowCoreEffText
        {
            [FieldIndexAttr(Index = 0)]
            public ulong ID;

            [FieldIndexAttr(Index = 1)]
            public ulong EffDescOff;
        }

        public HollowCoreConvertLevelParam[] ConvertLParams;

        public class HollowCoreConvertLevelParam
        {
            [FieldIndexAttr(Index = 0)]
            public uint ID;

            [FieldIndexAttr(Index = 1)]
            public uint Value;
        }

        public HollowCoreCalcLevelParam[] CalcLParams;

        public class HollowCoreCalcLevelParam
        {
            [FieldIndexAttr(Index = 0)]
            public float Unknown1;

            [FieldIndexAttr(Index = 1)]
            public float Unknown2;

            [FieldIndexAttr(Index = 2)]
            public uint Unknown3;

            [FieldIndexAttr(Index = 3)]
            public float Unknown4;

            [FieldIndexAttr(Index = 4)]
            public float Unknown5;
        }

        public HollowCoreVoice[] Voices;

        public class HollowCoreVoice
        {
            [FieldIndexAttr(Index = 0)]
            public ushort ID;

            [FieldIndexAttr(Index = 1)]
            public ushort Number;

            [FieldIndexAttr(Index = 2)]
            public uint Empty1;

            [FieldIndexAttr(Index = 3)]
            public ulong VIDArrOff;

            [FieldIndexAttr(Index = 4)]
            public ulong VIDArrLen;
        }

        public BottomData Extra;

        private readonly string filename = "t_hollowcore.tbl";

        public HollowCoreTable()
        {
            Load();
        }

        public override void Load()
        {
            int i = 0;
            byte[] buffer = ReadHeader(filename, ref i);

            if (buffer == null)
            {
                return;
            }


            BaseParams = StaticField.MyBS.GetNode(Nodes, typeof(HollowCoreBaseParam[]), buffer, ref i);
            LevelParams = StaticField.MyBS.GetNode(Nodes, typeof(HollowCoreLevelParam[]), buffer, ref i);
            EffParams = StaticField.MyBS.GetNode(Nodes, typeof(HollowCoreEffParam[]), buffer, ref i);
            EffTexts = StaticField.MyBS.GetNode(Nodes, typeof(HollowCoreEffText[]), buffer, ref i);
            ConvertLParams = StaticField.MyBS.GetNode(Nodes, typeof(HollowCoreConvertLevelParam[]), buffer, ref i);
            CalcLParams = StaticField.MyBS.GetNode(Nodes, typeof(HollowCoreCalcLevelParam[]), buffer, ref i);
            Voices = StaticField.MyBS.GetNode(Nodes, typeof(HollowCoreVoice[]), buffer, ref i);

            Extra = new BottomData(Nodes, "HollowCoreVoice", buffer);
            //DebugLog();
        }

        public void DebugLog()
        {
            FileTools.LogPath = ".\\log.txt";

            for (int i = 0; i < EffTexts.Length; i++)
            {
                FileTools.WriteLog(EffTexts[i].ID.ToString());
                
                //int d1inx = ShardSText.Offsets.FindIndex(o => o == ShardSkills[i].DescriptionOff1);
                //FileTools.WriteLog(ShardSText.Texts[d1inx]);
                /*int ninx = HCText.Offsets.FindIndex(o => o == EffTexts[i].EffDescOff);
                if (ninx != -1)
                {
                    FileTools.WriteLog("￥");
                    FileTools.WriteLog(FileTools.GetSimplified(HCText.Texts[ninx]));
                }*/

                //FileTools.WriteLog("/");
                
                
                /*FileTools.WriteLog(",");
                FileTools.WriteLog(BaseParams[i].Unknown8.ToString());
                FileTools.WriteLog(",");
                FileTools.WriteLog(BaseParams[i].Unknown9.ToString());
                FileTools.WriteLog(",");
                FileTools.WriteLog(BaseParams[i].Unknown10.ToString());
                FileTools.WriteLog(",");
                FileTools.WriteLog(BaseParams[i].Unknown12.ToString());*/

                /*for (int j = 0; j < ShardSkills[i].Effects.Length; j++)
                {
                    if (ShardSkills[i].Effects[j].ID != 0)
                    {
                        FileTools.WriteLog("/");
                        FileTools.WriteLog("(");
                        FileTools.WriteLog(ShardSkills[i].Effects[j].ID.ToString());
                        FileTools.WriteLog(",");
                        FileTools.WriteLog(ShardSkills[i].Effects[j].Param1.ToString());
                        FileTools.WriteLog(",");
                        FileTools.WriteLog(ShardSkills[i].Effects[j].Param2.ToString());
                        FileTools.WriteLog(",");
                        FileTools.WriteLog(ShardSkills[i].Effects[j].Param3.ToString());
                        FileTools.WriteLog(")");
                    }
                }*/
                FileTools.WriteLog("\n");
            }

            FileTools.CloseLog();
        }

        public override void Save()
        {
            List<byte> modify = new List<byte>();

            modify.AddRange(Encoding.UTF8.GetBytes(Flag));

            StaticField.MyBS.Serialization(SHLength, modify);

            StaticField.MyBS.Serialization(Nodes, modify);

            StaticField.MyBS.Serialization(BaseParams, modify);
            StaticField.MyBS.Serialization(LevelParams, modify);
            StaticField.MyBS.Serialization(EffParams, modify);
            StaticField.MyBS.Serialization(EffTexts, modify);
            StaticField.MyBS.Serialization(ConvertLParams, modify);
            StaticField.MyBS.Serialization(CalcLParams, modify);
            StaticField.MyBS.Serialization(Voices, modify);

            modify.AddRange(Extra.ExtraData);
            //modify.AddRange(ExtraData);

            FileTools.BufferToFile(StaticField.TBLPath + filename, modify.ToArray());
        }

        public override void DataToUI(MainWindow mw, MainFunc mf, int i)
        {
            HollowCoreLevelParam hc = LevelParams[i];

            ulong n1off = Array.Find(mf.itemTable.Items, item => item.ID == hc.ItemID).NameOff;

            ulong n2off = Array.Find(BaseParams, bp => bp.ItemID == hc.ItemID).TitleOff;

            mw.nameTBHC.Text = mf.itemTable.Extra.GetExtraData((int)n1off, typeof(string));
            mw.name2TBHC.Text = Extra.GetExtraData((int)n2off, typeof(string));
            mf.HollowCRichText = Extra.GetExtraData((int)hc.DescriptionOff, typeof(string));


            mw.expTBHC.Text = hc.Exp.ToString();
            mw.maTBHC.Text = hc.MagicAttack.ToString();
            mw.epTBHC.Text = hc.EP.ToString();


            mw.eff1TBHC.Text = hc.Effects[0].ID.ToString();
            mw.effp11TBHC.Text = hc.Effects[0].Param1.ToString();
            mw.effp12TBHC.Text = hc.Effects[0].Param2.ToString();
            mw.effp13TBHC.Text = hc.Effects[0].Param3.ToString();

            mw.eff2TBHC.Text = hc.Effects[1].ID.ToString();
            mw.effp21TBHC.Text = hc.Effects[1].Param1.ToString();
            mw.effp22TBHC.Text = hc.Effects[1].Param2.ToString();
            mw.effp23TBHC.Text = hc.Effects[1].Param3.ToString();

            mw.eff3TBHC.Text = hc.Effects[2].ID.ToString();
            mw.effp31TBHC.Text = hc.Effects[2].Param1.ToString();
            mw.effp32TBHC.Text = hc.Effects[2].Param2.ToString();
            mw.effp33TBHC.Text = hc.Effects[2].Param3.ToString();

            mw.eff4TBHC.Text = hc.Effects[3].ID.ToString();
            mw.effp41TBHC.Text = hc.Effects[3].Param1.ToString();
            mw.effp42TBHC.Text = hc.Effects[3].Param2.ToString();
            mw.effp43TBHC.Text = hc.Effects[3].Param3.ToString();

            mw.eff5TBHC.Text = hc.Effects[4].ID.ToString();
            mw.effp51TBHC.Text = hc.Effects[4].Param1.ToString();
            mw.effp52TBHC.Text = hc.Effects[4].Param2.ToString();
            mw.effp53TBHC.Text = hc.Effects[4].Param3.ToString();

            mw.eff6TBHC.Text = hc.Effects[5].ID.ToString();
            mw.effp61TBHC.Text = hc.Effects[5].Param1.ToString();
            mw.effp62TBHC.Text = hc.Effects[5].Param2.ToString();
            mw.effp63TBHC.Text = hc.Effects[5].Param3.ToString();

            mw.eff7TBHC.Text = hc.Effects[6].ID.ToString();
            mw.effp71TBHC.Text = hc.Effects[6].Param1.ToString();
            mw.effp72TBHC.Text = hc.Effects[6].Param2.ToString();
            mw.effp73TBHC.Text = hc.Effects[6].Param3.ToString();
        }

        public override void UIToData(MainWindow mw, MainFunc mf, int i)
        {
            HollowCoreLevelParam hc = LevelParams[i];

            string name = mw.name2TBHC.Text;
            string desc1 = mf.HollowCRichText;

            int inx = Array.FindIndex(BaseParams, t => t.ItemID == hc.ItemID);

            ulong noff = BaseParams[inx].TitleOff;

            string namel = Extra.GetExtraData((int)noff, typeof(string));
            string desc1l = Extra.GetExtraData((int)hc.DescriptionOff, typeof(string));

            ulong diff1 = StaticField.MyBS.GetStringDiff(namel, name);
            ulong diff2 = StaticField.MyBS.GetStringDiff(desc1l, desc1);

            Extra.SetExtraData((int)noff, namel, SetValue(namel, name));
            Extra.SetExtraData((int)hc.DescriptionOff, desc1l, SetValue(desc1l, desc1));

            BaseParams[inx].TextOff1 += diff1;
            BaseParams[inx].NameOff += diff1;

            TextReSetOff(diff1, diff2, inx + 1, i + 1);


            SetValue(ref hc.Exp, mw.expTBHC.Text);
            SetValue(ref hc.MagicAttack, mw.maTBHC.Text);
            SetValue(ref hc.EP, mw.epTBHC.Text);


            SetValue(ref hc.Effects[0].ID, mw.eff1TBHC.Text);
            SetValue(ref hc.Effects[0].Param1, mw.effp11TBHC.Text);
            SetValue(ref hc.Effects[0].Param2, mw.effp12TBHC.Text);
            SetValue(ref hc.Effects[0].Param3, mw.effp13TBHC.Text);

            SetValue(ref hc.Effects[1].ID, mw.eff2TBHC.Text);
            SetValue(ref hc.Effects[1].Param1, mw.effp21TBHC.Text);
            SetValue(ref hc.Effects[1].Param2, mw.effp22TBHC.Text);
            SetValue(ref hc.Effects[1].Param3, mw.effp23TBHC.Text);

            SetValue(ref hc.Effects[2].ID, mw.eff3TBHC.Text);
            SetValue(ref hc.Effects[2].Param1, mw.effp31TBHC.Text);
            SetValue(ref hc.Effects[2].Param2, mw.effp32TBHC.Text);
            SetValue(ref hc.Effects[2].Param3, mw.effp33TBHC.Text);

            SetValue(ref hc.Effects[3].ID, mw.eff4TBHC.Text);
            SetValue(ref hc.Effects[3].Param1, mw.effp41TBHC.Text);
            SetValue(ref hc.Effects[3].Param2, mw.effp42TBHC.Text);
            SetValue(ref hc.Effects[3].Param3, mw.effp43TBHC.Text);

            SetValue(ref hc.Effects[4].ID, mw.eff5TBHC.Text);
            SetValue(ref hc.Effects[4].Param1, mw.effp51TBHC.Text);
            SetValue(ref hc.Effects[4].Param2, mw.effp52TBHC.Text);
            SetValue(ref hc.Effects[4].Param3, mw.effp53TBHC.Text);

            SetValue(ref hc.Effects[5].ID, mw.eff6TBHC.Text);
            SetValue(ref hc.Effects[5].Param1, mw.effp61TBHC.Text);
            SetValue(ref hc.Effects[5].Param2, mw.effp62TBHC.Text);
            SetValue(ref hc.Effects[5].Param3, mw.effp63TBHC.Text);

            SetValue(ref hc.Effects[6].ID, mw.eff7TBHC.Text);
            SetValue(ref hc.Effects[6].Param1, mw.effp71TBHC.Text);
            SetValue(ref hc.Effects[6].Param2, mw.effp72TBHC.Text);
            SetValue(ref hc.Effects[6].Param3, mw.effp73TBHC.Text);
        }

        private void TextReSetOff(ulong ndiff, ulong ddiff,int ni, int di)
        {
            ulong diff = ndiff + ddiff;
            if (diff == 0)
            {
                return;
            }

            for (; ni < BaseParams.Length; ni++)
            {
                BaseParams[ni].TextOff1 += ndiff;
                BaseParams[ni].TitleOff += ndiff;
                BaseParams[ni].TextOff2 += ndiff;
                BaseParams[ni].NameOff += ndiff;
            }

            for(int i = 0; i < di; i++)
            {
                LevelParams[i].DescriptionOff += ndiff;
            }

            for (; di < LevelParams.Length; di++)
            {
                LevelParams[di].DescriptionOff += diff;
            }

            for (int i = 0; i < EffParams.Length; i++)
            {
                EffParams[i].EffNameOff += diff;
            }

            for (int i = 0; i < EffTexts.Length; i++)
            {
                EffTexts[i].EffDescOff += diff;
            }

            for (int i = 0; i < Voices.Length; i++)
            {
                Voices[i].VIDArrOff += diff;
            }

        }

        public void VoiceAdd(MainWindow mw, MainFunc mf, int i)
        {
            mw.jumpVList.Items.Clear();
            HollowCoreLevelParam hc = LevelParams[i];

            ulong noff = Array.Find(mf.itemTable.Items, item => item.ID == hc.ItemID).NameOff;
            mw.whoTBV.Text = mf.itemTable.Extra.GetExtraData((int)noff, typeof(string));

            HollowCoreBaseParam hcbase = Array.Find(BaseParams, hcb => hcb.ItemID == hc.ItemID);

            foreach (HollowCoreVoice v in Voices)
            {
                if(v.ID != hcbase.VoiceID)
                {
                    continue;
                }

                for(ulong j = 0; j < v.VIDArrLen; j++)
                {
                    int vinx = Array.FindIndex(mf.voiceTable.Voices, v1 => 
                    v1.ID == Extra.GetExtraData((int)(v.VIDArrOff + j * 4), typeof(uint)));
                    
                    if(vinx == -1)
                    {
                        continue;
                    }

                    mw.jumpVList.Items.Add(mw.voiceList.Items[vinx]);
                }
            }
        }
    }
}
