using System;
using System.Collections.Generic;
using System.IO;
using Gtk;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PerleyMLMQTTlib;
using PerleyMLMQTTlib.Models;
using System.Threading.Tasks;
using TestApp.DataConversion;

namespace TestApp
{
    public partial class MSGConverterWindow : Gtk.Window
    {

        private List<string> msgs = new List<string>();
        private List<TrainingDataModel> newTraingingData = new List<TrainingDataModel>();
        public MSGConverterWindow() : base(Gtk.WindowType.Toplevel)
        {
            this.Build();
            getDatapathButton.Clicked += SelectData;
            StartConvertButton.Clicked += StartConverting;
            NextButton.Clicked += NextItem;
            SaveButton.Clicked += SaveWork;
        }



        private void StartConverting(object sender, EventArgs e)
        {
            DataViewEntry.Text = msgs[0];
            FileCrud.AutomatedLearning(msgs);
        }

        private void NextItem(object sender, EventArgs e)
        {
            if (newTraingingData.Count <= 0 || string.IsNullOrEmpty(questionStatementTypeEntry.Text) || string.IsNullOrEmpty(SentimentTypeEntry.Text))
            {
                MessageDialog md = new MessageDialog(this,
                DialogFlags.DestroyWithParent, MessageType.Info,
                ButtonsType.Close, "Missing data. Please ensure all fields are filled in.");
                md.Run();
                md.Destroy();
                return;
            }
            TrainingDataModel tdm = new TrainingDataModel();
            tdm.MessageContent = DataViewEntry.Text;
            tdm.questionOrStatement = new StatementTypeModel
            {
                ID = new Random().Next(),
                Statement = DataViewEntry.Text,
                StatementType = QuestionOrStatmentCheckbox.Active 
            };
            if (tdm.questionOrStatement.StatementType)
            {
                tdm.questionType = new QuestionTypeModel
                {
                    ID = new Random().Next(),
                    Statement = DataViewEntry.Text,
                    QuestionType = questionStatementTypeEntry.Text
                };
            }
            else
            {
                tdm.StatementType = new SentenceTypeModel
                {
                    ID = new Random().Next(),
                    Statement = DataViewEntry.Text,
                    sentenceType = questionStatementTypeEntry.Text 
                };
            }

            tdm.SentimentType = new SentimentModel
            {
                ID = new Random().Next(),
                Statement = DataViewEntry.Text,
                SentimentType = SentimentTypeEntry.Text
            };

            newTraingingData.Add(tdm);
            msgs.RemoveAt(0);
            DataViewEntry.Text = msgs[0];
        }
        private void SaveWork(object sender, EventArgs e)
        {
            string datapath = Environment.CurrentDirectory + "/jsondata.json";
            if (File.Exists(datapath))
            {
                datapath = Environment.CurrentDirectory + "/jsondata" + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".json";
            }
            string json = JsonConvert.SerializeObject(newTraingingData);
            using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + "/jsondata.json"))
            {
                sw.Write(json);
            }
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
                DatapathEntry.Text = fcd.Filename;
            fcd.Destroy();

            ExtractData();
        }

        private void ExtractData()
        {
            string jsondata = string.Empty;
            using (StreamReader sr = new StreamReader(DatapathEntry.Text))
            {
                jsondata = sr.ReadToEnd();
                sr.Close();
            }

            dynamic dynamicJson = JsonConvert.DeserializeObject<object>(jsondata) as dynamic;
            string ditems = JsonConvert.SerializeObject(dynamicJson["messages"]);
            List<object> deserialized = JsonConvert.DeserializeObject<List<object>>(ditems);
            foreach (var item in deserialized)
            {
                string json = JsonConvert.SerializeObject(item);
                dynamic parsedJSON = JObject.Parse(json);
                Console.WriteLine(parsedJSON);
                string s = parsedJSON["content"];
                msgs.Add(s);
            }
        }
    }
}
