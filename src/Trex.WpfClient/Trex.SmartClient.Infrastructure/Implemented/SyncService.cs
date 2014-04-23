using System;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Exceptions;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;
using log4net;

namespace Trex.SmartClient.Infrastructure.Implemented
{
    public class SyncService : ISyncService
    {
        private DispatcherTimer _dispatcherTimer;
        private readonly IConnectivityService _connectivityService;
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly IBusyService _busyService;
        private readonly DelegateCommand<object> _startSyncCommand;
        private readonly DelegateCommand<object> _resyncCommand;
        private readonly DelegateCommand<object> _userLoggedInCommand;
        private readonly DelegateCommand<object> _userLoggedOutCommand;
        private readonly DelegateCommand<object> _synchronizenewTasksCommand;
        private readonly DelegateCommand<object> _connectionChangedCommand;
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private string _syncKey = "ForceResync";

        private ICompanyService CompanyService { get; set; }
        private ICompanyRepository CompanyRepository { get; set; }
        private IProjectRepository ProjectRepository { get; set; }
        private IProjectService ProjectService { get; set; }
        private ITaskService TaskService { get; set; }
        private ITaskRepository TaskRepository { get; set; }
        private ITimeEntryService TimeEntryService { get; set; }
        private IUserSession UserSession { get; set; }
        private IUserService UserService { get; set; }
        private ITimeEntryTypeService TimeEntryTypeService { get; set; }
        private ITimeEntryTypeRepository TimeEntryTypeRepository { get; set; }
        private IAppSettings AppSettings { get; set; }

        public bool ForceResync { get; private set; }
        public bool SyncInProgress { get; private set; }

        public SyncService(IConnectivityService connectivityService, ICompanyService companyService, IProjectRepository projectRepository, ICompanyRepository companyRepository, IProjectService projectService,
                           ITaskService taskService, ITaskRepository taskRepository, ITimeEntryService timeEntryService, IUserSession userSession, IUserService userService, ITimeEntryTypeService timeEntryTypeService,
                           ITimeEntryTypeRepository timeEntryTypeRepository, IAppSettings appSettings, ITimeEntryRepository timeEntryRepository,
             IBusyService busyService)
        {
            _dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);
            _dispatcherTimer.Tick += DispatcherTimerTick;
            _connectivityService = connectivityService;
            _timeEntryRepository = timeEntryRepository;
            _busyService = busyService;
            AppSettings = appSettings;
            TimeEntryTypeRepository = timeEntryTypeRepository;
            TimeEntryTypeService = timeEntryTypeService;
            UserService = userService;
            UserSession = userSession;
            TimeEntryService = timeEntryService;
            TaskRepository = taskRepository;
            TaskService = taskService;
            ProjectService = projectService;
            CompanyRepository = companyRepository;
            ProjectRepository = projectRepository;
            CompanyService = companyService;
            _startSyncCommand = new DelegateCommand<object>(StartSync);
            _connectionChangedCommand = new DelegateCommand<object>(ConnectionChanged);
            _resyncCommand = new DelegateCommand<object>(Resync);
            _userLoggedInCommand = new DelegateCommand<object>(UserLoggedIn);
            _userLoggedOutCommand = new DelegateCommand<object>(UserLoggedOut);
            _synchronizenewTasksCommand = new DelegateCommand<object>(SynchronizeNewTasks);

            ApplicationCommands.StartSync.RegisterCommand(_startSyncCommand);
            ApplicationCommands.Resync.RegisterCommand(_resyncCommand);
            ApplicationCommands.LoginSucceeded.RegisterCommand(_userLoggedInCommand);
            ApplicationCommands.UserLoggedOut.RegisterCommand(_userLoggedOutCommand);
            ApplicationCommands.GetLatestTasks.RegisterCommand(_synchronizenewTasksCommand);
            ApplicationCommands.ConnectivityChanged.RegisterCommand(_connectionChangedCommand);
        }

        private void Resync(object obj)
        {
            ResetRepositories();
            DoSync();
        }

        private void ConnectionChanged(object obj)
        {
            if (_connectivityService.IsOnline && !_connectivityService.IsUnstable)
            {
                Start();
            }
            else
            {
                Stop();
            }
        }

        private void UserLoggedOut(object obj)
        {
            Stop();
            ResetRepositories();
        }

        private void UserLoggedIn(object obj)
        {
            Start();
        }



        private void StartSync(object obj)
        {
            DoSync();
        }

