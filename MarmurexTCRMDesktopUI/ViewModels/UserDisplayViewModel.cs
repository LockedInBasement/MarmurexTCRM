using Caliburn.Micro;
using MarmurexTCRMDesktopUI.Library.Api;
using MarmurexTCRMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MarmurexTCRMDesktopUI.ViewModels
{
    public class UserDisplayViewModel : Screen
    {
		private readonly StatusInfoViewModel _status;
		private readonly IWindowManager _window;
		private readonly IUserEndpoint _userEndPoint;

		private BindingList<UserModel> _users;

		public BindingList<UserModel> Users
		{
			get { return _users; }
			set 
			{ 
				_users = value;
				NotifyOfPropertyChange(() => Users);
			}
		}

		public UserDisplayViewModel(StatusInfoViewModel status, IWindowManager window, IUserEndpoint userEndPoint)
		{
			_status = status;
			_window = window;
			_userEndPoint = userEndPoint;
		}

		public async Task LoadUsers()
		{
			var userList = await _userEndPoint.GetAll();
			Users = new BindingList<UserModel>(userList);
		}

		protected override async void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);

			try
			{
				await LoadUsers();
			}
			catch (Exception ex)
			{
				dynamic settings = new ExpandoObject();
				settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				settings.ResizeMode = ResizeMode.NoResize;
				settings.Title = "System ERROR";

				if (ex.Message == "Unathorized")
				{
					_status.UpdateMessage("Unathorized", "Permission");
					_window.ShowDialog(_status, null, settings);
				}

				TryClose();
			}
		}
	}
}
