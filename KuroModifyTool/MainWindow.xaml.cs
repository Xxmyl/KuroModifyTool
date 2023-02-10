using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KuroModifyTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainFunc mainFunc;
        private ArtsDriverUIFunc adUIFunc;
        public MainWindow()
        {
            InitializeComponent();
            mainFunc = new MainFunc(this);
            adUIFunc = new ArtsDriverUIFunc(this);
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            mainFunc.SaveTbl();
        }

        private void changeBtn_Click(object sender, RoutedEventArgs e)
        {
            mainFunc.BatchModify();
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            adUIFunc.IsActiveEvent = false;
            mainFunc.ListSelectItem(sender as ListBox);
            adUIFunc.SelectList(sender as ListBox);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mainFunc.Init();
        }

        private void effGB_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mainFunc.SetCurrentEff((GroupBox)sender);
        }

        private void effectCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            mainFunc.EffCBSetItem((HandyControl.Controls.ComboBox)sender);
        }

        private void rangeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mainFunc.RangeCBSetItem((HandyControl.Controls.ComboBox)sender);
        }

        private void tblTabC_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.Source is TabControl)
            {
                mainFunc.SetTabC((TabControl)sender);
            }
        }

        private void SearchBar_SearchStarted(object sender, HandyControl.Data.FunctionEventArgs<string> e)
        {
            mainFunc.SearchStart(e.Info);
        }

        private void searchList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mainFunc.SearchSetItem((ListBox)sender);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*string str = inTB.Text;
            string[] ss = str.Split(' ');
            byte[] buf = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                buf[i] = byte.Parse(ss[i]);
            }

            outTB.Text = BitConverter.ToSingle(buf, 0).ToString();*/
        }

        private void fixCBAD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            adUIFunc.SoltFixedChange((HandyControl.Controls.ComboBox)sender);
        }

        private void cusCBAD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            adUIFunc.SoltCustomChange((HandyControl.Controls.ComboBox)sender);
        }

        private void sumCBAD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            adUIFunc.SoltSumChange((HandyControl.Controls.ComboBox)sender);
        }

        private void jumpVList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBox lb = (ListBox)sender;

            if(e.LeftButton == MouseButtonState.Pressed)
            {
                if(lb.SelectedIndex == -1)
                {
                    return;
                }
                FileTools.PlayOpus(lb.SelectedItem as string);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FileTools.OutPutOpus(whoTBV.Text, jumpVList.SelectedItems);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            mainFunc.JumpVoice();
        }

        private void jumpVList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mainFunc.SearchSetItem((ListBox)sender);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mainFunc.AppClose();
        }
    }
}
