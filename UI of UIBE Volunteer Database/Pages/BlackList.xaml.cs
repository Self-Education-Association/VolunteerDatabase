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
using VolunteerDatabase.Helper;
using VolunteerDatabase.Entity;
using VolunteerDatabase.Interface;


namespace Desktop.Pages
{
    /// <summary>
    /// Interaction logic for BlackList.xaml
    /// </summary>
    public partial class BlackList : Window
    {
        private Volunteer Vol;
        List<BlackListRecord> sourceList;
        public BlackList(Volunteer vol)
        {
            Vol = vol;
            InitializeComponent();
            sourceList = Vol.BlackListRecords;
        }
        //未作绑定，待添加
    }
}
