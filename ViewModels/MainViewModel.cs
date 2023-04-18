using Sudoku.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Sudoku.ViewModels
{
    public class MainViewModel 
    { 
        public MainViewModel()
        {
            Director.Instance().MainViewModel = this;          
        }

        public void GenerateNumbers()
        {
            int easyBonus = 0;
            switch(Director.Instance().GameLevel)
            {
                case GameLevel.Easy: easyBonus = 14; break;
                case GameLevel.Medium: easyBonus = 7; break;
                case GameLevel.Hard: easyBonus = 0; break;
            }

            SudokuLogics.Instance().Generate(easyBonus);
            Director.Instance().UpdateRows(SudokuLogics.Instance().Matrix);
            Director.Instance().SelectFirstButton();
        }

        private RelayCommand сlickCommand;
        public RelayCommand СlickCommand => сlickCommand ??
                    (сlickCommand = new RelayCommand(obj =>
                    {
                        if (int.TryParse(obj.ToString(), out int number))
                        {                           
                            Vector pos = SudokuLogics.Instance().CurrentPosition;                            

                            if (!SudokuLogics.Instance().IsSameAsOriginal(row: (int)pos.Y, col: (int)pos.X, (int)Director.Instance().SelectedButton.Content))
                            {
                                Director.Instance().SelectedButton.Content = number;

                                if (SudokuLogics.Instance().IsSameAsOriginal(row: (int)pos.Y, col: (int)pos.X, number))
                                {
                                    Director.Instance().SelectedButton.Foreground = (SolidColorBrush)Application.Current.FindResource("ForgroundColor");
                                }
                                else
                                {
                                    Director.Instance().SelectedButton.Foreground = (SolidColorBrush)Application.Current.FindResource("BadForgroundColor");
                                }

                                // Console.WriteLine(SudokuLogics.Instance().IsSafe(row: (int)pos.Y, col: (int)pos.X, number));
                            }
                        }
                    }));

        private RelayCommand newGameCommand;
        public RelayCommand NewGameCommand => newGameCommand ??
                    (newGameCommand = new RelayCommand(obj =>
                    {
                        Director.Instance().NewGame();
                    }));

        public void NewGame()
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            Director.Instance().DeselectAllRows();
            GenerateNumbers();

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Arrow;
        }
    }   
}
