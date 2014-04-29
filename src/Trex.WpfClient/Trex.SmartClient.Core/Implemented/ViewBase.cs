using System.Windows.Controls;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Core.Implemented
{
    public abstract class ViewBase : UserControl, IViewInitializable
    {        
        /// <summary>
        /// Indicates if viewmodel has been initialized at least one time
        /// </summary>
        private bool _initialized = false;

        protected ViewBase()
        {
            // Default true;
            SingleInitializationOnly = true;
        }

        /// <summary>
        /// Indicating if initialize should be called 
        /// </summary>
        protected bool SingleInitializationOnly { get; set; }

        /// <summary>
        /// External handel for applying datacontext viewmodel
        /// </summary>
        /// <param name="viewModel"></param>
        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
        }

        /// <summary>
        /// External handle for initializing the viewmodel
        /// </summary>
        public void InitializeViewModel()
        {
            var initializableContext = DataContext as IViewModelInitializable;
            if (initializableContext == null || StopInitializing)
                return;

            initializableContext.Initialize();
            _initialized = true;
        }

        /// <summary>
        /// Indicates that the viewmodel already has been initialized
        /// and no further initialization is needed.
        /// </summary>
        private bool StopInitializing
        {
            get { return (_initialized && SingleInitializationOnly); }
        }
    }
}