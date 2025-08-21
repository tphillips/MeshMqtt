namespace Api
{
	public class Global
	{
		public static string host = System.Environment.GetEnvironmentVariable("host") ?? "192.168.4.197";
		public static int port = System.Environment.GetEnvironmentVariable("port") != null ? int.Parse(System.Environment.GetEnvironmentVariable("port")) : 1883;
		public static string username = System.Environment.GetEnvironmentVariable("username") ?? "mesh";
		public static string password = System.Environment.GetEnvironmentVariable("password") ?? "******";
		public static string userId = System.Environment.GetEnvironmentVariable("userId") ?? "ba67019c";
		public static string nodeNumber = System.Environment.GetEnvironmentVariable("nodeNumber") ?? "3127312796";
		public static string meshtasticMqttRootTopic = System.Environment.GetEnvironmentVariable("meshtasticMqttRootTopic") ?? "mesh/messages";
		public static string channelNumber = System.Environment.GetEnvironmentVariable("channelNumber") ?? "1";

	}
}