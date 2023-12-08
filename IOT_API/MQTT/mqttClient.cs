using System.Text;
using System.Threading.Tasks;
using IOT_API.Helper;
using IOT_API.Models;
using IOT_API.Repositories;
using IOT_API.Services;
using IOT_API.ViewModels;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;
using MQTTnet.Protocol;

namespace IOT_API.MQTT;
public class MQTTManager
{
    // private readonly DeviceDataHub _deviceDataHub;
    private readonly IMqttClient _mqttClient;
    private readonly MqttFactory _factory;
    private readonly MQTTRepository _mqttRepository;

    [Obsolete]
    public MQTTManager(MQTTRepository mqttRepository)
    {
        // _deviceDataHub = deviceDataHub;

        _mqttRepository = mqttRepository;

        _factory = new MqttFactory();

        _mqttClient = _factory.CreateMqttClient();

        _mqttClient.ConnectAsync(GetMqttClientOptions(), CancellationToken.None).Wait();

        if (_mqttClient.IsConnected)
        {
            Console.WriteLine("Connected to MQTT broker.");
        }
        else
        {
            Console.WriteLine("Failed to connect to MQTT broker.");
        }

        _mqttClient.SubscribeAsync(Sub_Options(_factory), CancellationToken.None).Wait();

        _mqttClient.ApplicationMessageReceivedAsync += delegate (MqttApplicationMessageReceivedEventArgs args)
        {
            HandleMessage(args);
            // Do some work with the message...
            // Console.WriteLine($"Received message on topic {args.ApplicationMessage.Topic}: {Encoding.UTF8.GetString(args.ApplicationMessage.Payload)}");
            // Now respond to the broker with a reason code other than success.
            args.ReasonCode = MqttApplicationMessageReceivedReasonCode.ImplementationSpecificError;
            args.ResponseReasonString = "That did not work!";

            // User properties require MQTT v5!
            args.ResponseUserProperties.Add(new MqttUserProperty("My", "Data"));

            // Now the broker will resend the message again.
            return Task.CompletedTask;
        };
    }

    [Obsolete]
    private void HandleMessage(MqttApplicationMessageReceivedEventArgs args)
    {
        Console.WriteLine($"Handler topic: {args.ApplicationMessage.Topic} with message: {Encoding.UTF8.GetString(args.ApplicationMessage.Payload)}");

        // var applicationMessage = new MqttApplicationMessageBuilder()
        //         .WithTopic("1/esp_to_ui/data_realtime")
        //         .WithPayload("29.98, 150.3, 6.58, 20.5")
        //         .Build();
        // _mqttClient.PublishAsync(applicationMessage, CancellationToken.None).Wait();


        // UI get data realtime
        if (args.ApplicationMessage.Topic == "/ui_to_esp/data_realtime")
        {
            string topic = args.ApplicationMessage.Payload + args.ApplicationMessage.Topic;
            var msg = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .Build();
            _mqttClient.PublishAsync(msg, CancellationToken.None).Wait();
        }

        if (args.ApplicationMessage.Topic == "/esp_to_ui/data_realtime")
        {
            Tuple<string, string> separate = SplitFirstAndRest(Encoding.UTF8.GetString(args.ApplicationMessage.Payload));

            string topic = separate.Item1 + args.ApplicationMessage.Topic;

            var msg = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(separate.Item2)
                .Build();
            _mqttClient.PublishAsync(msg, CancellationToken.None).Wait();
        }

        if (args.ApplicationMessage.Topic == "sensor/esp_to_sv/data_interval")
        {
            Tuple<string, string> split = SplitFirstAndRest(Encoding.UTF8.GetString(args.ApplicationMessage.Payload));
            List<string> data_device = ConvertCustom.ConvertStringToList(split.Item2);
            DeviceDataViewModel data = new(int.Parse(split.Item1), data_device);
            _mqttRepository.CreateSensorDataInterval(data);
        }

        if (args.ApplicationMessage.Topic == "sensor/esp_to_sv/issue")
        {
            Tuple<string, string> split = SplitFirstAndRest(Encoding.UTF8.GetString(args.ApplicationMessage.Payload));
            int project_id = int.Parse(split.Item1);
            Tuple<string, string> split2 = SplitFirstAndRest(split.Item2);
            int device = int.Parse(split2.Item1);
            Tuple<string, string> split3 = SplitFirstAndRest(split2.Item2);
            string reason = split3.Item1;
            string message = split3.Item2;

            Alarm issue = new(project_id, device, reason, message);

            _mqttRepository.CreateSensorIssue(issue);
        }
    }

