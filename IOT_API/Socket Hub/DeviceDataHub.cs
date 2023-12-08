using IOT_API.MQTT;
using Microsoft.AspNetCore.SignalR;

namespace IOT_API.Hubs;

public class DeviceDataHub : Hub
{
    // private readonly MQTTManager _mqttManager;

    // public DeviceDataHub(MQTTManager mqttManager)
    // {
    //     _mqttManager = mqttManager;
    // }
    public override async Task OnConnectedAsync()
    {
        await Clients.Caller.SendAsync("ConnectedMessage", $"{Context.ConnectionId} has joined.");
    }

    public async Task SendTestMess(string message)
    {
        await Clients.Caller.SendAsync("ServerSendTestMessage", message);
        Console.WriteLine("test");
        // _mqttManager.PublishMessage
    }
}