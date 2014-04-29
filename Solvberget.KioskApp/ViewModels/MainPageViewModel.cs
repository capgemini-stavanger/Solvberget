using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solvberget.KioskApp.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {

        private string _test;
        public string Test
        {
            get { return _test; }
            set { _test = value; NotifyPropertyChanged("Test"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
