using System.Windows.Controls;

namespace SoursePrj.UserControls
{
    /// <summary>
    /// Interaction logic for UserControl_StepParams.xaml
    /// </summary>
    public partial class UserControl_StepParams : UserControl
    {
        public UserControl_StepParams()
        {
            InitializeComponent();
        }

        private void textBoxUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            var a = 0;
            //if (textBoxUser.Text == "User")
            //    textBoxUser.Text = "Пользователь";

            //if (textBoxUser.Text == "Admin")
            //    textBoxUser.Text = "Инженерный пользователь";
        }
    }
}
