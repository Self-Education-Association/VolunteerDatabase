using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Helper;
using FirstFloor.ModernUI.Windows.Controls;

namespace Desktop.Pages
{
    /// <summary>
    /// DealWithConflict.xaml 的交互逻辑
    /// </summary>
    public partial class DealWithConflict : UserControl
    {
        private Window fatherWindow;
        private Project project;
        private Database database;
        private List<Volunteer> fullList = new List<Volunteer>();
        private List<Volunteer> conflictList = new List<Volunteer>();
        private List<Volunteer> normalList = new List<Volunteer>();
        private VolunteerHelper vhelper = VolunteerHelper.GetInstance();
        private bool isOld = false;
        private List<Volunteer> CheckedList = new List<Volunteer>();
        private List<DataGridRow> theNewRows = new List<DataGridRow>();
        private List<DataGridRow> theOldRows = new List<DataGridRow>();
        private List<DataGridRow> theOldAndNewRows = new List<DataGridRow>();
        public DealWithConflict(Project p, List<Volunteer> fulllist,Window window)//list为完整list
        {
            InitializeComponent();
            project = p;
            fatherWindow = window;
            fullList = fulllist;
            database = DatabaseContext.GetInstance();
            foreach (Volunteer item in fullList)
            {
                if(database.Volunteers.Where(v=>v.StudentNum==item.StudentNum).ToList().Count()!=0)
                {
                    conflictList.Add(item);
                }
                else
                {
                    normalList.Add(item);
                }
            }
            if (conflictList.Count() == 0)
            {
                ModernDialog.ShowMessage("导入信息成功!","",MessageBoxButton.OK);
                Csvviewer viewer = new Csvviewer(project,fullList,fatherWindow);
                fatherWindow.Content = viewer;
                //this.Visibility=Visibility.Collapsed;
            }
            
            List<Volunteer> theOldAndNew = new List<Volunteer>();
            Volunteer theOld;
            foreach (Volunteer theNew in conflictList)
            {
                theOldAndNew.Add(theNew);
                theOld = vhelper.FindVolunteer(theNew.StudentNum);
                theOldAndNew.Add(theOld);
            }
            isOld = false;
            conflictList = theOldAndNew;
            csvGrid.ItemsSource = conflictList;
        }

        private void csvGrid_LoadingRow(object sender, DataGridRowEventArgs e)//inconfirmed.
        {
            
            if(isOld)
            {
                e.Row.Header = "旧";
                isOld = false;
                theOldRows.Add(e.Row);
                theOldAndNewRows.Add(e.Row);
            }
            else
            {
                e.Row.Background = new SolidColorBrush(Colors.Red);
                e.Row.Header = "新";
                isOld = true;
                theNewRows.Add(e.Row);
                theOldAndNewRows.Add(e.Row);
            }
            
        }

        private void SelectTheNew_Click(object sender, RoutedEventArgs e)
        {
            foreach (var theNew in theNewRows)
            {
                //DataGridCheckBoxColumn checkcolumn = csvGrid.Columns[0] as DataGridCheckBoxColumn;
                CheckBox cbox = csvGrid.Columns[0].GetCellContent(theNew) as CheckBox;
                cbox.IsChecked = !(cbox.IsChecked);
            }
            foreach (var theOld in theOldRows)
            {
                CheckBox cbox = csvGrid.Columns[0].GetCellContent(theOld) as CheckBox;
                cbox.IsChecked = false;
            }
           
        }

        private void SelectTheOld_Click(object sender, RoutedEventArgs e)
        {
            foreach (var theOld in theOldRows)
            {
                //DataGridCheckBoxColumn checkcolumn = csvGrid.Columns[0] as DataGridCheckBoxColumn;
                CheckBox cbox = csvGrid.Columns[0].GetCellContent(theOld) as CheckBox;
                cbox.IsChecked = true;
            }
            foreach (var theNew in theNewRows)
            {
                CheckBox cbox = csvGrid.Columns[0].GetCellContent(theNew) as CheckBox;
                cbox.IsChecked = false;
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            int position = 1;
            bool isValid = true;
            bool flag = false;
            for(int i=1;i<=theOldAndNewRows.Count();i++)
            {
                DataGridRow item = theOldAndNewRows[i - 1];
                CheckBox cbox = GetCheckBox(item);
                if((cbox.IsChecked==null || cbox.IsChecked==false) && position%2==1)//单数为新
                {
                    flag = false;
                    CheckedList.Add((item.DataContext as Volunteer));
                }
                if(position%2==1 && cbox.IsChecked==true)
                {
                    flag = true;//老项不能选了
                    CheckedList.Add((item.DataContext as Volunteer));
                }
                if(position%2 == 0 && flag == true && cbox.IsChecked==true)
                {
                    ModernDialog.ShowMessage("每一个志愿者只能保存一条记录,请检查选择是否有错.","错误提示",MessageBoxButton.OK);
                    isValid = false;
                    break;
                }
                if(position%2==0 && flag == false && cbox.IsChecked ==false)
                {
                    ModernDialog.ShowMessage("每一个志愿者必须选择至少一条记录,请检查是否有遗漏.","错误提示",MessageBoxButton.OK);
                    isValid = false;
                    break;
                }
                else
                {
                    CheckedList.Add((item.DataContext as Volunteer));
                }
            }
            if(!isValid)
            {
                CheckedList.Clear();
            }
            else
            {
                ModernDialog.ShowMessage("处理信息冲突完成!","操作成功",MessageBoxButton.OK);
                normalList.AddRange(CheckedList);
                Csvviewer viewer = new Csvviewer(project,normalList,fatherWindow);
                fatherWindow.Content = viewer;
                //window.Show();
            }
        }

        private CheckBox GetCheckBox(DataGridRow row)
        {
            CheckBox cbox = csvGrid.Columns[0].GetCellContent(row) as CheckBox;
            return cbox;
        }



        //private void CheckBox_Checked(object sender, RoutedEventArgs e)
        //{
        //    CheckBox box = sender as CheckBox;
        //    Volunteer v;
        //    bool? flag = box.IsChecked;
        //    v = box.DataContext as Volunteer;
        //    CheckedList.Add(v);
        //}

        //private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    CheckBox box = sender as CheckBox;
        //    Volunteer v;
        //    v = box.DataContext as Volunteer;
        //    CheckedList.Remove(v);
        //}
    }
}
