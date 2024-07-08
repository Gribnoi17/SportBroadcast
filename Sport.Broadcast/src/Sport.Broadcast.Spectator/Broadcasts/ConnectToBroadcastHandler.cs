using Microsoft.AspNetCore.SignalR.Client;

namespace Sport.Broadcast.Spectator.Broadcasts
{
    /// <summary>
    /// Обработчик подключения к трансляции.
    /// </summary>
    public class ConnectToBroadcastHandler
    {
        /// <summary>
        /// Подключитсья к трансляции.
        /// </summary>
        /// <param name="hubConnection">Подключение к хабу.</param>
        /// <returns>Id трансляции.</returns>
        public async Task<long> Handle(HubConnection hubConnection)
        {
            Console.Write("Напишите айди трансляции, к которой хотите подключиться: ");
            
            var broadcastId = Convert.ToInt32(Console.ReadLine());
            
            try
            { 
                Console.WriteLine("Начался процесс подключения к трансляции...");
                await hubConnection.InvokeAsync("JoinBroadcast", broadcastId);

                return broadcastId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка подключения. Проверье Id трансляции.");
                Program.IsConnectedToBroadcast = false;
                return -1;
            }
        }
    }
}