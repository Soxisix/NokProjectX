using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NokProjectX.Wpf.Context;
using ViewModelBase = GalaSoft.MvvmLight.ViewModelBase;

namespace NokProjectX.Wpf.ViewModel.About
{
    public class AboutViewModel : ViewModelBase
    {
        private readonly YumiContext _context;
        public AboutViewModel(YumiContext context)
        {
            _context = context;
        }
    }
}
