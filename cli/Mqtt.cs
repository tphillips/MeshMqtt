using System.Security.Cryptography.X509Certificates;
using MQTTnet;
using MQTTnet.Client;

public class Mqtt
{
	public static async Task SendMessage(string host, int port, string username, string password, string userId, string nodeNumber, string meshtasticMqttRootTopic, string body, string channelNumber, string topic, string clientId)
	{
		var factory = new MqttFactory();
		var mqttClient = factory.CreateMqttClient();
		var options = new MqttClientOptionsBuilder()
			.WithTcpServer(host, port)
			.WithCredentials(username, password)
			.WithClientId(clientId)
			.WithCleanSession()
			.Build();
		var connectResult = await mqttClient.ConnectAsync(options);
		if (connectResult.ResultCode == MqttClientConnectResultCode.Success)
		{
			Console.WriteLine("Connected. Sending . . .");
			var message = new MqttApplicationMessageBuilder()
				.WithTopic(topic)
				.WithPayload($"{{\"from\":{nodeNumber},\"channel\":{channelNumber},\"payload\":\"{body}\",\"type\":\"sendtext\"}}")
				.WithRetainFlag()
				.Build();
			await mqttClient.PublishAsync(message);
			await mqttClient.DisconnectAsync();
			Console.WriteLine("OK");
		}
		else
		{
			Console.WriteLine($"Failed to connect to MQTT broker: {connectResult.ResultCode}");
		}
	}
}