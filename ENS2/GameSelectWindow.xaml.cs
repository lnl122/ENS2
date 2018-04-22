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
    /// <summary>
    /// Логика взаимодействия для GameSelectWindow.xaml
    /// </summary>
    public partial class GameSelectWindow : Window
    {
        public List<GameInfo> Games { get; set; }

        public GameSelectWindow()
        {
            InitializeComponent();
        }
    }
}
