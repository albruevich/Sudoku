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

            //test test
        }

        public void SetNumbers(int[] numbers)
        {
            rowViewModel.Numbers = numbers;
        }
    }
}
