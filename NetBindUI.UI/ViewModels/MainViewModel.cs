using NetBindUI.Contracts.Interfaces.Network;
using NetBindUI.Contracts.Interfaces.Process;
using NetBindUI.Contracts.Events;
using NetBindUI.UI.Commands;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace NetBindUI.UI.ViewModels
{
    /// <summary>
    /// Main view model for the application window
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly INetworkService _networkService;
        private readonly IProcessService _processService;
        private INetworkInterface? _selectedInterface;
        private IProcessInfo? _selectedProcess;
        private string _executablePath = string.Empty;
        private string _arguments = string.Empty;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class
        /// </summary>
        /// <param name="networkService">Network service instance</param>
        /// <param name="processService">Process service instance</param>
        public MainViewModel(INetworkService networkService, IProcessService processService)
        {
            _networkService = networkService;
            _processService = processService;

            NetworkInterfaces = new ObservableCollection<INetworkInterface>();
            ManagedProcesses = new ObservableCollection<IProcessInfo>();

            RefreshCommand = new RelayCommand(_ => RefreshData());
            BindProcessCommand = new RelayCommand(_ => BindProcess(), _ => CanBindProcess());
            UnbindProcessCommand = new RelayCommand(_ => UnbindProcess(), _ => CanUnbindProcess());
            StartProcessCommand = new RelayCommand(_ => StartProcess(), _ => CanStartProcess());

            RefreshData();

            _networkService.NetworkInterfaceChanged += OnNetworkInterfaceChanged;
            _processService.ProcessStateChanged += OnProcessStateChanged;
        }

        /// <summary>
        /// Collection of network interfaces
        /// </summary>
        public ObservableCollection<INetworkInterface> NetworkInterfaces { get; }

        /// <summary>
        /// Collection of managed processes
        /// </summary>
        public ObservableCollection<IProcessInfo> ManagedProcesses { get; }

        /// <summary>
        /// Path to the executable file
        /// </summary>
        public string ExecutablePath
        {
            get => _executablePath;
            set => SetProperty(ref _executablePath, value);
        }

        /// <summary>
        /// Command line arguments
        /// </summary>
        public string Arguments
        {
            get => _arguments;
            set => SetProperty(ref _arguments, value);
        }

        /// <summary>
        /// Currently selected network interface
        /// </summary>
        public INetworkInterface? SelectedInterface
        {
            get => _selectedInterface;
            set
            {
                if (SetProperty(ref _selectedInterface, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        /// <summary>
        /// Currently selected process
        /// </summary>
        public IProcessInfo? SelectedProcess
        {
            get => _selectedProcess;
            set
            {
                if (SetProperty(ref _selectedProcess, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        /// <summary>
        /// Command to refresh data
        /// </summary>
        public ICommand RefreshCommand { get; }

        /// <summary>
        /// Command to bind process to interface
        /// </summary>
        public ICommand BindProcessCommand { get; }

        /// <summary>
        /// Command to unbind process from interface
        /// </summary>
        public ICommand UnbindProcessCommand { get; }

        /// <summary>
        /// Command to start a new process
        /// </summary>
        public ICommand StartProcessCommand { get; }

        private async void RefreshData()
        {
            System.Diagnostics.Debug.WriteLine("RefreshData called");
            try
            {
                var selectedInterfaceId = SelectedInterface?.Id;
                var selectedProcessId = SelectedProcess?.ProcessId;

                System.Diagnostics.Debug.WriteLine($"Saved selectedInterfaceId: {selectedInterfaceId}");
                System.Diagnostics.Debug.WriteLine($"Saved selectedProcessId: {selectedProcessId}");

                var interfaces = await _networkService.GetNetworkInterfacesAsync();
                var processes = await _processService.GetManagedProcessesAsync();

                System.Diagnostics.Debug.WriteLine($"Fetched interfaces count: {interfaces.Count()}");
                System.Diagnostics.Debug.WriteLine($"Fetched processes count: {processes.Count()}");

                UpdateCollection(NetworkInterfaces, interfaces,
                    (existing, updated) => existing.Id == updated.Id);
                UpdateCollection(ManagedProcesses, processes,
                    (existing, updated) => existing.ProcessId == updated.ProcessId);

                System.Diagnostics.Debug.WriteLine($"Updated NetworkInterfaces count: {NetworkInterfaces.Count}");
                System.Diagnostics.Debug.WriteLine($"Updated ManagedProcesses count: {ManagedProcesses.Count}");

                if (selectedInterfaceId != null)
                {
                    SelectedInterface = NetworkInterfaces.FirstOrDefault(ni => ni.Id == selectedInterfaceId);
                    System.Diagnostics.Debug.WriteLine($"Restored SelectedInterface: {SelectedInterface?.Id}");
                }
                if (selectedProcessId != null)
                {
                    SelectedProcess = ManagedProcesses.FirstOrDefault(p => p.ProcessId == selectedProcessId);
                    System.Diagnostics.Debug.WriteLine($"Restored SelectedProcess: {SelectedProcess?.ProcessId}");
                }

                System.Diagnostics.Debug.WriteLine($"Final SelectedInterface: {SelectedInterface?.Id}");
                System.Diagnostics.Debug.WriteLine($"Final SelectedProcess: {SelectedProcess?.ProcessId}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing data: {ex.Message}");
            }
        }

        private void UpdateCollection<T>(ObservableCollection<T> collection, IEnumerable<T> newItems,
            Func<T, T, bool> areEqual)
        {
            var newItemsList = newItems.ToList();

            for (int i = collection.Count - 1; i >= 0; i--)
            {
                var existingItem = collection[i];
                if (!newItemsList.Any(newItem => areEqual(existingItem, newItem)))
                {
                    collection.RemoveAt(i);
                }
            }

            foreach (var newItem in newItemsList)
            {
                var existingItem = collection.FirstOrDefault(item => areEqual(item, newItem));
                if (existingItem != null)
                {
                    var index = collection.IndexOf(existingItem);
                    collection[index] = newItem;
                }
                else
                {
                    collection.Add(newItem);
                }
            }
        }

        private bool CanStartProcess()
        {
            var canStart = !string.IsNullOrWhiteSpace(ExecutablePath) && SelectedInterface != null;
            System.Diagnostics.Debug.WriteLine($"CanStartProcess: {canStart}");
            return canStart;
        }

        private async void StartProcess()
        {
            if (SelectedInterface == null) return;

            try
            {
                System.Diagnostics.Debug.WriteLine($"Starting process: {ExecutablePath} with args: {Arguments}");
                var process = await _processService.StartProcessAsync(
                    ExecutablePath,
                    Arguments,
                    SelectedInterface.Id);

                if (process != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Process started successfully: {process.ProcessId}");
                    RefreshData();
                }
                else
                {
                    MessageBox.Show("Failed to start process", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting process: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanBindProcess()
        {
            System.Diagnostics.Debug.WriteLine($"CanBindProcess: {SelectedInterface != null && SelectedProcess != null}");
            return SelectedInterface != null && SelectedProcess != null;
        }

        private async void BindProcess()
        {
            System.Diagnostics.Debug.WriteLine("BindProcess called");
            if (SelectedInterface == null || SelectedProcess == null) return;

            try
            {
                await _processService.BindProcessToInterfaceAsync(SelectedProcess.ProcessId, SelectedInterface.Id);
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error binding process: {ex.Message}");
            }
        }

        private bool CanUnbindProcess()
        {
            System.Diagnostics.Debug.WriteLine($"CanUnbindProcess: {SelectedProcess != null}");
            return SelectedProcess != null;
        }

        private async void UnbindProcess()
        {
            System.Diagnostics.Debug.WriteLine("UnbindProcess called");
            if (SelectedProcess == null) return;

            try
            {
                await _processService.UnbindProcessAsync(SelectedProcess.ProcessId);
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error unbinding process: {ex.Message}");
            }
        }

        private void OnNetworkInterfaceChanged(object? sender, NetworkInterfaceChangedEventArgs e)
        {
            RefreshData();
        }

        private void OnProcessStateChanged(object? sender, ProcessStateChangedEventArgs e)
        {
            RefreshData();
        }
    }
}