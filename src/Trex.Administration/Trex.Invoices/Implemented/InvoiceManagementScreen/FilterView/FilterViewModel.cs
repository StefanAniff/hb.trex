//using System;
//using System.Windows;
//using System.Windows.Controls;
//using Microsoft.Practices.Composite.Presentation.Commands;
//using Trex.Core.Implemented;
//using Trex.Invoices.Commands;
//using Trex.Invoices.Implemented;
//using Trex.Invoices.InvoiceManagementScreen.Interfaces;

//namespace Trex.Invoices.InvoiceManagementScreen.FilterView
//{
//    public class FilterViewModel : ViewModelBase, IFilterViewModel
//    {
//        public DelegateCommand<object> ApplyFilterCommand { get; set; }
//        public DelegateCommand<object> ResetFilterCommand { get; set; }
//        public DelegateCommand<object> AutoGenerateCommand { get; set; }
//        public DelegateCommand<object> FinalizeDraftCommand { get; set; } 

//        private DateTime? _endDate;
//        private bool _isShowAll;
//        private SaveFileDialog dialog;

//        public FilterViewModel()
//        {
//            //ApplyFilterCommand = new DelegateCommand<object>(ExecuteApplyFilter);
//            //ResetFilterCommand = new DelegateCommand<object>(ExecuteResetFilter);
//            //AutoGenerateCommand = new DelegateCommand<object>(ExecuteAutoGenerate);
//            //FinalizeDraftCommand = new DelegateCommand<object>(FinalizeDraftExecute);

//            EndDate = DateTime.Now;
//        }

         
//        //private void ExecuteResetFilter(object obj)
//        //{
//        //    InternalCommands.ApplyFilter.Execute(null);
//        //}

//        //private void ExecuteApplyFilter(object obj)
//        //{
//        //    InternalCommands.ApplyFilter.Execute(GetFilter());
//        //}

//        private Filter GetFilter()
//        {
//            return new Filter() { EndDate = EndDate, ShowAll = ShowAll};
//        }

//    }
//}