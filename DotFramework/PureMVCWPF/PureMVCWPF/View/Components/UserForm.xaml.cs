using PureMVCWPF.Model.Enum;
using PureMVCWPF.Model.VO;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PureMVCWPF.View.Components
{
    public enum UserFormMode
    {
        ADD,
        EDIT,
    }

    /// <summary>
    /// UserForm.xaml 的交互逻辑
    /// </summary>
    public partial class UserForm : UserControl
    {
        public UserVO User
        {
            get { return m_user; }
        }
        private UserVO m_user;

        public UserFormMode Mode
        {
            get { return m_mode; }
        }
        private UserFormMode m_mode = UserFormMode.ADD;

        public UserForm()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateButtons();
        }

        private void userName_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateButtons();
        }

        private void password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            UpdateButtons();
        }

        private void confirmPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            UpdateButtons();
        }

        private void department_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtons();
        }

        private void bUpdate_Click(object sender, RoutedEventArgs e)
        {
            m_user = new UserVO(userName.Text, firstName.Text, lastName.Text, email.Text, password.Password, (DeptEnum)department.SelectedItem);

            if (m_user.IsValid)
            {
                if (m_mode == UserFormMode.ADD)
                {
                    SetAddUser();
                }
                else
                {
                    SetUpdateUser();
                }
            }
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCancelUser();
        }

        public void ClearForm()
        {
            if (!CheckAccess())
            {
                Dispatcher.BeginInvoke(new ClearFormDelegate(ClearForm));
                return;
            }

            m_user = null;
            formGrid.DataContext = null;
            firstName.Text = lastName.Text = email.Text = userName.Text = "";
            password.Password = confirmPassword.Password = "";
            department.SelectedItem = DeptEnum.NONE_SELECTED;
            UpdateButtons();
        }
        private delegate void ClearFormDelegate();

        public void ShowUser(UserVO user, UserFormMode mode)
        {
            if (!CheckAccess())
            {
                Dispatcher.BeginInvoke(new ShowUserDelegate(ShowUser), new object[] { user, mode });
                return;
            }

            m_mode = mode;

            if (user == null)
            {
                ClearForm();
            }
            else
            {
                m_user = user;
                formGrid.DataContext = user;
                firstName.Text = user.FirstName;
                lastName.Text = user.LastName;
                email.Text = user.Email;
                userName.Text = user.UserName;
                password.Password = confirmPassword.Password = user != null ? user.Password : "";
                department.SelectedItem = user.Department;
                firstName.Focus();
                UpdateButtons();
            }
        }
        private delegate void ShowUserDelegate(UserVO user, UserFormMode mode);

        public event EventHandler AddUser;

        /// <summary>
        /// Fires the add user event
        /// </summary>
        /// <param name="args">The arguments for the event</param>
        protected virtual void OnAddUser(EventArgs args)
        {
            if (AddUser != null) AddUser(this, args);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void SetAddUser()
        {
            OnAddUser(new EventArgs());
        }

        public event EventHandler UpdateUser;

        /// <summary>
        /// Fires the update user event
        /// </summary>
        /// <param name="args">The arguments for the event</param>
        protected virtual void OnUpdateUser(EventArgs args)
        {
            if (UpdateUser != null) UpdateUser(this, args);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void SetUpdateUser()
        {
            OnUpdateUser(new EventArgs());
        }

        public event EventHandler CancelUser;

        protected virtual void OnCancelUser(EventArgs args)
        {
            if (CancelUser != null) CancelUser(this, args);
        }

        protected virtual void SetCancelUser()
        {
            OnCancelUser(new EventArgs());
        }

        private void UpdateButtons()
        {
            IsEnabled = (m_user != null);

            if (bUpdate != null)
            {
                bUpdate.IsEnabled = (userName.Text.Length > 0 && password.Password.Length > 0 && password.Password.Equals(confirmPassword.Password) &&
                    department.SelectedItem != null && department.SelectedItem != DeptEnum.NONE_SELECTED);
            }
        }

    }
}
