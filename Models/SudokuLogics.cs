using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        public Vector currentPosition { get; set; }

        #endregion Fields

        #region Public Methods

        public void Generate()
        {
            currentPosition = new Vector();

            int[][] grid = new int[9][];           

            for (int i = 0; i < 9; i++)
            {
                grid[i] = new int[9];

                for (int j = 0; j < 9; j++)
                    grid[i][j] = 0;
            }

            Matrix = grid;

            Solve();
            MakeUnique();           
        }

        public void Solve()
        {
            var guessArray = Enumerable.Range(1, 9).OrderBy(o => _rand.Next()).ToArray();
            BactTracking(guessArray);
        }

        public void Print()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                    Console.Write(Matrix[i][j]);
                Console.WriteLine();
            }
        }

        public bool IsSafe(int row, int col, int value)
        {
            return !UsedInRow(Matrix, row, value) && !UsedInCol(Matrix, col, value) && !UsedInBox(Matrix, row - row % 3, col - col % 3, value);
        }

        #endregion Public Methods

        #region Private Methods

        #region BackTrack

        private bool BactTracking(int[] guessArray)
        {
            int row = 0, col = 0;

            if (!FindEmptyLocation(ref row, ref col))
                return true;

            for (int num = 0; num < 9; num++)
            {
                if (IsSafe(row, col, guessArray[num]))
                {
                    Matrix[row][col] = guessArray[num];

                    if (BactTracking(guessArray))
                        return true;

                    Matrix[row][col] = 0;
                }
            }

            return false;
        }

        private bool FindEmptyLocation(ref int row, ref int col)
        {
            for (row = 0; row < 9; row++)
                for (col = 0; col < 9; col++)
                    if (Matrix[row][col] == 0)
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

        #endregion BackTrack

        #region Unique

        private void MakeUnique()
        {
            var randomIndexes = Enumerable.Range(0, 81).OrderBy(o => _rand.Next()).ToArray();
            var guessArray = Enumerable.Range(1, 9).OrderBy(o => _rand.Next()).ToArray();

            for (int i = 0; i < 81; i++)
            {
                int x = randomIndexes[i] / 9;
                int y = randomIndexes[i] % 9;
                int temp = Matrix[x][y];
                Matrix[x][y] = 0;

                int check = 0;
                CheckUniqueness(guessArray, ref check);
                if (check != 1)
                {
                    Matrix[x][y] = temp;
                }
            }
        }

        private void CheckUniqueness(int[] guessArray, ref int number)
        {
            int row = 0, col = 0;

            if (!FindEmptyLocation(ref row, ref col))
            {
                number++;
                return;
            }

            for (int i = 0; i < 9 && number < 2; i++)
            {
                if (IsSafe(row, col, guessArray[i]))
                {
                    Matrix[row][col] = guessArray[i];
                    CheckUniqueness(guessArray, ref number);
                }

                Matrix[row][col] = 0;
            }
        }

        #endregion Unique

        #endregion Private Methods
    }
}
