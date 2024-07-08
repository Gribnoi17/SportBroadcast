using System.Text;
using Microsoft.AspNetCore.SignalR.Client;
using Sport.Broadcast.Api.Contracts.Broadcasts.Responses;
using Sport.Broadcast.Commentator.Broadcasts;

namespace Sport.Broadcast.Commentator
{
    public class Program
    {
        public static bool SendingMessages;
        
        private static string _broadcastHubUrl = "http://localhost:2023/broadcast";
        private static bool _exitRequested;
        private static string _txtFilePath;
        private static HubConnection _hubConnection;
        private static RegisterBroadcastHandler _registerBroadcastHandler = new();
        
        private static readonly List<long> _idRegisteredBroadcasts = new();
        private static readonly StartBroadcastHandler _startBroadcastHandler = new();
        private static readonly SendMessageHandler _sendMessageHandler = new();

        public static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var enc1251 = Encoding.GetEncoding(1251);
 
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = enc1251;

            string nickname;
            
            while (true)
            {
                Console.Write("Введите ваш никнейм: ");
                nickname = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(nickname))
                {
                    break;
                }
                Console.WriteLine("Ник не должен быть пустым.");
            }

            var sanitizedNickname = new string(nickname
                .Where(c => !Path.GetInvalidFileNameChars().Contains(c))
                .ToArray());

            _txtFilePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "..", "..", "..", "..", "..", "..",
                "sport-broadcast-commentator",
                $"registered_broadcasts_{sanitizedNickname}.txt"
            );
            
            await LoadIdsFromTxt();

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_broadcastHubUrl)
                .Build();

            _hubConnection.On<BroadcastResponse>("StartBroadcast", async broadcastResponse =>
            {
                Console.WriteLine($"Трансляция запущена.\n");
                await _sendMessageHandler.SendMessage(_hubConnection, broadcastResponse);
            });

            _hubConnection.On<long>("StopBroadcast",
                   id =>
                {
                    Console.WriteLine($"Вы успешно завершили трансляцию с Id: {id}");
                    SendingMessages = false;
                });
            
            _hubConnection.On<string>("ReceiveError",
                  message =>
                {
                    Console.WriteLine($"{message}");
                    SendingMessages = false;
                });
                
            await _hubConnection.StartAsync();

            do
            {
                if (!SendingMessages)
                {
                    await ShowMenu();
                }
            } while (!_exitRequested);
        }
        
        private static async Task ShowMenu()
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Регистрация трансляции.");
            Console.WriteLine("2. Запуск трансляции.");
            Console.WriteLine("3. Выход.");
            
            Console.Write("Выберите действие: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    var id = await _registerBroadcastHandler.Handle();

                    if (id != -1 && !_idRegisteredBroadcasts.Contains(id))
                    {
                        _idRegisteredBroadcasts.Add(id);
                        await SaveIdsToTxt(id);
                    }
                    break;

                case "2":
                    await _startBroadcastHandler.Handle(_hubConnection, _idRegisteredBroadcasts);
                    break;

                case "3":
                    _exitRequested = true;
                    break;

                default:
                    Console.WriteLine("Неверный выбор. Пожалуйста, выберите снова.");
                    break;
            }
        }
        
        private static async Task LoadIdsFromTxt()
        {
            try
            {
                if (File.Exists(_txtFilePath))
                {
                    using (var reader = new StreamReader(_txtFilePath))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = await reader.ReadLineAsync();

                            if (string.IsNullOrWhiteSpace(line))
                            {
                                continue;
                            }

                            if (long.TryParse(line, out var id))
                            {
                                _idRegisteredBroadcasts.Add(id);
                            }
                            else
                            {
                                Console.WriteLine($"Ошибка преобразования строки в число: '{line.Trim()}'");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки созданных айди трансляций: {ex.Message}");
            }
        }

        private static async Task SaveIdsToTxt(long id)
        {
            try
            {
                using (var sw = new StreamWriter(_txtFilePath, true))
                {
                    await sw.WriteLineAsync($"{id}");
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения айди трансляции: {ex.Message}");
            }
        }
    }
}