        private void Start()
        {
            if (CompanyRepository.GetAll().Count == 0)
            {
                ResetRepositories();
            }
            _dispatcherTimer.Interval = AppSettings.SyncInterval;
            _dispatcherTimer.Start();
            DoSync();
        }

        private void Stop()
        {
            if (_dispatcherTimer.IsEnabled)
            {
                _dispatcherTimer.Stop();
            }
        }

        private void DispatcherTimerTick(object sender, EventArgs e)
        {
            DoSync();
        }

        public bool ShouldDisplayProgress
        {
            get { return ForceResync || !UserSession.UserStatistics.LastXDaysTimeEntries.Any(); }
        }

        private async void DoSync()
        {
            if (!_connectivityService.IsOnline)
            {
                ApplicationCommands.SyncCompleted.Execute(null);
                return;
            }

            if (SyncInProgress || !_connectivityService.IsOnline)
            {
                return;
            }

            SyncInProgress = true;

            ApplicationCommands.SyncStarted.Execute(ShouldDisplayProgress);

            try
            {
                if (ForceResync)
                {
                    _busyService.ShowBusy(_syncKey);

                    await Task.Run(() =>
                           {
                               SyncProgressChanged(20, "Synchronizing companies");
                               SynchronizeCompanies().Wait();
                               SyncProgressChanged(30, "Synchronizing projects");
                               SynchronizeProjects().Wait();
                               SyncProgressChanged(20, "Synchronizing tasks");
                               SynchronizeTasks().Wait();
                               SyncProgressChanged(5, "Synchronizing time entry types");
                               SynchronizeTimeEntryTypes().Wait();
                               SyncProgressChanged(10, "Synchronizing local time entries");
                               SynchronizeTimeEntries().Wait();
                               SyncProgressChanged(20, "Synchronizing user statistics");
                               UserSession.UserStatistics = UserService.GetPerformanceInfo(UserSession);
                               AppSettings.MinTimeEntryDate = UserService.GetTimeEntryMinStartDate(UserSession);
                           });
                }
                else
                {
                    SyncProgressChanged(20, "Synchronizing companies");
                    await SynchronizeCompanies();
                    SyncProgressChanged(30, "Synchronizing projects");
                    await SynchronizeProjects();
                    SyncProgressChanged(20, "Synchronizing tasks");
                    await SynchronizeTasks();
                    SyncProgressChanged(5, "Synchronizing time entry types");
                    await SynchronizeTimeEntryTypes();
                    SyncProgressChanged(10, "Synchronizing local time entries");
                    await SynchronizeTimeEntries();
                    SyncProgressChanged(20, "Synchronizing user statistics");
                    UserSession.UserStatistics = UserService.GetPerformanceInfo(UserSession);
                    AppSettings.MinTimeEntryDate = UserService.GetTimeEntryMinStartDate(UserSession);
                }


                SyncProgressChanged(10, "Synchronizing history list");
                var numOfDaysBack = UserSession.UserPreferences.StatisticsNumOfDaysBack;
                var startTime = DateTime.Now.Date.AddDays(-numOfDaysBack);
                var endTime = DateTime.Now;
                var lastXDaysTimeEntries = await TimeEntryService.GetTimeEntriesByDate(startTime, endTime);
                var unsyncedTimeEntries = _timeEntryRepository.GetUnsyncedTimeEntries();
                lastXDaysTimeEntries.AddRange(unsyncedTimeEntries.Where(x => !x.HasSyncError));
                UserSession.UserStatistics.LastXDaysTimeEntries = lastXDaysTimeEntries;

                AppSettings.LastSyncDate = DateTime.Now;
                AppSettings.Save();
                SyncProgressChanged(0, "Synchronization complete");
                SyncInProgress = false;
                ForceResync = false;
                ApplicationCommands.SyncCompleted.Execute(null);
            }
            catch (NotFoundByGuidException notFoundByGuidException)
            {
                Logger.Error(notFoundByGuidException);
                SyncInProgress = false;
                _resyncCommand.Execute(null);
            }
            catch (MissingHieracleDataException missingHieracleDataException)
            {
                Logger.Error(missingHieracleDataException);
                if (!ForceResync)
                {
                    SyncInProgress = false;
                    _resyncCommand.Execute(null);
                }
                else
                {
                    OnSyncFailed("Unknown Synchronization failure. Contact support");
                }
            }
            catch (Exception ex)
            {
                if (ex is AggregateException)
                {
                    var aggregateException = ex as AggregateException;
                    foreach (var e in aggregateException.Flatten().InnerExceptions)
                    {
                        HandleSyncError(e);
                    }
                }
                else
                {
                    HandleSyncError(ex);
                }
                OnSyncFailed("Synchronization failed due to a connectivity error. The application will retry in " +
                             AppSettings.SyncInterval.Minutes + " minutes.");
            }
            _busyService.HideBusy(_syncKey);
        }


