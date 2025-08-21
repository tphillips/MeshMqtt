# MeshMqtt

A *simple* command-line tool and API to send messages to a Meshtastic MQTT broker.  Why? Because with an mqtt integration with your Meshtastic nodes you can bridge the gap between the www and your mesh. Consider perhaps using this with Home Assistant.  Or down downtime alerts?  Or simply for range testing.

```
			Cli / Curl / Your Code / HA / IFTTT
						  |
					   MeshMqtt
						  |
				+---------------------+
				|     MQTT Server     |
				+---------------------+
						  |
						  |
				+----------------------+
				|   Meshtastic Node    |
				| (Internet Connected) |
				+----------------------+
						  |
		 +----------------+----------------+
		 |                                 |
+---------------------+         +---------------------+
| Meshtastic Node     |         | Meshtastic Node     |
| (No Internet)       |         | (No Internet)       |
+---------------------+         +---------------------+

	    (Messages relayed via mesh network)
```

## Docker Compose (The EASY way for self-hosters)
Clone the repo (assumes you have a folder in your home called dockers, but clone it wherever you like):
```
cd dockers
git clone https://github.com/tphillips/MeshMqtt.git
cd meshmqtt
nano compose.yaml
```
Set your environment up in there.  The following needs to be set with your values:
```
- host=192.168.4.197
- port=1883
- username=mesh
- password=******
- userId=ba67019c
- nodeNumber=3127312796
- meshtasticMqttRootTopic=mesh/messages
- channelNumber=1
```
You can also set the port that will be exposed by changing the port from 8097:
```
ports:
	- "8097:8080"
```
Start the service
```
docker compose up -d
```

### Use the CLI from the container
The cli is included in the container, so you can run it by attaching to the container.

```
sudo docker ps -a
```
Find the container running meshmqtt-api (Hint: it's probably `meshmqtt-api-1`).
Run via bash:
```
sudo docker exec -it meshmqtt-api-1 /bin/bash
>:/app# ./MeshMqtt
```
OR run the executable directly in the container:
```
sudo docker exec -it meshmqtt-api-1 /app/MeshMqtt --host=192.168.4.197 --username=mesh --password=MeshPass! --user-id=ba67019c --node-number=3127312796 --meshtastic-mqtt-root-topic=mesh/messages --body="Hello from the command line"
```

## API Usage
Set the properties of Global.cs via your docker enviroment variables, then start the container.

Send messages with: 

`curl -X 'POST' 'http://localhost:8097/messages?message=Hello'`

The api is useful to allow you to send data to your mesh via a simple curl command.  Avoiding the complexities of mqtt (and even the complexities of this cli).

A useful example would be a HomeAssistant integration.

Add a shell script to your tool box by adding the following to your Home Assistant `configuration.yaml`:

```
shell_command:
  send_mesh_message: "curl -X POST 'https://your.ip.or.url/messages?message={{ arguments }}'"

```

Then integrate into a HomeAssistant automation:

```
alias: Send mesh weather on change
description: "Sends a weather snapshot to Meshtastic on weather change"
triggers:
  - trigger: state
    entity_id:
      - weather.forecast_home
conditions: []
actions:
  - action: shell_command.send_mesh_message
    data_template:
      arguments: >-
        'Weather:+'{{states('weather.forecast_home')}}'.+Wind+is:+'{{state_attr('weather.forecast_home',
        'wind_speed')}}'mph.+Kitchen+temp+is:+'{{states('sensor.kitchen_temperature')
        | round(1)}}'c.+Sunsets:+'{{states('sensor.sun_next_setting')}}
mode: single
```

## CLI Usage (Not using Docker)

If you want to avoid Docker and use the cli, you will need to build it.
Run the cli program with required options:

```
meshmqtt --host=HOST --port=PORT --username=USER --password=PASS --user-id=ID --node-number=NUM --meshtastic-mqtt-root-topic=TOPIC --channel=1 --body=BODY
```

All options are required except `--port` (default: 1883), `--channel` (default: 1) and `--repeat-interval`.

### Example
```
meshmqtt --host=192.168.4.197 --username=mesh --password=MeshPass! --user-id=ba67019c --node-number=3127312796 --meshtastic-mqtt-root-topic=mesh/messages --body="Hello from the command line"
```

## Options
- `--host` : MQTT broker host (required)
- `--port` : MQTT broker port (default: 1883)
- `--username` : MQTT username (required)
- `--password` : MQTT password (required)
- `--user-id` : Meshtastic user ID of the network connected node WITHOUT THE LEADING ! (required)
- `--node-number` : Meshtastic node number of the network connected node (required)
- `--meshtastic-mqtt-root-topic` : Root topic as set in meshtastic MQTT settings (required)
- `--channel` : Meshtastic Channel Number. Defaults to 1!! (Your first manually added channel, not LongFast)
- `--body` : Message body (required)
- `--repeat-interval` : Repeat the message every x seconds. Default no repeat

The `--repeat-interval` setting is useful for range testing, for example.

## Requirements
- Docker

OR

- .NET 8 SDK or newer
- MQTTnet NuGet package

Plus

- An (a?) mqtt server

## What, No MQTT Server?!

I got you:

Mqtt `compose.yaml`
```
services:
  mqtt5:
    image: eclipse-mosquitto
    container_name: mqtt5
    ports:
      - "1883:1883"  # Default MQTT port
      - "9001:9001"  # MQTT port for WebSockets
    volumes:
      - ./config:/mosquitto/config:rw
      - ./data:/mosquitto/data:rw
      - ./log:/mosquitto/log:rw
    restart: unless-stopped
```


## License
MIT
