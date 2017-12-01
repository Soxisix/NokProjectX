using System;
using System.Collections.Generic;
using System.Diagnostics;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NokProjectX.Wpf.Common;
using NokProjectX.Wpf.Common.Messages;
using NokProjectX.Wpf.Views;

namespace NokProjectX.Wpf.ViewModel
{
    public class SideBarViewModel : ViewModelBase
    {
        public List<MenuList> MenuList { get; set; }

        public SideBarViewModel()
        {
            LoadMenu();
            NavigateCommand = new RelayCommand<int>(NavTo);
        }

        public RelayCommand<int> NavigateCommand { get; set; }
        private void NavTo(int obj)
        {
            switch (obj)
            {
                case 1:
                    MessengerInstance.Send(new NavigateTo{Content = null});
                    break;
                case 2:
                    MessengerInstance.Send(new NavigateTo { Content = new InventoryView() });
                    break;
                default:
                    break;
            }
        }

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
    }
}