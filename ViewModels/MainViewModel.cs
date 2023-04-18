using Sudoku.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Sudoku.ViewModels
{
    public class MainViewModel : DependencyObject
    {
        #region DependencyProperty

        public static readonly DependencyProperty MistakesTextProperty;
        public static readonly DependencyProperty TimerTextProperty;

        static MainViewModel()
        {
            MistakesTextProperty = DependencyProperty.Register("MistakesText", typeof(string), typeof(MainViewModel));
            TimerTextProperty = DependencyProperty.Register("TimerText", typeof(string), typeof(MainViewModel));
        }
        public string MistakesText
        {
            get { return (string)GetValue(MistakesTextProperty); }
            set { SetValue(MistakesTextProperty, value); }
        }
        public string TimerText
        {
            get { return (string)GetValue(TimerTextProperty); }
            set { SetValue(TimerTextProperty, value); }
        }
        #endregion DependencyProperty

        const float tickInterval = 1f;  
        
        public IMainView MainView { get; set; }

        public MainViewModel()
        {
            Director.Instance().MainViewModel = this;

            var dueTime = TimeSpan.FromSeconds(tickInterval);
            var interval = TimeSpan.FromSeconds(tickInterval);
            _ = RunPeriodicAsync(OnTick, dueTime, interval, CancellationToken.None);
        }

        public void GenerateNumbers()
        {
            UpdateMistakes();
            UpdateTimer();

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
                                        Director.Instance().IsPause = true;

                                        MessageBox.Show($"Victory!\n\nDificulty: {Director.Instance().GameLevel}" +
                                            $"\n\nTime: {string.Format("{0:00}:{1:00}", Director.Instance().Time / 60, Director.Instance().Time % 60)}",
                                            caption: "",
                                            button: MessageBoxButton.OK,
                                            icon: MessageBoxImage.Information,
                                            defaultResult: MessageBoxResult.None);
                                        SaveLoadManager.Instance().SaveGame();
                                    }
                                }
                                else
                                {
                                   
                                    Director.Instance().Mistakes++;
                                    UpdateMistakes();

                                    if (Director.Instance().Mistakes > 2) //defeat
                                    {
                                        Director.Instance().IsPause = true;
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

                                        SudokuLogics.Instance().Matrix[(int)pos.Y][(int)pos.X] = number;
                                    }
                                }                          
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

        private RelayCommand playPauseCommand;
        public RelayCommand PlayPauseCommand => playPauseCommand ??
                    (playPauseCommand = new RelayCommand(obj =>
                    {
                        Director.Instance().IsPause = !Director.Instance().IsPause;
                        MainView.SetPlayPauseImage();
                    }));

        public void NewGame()
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            Director.Instance().DeselectAllRows();
            GenerateNumbers();

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Arrow;
        }

        public void UpdateMistakes()
        {
            MistakesText = $"Mistakes: {Director.Instance().Mistakes}/3";                      
        }

        public void UpdateTimer()
        {
            TimerText = string.Format("{0:00}:{1:00}", Director.Instance().Time/60, Director.Instance().Time%60); 
        }

        private static async Task RunPeriodicAsync(Action onTick, TimeSpan dueTime, TimeSpan interval, CancellationToken token)
        {
            if (dueTime > TimeSpan.Zero)
                await Task.Delay(dueTime, token);

            while (!token.IsCancellationRequested)
            {
                onTick?.Invoke();

                if (interval > TimeSpan.Zero)
                    await Task.Delay(interval, token);
            }
        }

        private void OnTick()
        {
            if (Director.Instance().IsPause)
                return;

            Director.Instance().Time++;
            UpdateTimer();
        }
    }   

    public interface IMainView
    {
        void SetPlayPauseImage();
    }
}
