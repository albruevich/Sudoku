using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sudoku.ViewModels
{
    public class RowViewModel : DependencyObject
    {
        public static readonly DependencyProperty NumbersProperty;
        public int[] Numbers
        {
            get { return (int[])GetValue(NumbersProperty); }
            set { SetValue(NumbersProperty, value); }
        }
        static RowViewModel()
        {           
            NumbersProperty = DependencyProperty.Register("Numbers", typeof(int[]), typeof(RowViewModel));
        }

        private RelayCommand сlickCommand;
        public RelayCommand СlickCommand => сlickCommand ??
                    (сlickCommand = new RelayCommand(obj =>
                    {
                        Console.WriteLine("СlickCommand");
                    }));
    }

}
