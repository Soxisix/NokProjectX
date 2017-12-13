using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace NokProjectX.Wpf.Common
{
    public class SelectItemOnButtonClick : Behavior<ListView>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(Handler), true);
        }

        protected override void OnDetaching()
        {
            AssociatedObject.RemoveHandler(ButtonBase.ClickEvent, new RoutedEventHandler(Handler));
            base.OnDetaching();
        }

        private void Handler(object s, RoutedEventArgs e)
        {
            var clicked = (e.OriginalSource as FrameworkElement)?.DataContext;
            if (AssociatedObject.ItemContainerGenerator.ContainerFromItem(clicked) is ListBoxItem lbi)
                lbi.IsSelected = true;
        }
    }
}