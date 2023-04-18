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
        SolidColorBrush selectedNumberBrush, selectedBorderBrush, borderBrush;
        SolidColorBrush forgroundColor, badForgroundColor;

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
            selectedNumberBrush = (SolidColorBrush)Application.Current.FindResource("SelectedNumberColor");
            selectedBorderBrush = (SolidColorBrush)Application.Current.FindResource("SelectedBorderColor");
            borderBrush = (SolidColorBrush)Application.Current.FindResource("BorderColor");

            forgroundColor = (SolidColorBrush)Application.Current.FindResource("ForgroundColor");
            badForgroundColor = (SolidColorBrush)Application.Current.FindResource("BadForgroundColor");
        }

        public UIElementCollection Buttons { get; set; }    

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
                button.BorderThickness = new Thickness(1f);
                button.BorderBrush = borderBrush;
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
            Director.Instance().SelectCol();
            SelectThisRow();           
            Director.Instance().SelectAllNumbers((int)button.Content);
            button.Background = selectedCellBrush;
            button.BorderBrush = selectedBorderBrush;
            button.BorderThickness = new Thickness(2f);
        }

        public void SelectGroupOf3(int col)
        {           
            for (int c = col / 3 * 3; c < col / 3 * 3 + 3; c++)
            {
                Button button = Buttons[c] as Button;
                button.Background = selectedBrush;
            }          
        }

        public void SelectCellInCol(int col)
        {
            Button button = Buttons[col] as Button;
            button.Background = selectedBrush;
        }

        private void SelectThisRow()
        {      
            foreach(Button button in Buttons)            
                button.Background = selectedBrush;            
        }

        public void SelectAllNumbers(int number)
        {
            foreach (Button button in Buttons)
            {
                if(number != 0 && (int)button.Content == number )
                    button.Background = selectedNumberBrush;
            }
        }

        public void CheckAllNumbersForCorrectness()
        {
            int row = thisElement.GetRow();
            for(int col = 0; col < 9; col++)
            {
                Button button = (Button)Buttons[col];
                
                if (SudokuLogics.Instance().Matrix[row][col] != 0)
                {                
                    if (SudokuLogics.Instance().Matrix[row][col] != SudokuLogics.Instance().SolvedMatrix[row][col])
                        button.Foreground = badForgroundColor;
                    else
                        button.Foreground = forgroundColor;
                }
                else                
                    button.Foreground = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));                
            }
        }
    }

}
