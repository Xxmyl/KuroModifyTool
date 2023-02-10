using HandyControl.Interactivity;
using System;
using System.Collections.Generic;
using System.Text;

namespace KuroModifyTool.KuroTable
{
    internal class ItemTable : TBLCommon
    {
        public ItemTableData[] Items;

        public class ItemTableData
        {
            [FieldIndexAttr(Index = 0)]
            public uint ID;

            [FieldIndexAttr(Index = 1)]
            public uint CharLimit;

            [FieldIndexAttr(Index = 2)]
            public ulong TextOff1;

            [FieldIndexAttr(Index = 3)]
            public ulong TextOff2;

            [FieldIndexAttr(Index = 4)]
            public byte ItemKind1;

            [FieldIndexAttr(Index = 5)]
            public byte ItemKind2;

            [FieldIndexAttr(Index = 6)]
            public ushort ItemIcon;

            [FieldIndexAttr(Index = 7)]
            public ushort StatusIcon;

            [FieldIndexAttr(Index = 8)]
            public ushort Nature;

            [FieldIndexAttr(Index = 9)]
            public ushort Range;

            [FieldIndexAttr(Index = 10)]
            public ushort Unknown1;

            [FieldIndexAttr(Index = 11)]
            public float Unknown2;

            [FieldIndexAttr(Index = 12)]
            public float Unknown3;

            [FieldIndexAttr(Index = 13)]
            [BinStreamAttr(Length = 5)]
            public Effect[] Effects;

            [FieldIndexAttr(Index = 14)]
            public float Unknown4;

            [FieldIndexAttr(Index = 15)]
            public uint HP;

            [FieldIndexAttr(Index = 16)]
            public uint EP;

            [FieldIndexAttr(Index = 17)]
            public uint PhysicalAttack;

            [FieldIndexAttr(Index = 18)]
            public uint PhysicalDefense;

            [FieldIndexAttr(Index = 19)]
            public uint MagicAttack;

            [FieldIndexAttr(Index = 20)]
            public uint MagicDefense;

            [FieldIndexAttr(Index = 21)]
            public uint STR;

            [FieldIndexAttr(Index = 22)]
            public uint DEF;

            [FieldIndexAttr(Index = 23)]
            public uint AST;

            [FieldIndexAttr(Index = 24)]
            public uint ADF;

            [FieldIndexAttr(Index = 25)]
            public uint AGL;

            [FieldIndexAttr(Index = 26)]
            public uint DEX;

            [FieldIndexAttr(Index = 27)]
            public uint Accuracy;

            [FieldIndexAttr(Index = 28)]
            public uint Dodge;

            [FieldIndexAttr(Index = 29)]
            public uint MagicDodge;

            [FieldIndexAttr(Index = 30)]
            public uint Critical;

            [FieldIndexAttr(Index = 31)]
            public uint SPD;

            [FieldIndexAttr(Index = 32)]
            public uint MOV;

            [FieldIndexAttr(Index = 33)]
            public uint UpperLimit;

            [FieldIndexAttr(Index = 34)]
            public uint Price;

            [FieldIndexAttr(Index = 35)]
            public ulong TextOff3;

            [FieldIndexAttr(Index = 36)]
            public ulong NameOff;

            [FieldIndexAttr(Index = 37)]
            public ulong DescriptionOff;

            [FieldIndexAttr(Index = 38)]
            [BinStreamAttr(Length = 16)]
            public byte[] Data;
        }

        public ItemKindParam2[] KindParams;

        public class ItemKindParam2
        {
            [FieldIndexAttr(Index = 0)]
            public ulong ItemID;

            [FieldIndexAttr(Index = 1)]
            public ulong KindTextOff;
        }

        public ItemTabType[] TabTypes;

        public class ItemTabType
        {
            [FieldIndexAttr(Index = 0)]
            public int Unknown1;

            [FieldIndexAttr(Index = 1)]
            public ulong Unknown2;
        }

        public QuartzParam[] QuartzParams;

        public class QuartzParam
        {
            [FieldIndexAttr(Index = 0)]
            public ushort ItemID;

            [FieldIndexAttr(Index = 1)]
            public ushort EarthCost;

            [FieldIndexAttr(Index = 2)]
            public ushort WaterCost;

