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

namespace Sudoku
{
    /// <summary>
    /// Логика взаимодействия для RowView.xaml
    /// </summary>
    public partial class RowView : UserControl
    {
        public RowView()
        {
            InitializeComponent();
            rowViewModel.ThisElement = this;
        }

        public void SetNumbers(int[] numbers)
        {          
            rowViewModel.Buttons = buttonsGrid.Children;
            rowViewModel.CheckAllNumbersForCorrectness();                
                
            int i = 0;
            foreach(Button button in buttonsGrid.Children)
            {
                button.Content = numbers[i];           
                i++;
            }
        }
    }
}
