using Microsoft.AspNetCore.SignalR.Client;
using Sport.Broadcast.Spectator.Broadcasts;

namespace Sport.Broadcast.Spectator
{
    public class Program
    {
        public static bool IsConnectedToBroadcast = false;
        
        private static string _broadcastHubUrl = "http://localhost:2023/broadcast";
        private static bool _exitRequested;
        private static HubConnection _hubConnection;
        private static GetBroadcastsHandler _getBroadcastsHandler;
        private static ConnectToBroadcastHandler _connectToBroadcastHandler;
        private static DisconnectFromBroadcastHandler _disconnectFromBroadcastHandler;
        private static long _broadcastId;

        public static async Task Main(string[] args)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_broadcastHubUrl)
                .Build();

            _getBroadcastsHandler = new GetBroadcastsHandler();
            _connectToBroadcastHandler = new ConnectToBroadcastHandler();
            _disconnectFromBroadcastHandler = new DisconnectFromBroadcastHandler();
            
            _hubConnection.On<string>("ReceiveMessage", message => Console.WriteLine($"{message}"));
            _hubConnection.On<long>("StopBroadcast", id => Console.WriteLine($"Трансляция с Id: {id} закончилась."));
            
            _hubConnection.On<string, bool>("JoinBroadcast", (message, isSuccess) =>
            {
                if (isSuccess)
                {
                    IsConnectedToBroadcast = true;
                    Console.WriteLine($"{message}. Для отключения от трансляции нажмите '3'.");
                    return;
                }

                IsConnectedToBroadcast = false;
                Console.WriteLine($"{message}");
            });
            
            _hubConnection.On<string>("LeaveBroadcast", message => Console.WriteLine($"{message}"));

            await _hubConnection.StartAsync();
            
            Console.WriteLine("Приложение готова к работе.");

            do
            {
                await ShowMenu();

            } while (!_exitRequested);
        }
        
        public static async Task ShowMenu()
        {
            if (!IsConnectedToBroadcast)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine("1. Получить список трансляций по дате.");
                Console.WriteLine("2. Подключиться к трансляции.");
                Console.WriteLine("0. Выход.");

                Console.Write("Выберите действие: ");
            }

            var keyInfo = Console.ReadLine();

            if (IsConnectedToBroadcast && keyInfo != "3")
            {
                return;
            }

            if (!IsConnectedToBroadcast && keyInfo == "3")
            {
                Console.WriteLine("Неверный выбор. Пожалуйста, выберите действие из списка.");
                return;
            }

            switch (keyInfo)
            {
                case "1":
                    await _getBroadcastsHandler.Handle();
                    break;

                case "2":
                    _broadcastId = await _connectToBroadcastHandler.Handle(_hubConnection);
                    break;
                
                case "3":
                    await _disconnectFromBroadcastHandler.Handle(_hubConnection, _broadcastId);
                    break;
                
                case "0":
                    _exitRequested = true;
                    return;
                
                default:
                    Console.WriteLine("Неверный выбор. Пожалуйста, выберите действие из списка.");
                    break;
            }
        }
    }
}