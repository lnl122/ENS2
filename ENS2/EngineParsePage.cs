// Copyright © 2018 Antony S. Ovsyannikov aka lnl122 aka полвторого@en
// License: http://opensource.org/licenses/MIT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENS2
{
    public static class EngineParsePage
    {
        public static string GetUserId(string page)
        {
            string p = page.Substring(page.IndexOf("&uid=") + 5);
            p = p.Substring(0, p.IndexOf("\""));
            return p;
        }

        public static string GetUserDomain(string page)
        {
            string p = page.Substring(page.IndexOf("Прописка:") + 9);
            p = p.Substring(p.IndexOf("href=\"") + 6);
            p = p.Substring(0, p.IndexOf("/\""));
            p = p.Replace("http://", "");
            return p;
        }

        public static string GetUserTeam(string page)
        {
            int idx = page.IndexOf("/Teams/TeamDetails.aspx?tid=");
            if (idx == -1)
            {
                return "";
            }
            string p = page.Substring(idx + 28);
            p = p.Substring(p.IndexOf(">") + 1);
            p = p.Substring(0, p.IndexOf("<"));
            return p;
        }

        public static string GetUserTeamId(string page)
        {
            int idx = page.IndexOf("/Teams/TeamDetails.aspx?tid=");
            if (idx == -1)
            {
                return "";
            }
            string p = page.Substring(idx + 28);
            p = p.Substring(0, p.IndexOf("\""));
            return p;
        }

        public static string GetGameType(string page)
        {
            string result = "???";
            int idx = page.IndexOf("<span>Игра</span>:</span>"); //25
            if (idx > 0)
            {
                string p = page.Substring(idx + 25);
                idx = p.IndexOf("</span>");
                if (idx > 0)
                {
                    p = p.Substring(0, idx);
                    idx = p.LastIndexOf(">");
                    if (idx > 0)
                    {
                        result = p.Substring(idx + 1);
                    }
                }
            }
            return result;
        }

        public static bool GetIsCommand(string page)
        {
            bool result = false;
            int idx = page.IndexOf("<span>Играем</span>:</span>"); //27
            if (idx > 0)
            {
                string p = page.Substring(idx + 27);
                idx = p.IndexOf("</span>");
                if (idx > 0)
                {
                    p = p.Substring(0, idx);
                    if(p.IndexOf("Командами") > 0)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        public static bool GetGameIsStorm(string page)
        {
            bool result = false;
            int idx = page.IndexOf("<span>Последовательность прохождения</span>:</span>"); //51
            if (idx > 0)
            {
                string p = page.Substring(idx + 51);
                idx = p.IndexOf("</span>");
                if (idx > 0)
                {
                    p = p.Substring(0, idx);
                    if (p.IndexOf("Штурмовая") > 0)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        public static string GetGameBeginDate(string page)
        {
            string result = "???";
            int idx = page.IndexOf("<span>Начало игры</span>:</span>"); //32
            if (idx > 0)
            {
                string p = page.Substring(idx + 32);
                idx = p.IndexOf(" (<a");
                if (idx > 0)
                {
                    p = p.Substring(0, idx);
                    idx = p.LastIndexOf(">");
                    if (idx > 0)
                    {
                        result = p.Substring(idx + 1);
                    }
                }
            }
            return result;
        }

        public static string GetGameName(string page)
        {
            string result = "???";
            int idx = page.IndexOf("lnkGameTitle"); //12
            if (idx > 0)
            {
                string p = page.Substring(idx + 12);
                idx = p.IndexOf("</a>");
                if (idx > 0)
                {
                    p = p.Substring(0, idx);
                    idx = p.LastIndexOf(">");
                    if (idx > 0)
                    {
                        result = p.Substring(idx + 1);
                    }
                }
            }
            return result;
        }

        public static string GetGameLink(string page)
        {
            // http://magadan.en.cx/GameDetails.aspx?gid=60869
            // http://magadan.en.cx/gameengines/encounter/play/60869
            return page.Replace("GameDetails.aspx?gid=", "gameengines/encounter/play/");
        }

        public static bool GetGameIsPlayable(string page)
        {
            bool result = false;
            int idx = page.IndexOf("GameDetail_GameEnterBox_divEnterGameHolder");
            if(idx > 0)
            {
                result = true;
            }
            return result;
        }



        /// <summary>
        /// проверяет страницу на предмет поиска строки с logout
        /// </summary>
        /// <param name="page">код страницы</param>
        /// <returns>true - если мы залогонены, false - если нужен логон</returns>
        public static bool IsLogonSussefully(string page)
        {
            if (page.IndexOf("action=logout") != -1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// проверяет страницу на предмет поиска строки с Login.aspx
        /// </summary>
        /// <param name="page">код страницы</param>
        /// <returns>true - есть необходимость в логоне</returns>
        public static bool IsNeedLogon(string page)
        {
            if (page.IndexOf("action=\"/Login.aspx") != -1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// разбираеи страницу движка со сведениями об играх одного типа из информации об игроке
        /// </summary>
        /// <param name="page">страница</param>
        /// <returns>список строк со ссылками на игры</returns>
        public static List<string> GetGamesLinks(string page)
        {
            // ищем где строки с играми
            List<string> result = new List<string>();
            int idx = page.IndexOf("_lnkGameTitle");
            if (idx > 0)
            {
                // находим до строки с игрой начало строки таблицы
                string t1 = page.Substring(0, idx);
                idx = t1.LastIndexOf("<tr");
                if (idx > 0)
                {
                    // где они заканчиваются
                    t1 = page.Substring(idx);
                    idx = t1.LastIndexOf("_lnkGameTitle");
                    if (idx > 0)
                    {
                        // конец последней строки таблицы
                        string t2 = t1.Substring(idx);
                        int idx2 = t2.IndexOf("</tr>");
                        if (idx2 > 0)
                        {
                            // разбиваем по строкам
                            t2 = t1.Substring(0, idx + idx2);
                            string[] arr = System.Text.RegularExpressions.Regex.Split(t2, "<tr");
                            foreach (string tr in arr)
                            {
                                // если с строке таблицы есть описание игры
                                if (tr.IndexOf("_trGameRow") > 0)
                                {
                                    // разбиваем  по колонкам
                                    string[] fields = System.Text.RegularExpressions.Regex.Split(tr, "<td");
                                    if (fields.Length == 5)
                                    {
                                        // проверим уже завершена или нет
                                        if (fields[3].IndexOf("Место") <= 0)
                                        {
                                            // выбираем ссылки
                                            string[] arr3 = System.Text.RegularExpressions.Regex.Split(fields[4], "href=\"");
                                            foreach (string link in arr3)
                                            {
                                                // выбираем только ссылки на игры (без ссылок на команду/игрока)
                                                if (link.IndexOf("GameDetails.aspx?gid=") > 0)
                                                {
                                                    // вырезаем их и складываем в результат
                                                    idx = link.IndexOf("\"");
                                                    if (idx > 0)
                                                    {
                                                        result.Add(link.Substring(0, idx));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

    }
}
