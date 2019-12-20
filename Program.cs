using System;
using System.Threading.Tasks;
using Gtk;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using PerleyMLMQTTlib;

namespace TestApp
{
    public class MainClass
    {
        public static FrontEndNodeClient client = new FrontEndNodeClient();
        public static void Main(string[] args)
        {
            client.BuildClient().Wait();
            Application.Init();
            MainWindow win = new MainWindow();
            win.Show();
            Application.Run();
        }
    }

    public class FrontEndNodeClient
    {
        public IMqttClient mqttClient { get; set; }
        string clientID = "PerleyTrainingNode";
        string Subscription = "perleyBrain";
        string mainURL = "broker.hivemq.com:8000/mqtt";

        public async Task<bool> BuildClient()
        {
            try
            {
                // Create a new MQTT client.
                var factory = new MqttFactory();
                mqttClient = factory.CreateMqttClient();

                // Create TCP based options using the builder.
                var options = new MqttClientOptionsBuilder()
                    .WithClientId(clientID)
                    //.WithTcpServer("test.mosquitto.org")
                    .WithWebSocketServer(mainURL)
                    .WithCleanSession();

                mqttClient.UseDisconnectedHandler(async e =>
                {
                    Console.WriteLine("### DISCONNECTED FROM SERVER ###");
                    await Task.Delay(TimeSpan.FromSeconds(5));

                    try
                    {
                        await mqttClient.ConnectAsync(options.Build(), System.Threading.CancellationToken.None); // Since 3.0.5 with CancellationToken
                    }
                    catch
                    {
                        Console.WriteLine("### RECONNECTING FAILED ###");
                    }
                });

                mqttClient.UseApplicationMessageReceivedHandler(e =>
                {
                    Console.WriteLine(System.Text.Encoding.UTF8.GetString(e.ApplicationMessage.Payload));

                });

                mqttClient.UseConnectedHandler(async e =>
                {
                    Console.WriteLine("### CONNECTED WITH SERVER ###");

                    //recieves messages via clients own channel, aslo subscribes to a general topic for mass messages.
                    await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(Subscription).Build());
                    await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(Subscription + "/" + clientID).Build());

                    Console.WriteLine("### SUBSCRIBED ###");
                });

                await mqttClient.ConnectAsync(options.Build(), System.Threading.CancellationToken.None); // Since 3.0.5 with CancellationToken
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async void SendMessage(string message, string client)
        {
            var _message = new MqttApplicationMessageBuilder()
                .WithTopic(client)
                .WithPayload(message)
                .WithExactlyOnceQoS()
                .Build();

            await mqttClient.PublishAsync(_message);
        }
    }
}
