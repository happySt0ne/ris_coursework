using TcpLibrary;

var settings = ServersHelper.ReadServers("../Servers.json");
var server = new Servers();
server.ConfigureServers(settings);

