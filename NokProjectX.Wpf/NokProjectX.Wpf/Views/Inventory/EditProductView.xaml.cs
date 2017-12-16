namespace NokProjectX.Wpf.Views.Inventory
{
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for EditProductView.xaml
    /// </summary>
    public partial class EditProductView : UserControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EditProductView"/> class.
        /// </summary>
        public EditProductView()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The TextBox_PreviewTextInput
        /// </summary>
        /// <param name="sender">The <see cref="object"/></param>
        /// <param name="e">The <see cref="TextCompositionEventArgs"/></param>
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        /// <summary>
        /// The TextBox_PreviewTextInput_1
        /// </summary>
        /// <param name="sender">The <see cref="object"/></param>
        /// <param name="e">The <see cref="TextCompositionEventArgs"/></param>
        private void TextBox_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
        }

        #endregion
    }
}
