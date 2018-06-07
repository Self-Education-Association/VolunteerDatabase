using System.Collections.Generic;
using System;
using System.Windows;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Helper;

namespace VolunteerDatabase.Desktop.Pages
{
    /// <summary>
    /// Interaction logic for BlackList.xaml
    /// </summary>
    public partial class BlackList : Window
    {
        private Volunteer volunteer;
        BlackListHelper bhelper;
        List<BlackListRecord> sourceList;
        Project project;
        IdentityPage id= IdentityPage.GetInstance();

        private int BlkPgeIndex { get; set; }
        private int MaxBlkItems = 5;
        private int BlkCount { get { return volunteer.BlackListRecords.Count; } }


        public BlackList(Volunteer v,Project p)
        {
            InitializeComponent();
            volunteer = v;
            project = p;
            bhelper = BlackListHelper.GetInstance();
            if (project == null) btnAdd.IsEnabled = false;
            sourceList = v.BlackListRecords;
            ShowBlkGrid();
        }


        private void ShowBlkGrid()
        {
            List<BlackListRecord> records = new List<BlackListRecord>();
            List<BlackListViewModel> viewRecords = new List<BlackListViewModel>();
            for (int i = BlkPgeIndex; i <= BlkPgeIndex + MaxBlkItems; i++)//?
            {
                if (i > BlkCount - 1) break;
                records.Add(volunteer.BlackListRecords[i]);
            }
            BlkPge.Content = string.Format("{0}/{1}", BlkPgeIndex / MaxBlkItems + 1, BlkCount / MaxBlkItems + 1);
            LblBlkListEpt.Visibility = volunteer.BlackListRecords.Count == 0 ? Visibility.Visible : Visibility.Hidden;
            foreach (BlackListRecord r in records)
            {
                viewRecords.Add(new BlackListViewModel(r));
            }
            dgBlk.ItemsSource = viewRecords;
            //把ItemSource连到ViewModel上面去
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //add a new record.
            //test function. No new window
            AddBlackList addBlack = new AddBlackList(volunteer, project);
            addBlack.FinishAddEvent += ShowBlkGrid;
            addBlack.Show();
            //bhelper.AddBlackListRecord("暴力膜蛤。", volunteer, id.Claims.User, DateTime.Now.AddYears(100), id.Claims.User.Organization, project);//如果用上holder 改成holder
            //MessageBox.Show("被黑了一波。");
        }



        private void BlkPgePrevious_Click(object sender, RoutedEventArgs e)
        {
            BlkPgeIndex = (BlkPgeIndex - MaxBlkItems < 0) ? BlkPgeIndex : BlkPgeIndex - MaxBlkItems;
            if (BlkPgeIndex - MaxBlkItems < 0) BlkPgePrevious.IsEnabled = false;
            BlkPgeNext.IsEnabled = true;
            ShowBlkGrid();
        }

        private void BlkPgeNext_Click(object sender, RoutedEventArgs e)
        {
            BlkPgeIndex = (BlkPgeIndex + MaxBlkItems > BlkCount) ? BlkPgeIndex : BlkPgeIndex + MaxBlkItems;
            if (BlkPgeIndex + MaxBlkItems > BlkCount) BlkPgeNext.IsEnabled = false;
            BlkPgePrevious.IsEnabled = true;
            ShowBlkGrid();
        }
    }
}
