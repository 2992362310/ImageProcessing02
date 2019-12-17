using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing02
{
    class ProgressBar : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private double _minimum;
        public double Minimum
        {
            get { return this._minimum; }
            set
            {
                this._minimum = value;
                RaisePropertyChanged("Minimum");
            }
        }

        private double _maximum = 100;
        public double Maximum
        {
            get { return this._maximum; }
            set
            {
                this._maximum = value;
                RaisePropertyChanged("Maximum");
            }
        }

        private double _value;
        public double Value
        {
            get { return _value; }
            set
            {
                this._value = value;
                RaisePropertyChanged("Value");
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            if(this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
