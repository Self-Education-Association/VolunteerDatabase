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
        private List<Volunteer> csvList;
        private List<DataGridRow> rows = new List<DataGridRow>();
        private Project project;
        private bool hasAllSelected = false;
        public Csvviewer(Project p, List<Volunteer> list,Window window)//csv导入后弹出,加载页面=>datagrid列表显示，若有冲突:"一个以上冲突，请您确认",对应行标红,保留与否：按钮.保留原数据/新数据，下一步:更改的确认,导入成功，detail页面的显示
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
            DataGridRow item = e.Row;
            if (item != null)
            {
                rows.Add(item);
            }

            //var item = e.Row.Item as Volunteer;
            //if (item != null)
            //{
            //   if (chelper.conflictNums.Contains(item.StudentNum))
            //   {
            //       e.Row.Background = new SolidColorBrush(Colors.Red);
            //   }
            //}

        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cbox;
            foreach (var item in rows)
            {
                cbox = csvGrid.Columns[0].GetCellContent(item) as CheckBox;
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
            foreach (Volunteer v in csvGrid.SelectedItems)
            {
                if(vhelper.FindVolunteer(v.StudentNum)==null)
                {
                    vhelper.AddVolunteer(v);
                }
                if (!pphelper.SingleVolunteerInputById(v.StudentNum, project).Succeeded)
                    errors.Add(v.StudentNum.ToString());
            }
            foreach (string error in errors)
            {
                ModernDialog.ShowMessage("学号为:["+error+"]的志愿者添加失败!","错误信息",MessageBoxButton.OK);
            }
            if(errors.Count()==0)
            {
                ModernDialog.ShowMessage("共添加:"+ csvGrid.SelectedItems.Count+"位志愿者.","添加成功",MessageBoxButton.OK);
                fatherWindow.Close();
            }
        }
    }
}
