using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstFloor.ModernUI.Windows.Controls;
using System.Windows;

namespace VolunteerDatabase.Desktop.Pages
{
    internal static class InformAndWarning
    {
        internal static void Inform(string content)
        {
            ModernDialog.ShowMessage(content, "提示", MessageBoxButton.OK);

        }
    }
}
