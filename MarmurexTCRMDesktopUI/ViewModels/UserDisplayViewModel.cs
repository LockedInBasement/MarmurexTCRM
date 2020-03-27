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

		private UserModel _selectedUser;

		public UserModel SelectedUser
		{
			get { return _selectedUser; }
			set 
			{ 
				_selectedUser = value;
				SelectedUserName = value.Email;
				UserRoles.Clear();
				UserRoles = new BindingList<string>(value.Roles.Select(x => x.Value).ToList());
				LoadRoles();
				NotifyOfPropertyChange(() => SelectedUser);
			}
		}

		private string _selectedUserRole;

		public string SelectedUserRole
		{
			get { return _selectedUserRole; }
			set 
			{ 
				_selectedUserRole = value;
				NotifyOfPropertyChange(() => SelectedUserRole);
			}
		}

		private string _selectedAvaliableRole;

		public string SelectedAvaliableRole
		{
			get { return _selectedAvaliableRole; }
			set 
			{ 
				_selectedAvaliableRole = value;
				NotifyOfPropertyChange(() => SelectedAvaliableRole);
			}
		}

		private string _selectedUserName;

		public string SelectedUserName
		{
			get { return _selectedUserName; }
			set 
			{ 
				_selectedUserName = value;
				NotifyOfPropertyChange(() => SelectedUserName);
			}
		}

		private BindingList<string> _userRoles = new BindingList<string>();

		public BindingList<string> UserRoles
		{
			get { return _userRoles; }
			set
			{
				_userRoles = value;
				NotifyOfPropertyChange(() => UserRoles);
			}
		}

		private BindingList<string> _availableRole = new BindingList<string>();

		public BindingList<string> AvailableRole
		{
			get { return _availableRole; }
			set 
			{
				_availableRole = value;
				NotifyOfPropertyChange(() => AvailableRole);
			}
		}

		public UserDisplayViewModel(StatusInfoViewModel status, IWindowManager window, IUserEndpoint userEndPoint)
		{
			_status = status;
			_window = window;
			_userEndPoint = userEndPoint;
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
					await _window.ShowDialogAsync(_status, null, settings);
				}
				else
				{
					_status.UpdateMessage("Fatal Exception", ex.Message);
					await _window.ShowDialogAsync(_status, null, settings);
				}

				TryCloseAsync();
			}
		}

		public async Task LoadUsers()
		{
			var userList = await _userEndPoint.GetAll();
			Users = new BindingList<UserModel>(userList);
		}

		public async void AddSelectedRole()
		{
			try
			{
				await _userEndPoint.AddUserToRole(SelectedUser.Id, SelectedAvaliableRole);
				UserRoles.Add(SelectedAvaliableRole);
				AvailableRole.Remove(SelectedAvaliableRole);
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async void RemoveSelectedRole()
		{
			try
			{
				await _userEndPoint.RemoveUserFromRole(SelectedUser.Id, SelectedUserRole);
				AvailableRole.Add(SelectedUserRole);
				UserRoles.Remove(SelectedUserRole);
			}
			catch (Exception)
			{

				throw;
			}
		}

		private async Task LoadRoles()
		{
			var roles = await _userEndPoint.GetAllRoles();

			foreach (var role in roles)
			{
				if (UserRoles.IndexOf(role.Value) < 0)
				{
					AvailableRole.Add(role.Value);
				}
			}
		}
	}
}
