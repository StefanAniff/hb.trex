using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Infrastructure.Releasenotes;

namespace Trex.SmartClient.MenuView.Designdata
{
    public class DesignVersionViewModel : IVersionViewModel
    {
        public DelegateCommand<object> GoToNext { get; set; }
        public DelegateCommand<object> GoToPrevious { get; set; }


        public DesignVersionViewModel()
        {
        }
        public List<IRelease> Releases
        {
            get
            {
                var list = new List<IRelease>();
                list.Add(new Version3_4_0());
                list.Add(new Version3_5_0());
                return list;
            }
            set { throw new NotImplementedException(); }
        }

        public bool GoToNextCanExecute
        {
            get { return true; }
        }

        public bool GoToPreviousCanExecute
        {
            get { return true; }
        }

        public IRelease SelectedRelease
        {
            get { return Releases.First(); }
        }
    }
}
