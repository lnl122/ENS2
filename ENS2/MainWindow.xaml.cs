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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ENS2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // если тестируем компонент интерактивно - пишем код сюда

        }

        // открыть модальную форму ввода параметров игрока и логона в движке
        private void MenuItem_Engine_Logon_Click(object sender, RoutedEventArgs e)
        {
            //MenuItem menuItem = (MenuItem)sender;
            LogonEngineWindow LogonWindow = new LogonEngineWindow();
            LogonWindow.ShowDialog();
            UpdateActivityMenuItem();
        }

        private void MenuItem_Engine_Logoff_Click(object sender, RoutedEventArgs e)
        {
            (Engine.Instance).Logoff();
            UpdateActivityMenuItem();
        }

        private void UpdateActivityMenuItem()
        {
            bool isLoggedUser = Engine.IsLoggedUser;
            bool isLoggedInGame = Engine.IsLoggedInGame;
            MenuItemId_Logon.IsEnabled = !isLoggedUser;
            MenuItemId_Logoff.IsEnabled = isLoggedUser;
            MenuItemId_GameConnect.IsEnabled = isLoggedUser && !isLoggedInGame;
            MenuItemId_GameStats.IsEnabled = isLoggedInGame;
            MenuItemId_GameDisconnect.IsEnabled = isLoggedInGame;
        }
    }
}
