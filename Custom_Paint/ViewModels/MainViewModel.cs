using Custom_Paint.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custom_Paint.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private AppNavigation _navigation { get; set; }

        public ViewModelBase? CurrentVM => _navigation.CurrentVM;

        public MainViewModel(AppNavigation navigation) {
            _navigation  = navigation;
            _navigation.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
           OnPropertyChanged(nameof(CurrentVM));
        }
    }
}
