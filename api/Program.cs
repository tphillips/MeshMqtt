using Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/messages", (string message) =>
{
    var topic = $"{Global.meshtasticMqttRootTopic}/2/json/Mqtt/{Global.userId}";
    Mqtt.SendMessage(
        Global.host,
        Global.port,
        Global.username,
        Global.password,
        Global.userId,
        Global.nodeNumber,
        Global.meshtasticMqttRootTopic,
        message,
        Global.channelNumber,
        topic,
        Guid.NewGuid().ToString()
    ).GetAwaiter().GetResult();
    return message;
})
.WithName("messages")
.WithOpenApi();

app.Run();
