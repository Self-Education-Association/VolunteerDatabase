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

        //所有志愿者
        private List<Volunteer> fullList = new List<Volunteer>();

        //有冲突的志愿者
        private List<Volunteer> conflictList = new List<Volunteer>();

        //正常的志愿者
        private List<Volunteer> normalList = new List<Volunteer>();
        private VolunteerHelper vhelper = VolunteerHelper.GetInstance();

        //显示的列表
        private List<csvItemViewModel> itemList;

        //显示部分
        private int CnfCount { get { return itemList.Count; } }
        private int CnfPgeIndex { get; set; }
        private int MaxCnfItems = 18;

        private void ShowCnfGrid()
        {
            List<csvItemViewModel> Cnfs = new List<csvItemViewModel>();
            for (int i = CnfPgeIndex; i <= CnfPgeIndex + MaxCnfItems; i++)
            {
                if (i > CnfCount - 1) break;
                Cnfs.Add(itemList[i]);
            }
            CnfPge.Content = string.Format("{0}/{1}", CnfPgeIndex / MaxCnfItems + 1, CnfCount / MaxCnfItems + 1);
            csvGrid.ItemsSource = Cnfs;
        }
        public DealWithConflict(Project p, List<Volunteer> fulllist, InputWindow window)//list为完整list
        {
            InitializeComponent();
            project = p;
            fatherWindow = window;
            fullList = fulllist;
            database = DatabaseContext.GetInstance();
            itemList = new List<csvItemViewModel>();
            foreach (Volunteer item in fullList)
            {
                if (database.Volunteers.Where(v => v.StudentNum == item.StudentNum).ToList().Count() != 0)
                {
                    conflictList.Add(item);
                }
                else
                {
                    if (vhelper.FindVolunteer(item.StudentNum) == null)
                        normalList.Add(item);
                    else
                        normalList.Add(vhelper.FindVolunteer(item.StudentNum));
                }
            }

            if (conflictList.Count() == 0)
            {
                List<csvItemViewModel> finalList = new List<csvItemViewModel>();
                foreach (Volunteer v in normalList)
                {
                    finalList.Add(new csvItemViewModel(v));
                }
                MessageBox.Show("导入信息成功!", "", MessageBoxButton.OK);
                Csvviewer viewer = new Csvviewer(project, finalList, fatherWindow);
                fatherWindow.Content = viewer;

                //this.Visibility=Visibility.Collapsed;
            }
            else
            {
                foreach (Volunteer v in conflictList)
                {
                    Volunteer old = vhelper.FindVolunteer(v.StudentNum);
                    csvItemViewModel vmNew = new csvItemViewModel(v);
                    csvItemViewModel vmOld = new csvItemViewModel(old);
                    vmOld.IsOld = true;
                    vmNew.IsOld = false;
                    vmNew.Pair = vmOld;
                    vmOld.Pair = vmNew;
                    itemList.Add(vmNew); //同一个学号，查找到的记为old,新传入的记为new，并将他们对应
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
                ShowCnfGrid();
            }
        }

        private void csvGrid_LoadingRow(object sender, DataGridRowEventArgs e)//inconfirmed.
        {
            csvItemViewModel vm = e.Row.Item as csvItemViewModel;
            if (vm.IsOld)
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
            foreach (csvItemViewModel item in itemList)
            {
                if (item.IsOld == false)
                {
                    item.Selected = true;//vmOld可选中
                }
                else
                {
                    item.Selected = false;//vmNew不可选中
                }
            }
        }

        private void SelectTheOld_Click(object sender, RoutedEventArgs e)
        {
            foreach (csvItemViewModel item in itemList)
            {
                if (item.IsOld == true)
                {
                    item.Selected = true;//vmOld将可以被选中
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
            string error = "记录选择有误,请检查选择.";
            foreach (csvItemViewModel item in itemList)
            {
                if (item.Selected)
                {
                    if (item.Pair.Selected)//vmOld的pair的初始为false，所以应该没有这部分？
                    {
                        succeeded = false;
                        error = "每一个志愿者只能保存一条记录,请检查选择是否有错.";
                        break;
                    }
                    else
                    {
                        if (!finalList.Contains(item))//添加了vmOld ？
                            finalList.Add(item);
                    }
                }
                else  //即不可被选中,为vmNew
                {
                    if (!item.Pair.Selected)//pair初始都为false
                    {
                        finalList.Add(item.IsOld ? item : item.Pair);//添加vmOld的pair  ?不应该添加vmOld?
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
            if (!succeeded)
            {
                MessageBox.Show(error);
                succeeded = true;
            }
            else
            {
                foreach (Volunteer v in normalList)
                {
                    finalList.Add(new csvItemViewModel(v));//添加所有vmNew
                }
                foreach (csvItemViewModel v in finalList)
                {
                    if(vhelper.FindVolunteer(v.StudentNum)!=null)//挂上评分
                        v.Volunteer = vhelper.FindVolunteer(v.Volunteer.StudentNum);
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
            if(vm!=null)
                vm.Selected = !vm.Selected;
        }
        private void CnfPgePrevious_Click(object sender, RoutedEventArgs e)
        {
            CnfPgeIndex = (CnfPgeIndex - MaxCnfItems < 0) ? CnfPgeIndex : CnfPgeIndex - MaxCnfItems; //?
            if (CnfPgeIndex - MaxCnfItems < 0) CnfPgePrevious.IsEnabled = false;
            CnfPgeNext.IsEnabled = true;
            ShowCnfGrid();
        }

        private void CnfPgeNext_Click(object sender, RoutedEventArgs e)
        {
            CnfPgeIndex = (CnfPgeIndex + MaxCnfItems > CnfCount) ? CnfPgeIndex : CnfPgeIndex + MaxCnfItems;
            if (CnfPgeIndex + MaxCnfItems > CnfCount) CnfPgeNext.IsEnabled = false;
            CnfPgePrevious.IsEnabled = true;
            ShowCnfGrid();
        }
    }
}