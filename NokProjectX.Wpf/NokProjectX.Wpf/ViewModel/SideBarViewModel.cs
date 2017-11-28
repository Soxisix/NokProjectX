using System.Collections.Generic;
using GalaSoft.MvvmLight;
using NokProjectX.Wpf.Common;

namespace NokProjectX.Wpf.ViewModel
{
    public class SideBarViewModel : ViewModelBase
    {
        public List<MenuList> MenuList { get; set; }

        public SideBarViewModel()
        {
            MenuList = new List<MenuList>()
            {
                new MenuList()
                {
                    Hovered = "/NokProjectX.Wpf;component/Resources/homeBlue.png",
                    UnHovered = "/NokProjectX.Wpf;component/Resources/homeBlack.png"
                },
                new MenuList()
                {
                    Hovered = "/NokProjectX.Wpf;component/Resources/inventoryBlue.png",
                    UnHovered = "/NokProjectX.Wpf;component/Resources/inventoryBlack.png"
                },
                new MenuList()
                {
                    Hovered = "/NokProjectX.Wpf;component/Resources/reportBlue.png",
                    UnHovered = "/NokProjectX.Wpf;component/Resources/reportBlack.png"
                },
                new MenuList()
                {
                    Hovered = "/NokProjectX.Wpf;component/Resources/settingsBlue.png",
                    UnHovered = "/NokProjectX.Wpf;component/Resources/settingsBlack.png"
                },
                new MenuList()
                {
                    Hovered = "/NokProjectX.Wpf;component/Resources/aboutBlue.png",
                    UnHovered = "/NokProjectX.Wpf;component/Resources/aboutBlack.png"
                },
                new MenuList()
                {
                    Hovered = "/NokProjectX.Wpf;component/Resources/signoutBlue.png",
                    UnHovered = "/NokProjectX.Wpf;component/Resources/signoutBlack.png"
                },
            };
        }
    }
}