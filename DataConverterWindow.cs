using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Gtk;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestApp
{
    public partial class DataConverterWindow : Gtk.Window
    {
        public DataConverterWindow() : base(Gtk.WindowType.Toplevel)
        {
            this.Build();
            openFileButton.Clicked += getDataClicked;
            openFolderButton.Clicked += getOutputPath;
            ConvertButton.Clicked += ConvertData;
        }

        private void ConvertData(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(dataSourceEntry.Text) || string.IsNullOrEmpty(newFileNameEntry.Text) || string.IsNullOrEmpty(outputPathEntry.Text) || string.IsNullOrEmpty(PropertiesToAddEntry.Text))
            {
                MessageDialog md = new MessageDialog(this,
                DialogFlags.DestroyWithParent, MessageType.Info,
                ButtonsType.Close, "Missing fields, please make sure all data fields are filled in...");
                md.Run();
                md.Destroy();
                return;
            }

            List<object> items = new List<object>();
            string data = string.Empty;

            using (StreamReader sr = new StreamReader(dataSourceEntry.Text))
            {
                data = sr.ReadToEnd();
                sr.Close();
            }

            //build file;
            string tsvData = string.Empty;

            //build file header
            string headerValue = "";
            var split = Regex.Split(PropertiesToAddEntry.Text, ",");
            bool stringstart = true;
            foreach (var item in split)
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
            //set header for file
            tsvData += headerValue; 

            //build file data
            items = JsonConvert.DeserializeObject<List<object>>(data);
            foreach (var item in items)
            {
                string json = JsonConvert.SerializeObject(item);
                dynamic d = JObject.Parse(json);
                Console.WriteLine(d);

                bool propstart = true;
                foreach (string property in split)
                {
                    if (propstart)
                    {
                        tsvData += "\n";
                        tsvData += d[property];
                        propstart = false;
                    }
                    else
                    {
                        tsvData += "\t" + d[property];
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter(outputPathEntry.Text + 
                "/" + 
                newFileNameEntry.Text + 
                ".tsv"))
            {
                sw.Write(tsvData);
                sw.Close();
            }
        }

        private void getOutputPath(object sender, EventArgs e)
        {
            Gtk.FileChooserDialog fcd = new Gtk.FileChooserDialog("Open File", null, Gtk.FileChooserAction.SelectFolder);
            fcd.AddButton(Gtk.Stock.Cancel, Gtk.ResponseType.Cancel);
            fcd.AddButton(Gtk.Stock.Open, Gtk.ResponseType.Ok);
            fcd.DefaultResponse = Gtk.ResponseType.Ok;
            fcd.SelectMultiple = false;

            Gtk.ResponseType response = (Gtk.ResponseType)fcd.Run();
            if (response == Gtk.ResponseType.Ok)
                outputPathEntry.Text = fcd.CurrentFolder;
            fcd.Destroy();
        }

        private void getDataClicked(object sender, EventArgs e)
        {
            Gtk.FileChooserDialog fcd = new Gtk.FileChooserDialog("Open File", null, Gtk.FileChooserAction.Open);
            fcd.AddButton(Gtk.Stock.Cancel, Gtk.ResponseType.Cancel);
            fcd.AddButton(Gtk.Stock.Open, Gtk.ResponseType.Ok);
            fcd.DefaultResponse = Gtk.ResponseType.Ok;
            fcd.SelectMultiple = false;

            Gtk.ResponseType response = (Gtk.ResponseType)fcd.Run();
            if (response == Gtk.ResponseType.Ok)
                dataSourceEntry.Text = fcd.Filename;
            fcd.Destroy();
        }
    }
}
