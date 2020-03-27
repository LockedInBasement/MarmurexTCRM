using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MarmurexTCRMDesktopUI.EventModels; 
using MarmurexTCRMDesktopUI.Library.Models;
using MarmurexTCRMDesktopUI.Library.Api;
using System.Threading;

namespace MarmurexTCRMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object> , IHandle<LogOnEvent>
    {
        private IEventAggregator _events;
        private ILoggedInUserModel _user;
        private IAPIHelper _aPIHelper;

        public  ShellViewModel(IEventAggregator events, ILoggedInUserModel loggedInUserModel, IAPIHelper aPIHelper)
        {
            _events = events;
            _user = loggedInUserModel;
            _aPIHelper = aPIHelper;

            _events.SubscribeOnPublishedThread(this);
            ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
        }

        public bool IsLoggedIn
        {
            get
            {
                bool output = false;

                if(string.IsNullOrWhiteSpace(_user.Token) == false)
                {
                    output = true;
                }

                return output;
            }
        }

        public void ExitApplication()
        {
            TryCloseAsync();
        }

        public async Task UserManagment()
        {
            await ActivateItemAsync(IoC.Get<UserDisplayViewModel>(), new CancellationToken());
        }

        public async Task LogOut()
        {
            _user.ResetUser();
            _aPIHelper.LogOffUser();
            await ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(IoC.Get<LoginViewModel>(), cancellationToken);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
    }
}
