using Custom_Paint.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custom_Paint.Navigation
{
    public class AppNavigation
    {
        private ViewModelBase? _currentVM;

        public ViewModelBase? CurrentVM { set { _currentVM = value; OnCurrentViewModelChanged(); } get { return _currentVM; } }

        public void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }

        public event Action? CurrentViewModelChanged;
    }
}