    private static Tuple<string, string> SplitFirstAndRest(string input)
    {
        // Tìm vị trí của ký tự phẩy đầu tiên
        int commaIndex = input.IndexOf(',');

        // Kiểm tra xem có ký tự phẩy không và nếu có, thực hiện tách chuỗi
        if (commaIndex != -1)
        {
            // Tách chuỗi thành hai phần
            string part1 = input[..commaIndex];
            string part2 = input[(commaIndex + 2)..]; // +2 để bỏ qua ký tự phẩy và khoảng trắng sau nó

            // Trả về Tuple chứa hai chuỗi đã tách
            return new Tuple<string, string>(part1, part2);
        }

        // Nếu không có ký tự phẩy, trả về Tuple chứa chuỗi gốc và chuỗi rỗng
        return new Tuple<string, string>(input, string.Empty);
    }

    private static MqttClientSubscribeOptions Sub_Options(MqttFactory factory)
    {
        MqttClientSubscribeOptions mqttSubscribeOptions = factory.CreateSubscribeOptionsBuilder()
            .WithTopicFilter(
                f =>
                {
                    f.WithTopic("sensor/esp_to_sv/data_interval");
                })
            .WithTopicFilter(
                f =>
                {
                    f.WithTopic("/ui_to_esp/data_realtime");
                })
            .WithTopicFilter(
                f =>
                {
                    f.WithTopic("/esp_to_ui/data_realtime");
                })
            .WithTopicFilter(
                f =>
                {
                    f.WithTopic("ui_to_esp/monitor");
                })
            .WithTopicFilter(
                f =>
                {
                    f.WithTopic("sensor/esp_to_sv/issue");
                })
            // .WithTopicFilter(
            //     f =>
            //     {
            //         f.WithTopic("sensor/HCSR04");
            //     })
            // .WithTopicFilter(
            //     f =>
            //     {
            //         f.WithTopic("samples/topic/2").WithNoLocal();
            //     })
            // .WithTopicFilter(
            //     f =>
            //     {
            //         f.WithTopic("samples/topic/3").WithRetainHandling(MqttRetainHandling.SendAtSubscribe);
            //     })
            .Build();
        return mqttSubscribeOptions;
    }

    // private Task Connect()
    // {
    //     _mqttClient.ConnectAsync(GetMqttClientOptions(), CancellationToken.None).Wait();

    //     if (_mqttClient.IsConnected)
    //     {
    //         Console.WriteLine("Connected to MQTT broker.");
    //     }
    //     else
    //     {
    //         Console.WriteLine("Failed to connect to MQTT broker.");
    //     }
    // }

    // public async Task SubscribeMessage()
    // {
    //     await _mqttClient.SubscribeAsync(Sub_Options(_factory), CancellationToken.None);
    // }

    public async Task PublishMessage(string topic, string payload)
    {
        var message = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            .Build();

        await _mqttClient.PublishAsync(message);
    }

    private static MqttClientOptions GetMqttClientOptions()
    {
        // return new MqttClientOptionsBuilder()
        //     .WithTcpServer(Environment.GetEnvironmentVariable("MQTT_HOST"), int.Parse(Environment.GetEnvironmentVariable("MQTT_PORT") ?? ""))
        //     .Build();

        return new MqttClientOptionsBuilder().WithWebSocketServer(o => o.WithUri("192.168.1.123:9001")).Build();
    }

}
