using System;
using System.IO;

namespace TestApp.Json
{
    public class JsonController
    {
        private string gitConfigName = Environment.CurrentDirectory + "/gitconfig.json";

        public bool configExists()
        {
            return File.Exists(gitConfigName);
        }

        public bool SaveConfig(string data)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(gitConfigName))
                {
                    sw.Write(data);
                    sw.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public string GetConfig()
        {
            string data = "";
            try
            {
                using (StreamReader sr = new StreamReader(gitConfigName))
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
    }
}
