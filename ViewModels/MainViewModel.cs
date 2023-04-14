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
        public IMainView MainView { get; set; }            

        public void GenerateNumbers()
        {           
            SudokuLogics.Instance().Generate();          
            MainView.UpdateRows(SudokuLogics.Instance().Matrix);
        }

        private RelayCommand сlickCommand;
        public RelayCommand СlickCommand => сlickCommand ??
                    (сlickCommand = new RelayCommand(obj =>
                    {
                        Console.WriteLine("obj: " + obj);

                        //if (int.TryParse(obj.ToString(), out int index))
                        {                        
                            //UIElement button = Buttons[index];
                            // Console.WriteLine("col: " + button.GetCol() + ", row: " + thisElement.GetRow());
                            // Console.WriteLine(SudokuLogics.Instance().IsSafe(thisElement.GetRow(), button.GetCol(), 1));
                        }
                    }));
    }   

    public interface IMainView
    {
        void UpdateRows(int[][] Numbers);
    }
}
