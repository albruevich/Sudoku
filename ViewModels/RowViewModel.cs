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
        SolidColorBrush gameButtonsBrush, selectedBrush, selectedCellBrush;

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

            gameButtonsBrush = (SolidColorBrush)Application.Current.FindResource("GameButtonsColor");
            selectedBrush = (SolidColorBrush)Application.Current.FindResource("SelectedColor");
            selectedCellBrush = (SolidColorBrush)Application.Current.FindResource("SelectedCellColor");
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
                            Director.Instance().DeselectAllRows();
                            SelectButton(button);
                        }                                                     
                    }));

        public void UpdateButtons(int[] numbers)
        {           
            rowView.SetNumbers(numbers);
        }

        public void DeselectAllButtons()
        {
            foreach (UIElement element in Buttons)
            {
                Button button = element as Button;
                button.Background = gameButtonsBrush;
            }                   
        }

        public void SelectFirstButton()
        {           
            SelectButton(Buttons[0] as Button);
        }

        private void SelectButton(Button button)
        {            
            SudokuLogics.Instance().CurrentPosition = new Vector(button.GetCol(), thisElement.GetRow());
            Director.Instance().SelectedButton = button;
            Director.Instance().SelectBox();
            SelectThisRow();
            button.Background = selectedCellBrush;
        }

        public void SelectGroupOf3(int col)
        {           
            for (int c = col / 3 * 3; c < col / 3 * 3 + 3; c++)
            {
                Button button = Buttons[c] as Button;
                button.Background = selectedBrush;
            }          
        }

        private void SelectThisRow()
        {      
            foreach(Button button in Buttons)            
                button.Background = selectedBrush;            
        }
    }

}