        private static void HandleSyncError(Exception ex)
        {
            if (ex is TimeoutException)
            {
                Logger.Info(ex);
            }
            else if (ex is EndpointNotFoundException)
            {
                Logger.Info(ex);
            }
            else if (ex is ServiceAccessException)
            {
                Logger.Info(ex);
            }
            else
            {
                Logger.Error(ex);
            }
        }

        private void SyncProgressChanged(int progress, string message)
        {
            if (ShouldDisplayProgress)
            {
                ApplicationCommands.SyncProgressChanged.Execute(new Tuple<int, string>(progress, message));
            }
        }

        public bool IsRunning
        {
            get { return _dispatcherTimer.IsEnabled; }
        }

        private void ResetRepositories()
        {
            ForceResync = true;
            AppSettings.LastSyncDate = DateTime.Today.AddYears(-200);
            AppSettings.Save();
            _timeEntryRepository.DeleteAllRepositories();
        }

        private async void SynchronizeNewTasks(object o)
        {
            SyncInProgress = true;

            ApplicationCommands.SyncStarted.Execute(ShouldDisplayProgress);

            try
            {
                SyncProgressChanged(30, "Synchronizing companies");
                await SynchronizeCompanies();
                SyncProgressChanged(30, "Synchronizing projects");
                await SynchronizeProjects();
                SyncProgressChanged(20, "Synchronizing tasks");
                await SynchronizeTasks();
                SyncInProgress = false;
                ApplicationCommands.SyncCompleted.Execute(null);
                ApplicationCommands.GetLatestTasksFinished.Execute(null);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

        }


        private async Task SynchronizeCompanies()
        {
            var companies = await CompanyService.GetUnsynced(AppSettings.LastSyncDate);
            CompanyRepository.AddOrUpdate(companies);
        }

        private async Task SynchronizeProjects()
        {
            var projects = await ProjectService.GetUnsynced(AppSettings.LastSyncDate);
            ProjectRepository.AddOrUpdate(projects);
        }

        private async Task SynchronizeTasks()
        {
            var tasks = await TaskService.GetUnsynced(AppSettings.LastSyncDate);
            TaskRepository.AddOrUpdate(tasks);
            await TaskService.SaveNewTasks();
        }

        private async Task SynchronizeTimeEntryTypes()
        {
            var timeEntryTypes = await TimeEntryTypeService.GetAllTimeEntryTypes();
            TimeEntryTypeRepository.Add(timeEntryTypes);
        }

        private async Task SynchronizeTimeEntries()
        {
            await TimeEntryService.SaveNewTimeEntries(AppSettings.LastSyncDate);
        }


        private void OnSyncFailed(string message)
        {
            SyncInProgress = false;
            if (ShouldDisplayProgress)
            {
                ApplicationCommands.SyncFailed.Execute(message);
            }
            _busyService.HideBusy(_syncKey);
        }

        public void Dispose()
        {
            if (_startSyncCommand != null) ApplicationCommands.StartSync.UnregisterCommand(_startSyncCommand);
            if (_resyncCommand != null) ApplicationCommands.Resync.UnregisterCommand(_resyncCommand);
            if (_userLoggedInCommand != null) ApplicationCommands.LoginSucceeded.UnregisterCommand(_userLoggedInCommand);
            if (_userLoggedOutCommand != null) ApplicationCommands.UserLoggedOut.UnregisterCommand(_userLoggedOutCommand);
            if (_synchronizenewTasksCommand != null) ApplicationCommands.GetLatestTasks.UnregisterCommand(_synchronizenewTasksCommand);
            if (_connectionChangedCommand != null) ApplicationCommands.ConnectivityChanged.UnregisterCommand(_connectionChangedCommand);
            if (_dispatcherTimer != null)
            {
                _dispatcherTimer.Stop();
                _dispatcherTimer.Tick -= DispatcherTimerTick;
                _dispatcherTimer = null;
            }
        }
    }
}
