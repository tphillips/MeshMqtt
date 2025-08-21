using MQTTnet;
using MQTTnet.Client;

class Program
{
    static string host = "";
    static int port = 1883;
    static string username = "";
    static string password = "";
    static string userId = "";
    static string nodeNumber = "";
    static string meshtasticMqttRootTopic = "";
    static string body = "";
    static string channelNumber = "1";
    static string topic = "";
    static string clientId = "";

    static async Task Main(string[] args)
    {
        ParseArgs(args);
        if (!CheckArgsShowUsage()) { return; }
        topic = $"{meshtasticMqttRootTopic}/2/json/Mqtt/{userId}";
        clientId = Guid.NewGuid().ToString();
        ShowArgs();
        await Mqtt.SendMessage(host, port, username, password, userId, nodeNumber, meshtasticMqttRootTopic, body, channelNumber, topic, clientId);
    }

	

	private static void ShowArgs()
	{
		Console.WriteLine($"MQTT Host: {host}");
		Console.WriteLine($"MQTT Port: {port}");
		Console.WriteLine($"MQTT Username: {username}");
		Console.WriteLine($"MQTT Password: {(string.IsNullOrEmpty(password) ? "<empty>" : "<hidden>")}");
		Console.WriteLine($"Meshtastic User ID: {userId}");
		Console.WriteLine($"Meshtastic Node Number: {nodeNumber}");
		Console.WriteLine($"Meshtastic MQTT Root Topic: {meshtasticMqttRootTopic}");
		Console.WriteLine($"Message Body: {body}");
		Console.WriteLine($"MQTT Topic: {topic}");
	}

	private static bool CheckArgsShowUsage()
	{
		bool missing = false;
		if (string.IsNullOrWhiteSpace(host)) { Console.WriteLine("Missing required option: --host"); missing = true; }
		if (string.IsNullOrWhiteSpace(username)) { Console.WriteLine("Missing required option: --username"); missing = true; }
		if (string.IsNullOrWhiteSpace(password)) { Console.WriteLine("Missing required option: --password"); missing = true; }
		if (string.IsNullOrWhiteSpace(userId)) { Console.WriteLine("Missing required option: --user-id"); missing = true; }
		if (string.IsNullOrWhiteSpace(nodeNumber)) { Console.WriteLine("Missing required option: --node-number"); missing = true; }
		if (string.IsNullOrWhiteSpace(meshtasticMqttRootTopic)) { Console.WriteLine("Missing required option: --meshtastic-mqtt-root-topic"); missing = true; }
		if (string.IsNullOrWhiteSpace(body)) { Console.WriteLine("Missing required option: --body"); missing = true; }
		if (missing)
		{
			Console.WriteLine("\nUsage:\n");
			Console.WriteLine("meshmqtt\n --host=<MQTT Server Address>\n " +
				"--port=<MQTT Server Port, defaults to 1883>\n " +
				"--username=<MQTT Username>\n " +
				"--password=<MQTT Password>\n " +
				"--user-id=<Meshtastic User ID of the network connected node WITHOUT THE !>\n " +
				"--node-number=<Meshtastic Node Number of the network connected node>\n " +
				"--meshtastic-mqtt-root-topic=<Root Topic set in meshtastic MQTT settings>\n " +
				"--channel=<Meshtastic Channel Number. Defaults to 1!! (Your first manually added channel, not LongFast)>\n " +
				"--body=<Message Body>");
			Console.WriteLine("\nAll options are required except --port (default: 1883). Example:");
			Console.WriteLine("  meshmqtt --host=192.168.0.1 --username=mesh --password=meshp --user-id=ba67019c --node-number=3127312796 --meshtastic-mqtt-root-topic=mesh/messages --body=Hello");
			return false;
		}

		return true;
	}

	private static void ParseArgs(string[] args)
	{
		foreach (var arg in args)
		{
			if (!arg.StartsWith("--")) continue;
			var eqIdx = arg.IndexOf('=');
			if (eqIdx < 0) continue;
			var key = arg.Substring(2, eqIdx - 2);
			var value = arg.Substring(eqIdx + 1);
			switch (key)
			{
				case "host": host = value; break;
				case "port": if (int.TryParse(value, out var p)) port = p; break;
				case "username": username = value; break;
				case "password": password = value; break;
				case "user-id": userId = "!" + value; break;
				case "node-number": nodeNumber = value; break;
				case "meshtastic-mqtt-root-topic": meshtasticMqttRootTopic = value; break;
				case "body": body = value; break;
				case "channel": channelNumber = value; break;
			}
		}
	}
}