using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FirstFloor.ModernUI.Windows.Controls;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Helper;
using System.Windows.Input;

namespace VolunteerDatabase.Desktop.Pages.InPutVolunteerInBatch
{
    /// <summary>
    /// DealWithConflict.xaml 的交互逻辑
    /// </summary>
    public partial class DealWithConflict : UserControl
    {
        private InputWindow fatherWindow;
        private Project project;
        private Database database;
        private List<Volunteer> fullList = new List<Volunteer>();
        private List<Volunteer> conflictList = new List<Volunteer>();
        private List<Volunteer> normalList = new List<Volunteer>();
        private VolunteerHelper vhelper = VolunteerHelper.GetInstance();
        private List<csvItemViewModel> itemList;
        

        public DealWithConflict(Project p, List<Volunteer> fulllist,InputWindow window)//list为完整list
        {
            InitializeComponent();
            project = p;
            fatherWindow = window;
            fullList = fulllist;
            database = DatabaseContext.GetInstance();
            itemList = new List<csvItemViewModel>();
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
                List<csvItemViewModel> finalList = new List<csvItemViewModel>();
                foreach(Volunteer v in normalList)
                {
                    finalList.Add(new csvItemViewModel(v));
                }
                MessageBox.Show("导入信息成功!","",MessageBoxButton.OK);
                Csvviewer viewer = new Csvviewer(project,finalList,fatherWindow);
                fatherWindow.Content = viewer;
                
                //this.Visibility=Visibility.Collapsed;
            }
            else
            {
                foreach(Volunteer v in conflictList)
                {
                    Volunteer old = vhelper.FindVolunteer(v.StudentNum);
                    csvItemViewModel vmNew = new csvItemViewModel(v);
                    csvItemViewModel vmOld = new csvItemViewModel(old);
                    vmOld.IsOld = true;
                    vmNew.IsOld = false;
                    vmNew.Pair = vmOld;
                    vmOld.Pair = vmNew;
                    itemList.Add(vmNew);
                    itemList.Add(vmOld);
                }
                /* List<Volunteer> theOldAndNew = new List<Volunteer>();
                 Volunteer theOld;
                 foreach (Volunteer theNew in conflictList)
                 {
                     theOldAndNew.Add(theNew);
                     theOld = vhelper.FindVolunteer(theNew.StudentNum);
                     theOldAndNew.Add(theOld);
                 }
                 isOld = false;
                 conflictList = theOldAndNew;
                 csvGrid.ItemsSource = conflictList;*/
                csvGrid.ItemsSource = itemList;
            }
            
            
        }

        private void csvGrid_LoadingRow(object sender, DataGridRowEventArgs e)//inconfirmed.
        {
            csvItemViewModel vm = e.Row.Item as csvItemViewModel;
            if(vm.IsOld)
            {
                e.Row.Header = "旧";
                e.Row.Background = new SolidColorBrush(Colors.Wheat);
            }
            else
            {
                e.Row.Header = "新";
                
            }
            
            
        }

        private void SelectTheNew_Click(object sender, RoutedEventArgs e)
        {
            foreach(csvItemViewModel item in itemList)
            {
                if(item.IsOld == false)
                {
                    item.Selected = true;
                }
                else
                {
                    item.Selected = false;
                }
            }
           
        }

        private void SelectTheOld_Click(object sender, RoutedEventArgs e)
        {
            foreach (csvItemViewModel item in itemList)
            {
                if(item.IsOld == true)
                {
                    item.Selected = true;
                }
                else
                {
                    item.Selected = false;
                }
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            List<csvItemViewModel> finalList = new List<csvItemViewModel>();
            bool succeeded = true;
            string error="记录选择有误,请检查选择.";
            foreach(csvItemViewModel item in itemList)
            {
                if(item.Selected)
                {
                    if(item.Pair.Selected)
                    {
                        succeeded = false;
                        error = "每一个志愿者只能保存一条记录,请检查选择是否有错.";
                        break;
                    }
                    else
                    {
                        if(!finalList.Contains(item))
                        finalList.Add(item);
                    }
                }
                else
                {
                    if(!item.Pair.Selected)
                    {
                        finalList.Add(item.IsOld ? item : item.Pair);
                        if (item.IsOld)
                        {
                            item.Selected = true;
                        }
                        else
                        {
                            item.Pair.Selected = true;
                        }
                    }
                   
                }
            }
            if(!succeeded)
            {
                MessageBox.Show(error);
                succeeded = true;
            }
            else
            {
                foreach(Volunteer v in normalList)
                {
                    finalList.Add(new csvItemViewModel(v));
                }
                MessageBox.Show("完成了对志愿者库信息的更新.\n请确认最终要导入的志愿者.");
                Csvviewer viewer = new Csvviewer(project, finalList, fatherWindow);
                fatherWindow.Content = viewer;
            }
            /*
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
                MessageBox.Show("处理信息冲突完成!","操作成功",MessageBoxButton.OK);
                normalList.AddRange(CheckedList);
                Csvviewer viewer = new Csvviewer(project,normalList,fatherWindow);
                fatherWindow.Content = viewer;
                //window.Show();
            }*/
        }

       

        private void csvGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            csvItemViewModel vm = csvGrid.SelectedItem as csvItemViewModel;
            vm.Selected = !vm.Selected;
        }



        
    }
}
