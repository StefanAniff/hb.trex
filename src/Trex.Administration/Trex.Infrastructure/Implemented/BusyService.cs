using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Composite.Regions;
using Trex.Core.Interfaces;
using Trex.Core.Services;
using Trex.Infrastructure.Commands;
using Trex.Infrastructure.Extensions;

namespace Trex.Infrastructure.Implemented
{
    public class BusyService : IBusyService
    {
        #region Properties

        private readonly IRegionManager _regionManager;
        private readonly IRegionNames _regionNames;
        public IBusyView BusyView { get; set; }

        public bool IsBusy { get; set; }

        #endregion

        #region Constructors

        public BusyService(IRegionManager regionManager, IRegionNames regionNames)
        {
            _regionNames = regionNames;
            _regionManager = regionManager;
            IsBusy = false;
            ApplicationCommands.SystemBusy.RegisterCommand(new DelegateCommand<string>(ShowBusy));
            ApplicationCommands.SystemIdle.RegisterCommand(new DelegateCommand<object>(HideBusy));

            //Color color = new Color { A = 255, R = 255, B = 0, G = 255 }; // yellow
            //Color color = new Color { A = 255, R = 0, B = 0, G = 255 }; // green
            //Color color = new Color { A = 255, R = 16, B = 128, G = 80}; // blue shade
            //BusyControl = new FlowerLoadingControl { Visibility = Visibility.Collapsed, PetalBrush = new SolidColorBrush(color), Caption = "Loading, please wait ..." };
        }

        #endregion

        #region Public Methods

        public void ShowBusy(string message)
        {
            if (BusyView == null)
            {
                throw new ArgumentNullException("message", "BusyService has no view");
            }
            IsBusy = true;
            ShowInRegion();

            BusyView.Message = message;

            ((UserControl) BusyView).Visibility = Visibility.Visible;
        }

        public void HideBusy(object obj)
        {
            IsBusy = false;
            ((UserControl) BusyView).Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Private Methods

        private void ShowInRegion()
        {
            var region = _regionManager.Regions[_regionNames.BusyRegion];
            region.RemoveAll();
            region.AddAndActivateIfNotExists(BusyView);
        }

        #endregion
    }
}