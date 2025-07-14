using System.Windows.Controls;
using FUMini.UI.ViewModel.Admin;

namespace FUMini.UI.View.Admin
{
    public partial class CustomerManagementView : UserControl
    {
        public CustomerManagementView()
        {
            InitializeComponent();
            this.DataContextChanged += CustomerManagementView_DataContextChanged;
        }

        private void CustomerManagementView_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is CustomerManagementViewModel viewModel)
            {
                PasswordBox.PasswordChanged += (s, args) =>
                {
                    if (viewModel.EditingCustomer != null)
                    {
                        viewModel.EditingCustomer.Password = PasswordBox.Password;
                    }
                };
            }
        }
    }
}