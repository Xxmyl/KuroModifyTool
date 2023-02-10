using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuroModifyTool.KuroTable
{
    internal class VoiceTable : TBLCommon
    {
        public VoiceTableData[] Voices;

        public class VoiceTableData
        {
            [FieldIndexAttr(Index = 0)]
            public uint ID;

            [FieldIndexAttr(Index = 1)]
            public int Unknown1;

            [FieldIndexAttr(Index = 2)]
            public ulong FileNameOff;

            [FieldIndexAttr(Index = 3)]
            public int Unknown2;

            [FieldIndexAttr(Index = 4)]
            public float Unknown3;

            [FieldIndexAttr(Index = 5)]
            public int Unknown4;

            [FieldIndexAttr(Index = 6)]
            public float Unknown5;

            [FieldIndexAttr(Index = 7)]
            public ulong TextOff;
        }

        public BottomData Extra;

        public VoiceTable() : base("t_voice.tbl")
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


            Voices = StaticField.MyBS.GetNode(Nodes, typeof(VoiceTableData[]), buffer, ref i);

            Extra = new BottomData(Nodes, "VoiceTableData", buffer);
        }

        public override void Save()
        {
            List<byte> modify = new List<byte>();

            modify.AddRange(Encoding.UTF8.GetBytes(Flag));

            StaticField.MyBS.Serialization(SHLength, modify);

            StaticField.MyBS.Serialization(Nodes, modify);

            StaticField.MyBS.Serialization(Voices, modify);

            modify.AddRange(Extra.ExtraData);

            byte[] data = StaticField.MyBS.CLEPack(modify.ToArray(), StaticField.CurrentCLEF);
            FileTools.BufferToFile(StaticField.TBLPath1 + FileName, data);
            //FileTools.PackTbl(StaticField.LocalTbl + filename, StaticField.TBLPath1 + filename);
        }

        public override void DataToUI(MainWindow mw, MainFunc mf, int i)
        {
            VoiceTableData v = Voices[i];

            mw.nameTBV.Text = Extra.GetExtraData((int)v.FileNameOff, typeof(string));
            mw.textTBV.Text = Extra.GetExtraData((int)v.TextOff, typeof(string));
        }

        public override void UIToData(MainWindow mw, MainFunc mf, int i)
        {
            VoiceTableData v = Voices[i];

            string textl = Extra.GetExtraData((int)v.TextOff, typeof(string));

            string text = SetValue(textl, mw.textTBV.Text);

            ulong diff1 = StaticField.MyBS.GetStringDiff(textl, text);

            Extra.SetExtraData((int)v.TextOff, textl, text);

            TextReSetOff(diff1, i + 1);
        }

        private void TextReSetOff(ulong diff, int i)
        {
            if (diff == 0)
            {
                return;
            }

            for (; i < Voices.Length; i++)
            {
                Voices[i].FileNameOff += diff;
                Voices[i].TextOff += diff;
            }
        }
    }
}
