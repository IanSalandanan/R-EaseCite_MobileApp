using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang.Reflect;
using Java.Nio.FileNio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IT123P_Recitation_App
{
    internal class Transactions
    {
        private readonly string ipAdd = ""; 
        private readonly string filePath = "IT123P_APP";
        private string res;

        public string GetDataRequest(string tableName)
        {
            string parameter = $"read_operation.php?table={tableName}";
            return ReceiveTransactRequest(parameter);
        }

        public string GetDataRequest(string tableName, string getParam)
        {
            string parameter = $"read_operation.php?table={tableName}&param={getParam}";
            return ReceiveTransactRequest(parameter);
        }

        public string AddDataRequest(string tableName, string insertData, string insertData1)
        {
            string parameter = $"add_operation.php?table={tableName}&insert={insertData}&insert1={insertData1}";
            return ReceiveTransactRequest(parameter);
        }

        public string AddDataRequest(string tableName, string insertData)
        {
            string parameter = $"add_operation.php?table={tableName}&insert={insertData}";
            return ReceiveTransactRequest(parameter);
        }

        public string RemoveDataRequest(string tableName, string deleteData, string deleteData1)
        {
            string parameter = $"remove_operation.php?table={tableName}&remove={deleteData}&remove1={deleteData1}";
            return ReceiveTransactRequest(parameter);
        }
        public string RemoveDataRequest(string tableName, string deleteData)
        {
            string parameter = $"remove_operation.php?table={tableName}&remove={deleteData}";
            return ReceiveTransactRequest(parameter);
        }

        public string UpdateDataRequest(string tableName, string updateData, string origData)
        {
            string parameter = $"update_operation.php?table={tableName}&new_update={updateData}&orig_update={origData}";
            return ReceiveTransactRequest(parameter);
        }

        public string UpdateDataRequest(string tableName, string updateData, string origData, string suppData)
        {
            string parameter = $"update_operation.php?table={tableName}&new_update={updateData}&orig_update={origData}&supp_update={suppData}";
            return ReceiveTransactRequest(parameter);
        }

        private string ReceiveTransactRequest(string parameter)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"http://{ipAdd}/{filePath}/{parameter}");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                res = reader.ReadToEnd();
                response.Dispose();
                reader.Dispose();

                return res;
            }
            catch (Exception ex)
            {
                new Handler(Looper.MainLooper).Post(() =>
                {
                    Toast.MakeText(Application.Context, $"Transaction Error: {ex.Message}", ToastLength.Short).Show();
                });
                throw;
            }
        }
    }
}