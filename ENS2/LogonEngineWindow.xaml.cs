// Copyright © 2018 Antony S. Ovsyannikov aka lnl122 aka полвторого@en
// License: http://opensource.org/licenses/MIT

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
using System.Windows.Shapes;

namespace ENS2
{
    public partial class LogonEngineWindow : Window
    {
        public LogonEngineWindow()
        {
            InitializeComponent();
            this.Login.Text = Properties.Settings.Default.User_Name;
            this.Password.Text = Properties.Settings.Default.User_Password;
        }

        private void Button_ClickOK(object sender, RoutedEventArgs e)
        {
            Engine EN = Engine.Instance;
            bool sussefulLogon = EN.Logon(this.Login.Text, this.Password.Text);
            if (sussefulLogon)
            {
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Не удалось выполнить логон в движке.\r\n\r\nПроверьте логин и пароль, всяко бывает =)", "Ошибка логона", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
