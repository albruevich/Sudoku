using Sudoku.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sudoku.Models;

namespace Sudoku
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainView
    {      
        bool ignoreComboboxInStart = true;

        public MainWindow()
        {
            InitializeComponent();

            mainViewModel.MainView = this;

            if (!SaveLoadManager.Instance().LoadGame())           
                mainViewModel.GenerateNumbers();
            else
            {
                Director.Instance().UpdateRows(SudokuLogics.Instance().Matrix);
                Director.Instance().SelectFirstButton();
                Director.Instance().CheckAllNumbersForCorrectness();
                mainViewModel.UpdateMistakes();
                mainViewModel.UpdateTimer();
            }

            levelComboBox.SelectedIndex = (int)Director.Instance().GameLevel;
            ignoreComboboxInStart = false;
        }

        private void LevelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ignoreComboboxInStart)
                return;

            Director.Instance().GameLevel = (GameLevel)levelComboBox.SelectedIndex;
            Director.Instance().NewGame();
        }

        public void SetPlayPauseImage()
        {
            if (Director.Instance().IsPause)
            {
                playPauseButton.Content = FindResource("icon_play");
                regionsGrid.Visibility = Visibility.Hidden;
                framesGrid.Visibility = Visibility.Hidden;
                bigPlayNode.Visibility = Visibility.Visible;
            }
            else
            {
                playPauseButton.Content = FindResource("icon_pause");
                regionsGrid.Visibility = Visibility.Visible;
                framesGrid.Visibility = Visibility.Visible;
                bigPlayNode.Visibility = Visibility.Hidden;
            }
        }
    }
}
