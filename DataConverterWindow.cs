using System;
using System.Text.RegularExpressions;
using Gtk;
using TestApp.DataConversion;

namespace TestApp
{
    public partial class DataConverterWindow : Gtk.Window
    {
        public DataConverterWindow() : base(Gtk.WindowType.Toplevel)
        {
            this.Build();
            openFileButton.Clicked += getDataClicked;
            openFolderButton.Clicked += getOutputPath;
            ConvertButton.Clicked += DataConvert;
        }

        private void DataConvert(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(dataSourceEntry.Text) || string.IsNullOrEmpty(newFileNameEntry.Text) || string.IsNullOrEmpty(outputPathEntry.Text) || string.IsNullOrEmpty(PropertiesToAddEntry.Text))
            {
                ShowMessage("Missing fields, please make sure all data fields are filled in...");
            }

            try
            {
                FileCrud fileCrud = new FileCrud();
                string data = fileCrud.DataSourceReader(dataSourceEntry.Text);
                string[] split = Regex.Split(PropertiesToAddEntry.Text, ",");
                string header = fileCrud.BuildHeader(PropertiesToAddEntry.Text, split);
                string tsvdata = string.Empty;
                tsvdata += header;
                tsvdata += fileCrud.BuildFile(data, split);
                bool success = fileCrud.WriteDataToTSV(outputPathEntry.Text, newFileNameEntry.Text, tsvdata);
                if (success)
                {
                    ShowMessage("Success! new file created.");
                }
                else
                {
                    ShowMessage("Failed to create new file. Please try again");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ShowMessage("Error: something has gone very wrong, please try again. Ensure all fields are filled in properly, and with proper spelling.");
            }
        }

        #region ButtonRegion
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

        private void ShowMessage(string message)
        {
            MessageDialog md = new MessageDialog(this,
                DialogFlags.DestroyWithParent, MessageType.Info,
                ButtonsType.Close, message);
            md.Run();
            md.Destroy();
            return;
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
        #endregion
    }
}
