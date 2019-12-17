using System;
using Gtk;
using TestApp;
using TestApp.BashCommandLogic;

public partial class MainWindow : Gtk.Window
{
    BashCommander bash = new BashCommander();
    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
        getdatapathbutton.Clicked += SelectData;
        buildbutton.Clicked += RunBuilder;
        ConvertDataButton.Clicked += JSONConverterOpen;
    }

    private void JSONConverterOpen(object sender, EventArgs e)
    {
        DataConverterWindow gcw = new DataConverterWindow();
    }

    private void RunBuilder(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(datapathentry.Text))
        {
            MessageDialog md = new MessageDialog(this,
                DialogFlags.DestroyWithParent, MessageType.Info,
                ButtonsType.Close, "Missing fields, please make sure all data fields are filled in...");
            md.Run();
            md.Destroy();
            return;
        }


        string commander = bash.ModelBuildCommandBuilder(modeloutputentry.Text, datapathentry.Text, labelcolumnentry.Text, ignorecolumnentry.Text, headerCheckBox.Active, explorationTimeSpinner.Value);

        string newConsole = "gnome-terminal -x bash -ic '" + commander + "; ls; bash'";

        bash.ExecuteBashCommand(newConsole);

        do
        {
           
        } while (!bash.modelBuildProc.HasExited);
    }

    private void SelectData(object sender, EventArgs e)
    {
        Gtk.FileChooserDialog fcd = new Gtk.FileChooserDialog("Open File", null, Gtk.FileChooserAction.Open);
        fcd.AddButton(Gtk.Stock.Cancel, Gtk.ResponseType.Cancel);
        fcd.AddButton(Gtk.Stock.Open, Gtk.ResponseType.Ok);
        fcd.DefaultResponse = Gtk.ResponseType.Ok;
        fcd.SelectMultiple = false;

        Gtk.ResponseType response = (Gtk.ResponseType)fcd.Run();
        if (response == Gtk.ResponseType.Ok)
            datapathentry.Text = fcd.Filename;
        fcd.Destroy();
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        bash.modelBuildProc.Dispose();
        Application.Quit();
        a.RetVal = true;
    }
}
