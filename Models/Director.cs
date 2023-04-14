using Sudoku.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
