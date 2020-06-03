using PureMVCWPF.Model.Enum;
using PureMVCWPF.Model.VO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PureMVCWPF.View.Components
{
    /// <summary>
    /// RolePanel.xaml 的交互逻辑
    /// </summary>
    public partial class RolePanel : UserControl
    {
        private UserVO m_user;
        public UserVO User
        {
            get { return m_user; }
        }

        private IList<RoleEnum> m_roles;
        public IList<RoleEnum> Roles
        {
            get { return m_roles; }
        }

        public RoleEnum SelectedRole
        {
            get
            {
                if (roleList.SelectedItem != null) return (RoleEnum)roleList.SelectedItem;
                return null;
            }
        }

        public event EventHandler AddRole;

        protected virtual void OnAddRole(EventArgs args)
        {
            AddRole?.Invoke(this, args);
        }

        public event EventHandler RemoveRole;
        protected virtual void OnRemoveRole(EventArgs args)
        {
            RemoveRole?.Invoke(this, args);
        }

        public RolePanel()
        {
            InitializeComponent();


        }

        private void userRoles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            IsEnabled = (m_user != null);

            if(bRemove!=null)
            {
                bRemove.IsEnabled = userRoles.SelectedIndex != -1;
            }
            if(bAdd!=null)
            {
                bAdd.IsEnabled = !RoleEnum.NONE_SELECTED.Equals(SelectedRole);
            }
        }

        private void roleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtons();
        }

        private void bAdd_Click(object sender, RoutedEventArgs e)
        {
            SetAddRole();
        }

        private void SetAddRole()
        {
            OnAddRole(new EventArgs());
        }

        private void bRemove_Click(object sender, RoutedEventArgs e)
        {
            SetRemoveRole();
        }

        private void SetRemoveRole()
        {
            OnRemoveRole(new EventArgs());
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateButtons();
        }

        private delegate void ClearFormDelegate();
        public void ClearForm()
        {
            if(!CheckAccess())
            {
                Dispatcher.BeginInvoke(new ClearFormDelegate(ClearForm));
                return;
            }

            m_user = null;
            m_roles = null;
            formGrid.DataContext = null;
            userRoles.ItemsSource = null;
            roleList.SelectedItem = RoleEnum.NONE_SELECTED;

            UpdateButtons();
        }

        private delegate void ShowUserDelegate(UserVO user, IList<RoleEnum> roles);
        public void ShowUser(UserVO user,IList<RoleEnum> roles)
        {
            if(!CheckAccess())
            {
                Dispatcher.BeginInvoke(new ShowUserDelegate(ShowUser), new object[] { user, roles });
                return;
            }

            if(user == null)
            {
                ClearForm();
            }
            else
            {
                m_user = user;
                m_roles = roles;
                formGrid.DataContext = user;
                userRoles.ItemsSource = roles;
                roleList.SelectedItem = RoleEnum.NONE_SELECTED;
                UpdateButtons();
            }
        }

        public void ShowUserRoles(IList<RoleEnum> roles)
        {
            if (!CheckAccess())
            {
                Dispatcher.BeginInvoke(new ShowUserRolesDelegate(ShowUserRoles), new object[] { roles });
                return;
            }

            userRoles.ItemsSource = roles;
            UpdateButtons();
        }
        private delegate void ShowUserRolesDelegate(IList<RoleEnum> roles);
    }
}
