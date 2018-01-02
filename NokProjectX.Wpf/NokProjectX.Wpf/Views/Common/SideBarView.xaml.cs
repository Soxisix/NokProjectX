namespace NokProjectX.Wpf.Views.Common
{
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for SideBarView.xaml
    /// </summary>
    public partial class SideBarView : UserControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SideBarView"/> class.
        /// </summary>
        public SideBarView()
        {
            InitializeComponent();
        }

        #endregion

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
