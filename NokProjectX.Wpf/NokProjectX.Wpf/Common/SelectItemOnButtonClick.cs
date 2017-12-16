namespace NokProjectX.Wpf.Common
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Interactivity;

    /// <summary>
    /// Defines the <see cref="SelectItemOnButtonClick" />
    /// </summary>
    public class SelectItemOnButtonClick : Behavior<ListView>
    {
        /// <summary>
        /// The OnAttached
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(Handler), true);
        }

        /// <summary>
        /// The OnDetaching
        /// </summary>
        protected override void OnDetaching()
        {
            AssociatedObject.RemoveHandler(ButtonBase.ClickEvent, new RoutedEventHandler(Handler));
            base.OnDetaching();
        }

        /// <summary>
        /// The Handler
        /// </summary>
        /// <param name="s">The <see cref="object"/></param>
        /// <param name="e">The <see cref="RoutedEventArgs"/></param>
        private void Handler(object s, RoutedEventArgs e)
        {
            var clicked = (e.OriginalSource as FrameworkElement)?.DataContext;
            if (AssociatedObject.ItemContainerGenerator.ContainerFromItem(clicked) is ListBoxItem lbi)
                lbi.IsSelected = true;
        }
    }
}
