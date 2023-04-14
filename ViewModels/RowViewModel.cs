using Sudoku.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sudoku.ViewModels
{
    public class RowViewModel : DependencyObject
    {
        public UIElement thisElement { get; set; }  

        public UIElementCollection Buttons { get; set; }

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
                        if(int.TryParse(obj.ToString(), out int index))
                        {
                            UIElement button = Buttons[index];                          
                           // Console.WriteLine("col: " + button.GetCol() + ", row: " + thisElement.GetRow());
                          // Console.WriteLine(SudokuLogics.Instance().IsSafe(thisElement.GetRow(), button.GetCol(), 1));
                        }                                                     
                    }));
    }

}
