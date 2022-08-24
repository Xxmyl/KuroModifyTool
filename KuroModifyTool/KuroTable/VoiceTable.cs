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

        private readonly string filename = "t_voice.tbl";

        public VoiceTable()
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

            FileTools.BufferToFile(StaticField.TBLPath + filename, modify.ToArray());
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

            string text = mw.textTBV.Text;

            string textl = Extra.GetExtraData((int)v.TextOff, typeof(string));

            ulong diff1 = StaticField.MyBS.GetStringDiff(textl, text);

            Extra.SetExtraData((int)v.TextOff, textl, SetValue(textl, text));

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
