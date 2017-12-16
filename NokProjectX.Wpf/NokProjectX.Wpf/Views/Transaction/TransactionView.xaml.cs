namespace NokProjectX.Wpf.Views.Transaction
{
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for TransactionView.xaml
    /// </summary>
    public partial class TransactionView : UserControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionView"/> class.
        /// </summary>
        public TransactionView()
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

        #endregion

        private void TextBox_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }
    }
}
