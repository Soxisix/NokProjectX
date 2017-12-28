using MaterialDesignThemes.Wpf;
using Microsoft.Practices.ServiceLocation;
using NokProjectX.Wpf.Views.About;
using NokProjectX.Wpf.Views.Reports;
using NokProjectX.Wpf.Views.Settings;

namespace NokProjectX.Wpf.ViewModel.Common
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using NokProjectX.Wpf.Common;
    using NokProjectX.Wpf.Common.Messages;
    using NokProjectX.Wpf.Views.Transaction;
    using System.Collections.Generic;
    using InventoryView = NokProjectX.Wpf.Views.Inventory.InventoryView;

    /// <summary>
    /// Defines the <see cref="SideBarViewModel" />
    /// </summary>
    public class SideBarViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SideBarViewModel"/> class.
        /// </summary>
        public SideBarViewModel()
        {
            LoadMenu();
            NavigateCommand = new RelayCommand<int>(NavTo);
        }

        /// <summary>
        /// Gets or sets the MenuList
        /// </summary>
        public List<MenuList> MenuList { get; set; }

        /// <summary>
        /// Gets or sets the NavigateCommand
        /// </summary>
        public RelayCommand<int> NavigateCommand { get; set; }

        /// <summary>
        /// The LoadMenu
        /// </summary>
        private void LoadMenu()
        {
            MenuList = new List<MenuList>()
            {
                new MenuList()
                {
                    MenuIndex = 1,
                    Hovered = "/NokProjectX.Wpf;component/Resources/homeBlue.png",
                    UnHovered = "/NokProjectX.Wpf;component/Resources/homeBlack.png"
                },
                new MenuList()
                {
                    MenuIndex = 2,
                    Hovered = "/NokProjectX.Wpf;component/Resources/inventoryBlue.png",
                    UnHovered = "/NokProjectX.Wpf;component/Resources/inventoryBlack.png"
                },
                new MenuList()
                {
                    MenuIndex = 3,
                    Hovered = "/NokProjectX.Wpf;component/Resources/reportBlue.png",
                    UnHovered = "/NokProjectX.Wpf;component/Resources/reportBlack.png"
                },
                new MenuList()
                {
                    MenuIndex = 4,
                    Hovered = "/NokProjectX.Wpf;component/Resources/settingsBlue.png",
                    UnHovered = "/NokProjectX.Wpf;component/Resources/settingsBlack.png"
                },
                new MenuList()
                {
                    MenuIndex = 5,
                    Hovered = "/NokProjectX.Wpf;component/Resources/aboutBlue.png",
                    UnHovered = "/NokProjectX.Wpf;component/Resources/aboutBlack.png"
                },
                new MenuList()
                {
                    MenuIndex = 6,
                    Hovered = "/NokProjectX.Wpf;component/Resources/signoutBlue.png",
                    UnHovered = "/NokProjectX.Wpf;component/Resources/signoutBlack.png"
                },
            };
        }

        /// <summary>
        /// The NavTo </summary>
        /// <param name="obj">The <see cref="int"/></param>
        private void NavTo(int obj)
        {
            switch (obj)
            {
                case 1:
                    MessengerInstance.Send(new NavigateTo { Content = new TransactionView() });
                    break;
                case 2:
                    MessengerInstance.Send(new NavigateTo { Content = new InventoryView() });
                    break;
                case 3:
                    MessengerInstance.Send(new NavigateTo { Content = new ReportView() });
                    break;
                case 4:
                    MessengerInstance.Send(new NavigateTo { Content = new SettingsView() });
                    break;
                case 5:
                    MessengerInstance.Send(new NavigateTo { Content = new AboutView() });
                    break;

            }
        }
    }
}
