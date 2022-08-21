using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuroModifyTool.KuroTable
{
    internal class ShardSkillTable : TBLCommon
    {
        public ShardSkillParam[] ShardSkills;

        public class ShardSkillParam
        {
            [FieldIndexAttr(Index = 0)]
            public ushort ID;

            [FieldIndexAttr(Index = 1)]
            public byte Category;

            [FieldIndexAttr(Index = 2)]
            public byte Unknown1;

            [FieldIndexAttr(Index = 3)]
            public byte EarthCost;

            [FieldIndexAttr(Index = 4)]
            public byte WaterCost;

            [FieldIndexAttr(Index = 5)]
            public byte FireCost;

            [FieldIndexAttr(Index = 6)]
            public byte WindCost;

            [FieldIndexAttr(Index = 7)]
            public byte TimeCost;

            [FieldIndexAttr(Index = 8)]
            public byte SpaceCost;

            [FieldIndexAttr(Index = 9)]
            public byte MirageCost;

            [FieldIndexAttr(Index = 10)]
            public byte BaseChance;

            [FieldIndexAttr(Index = 11)]
            public byte SclmBaseChance;

            [FieldIndexAttr(Index = 12)]
            public byte SboostChance;

            [FieldIndexAttr(Index = 13)]
            public byte SclmSboostChance;

            [FieldIndexAttr(Index = 14)]
            public byte FullboostChance;

            [FieldIndexAttr(Index = 15)]
            public byte Empty1;

            [FieldIndexAttr(Index = 16)]
            public byte Unknown2;

            [FieldIndexAttr(Index = 17)]
            public ushort UpgradeID;

            [FieldIndexAttr(Index = 18)]
            public uint Empty2;

            [FieldIndexAttr(Index = 19)]
            public ulong TextOff1;

            [FieldIndexAttr(Index = 20)]
            public ulong Empty3;

            [FieldIndexAttr(Index = 21)]
            public ulong TextOff2;

            [FieldIndexAttr(Index = 22)]
            public uint Condition1;

            [FieldIndexAttr(Index = 23)]
            public uint Condition2;

            [FieldIndexAttr(Index = 24)]
            public uint Unknown3;

            [FieldIndexAttr(Index = 25)]
            public uint Empty4;

            [FieldIndexAttr(Index = 26)]
            public ushort Unknown4;

            [FieldIndexAttr(Index = 27)]
            public ushort SubstituteID;

            [FieldIndexAttr(Index = 28)]
            [BinStreamAttr(Length = 2)]
            public Effect[] Effects;

            [FieldIndexAttr(Index = 29)]
            public uint Unknown5;

            [FieldIndexAttr(Index = 30)]
            public ulong TextOff3;

            [FieldIndexAttr(Index = 31)]
            public ulong Number;

            [FieldIndexAttr(Index = 32)]
            public ulong NameOff;

            [FieldIndexAttr(Index = 33)]
            public ulong DescriptionOff1;

            [FieldIndexAttr(Index = 34)]
            public ulong DescriptionOff2;
        }

        public TextData ShardSText;

        private readonly string filename = "t_shard_skill.tbl";

        public ShardSkillTable()
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

            //ShardSkillParam
            ShardSkills = StaticField.MyBS.GetNode(Nodes, "ShardSkillParam", typeof(ShardSkillParam[]), buffer, ref i);

            ShardSText = new TextData(TextData.GetTextStartOff(Nodes, "ShardSkillParam"), buffer.Length);
            StaticField.MyBS.GetTextData(buffer, ShardSText);
        }

        public void DebugLog()
        {
            FileTools.LogPath = ".\\log.txt";

            for (int i = 0; i < ShardSkills.Length; i++)
            {
                FileTools.WriteLog(ShardSkills[i].ID.ToString());
                FileTools.WriteLog(":");
                int d1inx = ShardSText.Offsets.FindIndex(o => o == ShardSkills[i].DescriptionOff1);
                FileTools.WriteLog(ShardSText.Texts[d1inx]);

                FileTools.WriteLog("/");
                FileTools.WriteLog(ShardSkills[i].Condition1.ToString());
                FileTools.WriteLog(",");
                FileTools.WriteLog(ShardSkills[i].Condition2.ToString());
                FileTools.WriteLog(",");
                FileTools.WriteLog(ShardSkills[i].Unknown3.ToString());

                FileTools.WriteLog("/");
                FileTools.WriteLog(ShardSkills[i].Unknown4.ToString());
                FileTools.WriteLog(",");
                FileTools.WriteLog(ShardSkills[i].SubstituteID.ToString());

                for (int j = 0; j < ShardSkills[i].Effects.Length; j++)
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
                }
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

            StaticField.MyBS.Serialization(ShardSkills, modify);

            StaticField.MyBS.SetTextData(modify, ShardSText);

            FileTools.BufferToFile(StaticField.TBLPath + filename, modify.ToArray());
        }

        public override void DataToUI(MainWindow mw, MainFunc mf, int i)
        {
            ShardSkillParam skill = ShardSkills[i];

            int ninx = ShardSText.Offsets.FindIndex(o => o == skill.NameOff);
            int d1inx = ShardSText.Offsets.FindIndex(o => o == skill.DescriptionOff1);
            int d2inx = ShardSText.Offsets.FindIndex(o => o == skill.DescriptionOff2);
            mw.nameTBSS.Text = ShardSText.Texts[ninx];
            mf.ShardS1RichText = ShardSText.Texts[d1inx];
            mf.ShardS2RichText = ShardSText.Texts[d2inx];


            mw.baseCTB.Text = skill.BaseChance.ToString();
            mw.sBCTB.Text = skill.SboostChance.ToString();
            mw.fullBCTB.Text = skill.FullboostChance.ToString();
            mw.cond1TB.Text = skill.Condition1.ToString();
            mw.cond2TB.Text = skill.Condition2.ToString();
            mw.substiTB.Text = skill.SubstituteID.ToString();


            mw.earthTBSS.Text = skill.EarthCost.ToString();
            mw.waterTBSS.Text = skill.WaterCost.ToString();
            mw.fireTBSS.Text = skill.FireCost.ToString();
            mw.windTBSS.Text = skill.WindCost.ToString();
            mw.timeTBSS.Text = skill.TimeCost.ToString();
            mw.spaceTBSS.Text = skill.SpaceCost.ToString();
            mw.mirageTBSS.Text = skill.MirageCost.ToString();


            mw.eff1TBSS.Text = skill.Effects[0].ID.ToString();
            mw.effp11TBSS.Text = skill.Effects[0].Param1.ToString();
            mw.effp12TBSS.Text = skill.Effects[0].Param2.ToString();
            mw.effp13TBSS.Text = skill.Effects[0].Param3.ToString();

            mw.eff2TBSS.Text = skill.Effects[1].ID.ToString();
            mw.effp21TBSS.Text = skill.Effects[1].Param1.ToString();
            mw.effp22TBSS.Text = skill.Effects[1].Param2.ToString();
            mw.effp23TBSS.Text = skill.Effects[1].Param3.ToString();
        }

        public override void UIToData(MainWindow mw, MainFunc mf, int i)
        {
            ShardSkillParam skill = ShardSkills[i];

            string name = mw.nameTBSS.Text;
            string desc1 = mf.ShardS1RichText;
            string desc2 = mf.ShardS2RichText;

            int ninx = ShardSText.Offsets.FindIndex(o => o == skill.NameOff);
            int d1inx = ShardSText.Offsets.FindIndex(o => o == skill.DescriptionOff1);
            int d2inx = ShardSText.Offsets.FindIndex(o => o == skill.DescriptionOff2);

            ulong diff1 = StaticField.MyBS.GetStringDiff(ShardSText.Texts[ninx], name);
            ulong diff2 = StaticField.MyBS.GetStringDiff(ShardSText.Texts[d1inx], desc1);
            ulong diff3 = StaticField.MyBS.GetStringDiff(ShardSText.Texts[d2inx], desc2);

            ShardSText.Texts[ninx] = SetValue(ShardSText.Texts[ninx], name);
            ShardSText.Texts[d1inx] = SetValue(ShardSText.Texts[d1inx], desc1);
            ShardSText.Texts[d2inx] = SetValue(ShardSText.Texts[d2inx], desc2);

            skill.DescriptionOff1 += diff1;
            ShardSText.Offsets[d1inx] += diff1;
            skill.DescriptionOff2 += diff1 + diff2;
            ShardSText.Offsets[d2inx] += diff1 + diff2;

            TextReSetOff(diff1 + diff2 + diff3, i + 1, d2inx + 1);

            SetValue(ref skill.BaseChance, mw.baseCTB.Text);
            SetValue(ref skill.SboostChance, mw.sBCTB.Text);
            SetValue(ref skill.FullboostChance, mw.fullBCTB.Text);
            SetValue(ref skill.Condition1, mw.cond1TB.Text);
            SetValue(ref skill.Condition2, mw.cond2TB.Text);
            SetValue(ref skill.SubstituteID, mw.substiTB.Text);


            SetValue(ref skill.EarthCost, mw.earthTBSS.Text);
            SetValue(ref skill.WaterCost, mw.waterTBSS.Text);
            SetValue(ref skill.FireCost, mw.fireTBSS.Text);
            SetValue(ref skill.WindCost, mw.windTBSS.Text);
            SetValue(ref skill.TimeCost, mw.timeTBSS.Text);
            SetValue(ref skill.SpaceCost, mw.spaceTBSS.Text);
            SetValue(ref skill.MirageCost, mw.mirageTBSS.Text);


            SetValue(ref skill.Effects[0].ID, mw.eff1TBSS.Text);
            SetValue(ref skill.Effects[0].Param1, mw.effp11TBSS.Text);
            SetValue(ref skill.Effects[0].Param2, mw.effp12TBSS.Text);
            SetValue(ref skill.Effects[0].Param3, mw.effp13TBSS.Text);

            SetValue(ref skill.Effects[1].ID, mw.eff2TBSS.Text);
            SetValue(ref skill.Effects[1].Param1, mw.effp21TBSS.Text);
            SetValue(ref skill.Effects[1].Param2, mw.effp22TBSS.Text);
            SetValue(ref skill.Effects[1].Param3, mw.effp23TBSS.Text);
        }

        private void TextReSetOff(ulong diff, int i, int j)
        {
            if (diff == 0)
            {
                return;
            }

            for (; i < ShardSkills.Length; i++)
            {
                ShardSkills[i].TextOff1 += diff;
                ShardSkills[i].TextOff2 += diff;
                ShardSkills[i].TextOff3 += diff;
                ShardSkills[i].NameOff += diff;
                ShardSkills[i].DescriptionOff1 += diff;
                ShardSkills[i].DescriptionOff2 += diff;
            }

            for (; j < ShardSText.Offsets.Count; j++)
            {
                ShardSText.Offsets[j] += diff;
            }
        }
    }
}
