using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using FirstFloor.ModernUI.Windows.Controls;
using VolunteerDatabase.Helper;
using VolunteerDatabase.Interface;

namespace VolunteerDatabase.Desktop
{
    /// <summary>
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Window
    {
        

        protected WelcomePage()
        {
            InitializeComponent();
        }


        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