            [FieldIndexAttr(Index = 3)]
            public ushort FireCost;

            [FieldIndexAttr(Index = 4)]
            public ushort WindCost;

            [FieldIndexAttr(Index = 5)]
            public ushort TimeCost;

            [FieldIndexAttr(Index = 6)]
            public ushort SpaceCost;

            [FieldIndexAttr(Index = 7)]
            public ushort MirageCost;

            [FieldIndexAttr(Index = 8)]
            public byte EarthAmount;

            [FieldIndexAttr(Index = 9)]
            public byte WaterAmount;

            [FieldIndexAttr(Index = 10)]
            public byte FireAmount;

            [FieldIndexAttr(Index = 11)]
            public byte WindAmount;

            [FieldIndexAttr(Index = 12)]
            public byte TimeAmount;

            [FieldIndexAttr(Index = 13)]
            public byte SpaceAmount;

            [FieldIndexAttr(Index = 14)]
            public byte MirageAmount;

            [FieldIndexAttr(Index = 15)]
            public byte Unknown1;

            [FieldIndexAttr(Index = 16)]
            public uint Unknown2;
        }

        public BottomData Extra;

        public ItemTable() : base("t_item.tbl")
        {
        }

        public override void Load()
        {
            int i = 0;
            byte[] buffer = ReadHeader(StaticField.TBLPath + FileName, ref i);

            if (buffer == null)
            {
                return;
            }

            //ItemTableData
            Items = StaticField.MyBS.GetNode(Nodes, typeof(ItemTableData[]), buffer, ref i);

            //ItemKindParam2
            KindParams = StaticField.MyBS.GetNode(Nodes, typeof(ItemKindParam2[]), buffer, ref i);

            //ItemTabType
            TabTypes = StaticField.MyBS.GetNode(Nodes, typeof(ItemTabType[]), buffer, ref i);

            //QuartzParam
            QuartzParams = StaticField.MyBS.GetNode(Nodes, typeof(QuartzParam[]), buffer, ref i);
            
            Extra = new BottomData(Nodes, "QuartzParam", buffer);

            //DebugLog();
        }
        public void DebugLog()
        {
            FileTools.LogPath = ".\\log.txt";

            Dictionary<uint, string> kinddic = new Dictionary<uint, string>();
            for (int i = 0; i < KindParams.Length; i++)
            {
                kinddic.Add((uint)KindParams[i].ItemID, Extra.GetExtraData((int)KindParams[i].KindTextOff, typeof(string)));
            }

            for (int i = 0; i < Items.Length; i++)
            {
                FileTools.WriteLog(Extra.GetExtraData((int)Items[i].NameOff, typeof(string)));
                FileTools.WriteLog("(");
                FileTools.WriteLog(kinddic[Items[i].ItemKind1]);
                FileTools.WriteLog(",");
                FileTools.WriteLog(kinddic[Items[i].ItemKind2]);
                FileTools.WriteLog(",");
                FileTools.WriteLog(Items[i].ItemIcon.ToString());
                FileTools.WriteLog(",");
                FileTools.WriteLog(Items[i].Nature.ToString());
                FileTools.WriteLog(",");
                FileTools.WriteLog(StaticField.RangeList.Find(od => ushort.Parse(od.ID) == Items[i].Range).Description);
                FileTools.WriteLog(")");
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

            StaticField.MyBS.Serialization(Items, modify);
            StaticField.MyBS.Serialization(KindParams, modify);
            StaticField.MyBS.Serialization(TabTypes, modify);
            StaticField.MyBS.Serialization(QuartzParams, modify);

            modify.AddRange(Extra.ExtraData);

            byte[] data = StaticField.MyBS.CLEPack(modify.ToArray(), StaticField.CurrentCLEF);
            FileTools.BufferToFile(StaticField.TBLPath + FileName, data);
            //FileTools.PackTbl(StaticField.LocalTbl + filename, StaticField.TBLPath + filename);
        }

        public override void DataToUI(MainWindow mw, MainFunc mf, int i)
        {
            ItemTableData item = Items[i];

            mw.nameTB.Text = Extra.GetExtraData((int)item.NameOff, typeof(string));
            mf.ItemRichText = Extra.GetExtraData((int)item.DescriptionOff, typeof(string));

            mw.uplimitTB.Text = item.UpperLimit.ToString();
            mw.priceTB.Text = item.Price.ToString();


            mw.hpTB.Text = item.HP.ToString();
            mw.epTB.Text = item.EP.ToString();
            mw.strTB.Text = item.STR.ToString();
            mw.defTB.Text = item.DEF.ToString();
            mw.atsTB.Text = item.AST.ToString();
            mw.adfTB.Text = item.ADF.ToString();
            mw.aglTB.Text = item.AGL.ToString();
            mw.dexTB.Text = item.DEX.ToString();
            mw.spdTB.Text = item.SPD.ToString();
            mw.movTB.Text = item.MOV.ToString();
            mw.paTB.Text = item.PhysicalAttack.ToString();
            mw.pdTB.Text = item.PhysicalDefense.ToString();
            mw.maTB.Text = item.MagicAttack.ToString();
            mw.mdTB.Text = item.MagicDefense.ToString();
            mw.hitTB.Text = item.Accuracy.ToString();
            mw.dodgeTB.Text = item.Dodge.ToString();
            mw.mdodgeTB.Text = item.MagicDodge.ToString();
            mw.criTB.Text = item.Critical.ToString();


            mw.eff1TB.Text = item.Effects[0].ID.ToString();
            mw.effp11TB.Text = item.Effects[0].Param1.ToString();
            mw.effp12TB.Text = item.Effects[0].Param2.ToString();
            mw.effp13TB.Text = item.Effects[0].Param3.ToString();

            mw.eff2TB.Text = item.Effects[1].ID.ToString();
            mw.effp21TB.Text = item.Effects[1].Param1.ToString();
            mw.effp22TB.Text = item.Effects[1].Param2.ToString();
            mw.effp23TB.Text = item.Effects[1].Param3.ToString();

            mw.eff3TB.Text = item.Effects[2].ID.ToString();
            mw.effp31TB.Text = item.Effects[2].Param1.ToString();
            mw.effp32TB.Text = item.Effects[2].Param2.ToString();
            mw.effp33TB.Text = item.Effects[2].Param3.ToString();

            mw.eff4TB.Text = item.Effects[3].ID.ToString();
            mw.effp41TB.Text = item.Effects[3].Param1.ToString();
            mw.effp42TB.Text = item.Effects[3].Param2.ToString();
            mw.effp43TB.Text = item.Effects[3].Param3.ToString();

            mw.eff5TB.Text = item.Effects[4].ID.ToString();
            mw.effp51TB.Text = item.Effects[4].Param1.ToString();
            mw.effp52TB.Text = item.Effects[4].Param2.ToString();
            mw.effp53TB.Text = item.Effects[4].Param3.ToString();

            QuartzParam quartz = Array.Find<QuartzParam>(QuartzParams, qp => qp.ItemID == item.ID);

            if (quartz == null)
            {
                return;
            }

            mw.earthTB.Text = quartz.EarthAmount.ToString();
            mw.waterTB.Text = quartz.WaterAmount.ToString();
            mw.fireTB.Text = quartz.FireAmount.ToString();
            mw.windTB.Text = quartz.WindAmount.ToString();
            mw.timeTB.Text = quartz.TimeAmount.ToString();
            mw.spaceTB.Text = quartz.SpaceAmount.ToString();
            mw.mirageTB.Text = quartz.MirageAmount.ToString();
        }

        public override void UIToData(MainWindow mw, MainFunc mf, int i)
        {
            ItemTableData item = Items[i];

            string namel = Extra.GetExtraData((int)item.NameOff, typeof(string));
            string desc1l = Extra.GetExtraData((int)item.DescriptionOff, typeof(string));

            string name = SetValue(namel, mw.nameTB.Text);
            string desc1 = SetValue(desc1l, mf.ItemRichText);

            ulong diff1 = StaticField.MyBS.GetStringDiff(namel, name);
            ulong diff2 = StaticField.MyBS.GetStringDiff(desc1l, desc1);

            item.DescriptionOff += diff1;

            Extra.SetExtraData((int)item.NameOff, namel, SetValue(namel, name));
            Extra.SetExtraData((int)item.DescriptionOff, desc1l, desc1);

            TextReSetOff(diff1 + diff2, i + 1);


            SetValue(ref item.UpperLimit, mw.uplimitTB.Text);
            SetValue(ref item.Price, mw.priceTB.Text);


            SetValue(ref item.HP, mw.hpTB.Text);
            SetValue(ref item.EP, mw.epTB.Text);
            SetValue(ref item.STR, mw.strTB.Text);
            SetValue(ref item.DEF, mw.defTB.Text);
            SetValue(ref item.AST, mw.atsTB.Text);
            SetValue(ref item.ADF, mw.adfTB.Text);
            SetValue(ref item.AGL, mw.aglTB.Text);
            SetValue(ref item.DEX, mw.dexTB.Text);
            SetValue(ref item.SPD, mw.spdTB.Text);
            SetValue(ref item.MOV, mw.movTB.Text);
            SetValue(ref item.PhysicalAttack, mw.paTB.Text);
            SetValue(ref item.PhysicalDefense, mw.pdTB.Text);
            SetValue(ref item.MagicAttack, mw.maTB.Text);
            SetValue(ref item.MagicDefense, mw.mdTB.Text);
            SetValue(ref item.Accuracy, mw.hitTB.Text);
            SetValue(ref item.Dodge, mw.dodgeTB.Text);
            SetValue(ref item.MagicDodge, mw.mdodgeTB.Text);
            SetValue(ref item.Critical, mw.criTB.Text);

            
            SetValue(ref item.Effects[0].ID, mw.eff1TB.Text);
            SetValue(ref item.Effects[0].Param1, mw.effp11TB.Text);
            SetValue(ref item.Effects[0].Param2, mw.effp12TB.Text);
            SetValue(ref item.Effects[0].Param3, mw.effp13TB.Text);

            SetValue(ref item.Effects[1].ID, mw.eff2TB.Text);
            SetValue(ref item.Effects[1].Param1, mw.effp21TB.Text);
            SetValue(ref item.Effects[1].Param2, mw.effp22TB.Text);
            SetValue(ref item.Effects[1].Param3, mw.effp23TB.Text);

            SetValue(ref item.Effects[2].ID, mw.eff3TB.Text);
            SetValue(ref item.Effects[2].Param1, mw.effp31TB.Text);
            SetValue(ref item.Effects[2].Param2, mw.effp32TB.Text);
            SetValue(ref item.Effects[2].Param3, mw.effp33TB.Text);

            SetValue(ref item.Effects[3].ID, mw.eff4TB.Text);
            SetValue(ref item.Effects[3].Param1, mw.effp41TB.Text);
            SetValue(ref item.Effects[3].Param2, mw.effp42TB.Text);
            SetValue(ref item.Effects[3].Param3, mw.effp43TB.Text);

            SetValue(ref item.Effects[4].ID, mw.eff5TB.Text);
            SetValue(ref item.Effects[4].Param1, mw.effp51TB.Text);
            SetValue(ref item.Effects[4].Param2, mw.effp52TB.Text);
            SetValue(ref item.Effects[4].Param3, mw.effp53TB.Text);

            QuartzParam quartz = Array.Find<QuartzParam>(QuartzParams, qp => qp.ItemID == item.ID);

            if (quartz == null)
            {
                return;
            }

            SetValue(ref quartz.EarthAmount, mw.earthTB.Text);
            SetValue(ref quartz.WaterAmount, mw.waterTB.Text);
            SetValue(ref quartz.FireAmount, mw.fireTB.Text);
            SetValue(ref quartz.WindAmount, mw.windTB.Text);
            SetValue(ref quartz.TimeAmount, mw.timeTB.Text);
            SetValue(ref quartz.SpaceAmount, mw.spaceTB.Text);
            SetValue(ref quartz.MirageAmount, mw.mirageTB.Text);
        }

        private void TextReSetOff(ulong diff, int i)
        {
            if (diff == 0)
            {
                return;
            }

            for (; i < Items.Length; i++)
            {
                Items[i].TextOff1 += diff;
                Items[i].TextOff2 += diff;
                Items[i].TextOff3 += diff;
                Items[i].NameOff += diff;
                Items[i].DescriptionOff += diff;
            }

            for (i = 0; i < KindParams.Length; i++)
            {
                KindParams[i].KindTextOff += diff;
            }
        }
    }
}
