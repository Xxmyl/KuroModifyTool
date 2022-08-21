using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuroModifyTool.KuroTable
{
    internal class ShopTable : TBLCommon
    {
        public ShopInfo[] Infos;

        public class ShopInfo
        {
            [FieldIndexAttr(Index = 0)]
            public ulong ID;

            [FieldIndexAttr(Index = 1)]
            public ulong NameOff;

            [FieldIndexAttr(Index = 2)]
            public ulong Unknown1;

            [FieldIndexAttr(Index = 3)]
            public ulong TextOff1;

            [FieldIndexAttr(Index = 4)]
            public ushort Empty1;

            [FieldIndexAttr(Index = 5)]
            public short Unknown2;

            [FieldIndexAttr(Index = 6)]
            public float Unknown3;

            [FieldIndexAttr(Index = 7)]
            public float Unknown4;

            [FieldIndexAttr(Index = 8)]
            public float Unknown5;

            [FieldIndexAttr(Index = 9)]
            public float Unknown6;

            [FieldIndexAttr(Index = 10)]
            public float Unknown7;

            [FieldIndexAttr(Index = 11)]
            public float Unknown8;

            [FieldIndexAttr(Index = 12)]
            public float Unknown9;

            [FieldIndexAttr(Index = 14)]
            public int Unknown10;

            [FieldIndexAttr(Index = 15)]
            public int Unknown11;

            [FieldIndexAttr(Index = 16)]
            public int Unknown12;

            [FieldIndexAttr(Index = 17)]
            public int Unknown13;
        }

        public ShopItem[] ShopItems;

        public class ShopItem
        {
            [FieldIndexAttr(Index = 0)]
            public ushort ID;

            [FieldIndexAttr(Index = 1)]
            public short ItemID;

            [FieldIndexAttr(Index = 2)]
            public int Unknown1;

            [FieldIndexAttr(Index = 3)]
            public ulong Off1;

            [FieldIndexAttr(Index = 4)]
            public int Unknown2;

            [FieldIndexAttr(Index = 5)]
            public int Unknown3;

            [FieldIndexAttr(Index = 6)]
            public ulong Off2;

            [FieldIndexAttr(Index = 7)]
            public int Unknown4;

            [FieldIndexAttr(Index = 8)]
            public int Unknown5;
        }

        public ShopTypeDesc[] TypeDescs;

        public class ShopTypeDesc
        {
            [FieldIndexAttr(Index = 0)]
            public ulong ID;

            [FieldIndexAttr(Index = 1)]
            public ulong Off1;

            [FieldIndexAttr(Index = 2)]
            public byte Unknown1;

            [FieldIndexAttr(Index = 3)]
            public byte Unknown2;

            [FieldIndexAttr(Index = 4)]
            public byte Unknown3;

            [FieldIndexAttr(Index = 5)]
            public byte Unknown4;

            [FieldIndexAttr(Index = 6)]
            public byte Unknown5;

            [FieldIndexAttr(Index = 7)]
            public byte Unknown6;

            [FieldIndexAttr(Index = 8)]
            public byte Unknown7;

            [FieldIndexAttr(Index = 9)]
            public byte Unknown8;
        }

        public ShopConv[] Convs;

        public class ShopConv
        {
            [FieldIndexAttr(Index = 0)]
            public uint ID;

            [FieldIndexAttr(Index = 1)]
            public float Unknown1;

            [FieldIndexAttr(Index = 2)]
            public float Unknown2;

            [FieldIndexAttr(Index = 3)]
            public float Unknown3;

            [FieldIndexAttr(Index = 4)]
            public float Unknown4;

            [FieldIndexAttr(Index = 5)]
            public float Unknown5;

            [FieldIndexAttr(Index = 6)]
            public float Unknown6;

            [FieldIndexAttr(Index = 7)]
            public float Unknown7;

            [FieldIndexAttr(Index = 8)]
            public float Unknown8;
        }

        public TradeItem[] TradeItems;

        public class TradeItem
        {
            [FieldIndexAttr(Index = 0)]
            public uint OfferedItemID;

            [FieldIndexAttr(Index = 1)]
            [BinStreamAttr(Length = 6)]
            public ShopEffect[] Effects;
        }

        public TextData ShopText;

        private readonly string filename = "t_shop.tbl";
        //Debug
        private ItemTable TestItem;

        public ShopTable(ItemTable item)
        {
            TestItem = item;
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

            
            Infos = StaticField.MyBS.GetNode(Nodes, typeof(ShopInfo[]), buffer, ref i);
            ShopItems = StaticField.MyBS.GetNode(Nodes, typeof(ShopItem[]), buffer, ref i);
            TypeDescs = StaticField.MyBS.GetNode(Nodes, typeof(ShopTypeDesc[]), buffer, ref i);
            Convs = StaticField.MyBS.GetNode(Nodes, typeof(ShopConv[]), buffer, ref i);
            TradeItems = StaticField.MyBS.GetNode(Nodes, typeof(TradeItem[]), buffer, ref i);

            ShopText = new TextData(TextData.GetTextStartOff(Nodes, "TradeItem"), (int)ShopItems.First().Off1);
            StaticField.MyBS.GetTextData(buffer, ShopText);
            DebugLog();
        }

        public void DebugLog()
        {
            FileTools.LogPath = ".\\log.txt";

            for (int i = 0; i < ShopItems.Length; i++)
            {
                /*FileTools.WriteLog(Infos[i].ID.ToString());
                FileTools.WriteLog(":");
                int d1inx = ShopText.Offsets.FindIndex(o => o == Infos[i].NameOff);
                FileTools.WriteLog(ShopText.Texts[d1inx]);
                FileTools.WriteLog(",");
                FileTools.WriteLog(Infos[i].Unknown2.ToString());
                FileTools.WriteLog(",");
                FileTools.WriteLog(Infos[i].Unknown3.ToString());
                FileTools.WriteLog(",");
                FileTools.WriteLog(Infos[i].Unknown4.ToString());
                FileTools.WriteLog(",");
                FileTools.WriteLog(Infos[i].Unknown5.ToString());
                FileTools.WriteLog(",");
                FileTools.WriteLog(Infos[i].Unknown6.ToString());
                FileTools.WriteLog(",");
                FileTools.WriteLog(Infos[i].Unknown7.ToString());
                FileTools.WriteLog(",");
                FileTools.WriteLog(Infos[i].Unknown8.ToString());
                FileTools.WriteLog(",");
                FileTools.WriteLog(Infos[i].Unknown9.ToString());
                FileTools.WriteLog(",");
                FileTools.WriteLog(Infos[i].Unknown10.ToString());
                FileTools.WriteLog(",");
                FileTools.WriteLog(Infos[i].Unknown11.ToString());
                FileTools.WriteLog(",");
                FileTools.WriteLog(Infos[i].Unknown12.ToString());
                FileTools.WriteLog(",");
                FileTools.WriteLog(Infos[i].Unknown13.ToString());*/

                FileTools.WriteLog(ShopItems[i].ID.ToString());
                FileTools.WriteLog(":");
                ItemTable.ItemTableData itemf = Array.Find(TestItem.Items, it => it.ID == ShopItems[i].ItemID);
                int dinx = TestItem.ItemText.Offsets.FindIndex(o => o == itemf.NameOff);
                FileTools.WriteLog(TestItem.ItemText.Texts[dinx]);
                FileTools.WriteLog(",");
                /*int d1inx = ShopText.Offsets.FindIndex(o => o == Infos[i].NameOff);
                FileTools.WriteLog(ShopText.Texts[d1inx]);
                FileTools.WriteLog(",");*/
                FileTools.WriteLog(ShopItems[i].Unknown1.ToString());
                FileTools.WriteLog(",");
                FileTools.WriteLog(ShopItems[i].Unknown2.ToString());
                FileTools.WriteLog(",");
                FileTools.WriteLog(ShopItems[i].Unknown3.ToString());
                FileTools.WriteLog(",");
                FileTools.WriteLog(ShopItems[i].Unknown4.ToString());
                FileTools.WriteLog(",");
                FileTools.WriteLog(ShopItems[i].Unknown5.ToString());
                /*FileTools.WriteLog(",");
                FileTools.WriteLog(Convs[i].Unknown6.ToString());
                FileTools.WriteLog(",");
                FileTools.WriteLog(Convs[i].Unknown7.ToString());
                FileTools.WriteLog(",");
                FileTools.WriteLog(Convs[i].Unknown8.ToString());*/

                /*for (int j = 0; j < TradeItems[i].Effects.Length; j++)
                {
                    
                    ItemTable.ItemTableData item = Array.Find(TestItem.Items, it => it.ID == TradeItems[i].Effects[j].TradeItemID);
                    if(item == null)
                    {
                        continue;
                    }
                    int d1inx = TestItem.ItemText.Offsets.FindIndex(o => o == item.NameOff);
                    FileTools.WriteLog("(");
                    FileTools.WriteLog(TestItem.ItemText.Texts[d1inx]);
                    //FileTools.WriteLog(TradeItems[i].Effects[j].TradeItemID.ToString());
                    FileTools.WriteLog(",");
                    FileTools.WriteLog(TradeItems[i].Effects[j].RequirAmount.ToString());
                    FileTools.WriteLog(")");
                }*/
                FileTools.WriteLog("\n");
            }

            FileTools.CloseLog();
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override void DataToUI(MainWindow mw, MainFunc mf, int i)
        {
            throw new NotImplementedException();
        }

        public override void UIToData(MainWindow mw, MainFunc mf, int i)
        {
            throw new NotImplementedException();
        }
    }
}
