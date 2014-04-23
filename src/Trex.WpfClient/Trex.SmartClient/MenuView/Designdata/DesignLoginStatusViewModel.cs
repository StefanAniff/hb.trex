using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.MenuView.Designdata
{
    public class DesignLoginStatusViewModel : ILoginStatusViewModel
    {
        public string UserName
        {
            get { return "username"; }
        }

        public string ButtonText
        {
            get { return "Log out"; }
        }

        public bool IsVisible
        {
            get { return true; }
        }

        public DelegateCommand<object> LogOut
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public DelegateCommand<object> ChangePassword
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}
