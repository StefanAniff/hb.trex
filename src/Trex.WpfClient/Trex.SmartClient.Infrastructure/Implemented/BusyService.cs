using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Regions;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Extensions;

namespace Trex.SmartClient.Infrastructure.Implemented
{
    public class BusyService : ViewModelBase, IBusyService
    {
        private readonly IRegionManager _regionManager;
        public IView BusyView { get; set; }
        private readonly IRegionNames _regionNames;

        private readonly HashSet<string> _lockKeys = new HashSet<string>();

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            private set
            {
                _isBusy = value;
                OnPropertyChanged(() => IsBusy);
            }
        }

        public BusyService(IRegionManager regionManager, IRegionNames regionNames)
        {
            _regionNames = regionNames;
            _regionManager = regionManager;
            IsBusy = false;

            //Color color = new Color { A = 255, R = 255, B = 0, G = 255 }; // yellow
            //Color color = new Color { A = 255, R = 0, B = 0, G = 255 }; // green
            //Color color = new Color { A = 255, R = 16, B = 128, G = 80}; // blue shade
            //BusyControl = new FlowerLoadingControl { Visibility = Visibility.Collapsed, PetalBrush = new SolidColorBrush(color), Caption = "Loading, please wait ..." };
        }

        public void ShowBusy(string key)
        {
            if (BusyView == null)
            {
                throw new ArgumentNullException("BusyView", "BusyService has no view");
            }
            _lockKeys.Add(key);
            IsBusy = true;
            ShowInRegion();

            ((UserControl) BusyView).Visibility = Visibility.Visible;
        }

        public void HideBusy(string key)
        {
            _lockKeys.Remove(key);

            if (_lockKeys.Count == 0)
            {
                IsBusy = false;
                ((UserControl) BusyView).Visibility = Visibility.Collapsed;
                var region = _regionManager.Regions[_regionNames.BusyRegion];
                region.RemoveAll();
            }
        }

        private void ShowInRegion()
        {
            var region = _regionManager.Regions[_regionNames.BusyRegion];
            region.RemoveAll();
            region.AddAndActivateIfNotExists(BusyView);
        }
    }
}