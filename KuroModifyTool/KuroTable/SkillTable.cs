using System.Collections.Generic;
using System.Text;

namespace KuroModifyTool.KuroTable
{
    internal class SkillTable : TBLCommon
    {
        public SkillParam[] Skills;

        public class SkillParam
        {
            [FieldIndexAttr(Index = 0)]
            public ushort ID;

            [FieldIndexAttr(Index = 1)]
            public uint CharLimit;

            [FieldIndexAttr(Index = 2)]
            public short Unknown1;

            [FieldIndexAttr(Index = 3)]
            public ulong TextOff1;

            [FieldIndexAttr(Index = 4)]
            public byte Category;

            [FieldIndexAttr(Index = 5)]
            public byte Nature;

            [FieldIndexAttr(Index = 6)]
            public int Unknown2;

            [FieldIndexAttr(Index = 7)]
            public short Unknown3;

            [FieldIndexAttr(Index = 8)]
            public ulong TextOff2;

            [FieldIndexAttr(Index = 9)]
            public uint RangeType;

            [FieldIndexAttr(Index = 10)]
            public float RangeParam1;

            [FieldIndexAttr(Index = 11)]
            public float RangeParam2;

            [FieldIndexAttr(Index = 12)]
            public float RangeParam3;

            [FieldIndexAttr(Index = 13)]
            [BinStreamAttr(Length = 5)]
            public Effect[] Effects;

            [FieldIndexAttr(Index = 14)]
            public float Unknown4;

            [FieldIndexAttr(Index = 15)]
            public ushort Drive;

            [FieldIndexAttr(Index = 16)]
            public ushort Stiff;

            [FieldIndexAttr(Index = 17)]
            public ushort Cost;

            [FieldIndexAttr(Index = 18)]
            public short LearnLevel;

            [FieldIndexAttr(Index = 19)]
            public ushort SortID;

            [FieldIndexAttr(Index = 20)]
            public short Unknown5;

            [FieldIndexAttr(Index = 21)]
            public ulong TextOff3;

            [FieldIndexAttr(Index = 22)]
            public ulong NameOff;

            [FieldIndexAttr(Index = 23)]
            public ulong DescriptionOff1;

            [FieldIndexAttr(Index = 24)]
            public ulong DescriptionOff2;
        }

        public SkillGetParam[] GetParams;

        public class SkillGetParam
        {
            [FieldIndexAttr(Index = 0)]
            public ushort CharID;

            [FieldIndexAttr(Index = 1)]
            public ushort SkillID1;

            [FieldIndexAttr(Index = 2)]
            public ushort SkillID2;
        }

        public SkillChangeParam[] ChangeParams;

        public class SkillChangeParam
        {
            [FieldIndexAttr(Index = 0)]
            public ushort CharID;

            [FieldIndexAttr(Index = 1)]
            public short Unknown;

            [FieldIndexAttr(Index = 2)]
            public ushort SkillID1;

            [FieldIndexAttr(Index = 3)]
            public ushort SkillID2;
        }

        public SkillGrendelParam[] GrendelParams;

        public class SkillGrendelParam
        {
            [FieldIndexAttr(Index = 0)]
            public ushort CharID;

            [FieldIndexAttr(Index = 1)]
            public ushort GrendelCharID;
        }

        public TextData SkillText;

        private readonly string filename = "t_skill.tbl";

        public SkillTable()
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

            //SkillParam
            Skills = StaticField.MyBS.GetNode(Nodes, "SkillParam", typeof(SkillParam[]), buffer, ref i);

            //SkillGetParam
            GetParams = StaticField.MyBS.GetNode(Nodes, "SkillGetParam", typeof(SkillGetParam[]), buffer, ref i);

            //SkillChangeParam
            ChangeParams = StaticField.MyBS.GetNode(Nodes, "SkillChangeParam", typeof(SkillChangeParam[]), buffer, ref i);

            //SkillGrendelParam
            GrendelParams = StaticField.MyBS.GetNode(Nodes, "SkillGrendelParam", typeof(SkillGrendelParam[]), buffer, ref i);


            SkillText = new TextData(TextData.GetTextStartOff(Nodes, "SkillGrendelParam"), buffer.Length);
            StaticField.MyBS.GetTextData(buffer, SkillText);
            //DebugLog();
        }

