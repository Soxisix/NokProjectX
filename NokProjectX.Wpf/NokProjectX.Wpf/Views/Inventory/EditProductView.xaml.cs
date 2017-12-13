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

namespace NokProjectX.Wpf.Views.Inventory
{
    /// <summary>
    /// Interaction logic for EditProductView.xaml
    /// </summary>
    public partial class EditProductView : UserControl
    {
        public EditProductView()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void TextBox_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
//            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || char.))
//            { e.Handled = true; }
//            TextBox txtDecimal = sender as TextBox;
//            if (e.KeyChar == '.' && txtDecimal.Text.Contains("."))
//            {
//                e.Handled = true;
//            }
        }
    }
}