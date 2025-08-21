# MeshMqtt

A simple command-line tool and API to send messages to a Meshtastic MQTT broker.  More for information purposes but perfectly usable.  You are probably looking for this:

`.WithPayload($"{{\"from\":{nodeNumber},\"channel\":{channelNumber},\"payload\":\"{body}\",\"type\":\"sendtext\"}}")`

## API Usage
Set the properties of Global.cs via your docker enviroment variables, then start the container.

Send messages with: 

`curl -X 'POST' 'http://localhost:8080/messages?message=Hello'`

## CLI Usage

Run the cli program with required options:

```
meshmqtt --host=HOST --port=PORT --username=USER --password=PASS --user-id=ID --node-number=NUM --meshtastic-mqtt-root-topic=TOPIC --channel=1 --body=BODY
```

All options are required except `--port` (default: 1883) and `--channel` (default: 1).

### Example
```
meshmqtt --host=192.168.4.197 --username=mesh --password=MeshPassword123! --user-id=ba67019c --node-number=3127312796 --meshtastic-mqtt-root-topic=mesh/messages --body="Hello from the command line"
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

## Requirements
- .NET 8 SDK or newer
- MQTTnet NuGet package

## License
MIT
