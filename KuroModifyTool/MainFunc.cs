using KuroModifyTool.KuroTable;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace KuroModifyTool
{
    internal class MainFunc
    {
        public ItemTable itemTable;
        public SkillTable skillTable;
        public ShardSkillTable shardSTable;
        public HollowCoreTable hollowCTable;
        public ArtsDriverTable artsDTable;


        private Paragraph itemParag;
        private Paragraph skillParag1;
        private Paragraph skillParag2;
        private Paragraph shardSParag1;
        private Paragraph shardSParag2;
        private Paragraph hollowCParag1;
        public string ItemRichText { get { return GetRichTB(itemParag); } set { SetRichTB(itemParag, value); } }
        public string Skill1RichText { get { return GetRichTB(skillParag1); } set { SetRichTB(skillParag1, value); } }
        public string Skill2RichText { get { return GetRichTB(skillParag2); } set { SetRichTB(skillParag2, value); } }
        public string ShardS1RichText { get { return GetRichTB(shardSParag1); } set { SetRichTB(shardSParag1, value); } }
        public string ShardS2RichText { get { return GetRichTB(shardSParag2); } set { SetRichTB(shardSParag2, value); } }
        public string HollowCRichText { get { return GetRichTB(hollowCParag1); } set { SetRichTB(hollowCParag1, value); } }


        private HandyControl.Controls.TextBox CurrentEff;
        private HandyControl.Controls.SimpleText CurrentP1;
        private HandyControl.Controls.SimpleText CurrentP2;
        private HandyControl.Controls.SimpleText CurrentP3;


        private int CurrentInx;
        private string CurrentTabC;


        private List<FrameworkElement> itemCtls;
        private List<FrameworkElement> skillCtls;
        private List<FrameworkElement> shardSCtls;
        private List<FrameworkElement> hollowCCtls;
        private List<FrameworkElement> ArtsDCtls;

        private MainWindow MW;

        private CommonOpenFileDialog dialog;

        private StringBuilder sb;

        public MainFunc(MainWindow w)
        {
            MW = w;
        }

        public void Init()
        {
            dialog = new CommonOpenFileDialog();
            if (!FileTools.initConfig(ChooseGameRootDir))
            {
                MessageBox.Show("請選擇游戏根目錄(包含ed9.exe的目录，(^_−)☆)");
                Environment.Exit(0);
                Process.GetCurrentProcess().Kill();
            }

            StaticField.MyBS = new MyBinStream();
            StaticField.EffectList = FileTools.GetEffectList(".\\KuroList\\EffectList.txt");
            StaticField.RangeList = FileTools.GetEffectList(".\\KuroList\\RangeList.txt");
            StaticField.HCEffectList = FileTools.GetEffectList(".\\KuroList\\HCEffectList.txt");
            StaticField.SkillDic = FileTools.GetSkillDic(".\\KuroList\\SkillDic.txt");

            CurrentInx = -1;

            sb = new StringBuilder();

            itemCtls = new List<FrameworkElement>();
            itemCtls.Add(MW.itemPanel);
            itemCtls.Add(MW.itemList);
            itemCtls.Add(MW.effectCB);
            itemCtls.Add(MW.searchList);

            skillCtls = new List<FrameworkElement>();
            skillCtls.Add(MW.skillPanel);
            skillCtls.Add(MW.skillList);
            skillCtls.Add(MW.effectCBS);
            skillCtls.Add(MW.searchSList);

            shardSCtls = new List<FrameworkElement>();
            shardSCtls.Add(MW.shardSPanel);
            shardSCtls.Add(MW.shardSList);
            shardSCtls.Add(MW.effectCBSS);
            shardSCtls.Add(MW.searchSSList);

            hollowCCtls = new List<FrameworkElement>();
            hollowCCtls.Add(MW.hollowCPanel);
            hollowCCtls.Add(MW.hollowCList);
            hollowCCtls.Add(MW.effectCBHC);
            hollowCCtls.Add(MW.searchHCList);

            ArtsDCtls = new List<FrameworkElement>();
            ArtsDCtls.Add(MW.artsDPanel);
            ArtsDCtls.Add(MW.artsDList);
            ArtsDCtls.Add(MW.searchADList);

            Thread thread = new Thread(ListInit);
            thread.Start();

            //EffectCB添加元素
            foreach (OtherDesc desc in StaticField.EffectList)
            {
                MW.effectCB.Items.Add(desc);
                MW.effectCBS.Items.Add(desc);
            }
            //RangeCB添加元素
            foreach (OtherDesc desc in StaticField.RangeList)
            {
                MW.rangeCB.Items.Add(desc);
            }
            //HCEffectCB添加元素
            foreach (OtherDesc desc in StaticField.HCEffectList)
            {
                MW.effectCBHC.Items.Add(desc);
            }
            //SkillCB添加元素
            foreach (OtherDic dic in StaticField.SkillDic)
            {
                MW.skillCBAD1.Items.Add(dic);
                MW.skillCBAD2.Items.Add(dic);
                MW.skillCBAD3.Items.Add(dic);
                MW.skillCBAD4.Items.Add(dic);
                MW.skillCBAD5.Items.Add(dic);
                MW.skillCBAD6.Items.Add(dic);
                MW.skillCBAD7.Items.Add(dic);
                MW.skillCBAD8.Items.Add(dic);
            }


            itemParag = new Paragraph();
            MW.descTB.Document.Blocks.Add(itemParag);

            skillParag1 = new Paragraph();
            MW.descTBS1.Document.Blocks.Add(skillParag1);

            skillParag2 = new Paragraph();
            MW.descTBS2.Document.Blocks.Add(skillParag2);

            shardSParag1 = new Paragraph();
            MW.descTBSS1.Document.Blocks.Add(shardSParag1);

            shardSParag2 = new Paragraph();
            MW.descTBSS2.Document.Blocks.Add(shardSParag2);

            hollowCParag1 = new Paragraph();
            MW.descTBHC.Document.Blocks.Add(hollowCParag1);
        }

        private void ChooseGameRootDir()
        {
            dialog.IsFolderPicker = true;
            dialog.Title = "請選擇游戏根目錄(包含ed9.exe的目录，(^_−)☆)";
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                StaticField.GamePath = dialog.FileName;
            }
        }

        private void ListInit()
        {
            itemTable = new ItemTable();

            skillTable = new SkillTable();

            shardSTable = new ShardSkillTable();

            hollowCTable = new HollowCoreTable();

            artsDTable = new ArtsDriverTable();

            MW.Dispatcher.BeginInvoke(new Action(() =>
            {
                MW.itemList.Items.Clear();

                if (itemTable.IsDecrypt)
                {
                    foreach (ItemTable.ItemTableData item in itemTable.Items)
                    {
                        int ninx = itemTable.ItemText.Offsets.FindIndex(o => o == item.NameOff);
                        MW.itemList.Items.Add(itemTable.ItemText.Texts[ninx]);
                    }
                }
                else
                {
                    MW.itemTabI.IsEnabled = false;
                }

                MW.skillList.Items.Clear();

                if (skillTable.IsDecrypt)
                {
                    foreach (SkillTable.SkillParam skill in skillTable.Skills)
                    {
                        int ninx = skillTable.SkillText.Offsets.FindIndex(o => o == skill.NameOff);
                        MW.skillList.Items.Add(skillTable.SkillText.Texts[ninx]);
                    }
                }
                else
                {
                    MW.skillTabI.IsEnabled = false;
                }

                MW.shardSList.Items.Clear();

                if (shardSTable.IsDecrypt)
                {
                    foreach (ShardSkillTable.ShardSkillParam shardS in shardSTable.ShardSkills)
                    {
                        int ninx = shardSTable.ShardSText.Offsets.FindIndex(o => o == shardS.NameOff);
                        MW.shardSList.Items.Add(shardSTable.ShardSText.Texts[ninx]);
                    }
                }
                else
                {
                    MW.shardSTabI.IsEnabled = false;
                }

                MW.hollowCList.Items.Clear();

                if (hollowCTable.IsDecrypt)
                {
                    foreach (HollowCoreTable.HollowCoreLevelParam hc in hollowCTable.LevelParams)
                    {
                        ulong off = Array.Find(itemTable.Items, item => item.ID == hc.ItemID).NameOff;
                        int ninx = itemTable.ItemText.Offsets.FindIndex(o => o == off);

                        MW.hollowCList.Items.Add(itemTable.ItemText.Texts[ninx] + "：等级" + hc.Level);
                    }
                }
                else
                {
                    MW.hollowCTabI.IsEnabled = false;
                }

                MW.artsDList.Items.Clear();

                if (artsDTable.IsDecrypt)
                {
                    foreach (ArtsDriverTable.DriverBaseTableData ad in artsDTable.BaseTableDatas)
                    {
                        ItemTable.ItemTableData item = Array.Find(itemTable.Items, i => i.ID == ad.ItemID);
                        if(item == null)
                        {
                            MW.artsDList.Items.Add("不在item中：" + ad.ItemID.ToString());
                        }
                        else
                        {
                            int ninx = itemTable.ItemText.Offsets.FindIndex(o => o == item.NameOff);

                            MW.artsDList.Items.Add(itemTable.ItemText.Texts[ninx]);
                        }
                    }
                }
                else
                {
                    MW.hollowCTabI.IsEnabled = false;
                }
            }));
        }

        public void SetTabC(TabControl tc)
        {
            if (tc.SelectedIndex == -1)
            {
                return;
            }

            CurrentTabC = (tc.SelectedItem as TabItem).Header as string;

            if(itemCtls == null || skillCtls == null)
            {
                return;
            }

            ListBox lb = GetCtl<ListBox>("ItemsList");
            CurrentInx = lb.SelectedIndex;
        }

        public void SaveTbl()
        {
            TBLCommon tbl = GetTable();

            if (CurrentInx != -1)
            {
                tbl.UIToData(MW, this, CurrentInx);
            }
            Thread thread = new Thread(tbl.Save);
            thread.Start();
        }
        
        public void ListSelectItem(ListBox lb)
        {
            if (lb.SelectedIndex == -1)
            {
                return;
            }

            TBLCommon tbl = GetTable();

            if (CurrentInx != -1)
            {
                tbl.UIToData(MW, this, CurrentInx);
            }

            if (lb.SelectedItems.Count == 1)
            {
                if (MW.changeBtn.IsEnabled)
                {
                    MW.changeBtn.IsEnabled = false;
                }
                
                CurrentInx = lb.SelectedIndex;
                tbl.DataToUI(MW, this, lb.SelectedIndex);
            }
            else if (lb.SelectedItems.Count > 1)
            {
                CurrentInx = -1;
                MW.changeBtn.IsEnabled = true;
                ClearText();
            }
        }

        private void ClearText()
        {
            Grid g = GetCtl<Grid>("ShowPanel");
            List<HandyControl.Controls.TextBox> tbs = new List<HandyControl.Controls.TextBox>();
            GetChildObjects<HandyControl.Controls.TextBox>(g, tbs);

            List<RichTextBox> rtbs = new List<RichTextBox>();
            GetChildObjects<RichTextBox>(g, rtbs);

            List<HandyControl.Controls.ComboBox> cbs = new List<HandyControl.Controls.ComboBox>();
            GetChildObjects<HandyControl.Controls.ComboBox>(g, cbs);

            foreach (HandyControl.Controls.TextBox tb in tbs)
            {
                tb.Clear();
            }

            foreach (RichTextBox rtb in rtbs)
            {
                (rtb.Document.Blocks.FirstBlock as Paragraph).Inlines.Clear();
            }

            foreach (HandyControl.Controls.ComboBox cb in cbs)
            {
                cb.SelectedIndex = -1;
            }
        }

        public void BatchModify()
        {
            ListBox lb = GetCtl<ListBox>("ItemsList");
            TBLCommon tbl = GetTable();
            int t = lb.SelectedIndex;
            for (int i = 0; i < lb.SelectedItems.Count; i++)
            {
                tbl.UIToData(MW, this, t + i);
            }

            lb.SelectedItems.Clear();
            lb.SelectedIndex = t;
        }

        private void SetRichTB(Paragraph pg, string text)
        {
            pg.Inlines.Clear();
            pg.Inlines.Add(new Run(text));
        }

        private string GetRichTB(Paragraph pg)
        {
            sb.Clear();
            foreach (Run r in pg.Inlines)
            {
                sb.Append(r.Text);
            }

            return sb.ToString();
        }

        public void SetCurrentEff(GroupBox gb)
        {
            ListBox lb = GetCtl<ListBox>("ItemsList");
            HandyControl.Controls.ComboBox cb = GetCtl<HandyControl.Controls.ComboBox>("EffectCBox");

            if (lb.SelectedIndex == -1) { return; }

            List<HandyControl.Controls.TextBox> tbs = new List<HandyControl.Controls.TextBox>();
            GetChildObjects<HandyControl.Controls.TextBox>(gb, tbs);
            if (tbs.Count == 0)
            {
                return;
            }

            CurrentEff = tbs.Find(tb => tb.Tag.Equals("effid"));

            List<HandyControl.Controls.SimpleText> texts = new List<HandyControl.Controls.SimpleText>();
            GetChildObjects<HandyControl.Controls.SimpleText>(gb, texts);
            foreach (HandyControl.Controls.SimpleText st in texts)
            {
                if (st.Tag == null)
                {
                    continue;
                }

                if (st.Tag.Equals("param1"))
                {
                    CurrentP1 = st;
                }
                else if (st.Tag.Equals("param2"))
                {
                    CurrentP2 = st;
                }
                else if (st.Tag.Equals("param3"))
                {
                    CurrentP3 = st;
                }
            }


            cb.SelectedIndex = GetEffList().FindIndex(eff => eff.ID.Equals(CurrentEff.Text));
        }

        public void EffCBSetItem(HandyControl.Controls.ComboBox cb)
        {
            if (cb.SelectedIndex == -1 || CurrentEff == null)
            {
                return;
            }

            OtherDesc desc = GetEffList()[cb.SelectedIndex];
            CurrentEff.Text = desc.ID;
            if (desc.Param1 != null && desc.Param1 != "")
            {
                CurrentP1.Text = desc.Param1;
            }
            else
            {
                CurrentP1.Text = "无参数";
            }

            if (desc.Param2 != null && desc.Param2 != "")
            {
                CurrentP2.Text = desc.Param2;
            }
            else
            {
                CurrentP2.Text = "无参数";
            }

            if (desc.Param3 != null && desc.Param3 != "")
            {
                CurrentP3.Text = desc.Param3;
            }
            else
            {
                CurrentP3.Text = "无参数";
            }
        }

        public void RangeCBSetItem(HandyControl.Controls.ComboBox cb)
        {
            if (cb.SelectedIndex == -1)
            {
                return;
            }

            OtherDesc desc = StaticField.RangeList[cb.SelectedIndex];

            if (desc.Param1 != null && desc.Param1 != "")
            {
                MW.rangep1ST.Text = desc.Param1;
            }

            if (desc.Param2 != null && desc.Param2 != "")
            {
                MW.rangep2ST.Text = desc.Param2;
            }

            if (desc.Param3 != null && desc.Param3 != "")
            {
                MW.rangep3ST.Text = desc.Param3;
            }
        }

        public void SearchStart(string s)
        {
            ListBox slb = GetCtl<ListBox>("SearchList");
            ListBox ilb = GetCtl<ListBox>("ItemsList"); ;

            string t = FileTools.GetTraditional(s);
            slb.Items.Clear();

            if (t == null || t == "")
            {
                return;
            }

            foreach (string n in ilb.Items)
            {
                if (n.Equals(t) || n.Contains(t))
                {
                    slb.Items.Add(n);
                }
            }
        }

        public void SearchSetItem(ListBox lb)
        {
            ListBox ilb = GetCtl<ListBox>("ItemsList");
            if (lb.SelectedIndex == -1)
            {
                return;
            }

            ilb.SelectedItem = lb.SelectedItem;
        }

        private T GetCtl<T>(string tag) where T : FrameworkElement
        {
            switch (CurrentTabC)
            {
                case "Item":
                    return itemCtls.Find(c => c.Tag.Equals(tag)) as T;
                case "Skill":
                    return skillCtls.Find(c => c.Tag.Equals(tag)) as T;
                case "ShardSkill":
                    return shardSCtls.Find(c => c.Tag.Equals(tag)) as T;
                case "HollowCore":
                    return hollowCCtls.Find(c => c.Tag.Equals(tag)) as T;
                case "ArtsDriver":
                    return ArtsDCtls.Find(c => c.Tag.Equals(tag)) as T;
                default :
                    return null;
            }
        }

        private TBLCommon GetTable()
        {
            switch (CurrentTabC)
            {
                case "Item":
                    return itemTable;
                case "Skill":
                    return skillTable;
                case "ShardSkill":
                    return shardSTable;
                case "HollowCore":
                    return hollowCTable;
                case "ArtsDriver":
                    return artsDTable;
                default:
                    return null;
            }
        }

        private List<OtherDesc> GetEffList()
        {
            switch (CurrentTabC)
            {
                case "Item":
                    return StaticField.EffectList;
                case "Skill":
                    return StaticField.EffectList;
                case "HollowCore":
                    return StaticField.HCEffectList;
                default:
                    return null;
            }
        }


        /// 获得指定元素的指定父元素 
        /// </summary> 
        /// <typeparam name="T">指定页面元素</typeparam> 
        /// <param name="obj"></param> 
        /// <returns></returns> 
        public T GetParentObject<T>(DependencyObject obj) where T : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);

            while (parent != null)
            {
                if (parent is T)
                {
                    return (T)parent;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        /// <summary> 
        /// 获得指定元素的指定子元素 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="obj"></param> 
        /// <returns></returns> 
        public void GetChildObjects<T>(DependencyObject obj, List<T> children) where T : FrameworkElement
        {
            DependencyObject child = null;

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T)
                {
                    children.Add(child as T);
                }

                GetChildObjects(child, children);
            }
        }
    }
}
