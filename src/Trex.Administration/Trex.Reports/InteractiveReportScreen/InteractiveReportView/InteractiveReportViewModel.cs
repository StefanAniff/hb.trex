using System;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.Core.Services;
using Trex.Infrastructure.Commands;

namespace Trex.Reports.InteractiveReportScreen.InteractiveReportView
{
    public class InteractiveReportViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;

        private ObservableCollection<TimeEntryViewModel> _result;

        public InteractiveReportViewModel(IDataService dataService)
        {
            _dataService = dataService;
            ReloadCommand = new DelegateCommand<object>(ExecuteReload, CanExecuteReload);
            StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            EndDate = DateTime.Today;
            LoadData();
        }

        public ObservableCollection<TimeEntryViewModel> Result
        {
            get { return _result; }
            set
            {
                _result = value;
                OnPropertyChanged("Result");
            }
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DelegateCommand<object> ReloadCommand { get; set; }

        private bool CanExecuteReload(object arg)
        {
            return true;
        }

        private void ExecuteReload(object obj)
        {
            LoadData();
        }

        private void LoadData()
        {
            ApplicationCommands.SystemBusy.Execute("Loading data");
            _dataService.GetTimeEntriesByPeriod(StartDate, EndDate).Subscribe(
                
                timeEntries =>
                    {
                        var tmpResult = new ObservableCollection<TimeEntryViewModel>();
                        Result = new ObservableCollection<TimeEntryViewModel>();

                        foreach (var timeEntry in timeEntries)
                        {
                            tmpResult.Add(new TimeEntryViewModel(timeEntry));
                        }

                        Result = tmpResult;
                        ApplicationCommands.SystemIdle.Execute(null);
                        
                    }
                );
        }

       
    }
}