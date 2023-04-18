﻿using Sudoku.Models;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

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
                                    Director.Instance().SelectedButton.Foreground = 
                                    (SolidColorBrush)Application.Current.FindResource("ForgroundColor");

                                    //check for victory
                                    int all = 0;
                                    for (int i = 0; i < 9; i++)
                                        for (int j = 0; j < 9; j++)
                                            if (SudokuLogics.Instance().Matrix[i][j] != 0)
                                                all++;

                                    //victory
                                    if (all == 81)
                                    {
                                        MessageBox.Show($"Victory!\n\nDificulty: {Director.Instance().GameLevel}",
                                            caption: "",
                                            button: MessageBoxButton.OK,
                                            icon: MessageBoxImage.Information,
                                            defaultResult: MessageBoxResult.None);
                                    }
                                }
                                else
                                {                                 
                                    Director.Instance().Mistakes++;

                                    if (Director.Instance().Mistakes > 2)
                                    {
                                        MessageBox.Show($"Defeat!\n\nDificulty: {Director.Instance().GameLevel}",
                                           caption: "",
                                           button: MessageBoxButton.OK,
                                           icon: MessageBoxImage.Error,
                                           defaultResult: MessageBoxResult.None);

                                        Director.Instance().NewGame();  
                                        SaveLoadManager.Instance().SaveGame();  
                                    }
                                    else
                                    {
                                        Director.Instance().SelectedButton.Foreground =                                   
                                        (SolidColorBrush)Application.Current.FindResource("BadForgroundColor");
                                    }
                                }

                                SudokuLogics.Instance().Matrix[(int)pos.Y][(int)pos.X] = number;

                               
                            }
                        }
                    }));

        private RelayCommand newGameCommand;
        public RelayCommand NewGameCommand => newGameCommand ??
                    (newGameCommand = new RelayCommand(obj =>
                    {
                        Director.Instance().NewGame();
                        SaveLoadManager.Instance().SaveGame();
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
