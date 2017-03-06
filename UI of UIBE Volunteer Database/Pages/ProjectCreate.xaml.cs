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

namespace Desktop.Pages
{
    /// <summary>
    /// Interaction logic for ProjectCreate.xaml
    /// </summary>
    public partial class ProjectCreate : UserControl
    {
        private IdentityPage basepage = IdentityPage.GetInstance();
        private AppUserIdentityClaims Claims { get; set; }
        public ProjectCreate()
        {
            Claims = basepage.Claims;
            //Login.GetClaims(sendClaimsEventHandler);
            InitializeComponent();
        }



        








    }
}
