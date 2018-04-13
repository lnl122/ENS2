// Copyright © 2018 Antony S. Ovsyannikov aka lnl122 aka полвторого@en
// License: http://opensource.org/licenses/MIT

// 2do
// ! есть константы по тексту - вынести во внешние настройки LeadZero

using System;

namespace ENS2
{
    /// <summary>
    /// реализует работу с лог-файлом
    /// </summary>
    interface ILog
    {
        void Write(string str);
        void Store(string str);
        void Flush();
    }

    public class Log : ILog
    {
        // имя текущего логгируемого модуля
        private string ModuleName = "";

        // объекты блокировки
        private static object LockWrite = new object();
        private static object LockStore = new object();

        // пути
        private static string PathToPages = "";
        // поток лог-файла, в него будет вестись дозапись лога
        private static System.IO.StreamWriter logfile;
        // прризнак проведенной инициализации
        private static bool isReady = false;
        // объекты для блокировки
        private static object LockInit = new object();
        // индекс (порядковый номер)  сохраняемого файла страниц (.http)
        private static int fileidx = 100;

        /// <summary>
        /// инициализирует лог файл, если нету его - создает. в т.ч. необходимые папки
        /// </summary>
        public Log(string mn = "")
        {
            if (mn != "") { ModuleName = mn + ": "; }
            if (!isReady)
            {
                lock (LockInit)
                {
                    if (!isReady)
                    {
                        string LogPath1 = CheckCreateFolder(Properties.Settings.Default.Log_LogfileFolder) + System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName + Properties.Settings.Default.Log_LogfileExtention;
                        PathToPages = CheckCreateFolder(Properties.Settings.Default.Log_PagesFolder);
                        logfile = new System.IO.StreamWriter(System.IO.File.AppendText(LogPath1).BaseStream);
                        logfile.AutoFlush = true;
                        isReady = true;
                    }
                }
            }
        }

        /// <summary>
        /// записывает строку текста в лог-файл
        /// </summary>
        /// <param name="str">строка для лог файла</param>
        public void Write(string str)
        {
            if (isReady)
            {
                lock (LockWrite)
                {
                    logfile.WriteLine("{0} {1}\t{2}\t{3}",
                        DateTime.Today.ToShortDateString(),
                        DateTime.Now.ToLongTimeString(),
                        ModuleName,
                        str);
                }
            }
        }

        /// <summary>
        /// записывает текст в отдельный файл
        /// </summary>
        /// <param name="modulename">имя файла/модуля</param>
        /// <param name="text">строка текста</param>
        public void Store(string text)
        {
            if (isReady)
            {
                lock (LockStore)
                {
                    fileidx++;
                    var dn = DateTime.Now;
                    string path = PathToPages + ModuleName.Replace(": ", "") + "_" + LeadZero(fileidx) + "_" +
                        LeadZero(dn.Year) + LeadZero(dn.Month) + LeadZero(dn.Day) +
                        LeadZero(dn.Hour) + LeadZero(dn.Minute) + LeadZero(dn.Second) + Properties.Settings.Default.Log_PagesExtention;
                    System.IO.File.WriteAllText(path, text, System.Text.Encoding.UTF8);

                }
            }
        }

        /// <summary>
        /// если папка есть, или если не было, но удалось создать - возвращает путь к ней, иначе - базовый путь
        /// </summary>
        /// <param name="basepath">базовый путь</param>
        /// <param name="folder">имя папки</param>
        /// <returns>путь к папке</returns>
        private static string CheckCreateFolder(string folder)
        {
            string basepath = Environment.CurrentDirectory;
            string path = basepath + @"\" + folder.Replace("\\", "");
            if (!System.IO.Directory.Exists(path))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                catch
                {
                    path = basepath;
                }
            }
            return path + "\\";
        }

        /// <summary>
        /// перевод числа в строку, добавление лидирующих нулей
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private string LeadZero(int num)
        {
            int zeros = 4;
            if (num < 100) { zeros = 2; }
            string str = "0000" + num.ToString();
            return str.Substring(str.Length - zeros);
        }

        /// <summary>
        /// выполняет принудительную запись лога на диск
        /// </summary>
        public void Flush()
        {
            if (isReady)
            {
                logfile.Flush();
            }
        }

        /// <summary>
        /// выполняет принудительную запись лога на диск, убивает объект лог-файла
        /// </summary>
        public void Close()
        {
            if (isReady)
            {
                logfile.Flush();
                logfile.Close();
                logfile = null;
                isReady = false;
            }
        }
    }
}
