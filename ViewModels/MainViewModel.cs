using Sudoku.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            SudokuLogics.Instance().Generate();
            Director.Instance().UpdateRows(SudokuLogics.Instance().Matrix);
            Director.Instance().SelectFirstButton();
        }

        private RelayCommand сlickCommand;
        public RelayCommand СlickCommand => сlickCommand ??
                    (сlickCommand = new RelayCommand(obj =>
                    {
                        if (int.TryParse(obj.ToString(), out int number))
                        {                           
                            Vector pos = SudokuLogics.Instance().currentPosition;                         
                            Console.WriteLine(SudokuLogics.Instance().IsSafe(row: (int)pos.Y, col: (int)pos.X, number));
                        }
                    }));     
    }   
}
