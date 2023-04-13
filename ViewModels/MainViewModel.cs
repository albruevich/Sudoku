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
        
        int[][] Matrix { get; set; }

        SudokuLogics sudokuLogics;       

        public void GenerateNumbers()
        {
            sudokuLogics = new SudokuLogics();
            Matrix = sudokuLogics.Generate();
            sudokuLogics.Print(Matrix);
             
            MainView.UpdateRows(Matrix);
        }      
    }   

    public interface IMainView
    {
        void UpdateRows(int[][] Numbers);
    }
}
