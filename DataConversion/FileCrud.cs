using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestApp.DataConversion
{
    public class FileCrud
    {
        public bool WriteDataToTSV(string outputpath, string filename, string tsvData)
        {
            bool completeSuccess = false;
            try
            {
                using (StreamWriter sw = new StreamWriter(outputpath +
                "/" +
                filename +
                ".tsv"))
                {
                    sw.Write(tsvData);
                    sw.Close();
                }
                completeSuccess = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return completeSuccess;
        }

        public string DataSourceReader(string dataPath)
        {
            string data = string.Empty;

            try
            {
                using (StreamReader sr = new StreamReader(dataPath))
                {
                    data = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return data;
        }

        public string BuildHeader(string PropertiesToAddEntry, string[] properties)
        {
            string headerValue = string.Empty;

            try
            {

                bool stringstart = true;
                foreach (var item in properties)
                {
                    if (stringstart)
                    {
                        headerValue += item;
                        stringstart = false;
                    }
                    else
                    {
                        headerValue += "\t" + item;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return headerValue;
        }

        public string BuildFile(string data, string[] properties)
        {
            //set header for file
            string tsvData = string.Empty;

            try
            {
                //build file data
                var items = JsonConvert.DeserializeObject<List<object>>(data);
                foreach (var item in items)
                {
                    string json = JsonConvert.SerializeObject(item);
                    dynamic parsedJSON = JObject.Parse(json);
                    Console.WriteLine(parsedJSON);

                    bool propstart = true;
                    foreach (string property in properties)
                    {
                        if (propstart)
                        {
                            tsvData += "\n";
                            tsvData += parsedJSON[property];
                            propstart = false;
                        }
                        else
                        {
                            tsvData += "\t" + parsedJSON[property];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return tsvData;
        }

        public static async void AutomatedLearning(List<string> msgs)
        {
            foreach (var item in msgs)
            {
                await Task.Delay(500);
                MainClass.client.SendMessage(item, "perleyBrain/PerleyCentralNode");
            }
        }
    }
}