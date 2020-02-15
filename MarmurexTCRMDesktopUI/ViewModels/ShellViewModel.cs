using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace MarmurexTCRMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        private LoginViewModel _loginVM;

        public ShellViewModel(LoginViewModel loginVM)
        {
            _loginVM = loginVM;
            ActivateItem(_loginVM);
        }

        string _perwperp;

        public string Perwperp { get => _perwperp; set => _perwperp = value; }
    }
}
