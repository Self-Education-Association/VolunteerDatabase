using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerDatabase.Entity;
using System.Windows;
using System.Windows.Controls;
using FirstFloor.ModernUI.Windows.Controls;
using System.ComponentModel;

namespace VolunteerDatabase.Desktop.Pages
{
    public class csvItemViewModel:INotifyPropertyChanged
    {
        private bool _Selected = false;

        private bool _IsOld = false;

        public csvItemViewModel Pair { get; set; }
        public bool IsOld { get { return _IsOld; } set { _IsOld = value; GetChanged("IsOld"); } }

        public bool Selected
        {
            get { return _Selected; }
            set {
                    _Selected = value;
                    if (value==true&&Pair!=null)
                    {
                        Pair.Selected = false;
                    }
                        GetChanged("Selected");
                }
        }

        public string StudentNum { get { return Volunteer.StudentNum; } }
        //public long StudentNum { get { return Volunteer.StudentNum; } }

        public string Name { get { return Volunteer.Name; } }
        public string Class { get { return Volunteer.Class; } }
        public string Mobile { get { return Volunteer.Mobile; } }
        public double AvgScore { get { return Volunteer.AvgScore; } }
        public Volunteer Volunteer { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private void GetChanged(string Name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(Name));
            }
        }

        public csvItemViewModel(Volunteer v,csvItemViewModel pair = null)
        {
            Volunteer = v;
            _Selected = false;
            Pair = pair;
        }
    }
}
