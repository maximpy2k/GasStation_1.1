using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SoursePrj.UserControls
{
    /// <summary>
    /// Interaction logic for UserControl_SendControll.xaml
    /// </summary>
    public partial class UserControl_StendControll : UserControl
    {
        public UserControl_StendControll()
        {
            InitializeComponent();
        }

        private String Sender = "";
        private void ScrollViewer_OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (TextBox_scroll.Text != Sender)
            {
                ScrollViewer.ScrollToEnd();
                Sender = TextBox_scroll.Text;
            }
        }
    }
}
