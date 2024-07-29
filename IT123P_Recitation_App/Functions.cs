using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Text.Json;
using Org.Apache.Http.Protocol;

namespace IT123P_Recitation_App
{
    internal class Functions
    {
        private readonly Activity currentAct;
        private readonly Type nextAct;

        private string jsonResponse;
        public Functions() { }

        public Functions(Activity currentAct, Type nextAct)
        {
            this.currentAct = currentAct;
            this.nextAct = nextAct;
        }

        public void NextActivity()
        {
            Intent intent = new Intent(currentAct, nextAct);
            currentAct.StartActivity(intent);
        }

        public void NextExtraActivity(Bundle bundle)
        {
            Intent intent = new Intent(currentAct, nextAct);
            intent.PutExtras(bundle);
            currentAct.StartActivity(intent);
        }

        public ArrayAdapter<string> DisplaySpinnerTexts(List<string> toDisplay)
        {
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(currentAct, Android.Resource.Layout.SimpleSpinnerItem, toDisplay);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

            return adapter;
        }
        public string GetSelectedText(Spinner spinner, int textPosition)
        {
            return spinner.GetItemAtPosition(textPosition).ToString();
        }

        public ArrayAdapter<string> DisplayItems(List<string> items)
        {
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(currentAct, Android.Resource.Layout.SimpleListItem1, items);
            return adapter;
        }

        public string GetSelectedEvent(List<string> eventList, int textPosition)
        {
            return eventList[textPosition];
        }

        public string GetViewItem(ListView viewToCheck, int textPosition)
        {
            return viewToCheck.GetItemAtPosition(textPosition).ToString();  
        }

        public List<string> PopulateRequestedList(List<string> emptyList, string fileName, string param)
        {
            try
            {
                Transactions transactions = new Transactions();

                if (param != null)
                {
                    jsonResponse = transactions.GetDataRequest(fileName, param);
                }
                else
                {
                    jsonResponse = transactions.GetDataRequest(fileName);
                }

                if (jsonResponse.Contains("No data found"))
                {
                    //Toast.MakeText(currentAct, "Empty List", ToastLength.Short).Show();
                    return emptyList;
                }

                return ParseResponse(emptyList, jsonResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PopulateRequestedList: {ex.Message}");
                return null;
            }
        }

        private List<string> ParseResponse(List<string> emptyList, string jsonResponse)
        {
            using (JsonDocument jsonDoc = JsonDocument.Parse(jsonResponse))
            {
                JsonElement root = jsonDoc.RootElement;

                if (root.ValueKind == JsonValueKind.Array)
                {
                    foreach (JsonElement element in root.EnumerateArray())
                    {
                        if (element.ValueKind == JsonValueKind.String) //new code
                        {
                            string listValues = element.GetString();
                            emptyList.Add(listValues);
                        }
                        else
                        {
                            throw new InvalidOperationException("Unexpected element type in JSON array.");
                        }
                    }
                }
                else
                {
                    throw new InvalidOperationException("Unexpected JSON format.");
                }

                return emptyList;
            }
        }
        public void AddData(string tableName, string param, string param1)
        {
            Transactions transactions = new Transactions();

            if (param1 != null)
            {
                jsonResponse = transactions.AddDataRequest(tableName, param, param1);
            }
            else
            {
                jsonResponse = transactions.AddDataRequest(tableName, param);
            }

            currentAct.RunOnUiThread(() =>
            {
                Toast.MakeText(currentAct, jsonResponse, ToastLength.Short).Show();
            });
        }

        public void RemoveData(string tableName, string param, string param1)
        {
            Transactions transactions = new Transactions();
            if (param1 != null)
            {
                jsonResponse = transactions.RemoveDataRequest(tableName, param, param1);
            }
            else
            {
                jsonResponse = transactions.RemoveDataRequest(tableName, param);
            }

            currentAct.RunOnUiThread(() =>
            {
                Toast.MakeText(currentAct, jsonResponse, ToastLength.Short).Show();
            });
        }

        public void UpdateData(string tableName, string newParam, string origParam, string suppParam)
        {
            Transactions transactions = new Transactions();

            if (suppParam != null)
            {
                jsonResponse = transactions.UpdateDataRequest(tableName, newParam, origParam, suppParam);
            }
            else
            {
                jsonResponse = transactions.UpdateDataRequest(tableName, newParam, origParam);
            }

            currentAct.RunOnUiThread(() =>
            {
                Toast.MakeText(currentAct, jsonResponse, ToastLength.Short).Show();
            });
        }

        public List<string> PickRandomItems(List<string> itemList, ArrayList blockList, int numLimit, string message)
        {
            Random randNum = new Random();
            List<string> selectedItems = new List<string>();
            HashSet<string> occuredItems = new HashSet<string>();
            string selectedItem;

            int subLimit = itemList.Count - blockList.Count;
            int loopLimit = Math.Min(numLimit, subLimit);

            if (!blockList.Count.Equals(itemList.Count()))
            {
                for (int roundLoop = 0; roundLoop < loopLimit; roundLoop++)
                {
                    do
                    {
                        int randGenNum = randNum.Next(itemList.Count());
                        selectedItem = itemList[randGenNum];

                    } while (blockList.Contains(selectedItem) || occuredItems.Contains(selectedItem));
                    selectedItems.Add(selectedItem);
                    occuredItems.Add(selectedItem);
                }
            } 
            else
            {
                Toast.MakeText(currentAct, $"All {message} have been picked.", ToastLength.Short).Show();
            }

            return selectedItems;
        }

        public void UnblockItem(ArrayList blockList, List<string> selectedItems)
        {
            if (blockList.Count > 0 && selectedItems.Count > 0)
            {
                blockList.RemoveAt(selectedItems.IndexOf(selectedItems[0]));
                Toast.MakeText(currentAct, "Item unblocked.", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(currentAct, "Item not found.", ToastLength.Short).Show();
            }
        }

        public ArrayList BlockItems(ArrayList blockList, List<string> selectedItems)
        {
            blockList.AddRange(selectedItems);
            return blockList;
        }

        public void ConfirmationPopUp(string title, string confirmMsg, string yesMsg, string noMsg, Action onConfirm)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(currentAct);
            builder.SetTitle(title);
            builder.SetMessage(confirmMsg);
            builder.SetPositiveButton("Yes", (senderAlert, args) =>
            {
                Toast.MakeText(currentAct, yesMsg, ToastLength.Short).Show();
                onConfirm?.Invoke();
            });
            builder.SetNegativeButton("No", (senderAlert, args) =>
            {
                Toast.MakeText(currentAct, noMsg, ToastLength.Short).Show();
            });

            AlertDialog dialog = builder.Create();
            dialog.Show();
        }

        public void ViewQuestion(string title, string confirmMsg, string okMsg)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(currentAct);
            builder.SetTitle(title);
            builder.SetMessage(confirmMsg);
            builder.SetPositiveButton(okMsg, (senderAlert, args) =>{});

            AlertDialog dialog = builder.Create();
            dialog.Show();
        }
    }
}