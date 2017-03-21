using System.Windows.Controls;
using FirstFloor.ModernUI.Windows.Controls;

namespace VolunteerDatabase.Desktop.TipsPages
{
    /// <summary>
    /// Interaction logic for ModernDialog1.xaml
    /// </summary>
    public partial class LoginDialog : ModernDialog
    {
        public LoginDialog()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] { this.OkButton, this.CancelButton };
        }
    }
}
