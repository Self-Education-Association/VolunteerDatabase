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

        #region 认证部分
        private AppUserIdentityClaims Claims { get; set; }
        private IdentityPage identitypage = IdentityPage.GetInstance();
        #endregion

        #region Helper部分
        private CsvHelper chelper;
        private VolunteerHelper vhelper;
        private ProjectProgressHelper pphelper;
        #endregion

        #region 父窗体
        private InputWindow fatherWindow;
        #endregion

        #region 核心显示列表
        private List<csvItemViewModel> csvList;
        private List<DataGridRow> rows = new List<DataGridRow>();
        #endregion

        #region 分页部分
        //Cdd: Candidate
        private int CddCount { get { return csvList.Count; } }
        private int CddPgeIndex { get; set; }
        private int MaxCddItems = 20;
        #endregion

        private Project project;
        private bool hasAllSelected = false;
        public Csvviewer(Project p, List<csvItemViewModel> list,InputWindow window)//csv导入后弹出,加载页面=>datagrid列表显示，若有冲突:"一个以上冲突，请您确认",对应行标红,保留与否：按钮.保留原数据/新数据，下一步:更改的确认,导入成功，detail页面的显示
        {
            InitializeComponent();
            fatherWindow = window;
            vhelper = VolunteerHelper.GetInstance();
            pphelper = ProjectProgressHelper.GetInstance();
            project = p;
            chelper = CsvHelper.GetInstance();
            csvList = list;
            ShowCddGrid();
            Claims = identitypage.Claims;
        }

        private void ShowCddGrid()
        {
            List<csvItemViewModel> cdds = new List<csvItemViewModel>();
            for (int i = CddPgeIndex; i <= CddPgeIndex + MaxCddItems; i++)
            {
                if (i > CddCount - 1) break;
                cdds.Add(csvList[i]);
            }
            CddPge.Content = string.Format("{0}/{1}", CddPgeIndex / MaxCddItems + 1, CddCount / MaxCddItems + 1);
            csvGrid.ItemsSource = cdds;
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
            foreach (var vm in csvList)
            {
                vm.Selected = hasAllSelected ? true : false;
            }
            hasAllSelected = !hasAllSelected;
        }

        private void SelectNotSelected_Click(object sender, RoutedEventArgs e)
        {
            foreach (var vm in csvList)
            {
                vm.Selected = !vm.Selected;
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
                            {
                                errors.Add(string.Join(",", addToProjectResult.Errors) + "没有成功添加入项目");
                            }
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
                        {
                            errors.Add(string.Join(",", addToProjectResult.Errors) + "没有成功添加入项目");
                        }
                        else
                        {
                            succeededList.Add(v);
                        }
                    }
                }
            }
            string errorstring = string.Join("\n",errors);
            if(errorstring!="")
                MessageBox.Show(errorstring);
            foreach(csvItemViewModel vm in succeededList)
            {
                csvList.Remove(vm);
            }
            if (csvList.Count != 0)
            {
                MessageBox.Show("最终，共往项目中添加:" + succeededList.Count + "位志愿者.\n现在显示的是没有选择的志愿者跟添加失败的志愿者.\n若不需要添加，请关闭该窗口.", "添加成功", MessageBoxButton.OK);
                ShowCddGrid();
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

        private void CddPgeNext_Click(object sender, RoutedEventArgs e)
        {
            CddPgeIndex = (CddPgeIndex + MaxCddItems > CddCount) ? CddPgeIndex : CddPgeIndex + MaxCddItems;
            if (CddPgeIndex + MaxCddItems > CddCount) CddPgeNext.IsEnabled = false;
            CddPgePrevious.IsEnabled = true;
            ShowCddGrid();
        }

        private void CddPgePrevious_Click(object sender, RoutedEventArgs e)
        {
            CddPgeIndex = (CddPgeIndex - MaxCddItems < 0) ? CddPgeIndex : CddPgeIndex - MaxCddItems;
            if (CddPgeIndex - MaxCddItems < 0) CddPgePrevious.IsEnabled = false;
            CddPgeNext.IsEnabled = true;
            ShowCddGrid();
        }
    }
}
