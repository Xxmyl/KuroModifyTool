using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace KuroModifyTool
{
    internal class ArtsDriverUIFunc
    {
        public int CurrentSum;
        public bool IsActiveEvent;

        private MainWindow MW;

        public static List<GroupBox> SoltGBList;
        public static List<HandyControl.Controls.ComboBox> LockCBList;
        public static List<HandyControl.Controls.ComboBox> SkillCBList;

        public ArtsDriverUIFunc(MainWindow mw)
        {
            MW = mw;
            IsActiveEvent = false;

            SoltGBList = new List<GroupBox>();
            SoltGBList.Add(MW.soltGBAD1);
            SoltGBList.Add(MW.soltGBAD2);
            SoltGBList.Add(MW.soltGBAD3);
            SoltGBList.Add(MW.soltGBAD4);
            SoltGBList.Add(MW.soltGBAD5);
            SoltGBList.Add(MW.soltGBAD6);
            SoltGBList.Add(MW.soltGBAD7);
            SoltGBList.Add(MW.soltGBAD8);

            LockCBList = new List<HandyControl.Controls.ComboBox>();
            LockCBList.Add(MW.lockCBAD1);
            LockCBList.Add(MW.lockCBAD2);
            LockCBList.Add(MW.lockCBAD3);
            LockCBList.Add(MW.lockCBAD4);
            LockCBList.Add(MW.lockCBAD5);
            LockCBList.Add(MW.lockCBAD6);
            LockCBList.Add(MW.lockCBAD7);
            LockCBList.Add(MW.lockCBAD8);

            SkillCBList = new List<HandyControl.Controls.ComboBox>();
            SkillCBList.Add(MW.skillCBAD1);
            SkillCBList.Add(MW.skillCBAD2);
            SkillCBList.Add(MW.skillCBAD3);
            SkillCBList.Add(MW.skillCBAD4);
            SkillCBList.Add(MW.skillCBAD5);
            SkillCBList.Add(MW.skillCBAD6);
            SkillCBList.Add(MW.skillCBAD7);
            SkillCBList.Add(MW.skillCBAD8);

            for (int i = 0; i < 9; i++)
            {
                MW.sumCBAD.Items.Add(i);
                MW.fixCBAD.Items.Add(i);
                MW.cusCBAD.Items.Add(i);
            }

            for (int i = 0; i < 7; i++)
            {
                MW.lockCBAD1.Items.Add(i);
                MW.lockCBAD2.Items.Add(i);
                MW.lockCBAD3.Items.Add(i);
                MW.lockCBAD4.Items.Add(i);
                MW.lockCBAD5.Items.Add(i);
                MW.lockCBAD6.Items.Add(i);
                MW.lockCBAD7.Items.Add(i);
                MW.lockCBAD8.Items.Add(i);
            }
        }

        private void SetNullSolt()
        {
            if(MW.fixCBAD.SelectedIndex == -1 || MW.cusCBAD.SelectedIndex == -1)
            {
                return;
            }

            for (int i = MW.fixCBAD.SelectedIndex; i < 8 - MW.cusCBAD.SelectedIndex; i++)
            {
                SoltGBList[i].Header = "槽位" + (i + 1).ToString() + "：无";
                LockCBList[i].SelectedIndex = 0;
                SkillCBList[i].SelectedIndex = 0;
            }
        }

        public void SelectList(ListBox lb)
        {
            if((MW.tblTabC.SelectedItem as TabItem).Header as string != "ArtsDriver")
            {
                return;
            }

            if(lb.SelectedItems.Count > 1)
            {
                return;
            }

            CurrentSum = MW.sumCBAD.SelectedIndex;

            SetNullSolt();
        }

        public void SoltSumChange(HandyControl.Controls.ComboBox cb)
        {
            if(cb.SelectedIndex == -1)
            {
                return;
            }

            if (IsActiveEvent)
            {
                if(cb.SelectedIndex == MW.cusCBAD.SelectedIndex + MW.fixCBAD.SelectedIndex)
                {
                    CurrentSum = cb.SelectedIndex;
                    return;
                }

                int diff = cb.SelectedIndex - CurrentSum;

                int diff1 = MW.cusCBAD.SelectedIndex + diff;

                if(diff1 < 0)
                {
                    MW.cusCBAD.SelectedIndex = 0;
                    MW.fixCBAD.SelectedIndex = MW.fixCBAD.SelectedIndex + diff1;
                }
                else
                {
                    MW.cusCBAD.SelectedIndex = diff1;
                }

                SetNullSolt();

                CurrentSum = cb.SelectedIndex;
            }
            else
            {
                IsActiveEvent = true;
            }
        }

        public void SoltFixedChange(HandyControl.Controls.ComboBox cb)
        {
            if (cb.SelectedIndex == -1)
            {
                return;
            }

            for (int i = 0; i < cb.SelectedIndex; i++)
            {
                SoltGBList[i].Header = "槽位" + (i + 1).ToString() + "：固定";
            }

            if(IsActiveEvent)
            {
                SetNullSolt();
            }
        }

        public void SoltCustomChange(HandyControl.Controls.ComboBox cb)
        {
            if (cb.SelectedIndex == -1)
            {
                return;
            }

            for (int i = 0; i < cb.SelectedIndex; i++)
            {
                SoltGBList[7 - i].Header = "槽位" + (8 - i).ToString() + "：自定义";
            }

            if (IsActiveEvent)
            {
                SetNullSolt();
            }
        }
    }
}
