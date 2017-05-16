using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net;
using System.IO;

namespace GeoCod
{
    class net
    {
        public string post(string url, string query)
        {
            WebResponse result = null;
            WebRequest req = null;
            Stream newStream = null;
            Stream ReceiveStream = null;
            StreamReader sr = null;


            try
            {
                // Url запрашиваемого методом POST скрипта
                req = WebRequest.Create(url);
                req.Method = "POST";
                req.Timeout = 120000;
                req.ContentType = "application/x-www-form-urlencoded";

                byte[] SomeBytes = null;

                SomeBytes = Encoding.GetEncoding(1251).GetBytes(query);// передача параметров
                req.ContentLength = SomeBytes.Length;
                newStream = req.GetRequestStream();
                newStream.Write(SomeBytes, 0, SomeBytes.Length);
                newStream.Close();

                // считываем результат работы
                result = req.GetResponse();
                ReceiveStream = result.GetResponseStream();
                Encoding encode = Encoding.GetEncoding(1251);
                sr = new StreamReader(ReceiveStream, encode);

                char[] read = new char[256];
                int count = sr.Read(read, 0, 256);
                string strOut = string.Empty;
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    strOut += str;
                    count = sr.Read(read, 0, 256);
                }
                return strOut;
            }

            catch (Exception ex)
            {
                return "ОШИБКА: не загруженно";
            }

            finally
            {
                if (newStream != null)
                    newStream.Close();
                if (ReceiveStream != null)
                    ReceiveStream.Close();

                if (sr != null)
                    sr.Close();

                if (result != null)
                    result.Close();
            }
        }
    }
}