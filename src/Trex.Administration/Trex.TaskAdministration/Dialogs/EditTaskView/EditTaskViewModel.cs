using System.ComponentModel.DataAnnotations;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.Core.Model;
using Trex.Core.Services;
using Trex.ServiceContracts;
using Trex.TaskAdministration.Commands;
using Trex.TaskAdministration.Resources;
using System;

namespace Trex.TaskAdministration.Dialogs.EditTaskView
{
    public class EditTaskViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly IEstimateService _estimateService;
        private readonly bool _isNew;
        private readonly Color _mustDivideColor;
        private readonly Color _okColor;
        private readonly Color _shouldDivideColor;
        private string _estimateStatusText;
        private SolidColorBrush _statusColor;
        private readonly Task _task;

        public EditTaskViewModel(Task task, IDataService dataService, IEstimateService estimateService)
        {
            _estimateService = estimateService;
            SaveCommand = new DelegateCommand<object>(SaveTask, CanSaveTask);
            CancelCommand = new DelegateCommand<object>(CancelEdit);
            _dataService = dataService;
            _task = task;

            _isNew = _task.Id == 0;

            //Todo: Get these colors from some resource
            _okColor = Color.FromArgb(255, 5, 193, 0);
            _shouldDivideColor = Color.FromArgb(255, 199, 220, 0);
            _mustDivideColor = Color.FromArgb(255, 220, 0, 0);
            UpdateStatus();
        }

        public string WindowTitle
        {
            get { return string.Concat(_task.TaskName, " ", _task.Project.ProjectName); }
        }

        [Required]
        public string Name
        {
            get { return _task.TaskName; }
            set
            {
                _task.TaskName = value;
                OnPropertyChanged("Name");
                OnPropertyChanged("WindowTitle");
                Validate("Name", value);
            }
        }

        public string Description
        {
            get { return _task.Description; }
            set
            {
                _task.Description = value;
                OnPropertyChanged("Description");
            }
        }

        public string EstimateRealistic
        {
            get { return _task.RealisticEstimate.ToString("N2"); }
            set
            {
                double estimate;
                if (!double.TryParse(value, out estimate)) return;
                _task.RealisticEstimate = estimate;

                OnPropertyChanged("EstimateRealistic");
                CalculateEstimate();
            }
        }

        public string EstimateOptimistic
        {
            get { return _task.BestCaseEstimate.ToString("N2"); }
            set
            {
                double estimate;
                if (!double.TryParse(value, out estimate)) return;
                _task.BestCaseEstimate = estimate;
                OnPropertyChanged("EstimateOptimistic");
                CalculateEstimate();
            }
        }

        public string EstimatePessimistic
        {
            get { return _task.WorstCaseEstimate.ToString("N2"); }
            set
            {
                double estimate;
                if (!double.TryParse(value, out estimate)) return;
                _task.WorstCaseEstimate = estimate;
                OnPropertyChanged("EstimatePessimistic");
                CalculateEstimate();
            }
        }

        public string EstimateCalculated
        {
            get { return _task.TimeEstimated.ToString("N2"); }

            set
            {
                double estimate;
                if (!double.TryParse(value, out estimate)) return;
                _task.TimeEstimated = estimate;
                OnPropertyChanged("EstimateCalculated");
            }
        }

        public string EstimateStatusText
        {
            get { return _estimateStatusText; }
            set
            {
                _estimateStatusText = value;
                OnPropertyChanged("EstimateStatusText");
            }
        }

        public EstimateStatus Status
        {
            get { return _estimateService.Status; }
        }

        public string TimeLeft
        {
            get { return _task.TimeLeft.ToString("N2"); }
            set
            {
                double timeLeft;
                if (!double.TryParse(value, out timeLeft)) return;
                _task.TimeLeft = timeLeft;
                OnPropertyChanged("TimeLeft");
            }
        }

        public SolidColorBrush StatusColor
        {
            get { return _statusColor; }
            set
            {
                _statusColor = value;
                OnPropertyChanged("StatusColor");
            }
        }

        public bool IsEstimatesEnabled
        {
            get { return _task.Project.IsEstimatesEnabled; }
        }

        public bool IsActive
        {
            get { return !_task.Inactive; }
            set
            {
                _task.Inactive = !value;
                OnPropertyChanged("IsActive");
            }
        }

        public DelegateCommand<object> SaveCommand { get; set; }
        public DelegateCommand<object> CancelCommand { get; set; }

        private bool CanSaveTask(object arg)
        {
            return !string.IsNullOrEmpty(Name);
        }

        private void CancelEdit(object obj)
        {
            _task.CancelChanges();

            InternalCommands.TaskEditCompleted.Execute(null);
        }

        private void SaveTask(object obj)
        {
            var project = _task.Project;
            _task.Project = null;

            _task.ChangeTracker.OriginalValues.Clear();
            _dataService.SaveTask(_task).Subscribe(
                task =>
                {

                    if (_isNew)
                    {
                        _task.Project = project;
                        _task.TaskID = task.TaskID;
                        _task.AcceptChanges();
                        InternalCommands.TaskAddCompleted.Execute(_task);
                    }
                    else
                    {
                        _task.Project = project;
                        _task.AcceptChanges();
                        InternalCommands.TaskEditCompleted.Execute(_task);
                    }
                }
                );
        }

        private void CalculateEstimate()
        {
            _estimateService.Calculate(_task.WorstCaseEstimate, _task.BestCaseEstimate, _task.RealisticEstimate);
            EstimateCalculated = _estimateService.EstimateCalculated.ToString("N");
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            if (_task.TimeEstimated == 0)
            {
                return;
            }

            switch (_estimateService.Status)
            {
                case EstimateStatus.Ok:
                    EstimateStatusText = EditTaskResources.StatusOkText;
                    StatusColor = new SolidColorBrush(_okColor);
                    break;

                case EstimateStatus.ShouldBeDivided:
                    EstimateStatusText = EditTaskResources.StatusShouldDivideText;
                    StatusColor = new SolidColorBrush(_shouldDivideColor);
                    break;

                case EstimateStatus.MustBeDivided:
                    EstimateStatusText = EditTaskResources.StatusMustDivideText;
                    StatusColor = new SolidColorBrush(_mustDivideColor);
                    break;
            }
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            SaveCommand.RaiseCanExecuteChanged();
        }
    }
}