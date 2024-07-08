using Microsoft.AspNetCore.SignalR.Client;

namespace Sport.Broadcast.Spectator.Broadcasts
{
    /// <summary>
    /// Обработчик отключения от трансляции.
    /// </summary>
    public class DisconnectFromBroadcastHandler
    {
        /// <summary>
        /// Отключиться от трансляции с определенным Id.
        /// </summary>
        /// <param name="hubConnection">Подключение к хабу.</param>
        /// <param name="broadcastId">Id трансляции.</param>
        public async Task Handle(HubConnection hubConnection, long broadcastId)
        {
            if (Program.IsConnectedToBroadcast)
            {
                await hubConnection.InvokeAsync("LeaveBroadcast", broadcastId);
                Program.IsConnectedToBroadcast = false;
            }
        }
    }
}