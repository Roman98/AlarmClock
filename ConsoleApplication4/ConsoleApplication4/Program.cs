using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
    class Program
    {
        static void Main(string[] args)
        {

            // Объект запроса
            HttpWebRequest rew = (HttpWebRequest)WebRequest.Create("http://studypay.ru/alarmclock.php");

            // Отправить запрос и получить ответ
            HttpWebResponse resp = (HttpWebResponse)rew.GetResponse();

            // Получить поток
            Stream str = resp.GetResponseStream();

            // Выводим в TextBox
            int ch;
            string message = "";
            for (int i = 1; ; i++)
            {
                ch = str.ReadByte();
                if (ch == -1) break;
                message += (char)ch;
            }

            Console.WriteLine(message);

            // Закрыть поток
            str.Close();
            Console.ReadKey();
        }
    }
}
