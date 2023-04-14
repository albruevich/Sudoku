using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Sudoku
{
    public static class Extentions
    {
        //ROW
        public static readonly DependencyProperty Row = 
            DependencyProperty.RegisterAttached("Row", typeof(int), typeof(Extentions), new PropertyMetadata(default(int)));

        public static void SetRow(UIElement element, int value)
        {
            element.SetValue(Row, value);
        }

        public static int GetRow(this UIElement element)
        {
            return (int)element.GetValue(Row);
        }

        //COL
        public static readonly DependencyProperty Col =
           DependencyProperty.RegisterAttached("Col", typeof(int), typeof(Extentions), new PropertyMetadata(default(int)));

        public static void SetCol(UIElement element, int value)
        {
            element.SetValue(Col, value);
        }

        public static int GetCol(this UIElement element)
        {
            return (int)element.GetValue(Col);
        }
    }
}
