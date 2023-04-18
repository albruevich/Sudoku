using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Sudoku.Models
{
    public class SudokuLogics
    {
        private static SudokuLogics instance;
        public static SudokuLogics Instance()
        {
            if (instance == null)
                instance = new SudokuLogics();
            return instance;
        }

        #region Fields

        private Random _rand = new Random();
        public int[][] Matrix { get; set; }

        public Vector CurrentPosition { get; set; }

        private int[][] SolvedMatrix { get; set; }

        #endregion Fields

        #region Public Methods

        public void Generate(int easyBonus)
        {
            CurrentPosition = new Vector();

            int[][] grid = new int[9][];
            SolvedMatrix = new int[9][];

            for (int i = 0; i < 9; i++)
            {
                grid[i] = new int[9];            
                for (int j = 0; j < 9; j++)                
                    grid[i][j] = 0;              
            }
            Solve(grid);
  
            for (int i = 0; i < _rand.Next(300, 600); i++ )
            {
                switch(_rand.Next(0, 5))
                {
                    case 0: SwapRows(grid); break;
                    case 1: SwapCols(grid); break;
                    case 2: SwapBoxVertically(grid); break;
                    case 3: SwapBoxHorizontally(grid); break;
                    case 4: SwapRowsAndColls(grid); break;
                }               
            }
        
            for (int i = 0; i < 9; i++)
            {               
                SolvedMatrix[i] = new int[9];
                for (int j = 0; j < 9; j++)                                  
                    SolvedMatrix[i][j] = grid[i][j];                
            }         

            HideCells(grid, easyBonus);
            Matrix = grid;           
        }

        public void Solve(int[][] grid)
        {
            var guessArray = Enumerable.Range(1, 9).OrderBy(o => _rand.Next()).ToArray();           
            BackTracking(grid, guessArray);
        }

        public void Print(int[][] grid)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(grid[i][j]);
                    if (j % 3 == 2) Console.Write("  ");
                }
                Console.WriteLine();
                if (i % 3 == 2)                
                    Console.WriteLine();                
            }
        }       

        public bool IsSameAsOriginal(int row, int col, int value)
        {            
            return SolvedMatrix[row][col] == value;
        }

        #endregion Public Methods

       
        #region BackTrack

        private bool BackTracking(int[][] grid, int[] guessArray)
        {
            int row = 0, col = 0;

            if (!FindEmptyLocation(grid, ref row, ref col))
                return true;

            for (int num = 0; num < 9; num++)
            {
                if (IsSafe(grid, row, col, guessArray[num]))
                {
                    grid[row][col] = guessArray[num];

                    if (BackTracking(grid, guessArray))
                        return true;

                    grid[row][col] = 0;
                }
            }

            return false;
        }

        private bool FindEmptyLocation(int[][] grid, ref int row, ref int col)
        {
            for (row = 0; row < 9; row++)
                for (col = 0; col < 9; col++)
                    if (grid[row][col] == 0)
                        return true;
            return false;
        }

        private bool UsedInRow(int[][] grid, int row, int value)
        {
            for (int i = 0; i < 9; i++)
                if (grid[row][i] == value)
                    return true;
            return false;
        }

        private bool UsedInCol(int[][] grid, int col, int value)
        {
            for (int i = 0; i < 9; i++)
                if (grid[i][col] == value)
                    return true;
            return false;
        }

        private bool UsedInBox(int[][] grid, int boxStartRow, int boxStartCol, int value)
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (grid[i + boxStartRow][j + boxStartCol] == value)
                        return true;
            return false;
        }

        private bool IsSafe(int[][] grid, int row, int col, int value)
        {
            return !UsedInRow(grid, row, value) && !UsedInCol(grid, col, value) && !UsedInBox(grid, row - row % 3, col - col % 3, value);
        }

        #endregion BackTrack

        #region Unique

        private void SwapRowsAndColls(int[][] grid)
        {           
            int cut = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (j < cut || i < cut)
                        continue;

                    int temp = grid[i][j];
                    grid[i][j] = grid[j][i];
                    grid[j][i] = temp;                   
                }
                cut++;
            }          
        }

        private void SwapRows(int[][] grid)
        {
            int randomRow = _rand.Next(0, 3);
            var rowsArray = Enumerable.Range(0, 3).OrderBy(o => _rand.Next()).ToArray();
            int row1 = randomRow * 3 + rowsArray[0];
            int row2 = randomRow * 3 + rowsArray[1];

            for (int i = 0; i < 9; i++ )
            {
                int temp = grid[row1][i];
                grid[row1][i] = grid[row2][i];
                grid[row2][i] = temp;
            }
        }      

        private void SwapCols(int[][] grid)
        {
            int randomCol = new Random().Next(0, 3);
            var colsArray = Enumerable.Range(0, 3).OrderBy(o => _rand.Next()).ToArray();
            int col1 = randomCol * 3 + colsArray[0];
            int col2 = randomCol * 3 + colsArray[1];

            for (int i = 0; i < 9; i++)
            {
                int temp = grid[i][col1];
                grid[i][col1] = grid[i][col2];
                grid[i][col2] = temp;
            }          
        }

        private void SwapBoxVertically(int[][] grid)
        {           
            var boxArray = Enumerable.Range(0, 3).OrderBy(o => _rand.Next()).ToArray();
            int boxCol1 = boxArray[0];
            int boxCol2 = boxArray[1];
           
            for (int i = 0; i < 3; i++)
            {
                for (int row = 0; row < 9; row++)
                {
                    int b1 = boxCol1 * 3 + i;
                    int b2 = boxCol2 * 3 + i;

                    int temp = grid[row][b1];
                    grid[row][b1] = grid[row][b2];
                    grid[row][b2] = temp;
                }              
            }          
        }

        private void SwapBoxHorizontally(int[][] grid)
        {
            var boxArray = Enumerable.Range(0, 3).OrderBy(o => _rand.Next()).ToArray();
            int boxRow1 = boxArray[0];
            int boxRow2 = boxArray[1];           

            for (int i = 0; i < 3; i++)
            {
                for (int col = 0; col < 9; col++)
                {
                    int b1 = boxRow1 * 3 + i;
                    int b2 = boxRow2 * 3 + i;

                    int temp = grid[b1][col];
                    grid[b1][col] = grid[b2][col];
                    grid[b2][col] = temp;
                }
            }
        }

        
        private void HideCells(int[][] grid, int easyBonus)
        {
            var randomIndexes = Enumerable.Range(0, 81).OrderBy(o => _rand.Next()).ToArray();
            var guessArray = Enumerable.Range(1, 9).OrderBy(o => _rand.Next()).ToArray();

            for (int i = 0; i < 81; i++)
            {                
                int x = randomIndexes[i] / 9;
                int y = randomIndexes[i] % 9;               
                
                int temp = grid[x][y];
                grid[x][y] = 0;

                int check = 0;
                CheckUniqueness(grid, guessArray, ref check);              
                if (check > 1)
                {
                    grid[x][y] = temp;                  
                }
            }

            easyBonus = Math.Min(easyBonus, 20);

            while (easyBonus > 0)
            {
                int randX = _rand.Next(0, 9);
                int randY = _rand.Next(0, 9);
                if (grid[randY][randX] == 0)
                {
                    grid[randY][randX] = SolvedMatrix[randY][randX];
                    easyBonus--;
                }
            }
        }

        private void CheckUniqueness(int[][] grid, int[] guessArray, ref int number)
        {
            int row = 0, col = 0;

            if (!FindEmptyLocation(grid, ref row, ref col))
            {
               // Console.WriteLine("not foond, number: " + number +  ", col: " + col + ", row: " + row);

                number++;
                return;
            }

          // Console.WriteLine("col: " + col + ", row: " + row);           

            for (int i = 0; i < 9 && number < 2; i++)
            {
                if (IsSafe(grid, row, col, guessArray[i]))
                {
                    grid[row][col] = guessArray[i];                   
                    CheckUniqueness(grid, guessArray, ref number);
                }

                grid[row][col] = 0;
            }
        }
        
        #endregion Unique


    }
}
