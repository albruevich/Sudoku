using Sudoku.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Sudoku.Models
{
    public class Director
    {
        private static Director instance;
        public static Director Instance()
        {
            if (instance == null)
                instance = new Director();
            return instance;
        }

        public MainViewModel MainViewModel { get; set; }
        public List<RowViewModel> RowViewModels = new List<RowViewModel>();
        public Button SelectedButton { get; set; }

        public GameLevel GameLevel { get; set; }

        public Director()
        {
            GameLevel = GameLevel.Easy;
        }
                  
        public void DeselectAllRows()
        {
            foreach(RowViewModel row in RowViewModels)            
                row.DeselectAllButtons();                       
        }

        public void UpdateRows(int[][] numbers)
        {
            int i = 0;
            foreach (RowViewModel row in RowViewModels)
            {
                row.UpdateButtons(numbers[i]);
                i++;
            }          
        }

        public void SelectFirstButton()
        {           
            RowViewModels[0].SelectFirstButton();
        }

        public void SelectBox()
        {
            int col = (int)SudokuLogics.Instance().CurrentPosition.X;
            int row = (int)SudokuLogics.Instance().CurrentPosition.Y;
         
            for (int r = row / 3 * 3; r < row / 3 * 3 + 3; r++)
            {              
                 RowViewModel rowViewModel = RowViewModels[r];
                 rowViewModel.SelectGroupOf3(col);
            }
        }

        public void SelectCol()
        {
            int col = (int)SudokuLogics.Instance().CurrentPosition.X;           
            foreach(RowViewModel rowView in RowViewModels)
            {
                rowView.SelectCellInCol(col);
            }          
        }

        public void SelectAllNumbers(int number)
        {           
            foreach (RowViewModel rowView in RowViewModels)
                rowView.SelectAllNumbers(number);
        }

        public void NewGame()
        {
            MainViewModel.NewGame();
        }
    }

    public enum GameLevel
    {
        Easy = 0,
        Medium = 1,
        Hard = 2
    }
}
