using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FirstFloor.ModernUI.Windows.Controls;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Helper;
using System.Windows.Media;
using System.Windows.Input;

namespace VolunteerDatabase.Desktop.Pages.InPutVolunteerInBatch
{
    /// <summary>
    /// csvviewer.xaml 的交互逻辑
    /// </summary>
    public partial class Csvviewer : UserControl
    {
        private AppUserIdentityClaims Claims { get; set; }
        private Window fatherWindow;
        private IdentityPage identitypage = IdentityPage.GetInstance();
        private CsvHelper chelper;
        private VolunteerHelper vhelper;
        private ProjectProgressHelper pphelper;
        private List<csvItemViewModel> csvList;
        private List<DataGridRow> rows = new List<DataGridRow>();
        
        private Project project;
        private bool hasAllSelected = false;
        public Csvviewer(Project p, List<csvItemViewModel> list,Window window)//csv导入后弹出,加载页面=>datagrid列表显示，若有冲突:"一个以上冲突，请您确认",对应行标红,保留与否：按钮.保留原数据/新数据，下一步:更改的确认,导入成功，detail页面的显示
        {
            InitializeComponent();
            fatherWindow = window;
            vhelper = VolunteerHelper.GetInstance();
            pphelper = ProjectProgressHelper.GetInstance();
            project = p;
            chelper = CsvHelper.GetInstance();
            csvList = list;
            
            csvGrid.ItemsSource = csvList;
            Claims = identitypage.Claims;

        }



        private void csvGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGridRow row = e.Row;
            if (row != null)
            {
                rows.Add(row);
            }

            var item = e.Row.Item as Volunteer;
            if (item != null)
            {
                if (chelper.conflictNums.Contains(item.StudentNum))
                {
                    e.Row.Background = new SolidColorBrush(Colors.DarkGray);
                }
            }

        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cbox;
            foreach (var row in rows)
            {
                cbox = csvGrid.Columns[0].GetCellContent(row) as CheckBox;
                if (!hasAllSelected)
                    cbox.IsChecked = true;
                else
                    cbox.IsChecked = false;
            }
            hasAllSelected = !(hasAllSelected);
        }

        private void SelectNotSelected_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cbox;
            foreach (var item in rows)
            {
                cbox = csvGrid.Columns[0].GetCellContent(item) as CheckBox;
                cbox.IsChecked = !(cbox.IsChecked);
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            
            //<Volunteer> finalList = csvGrid.SelectedItems as List<Volunteer>;
            List<string> errors = new List<string>();
            List<csvItemViewModel> succeededList = new List<csvItemViewModel>();
            
            foreach (csvItemViewModel v in csvList)
            {
                if(v.Selected)
                {
                    if (vhelper.FindVolunteer(v.StudentNum) == null)
                    {
                        var addResult = vhelper.AddVolunteer(v.Volunteer);
                        if (!addResult.Succeeded)
                        {
                            errors.Add(addResult.ErrorString);
                        }
                        else
                        {
                            var addToProjectResult = pphelper.SingleVolunteerInputById(v.StudentNum, project);
                            if (!addToProjectResult.Succeeded)
                                errors.Add(string.Join(",", addToProjectResult.Errors) + "没有成功添加入项目");
                            else
                            {
                                succeededList.Add(v);
                                
                            }
                        }
                    }
                    else
                    {
                        var addToProjectResult = pphelper.SingleVolunteerInputById(v.StudentNum, project);
                        if (!addToProjectResult.Succeeded)
                            errors.Add(string.Join(",", addToProjectResult.Errors) + "没有成功添加入项目");
                        else
                        {
                            succeededList.Add(v);
                        }

                    }
                   
                }
            }
            foreach (string error in errors)
            {
                MessageBox.Show(error);
                //ModernDialog.ShowMessage("学号为:["+failedNum+"]的志愿者添加失败!\n"+failedNum,"错误信息",MessageBoxButton.OK);
            }
            foreach(csvItemViewModel vm in succeededList)
            {
                csvList.Remove(vm);
            }
            if (csvList.Count != 0)
            {
                MessageBox.Show("最终，共往项目中添加:" + succeededList.Count + "位志愿者.\n现在显示的是没有选择的志愿者跟添加失败的志愿者.\n若不需要添加，请关闭该窗口.", "添加成功", MessageBoxButton.OK);
                csvGrid.ItemsSource = null;
                csvGrid.ItemsSource = csvList;
            }
            else
            {
                MessageBox.Show("最终，共往项目中添加: " + succeededList.Count + "位志愿者.\n所有列表中的志愿者已经添加完毕.");

                fatherWindow.Close();
            }
            
            
        }

        private void csvGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            csvItemViewModel vm = csvGrid.SelectedItem as csvItemViewModel;
            vm.Selected = !vm.Selected;
        }

        private void csvGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            csvItemViewModel vm = csvGrid.SelectedItem as csvItemViewModel;
            vm.Selected = !vm.Selected;
        }
    }
}