        public override void Save()
        {
            List<byte> modify = new List<byte>();

            modify.AddRange(Encoding.UTF8.GetBytes(Flag));

            StaticField.MyBS.Serialization(SHLength, modify);

            StaticField.MyBS.Serialization(Nodes, modify);

            StaticField.MyBS.Serialization(Skills, modify);
            StaticField.MyBS.Serialization(GetParams, modify);
            StaticField.MyBS.Serialization(ChangeParams, modify);
            StaticField.MyBS.Serialization(GrendelParams, modify);

            StaticField.MyBS.SetTextData(modify, SkillText);

            FileTools.BufferToFile(StaticField.TBLPath + filename, modify.ToArray());
        }

        public void DebugLog()
        {
            FileTools.LogPath = ".\\log.txt";

            for (int i = 0; i < Skills.Length; i++)
            {
                FileTools.WriteLog(Skills[i].ID.ToString());
                FileTools.WriteLog(":");
                int d1inx = SkillText.Offsets.FindIndex(o => o == Skills[i].NameOff);
                FileTools.WriteLog(SkillText.Texts[d1inx]);

                

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

        public override void DataToUI(MainWindow mw, MainFunc mf, int i)
        {
            SkillParam skill = Skills[i];

            int ninx = SkillText.Offsets.FindIndex(o => o == skill.NameOff);
            int d1inx = SkillText.Offsets.FindIndex(o => o == skill.DescriptionOff1);
            int d2inx = SkillText.Offsets.FindIndex(o => o == skill.DescriptionOff2);
            mw.nameTBS.Text = SkillText.Texts[ninx];
            mf.Skill1RichText = SkillText.Texts[d1inx];
            mf.Skill2RichText = SkillText.Texts[d2inx];

            mw.rangeCB.SelectedIndex = GetRangeType(skill);
            mw.rangep1TB.Text = skill.RangeParam1.ToString();
            mw.rangep2TB.Text = skill.RangeParam2.ToString();
            mw.rangep3TB.Text = skill.RangeParam3.ToString();
            mw.driveTB.Text = skill.Drive.ToString();
            mw.stiffTB.Text = skill.Stiff.ToString();
            mw.costTB.Text = skill.Cost.ToString();
            mw.learnlTB.Text = skill.LearnLevel.ToString();


            mw.eff1TBS.Text = skill.Effects[0].ID.ToString();
            mw.effp11TBS.Text = skill.Effects[0].Param1.ToString();
            mw.effp12TBS.Text = skill.Effects[0].Param2.ToString();
            mw.effp13TBS.Text = skill.Effects[0].Param3.ToString();

            mw.eff2TBS.Text = skill.Effects[1].ID.ToString();
            mw.effp21TBS.Text = skill.Effects[1].Param1.ToString();
            mw.effp22TBS.Text = skill.Effects[1].Param2.ToString();
            mw.effp23TBS.Text = skill.Effects[1].Param3.ToString();

            mw.eff3TBS.Text = skill.Effects[2].ID.ToString();
            mw.effp31TBS.Text = skill.Effects[2].Param1.ToString();
            mw.effp32TBS.Text = skill.Effects[2].Param2.ToString();
            mw.effp33TBS.Text = skill.Effects[2].Param3.ToString();

            mw.eff4TBS.Text = skill.Effects[3].ID.ToString();
            mw.effp41TBS.Text = skill.Effects[3].Param1.ToString();
            mw.effp42TBS.Text = skill.Effects[3].Param2.ToString();
            mw.effp43TBS.Text = skill.Effects[3].Param3.ToString();

            mw.eff5TBS.Text = skill.Effects[4].ID.ToString();
            mw.effp51TBS.Text = skill.Effects[4].Param1.ToString();
            mw.effp52TBS.Text = skill.Effects[4].Param2.ToString();
            mw.effp53TBS.Text = skill.Effects[4].Param3.ToString();
        }

        private int GetRangeType(SkillParam skill)
        {
            for (int i = 0; i < StaticField.RangeList.Count; i++)
            {
                OtherDesc desc = StaticField.RangeList[i];
                if (desc.ID.Equals(skill.RangeType.ToString()))
                {
                    if (!desc.ID.Equals("257") && !desc.ID.Equals("4388"))
                    {
                        return i;
                    }


                    if (desc.Description.Equals("自身") && skill.RangeParam1 == 0)
                    {
                        return i;
                    }
                    else if (desc.Description.Equals("单体") && skill.RangeParam1 != 0)
                    {
                        return i;
                    }
                    else if (desc.Description.Equals("自身+圆") && skill.RangeParam3 == 0)
                    {
                        return i;
                    }
                    else if (desc.Description.Equals("扇形") && skill.RangeParam3 != 0)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        public override void UIToData(MainWindow mw, MainFunc mf, int i)
        {
            SkillParam skill = Skills[i];

            string name = mw.nameTBS.Text;
            string desc1 = mf.Skill1RichText;
            string desc2 = mf.Skill2RichText;

            int ninx = SkillText.Offsets.FindIndex(o => o == skill.NameOff);
            int d1inx = SkillText.Offsets.FindIndex(o => o == skill.DescriptionOff1);
            int d2inx = SkillText.Offsets.FindIndex(o => o == skill.DescriptionOff2);

            ulong diff1 = StaticField.MyBS.GetStringDiff(SkillText.Texts[ninx], name);
            ulong diff2 = StaticField.MyBS.GetStringDiff(SkillText.Texts[d1inx], desc1);
            ulong diff3 = StaticField.MyBS.GetStringDiff(SkillText.Texts[d2inx], desc2);

            SkillText.Texts[ninx] = SetValue(SkillText.Texts[ninx], name);
            SkillText.Texts[d1inx] = SetValue(SkillText.Texts[d1inx], desc1);
            SkillText.Texts[d2inx] = SetValue(SkillText.Texts[d2inx], desc2);

            skill.DescriptionOff1 += diff1;
            SkillText.Offsets[d1inx] += diff1;
            skill.DescriptionOff2 += diff1 + diff2;
            SkillText.Offsets[d2inx] += diff1 + diff2;

            TextReSetOff(diff1 + diff2 + diff3, i + 1, d2inx + 1);

            if(mw.rangeCB.SelectedIndex != -1)
            {
                SetValue(ref skill.RangeType, StaticField.RangeList[mw.rangeCB.SelectedIndex].ID);
            }
            
            SetValue(ref skill.RangeParam1, mw.rangep1TB.Text);
            SetValue(ref skill.RangeParam2, mw.rangep2TB.Text);
            SetValue(ref skill.RangeParam3, mw.rangep3TB.Text);
            SetValue(ref skill.Drive, mw.driveTB.Text);
            SetValue(ref skill.Stiff, mw.stiffTB.Text);
            SetValue(ref skill.Cost, mw.costTB.Text);
            SetValue(ref skill.LearnLevel, mw.learnlTB.Text);


            SetValue(ref skill.Effects[0].ID, mw.eff1TBS.Text);
            SetValue(ref skill.Effects[0].Param1, mw.effp11TBS.Text);
            SetValue(ref skill.Effects[0].Param2, mw.effp12TBS.Text);
            SetValue(ref skill.Effects[0].Param3, mw.effp13TBS.Text);

            SetValue(ref skill.Effects[1].ID, mw.eff2TBS.Text);
            SetValue(ref skill.Effects[1].Param1, mw.effp21TBS.Text);
            SetValue(ref skill.Effects[1].Param2, mw.effp22TBS.Text);
            SetValue(ref skill.Effects[1].Param3, mw.effp23TBS.Text);

            SetValue(ref skill.Effects[2].ID, mw.eff3TBS.Text);
            SetValue(ref skill.Effects[2].Param1, mw.effp31TBS.Text);
            SetValue(ref skill.Effects[2].Param2, mw.effp32TBS.Text);
            SetValue(ref skill.Effects[2].Param3, mw.effp33TBS.Text);

            SetValue(ref skill.Effects[3].ID, mw.eff4TBS.Text);
            SetValue(ref skill.Effects[3].Param1, mw.effp41TBS.Text);
            SetValue(ref skill.Effects[3].Param2, mw.effp42TBS.Text);
            SetValue(ref skill.Effects[3].Param3, mw.effp43TBS.Text);

            SetValue(ref skill.Effects[4].ID, mw.eff5TBS.Text);
            SetValue(ref skill.Effects[4].Param1, mw.effp51TBS.Text);
            SetValue(ref skill.Effects[4].Param2, mw.effp52TBS.Text);
            SetValue(ref skill.Effects[4].Param3, mw.effp53TBS.Text);
        }

        private void TextReSetOff(ulong diff, int i, int j)
        {
            if (diff == 0)
            {
                return;
            }

            for (; i < Skills.Length; i++)
            {
                Skills[i].TextOff1 += diff;
                Skills[i].TextOff2 += diff;
                Skills[i].TextOff3 += diff;
                Skills[i].NameOff += diff;
                Skills[i].DescriptionOff1 += diff;
                Skills[i].DescriptionOff2 += diff;
            }

            for (; j < SkillText.Offsets.Count; j++)
            {
                SkillText.Offsets[j] += diff;
            }
        }
    }
}
