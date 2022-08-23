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

        public TextData VoiceText;

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

            VoiceText = new TextData(TextData.GetTextStartOff(Nodes, "VoiceTableData"), buffer.Length);
            StaticField.MyBS.GetTextData(buffer, VoiceText);
        }

        public override void Save()
        {
            List<byte> modify = new List<byte>();

            modify.AddRange(Encoding.UTF8.GetBytes(Flag));

            StaticField.MyBS.Serialization(SHLength, modify);

            StaticField.MyBS.Serialization(Nodes, modify);

            StaticField.MyBS.Serialization(Voices, modify);

            StaticField.MyBS.SetTextData(modify, VoiceText);

            FileTools.BufferToFile(StaticField.TBLPath + filename, modify.ToArray());
        }

        public override void DataToUI(MainWindow mw, MainFunc mf, int i)
        {
            VoiceTableData v = Voices[i];

            int ninx = VoiceText.Offsets.FindIndex(o => o == v.FileNameOff);
            int tinx = VoiceText.Offsets.FindIndex(o => o == v.TextOff);
            mw.nameTBV.Text = VoiceText.Texts[ninx];
            mw.textTBV.Text = VoiceText.Texts[tinx];
        }

        public override void UIToData(MainWindow mw, MainFunc mf, int i)
        {
            VoiceTableData v = Voices[i];

            string name = mw.nameTBV.Text;
            string text = mw.textTBV.Text;

            int ninx = VoiceText.Offsets.FindIndex(o => o == v.FileNameOff);
            int tinx = VoiceText.Offsets.FindIndex(o => o == v.TextOff);

            ulong diff1 = StaticField.MyBS.GetStringDiff(VoiceText.Texts[ninx], name);
            ulong diff2 = StaticField.MyBS.GetStringDiff(VoiceText.Texts[tinx], text);

            VoiceText.Texts[ninx] = SetValue(VoiceText.Texts[ninx], name);
            VoiceText.Texts[tinx] = SetValue(VoiceText.Texts[tinx], text);

            v.TextOff += diff1 + diff2;
            VoiceText.Offsets[tinx] += diff1 + diff2;

            TextReSetOff(diff1 + diff2, i + 1, tinx + 1);
        }

        private void TextReSetOff(ulong diff, int i, int j)
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

            for (; j < VoiceText.Offsets.Count; j++)
            {
                VoiceText.Offsets[j] += diff;
            }
        }
    }
}
