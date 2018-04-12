// при инициализации читать дефолтные имя и пасс из настроек

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
    /// Логика взаимодействия для LogonEngineWindow.xaml
    /// </summary>
    public partial class LogonEngineWindow : Window
    {
        public LogonEngineWindow()
        {
            //
            InitializeComponent();
        }

        private void Click_OK(object sender, RoutedEventArgs e)
        {
            // создать экз юзера
            // попробовать войти
            // если нет - сообщить и вернуться
            // если да - сохранить во внешних настройках 
            //         - заполнить инфу в юзере
            //         - передать в движек
            //         - обновить форму основного окна
        }
    }
}
