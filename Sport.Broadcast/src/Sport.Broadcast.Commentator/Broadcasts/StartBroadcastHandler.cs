using Microsoft.AspNetCore.SignalR.Client;

namespace Sport.Broadcast.Commentator.Broadcasts
{
    /// <summary>
    /// Обрабочтик запуска трансляции.
    /// </summary>
    public class StartBroadcastHandler
    {
        /// <summary>
        /// Запустить трансляцию.
        /// </summary>
        /// <param name="hubConnection">Подключение к Хабу.</param>
        /// <param name="idRegisteredBroadcasts">Список трансляций, которые может запустить комментатор.</param>
        public async Task Handle(HubConnection hubConnection, List<long> idRegisteredBroadcasts)
        {
            string id;
            
            while (true)
            {
                Console.Write("Введите айди трансляции, которую хотите начать: ");
                id = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(id) && char.IsDigit(id[0]) && id.Length == 1)
                {
                    break;
                }
                
                Console.WriteLine("Айди трансляции некоректный!");
            }

            var broadcastId = int.Parse(id);

            if (!idRegisteredBroadcasts.Contains(broadcastId))
            {
                Console.WriteLine("Ошибка запуска! Вы можете запустить только те трансляции, которые зарегестрировали.");
                return;
            }

            Program.SendingMessages = true;
            Console.WriteLine("Начался запуск трансляции...");
            await hubConnection.SendAsync("StartBroadcast", broadcastId);
        }
    }
}