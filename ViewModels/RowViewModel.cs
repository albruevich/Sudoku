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
using System.Windows.Media;
using System.Xml.Linq;

namespace Sudoku.ViewModels
{
    public class RowViewModel : DependencyObject
    {
        UIElement thisElement;
        public UIElement ThisElement
        {
            get { return thisElement; }
            set
            {                
                thisElement = value;
                rowView = (RowView)thisElement;
            }
        }
        RowView rowView;

        public RowViewModel()
        {
            Director.Instance().RowViewModels.Add(this);
        }

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
                            Button button = Buttons[index] as Button;
                            SudokuLogics.Instance().currentPosition = new Vector(button.GetCol(), thisElement.GetRow());

                            Director.Instance().DeselectAllRows();
                            button.Background = (SolidColorBrush)Application.Current.FindResource("SelectedColor");
                        }                                                     
                    }));

        public void UpdateButtons(int[] numbers)
        {           
            rowView.SetNumbers(numbers);
        }

        public void DeselectAllButtons()
        {
            SolidColorBrush brush = (SolidColorBrush)Application.Current.FindResource("GameButtonsColor");

            foreach (UIElement element in Buttons)
            {
                Button button = element as Button;
                button.Background = brush;
            }                   
        }

        public void SelectFirstButton()
        {
            Button button = Buttons[0] as Button;
            button.Background = (SolidColorBrush)Application.Current.FindResource("SelectedColor");
        }
    }

}
