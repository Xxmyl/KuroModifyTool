using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuroModifyTool.KuroTable
{
    internal class ArtsDriverTable : TBLCommon
    {
        public SlotOpenRate[] SlotOpenRates;
        public class SlotOpenRate
        {
            [FieldIndexAttr(Index = 0)]
            [BinStreamAttr(Length = 6)]
            public ushort[] Rates;
        }

        public DriverBaseTableData[] BaseTableDatas;

        public class DriverBaseTableData
        {
            [FieldIndexAttr(Index = 0)]
            public uint ItemID;

            [FieldIndexAttr(Index = 1)]
            public ushort Nature1;

            [FieldIndexAttr(Index = 2)]
            public ushort Unknown1;

            [FieldIndexAttr(Index = 3)]
            public ushort Nature2;

            [FieldIndexAttr(Index = 4)]
            public ushort CustomSolt;

            [FieldIndexAttr(Index = 5)]
            public ushort FixedSolt;

            [FieldIndexAttr(Index = 6)]
            public ushort SumSolt;
        }

        public DriverArtsTableData[] ArtsTableDatas;

        public class DriverArtsTableData
        {
            [FieldIndexAttr(Index = 0)]
            public uint ItemID;

            [FieldIndexAttr(Index = 1)]
            public ushort SoltNum;

            [FieldIndexAttr(Index = 2)]
            public ushort LockSoltLevel;

            [FieldIndexAttr(Index = 3)]
            public uint SkillID;
        }

        public ArtsDriverTable() : base("t_artsdriver.tbl")
        {
        }

        public override void Load()
        {
            int i = 0;
            byte[] buffer = ReadHeader(StaticField.TBLPath1 + FileName, ref i);

            if (buffer == null)
            {
                return;
            }

            SlotOpenRates = StaticField.MyBS.GetNode(Nodes, typeof(SlotOpenRate[]), buffer, ref i);
            BaseTableDatas = StaticField.MyBS.GetNode(Nodes, typeof(DriverBaseTableData[]), buffer, ref i);
            ArtsTableDatas = StaticField.MyBS.GetNode(Nodes, typeof(DriverArtsTableData[]), buffer, ref i);
        }

        public override void Save()
        {
            List<byte> modify = new List<byte>();

            modify.AddRange(Encoding.UTF8.GetBytes(Flag));

            StaticField.MyBS.Serialization(SHLength, modify);

            StaticField.MyBS.Serialization(Nodes, modify);

            StaticField.MyBS.Serialization(SlotOpenRates, modify);

            List<byte> subModify = new List<byte>();
            StaticField.MyBS.Serialization(BaseTableDatas, subModify);
            StaticField.MyBS.Serialization(ArtsTableDatas, subModify);

            for(int i = 0; i < 51; i++)
            {
                modify.AddRange(subModify);
            }

            byte[] data = StaticField.MyBS.CLEPack(modify.ToArray(), StaticField.CurrentCLEF);
            FileTools.BufferToFile(StaticField.TBLPath1 + FileName, data);
            //FileTools.PackTbl(StaticField.LocalTbl + filename, StaticField.TBLPath1 + filename);
        }

        public override void DataToUI(MainWindow mw, MainFunc mf, int i)
        {
            DriverBaseTableData ad = BaseTableDatas[i];

            ItemTable.ItemTableData item = Array.Find(mf.itemTable.Items, it => it.ID == ad.ItemID);
            if (item == null)
            {
                mw.nameTBAD.Text = "不在item中：" + ad.ItemID.ToString();
            }
            else
            {
                mw.nameTBAD.Text = mf.itemTable.Extra.GetExtraData((int)item.NameOff, typeof(string));
            }

            mw.fixCBAD.SelectedIndex = ad.FixedSolt;
            mw.cusCBAD.SelectedIndex = ad.CustomSolt;
            mw.sumCBAD.SelectedIndex = ad.SumSolt;

            for(int j = 0; j < 8; j++)
            {
                int sinx = StaticField.SkillDic.FindIndex(sd => sd.ID == ArtsTableDatas[i * 8 + j].SkillID.ToString());
                ArtsDriverUIFunc.SkillCBList[j].SelectedIndex = sinx;
                ArtsDriverUIFunc.LockCBList[j].SelectedIndex = ArtsTableDatas[i * 8 + j].LockSoltLevel;
            }
        }

        public override void UIToData(MainWindow mw, MainFunc mf, int i)
        {
            DriverBaseTableData ad = BaseTableDatas[i];

            SetValue(ref ad.FixedSolt, mw.fixCBAD.Text);
            SetValue(ref ad.CustomSolt, mw.cusCBAD.Text);
            SetValue(ref ad.SumSolt, mw.sumCBAD.Text);

            for (int j = 0; j < 8; j++)
            {
                if(ArtsDriverUIFunc.SkillCBList[j].SelectedIndex != -1)
                {
                    SetValue(ref ArtsTableDatas[i * 8 + j].SkillID, StaticField.SkillDic[ArtsDriverUIFunc.SkillCBList[j].SelectedIndex].ID);
                }

                SetValue(ref ArtsTableDatas[i * 8 + j].LockSoltLevel, ArtsDriverUIFunc.LockCBList[j].Text);
            }
        }
    }
}
