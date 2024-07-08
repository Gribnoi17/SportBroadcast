using System.Net.Http.Json;
using System.Text;
using Sport.Broadcast.Api.Contracts.Broadcasts.Enums;
using Sport.Broadcast.Api.Contracts.Broadcasts.Responses;

namespace Sport.Broadcast.Spectator.Broadcasts
{
    /// <summary>
    /// Обработчик получения списка трансляций.
    /// </summary>
    public class GetBroadcastsHandler
    {
        private const string _getBroadcastUrl = "http://localhost:2023/broadcasts?date=";
        private const int _mainBreakTime = 15;
        private const int _additionalBreakTime = 5;
        private const int _breakHalf = 0;
        private const int _firstHalf = 1;
        private const int _secondHalf = 2;
        private const int _firstAdditionalHalf = 3;
        private const int _secondAdditionalHalf = 4;
        private const int _penaltyHalf  = 5;

        /// <summary>
        /// Получить трансляции на определенную дату.
        /// </summary>
        public async Task Handle()
        {
            Console.Write("Введите дату в формате 'yyyy-MM-dd', на которую хотите получить трансляции: ");

            var dateBroadcasts = Console.ReadLine();

            if (DateOnly.TryParse(dateBroadcasts, out _))
            {
                var broadcasts = await SendRequestToGetBroadcasts(dateBroadcasts);

                if (broadcasts == null)
                {
                    return;
                }

                var response = new StringBuilder();

                foreach (var broadcast in broadcasts)
                {
                    response.Append($"Айди трансляции: {broadcast.Id}\n" +
                                    $"Команда, играющая дома: {broadcast.HomeTeam}\n" +
                                    $"Команда, играющая в гостях: {broadcast.GuestTeam}\n");

                    switch (broadcast.Status)
                    {
                        case BroadcastStatus.Started:
                        {
                            var currentTimeInMinutes = Math.Floor((DateTime.Now - broadcast.StartTime).TotalMinutes);

                            if (broadcast.CurrentHalf == _breakHalf)
                            {
                                response.AppendLine($"Перерыв.\n" +
                                                    $"Счет: {broadcast.ScoreOfHomeTeam}:{broadcast.ScoreOfGuestTeam}");
                                break;
                            }

                            if (broadcast.CurrentHalf == _penaltyHalf)
                            {
                                response.AppendLine("Серия пенальти.");
                                break;
                            }

                            response.AppendLine($"Тайм: {broadcast.CurrentHalf}");

                            switch (broadcast.CurrentHalf)
                            {
                                case _firstHalf:
                                    response.Append($"Игровое время: {currentTimeInMinutes} минута");
                                    break;

                                case _secondHalf:
                                    response.Append(
                                        $"Игровое время: {currentTimeInMinutes - _mainBreakTime - broadcast.TotalExtraTime + broadcast.ExtraTime} минута");
                                    break;

                                case _firstAdditionalHalf:
                                    response.Append(
                                        $"Игровое время: {currentTimeInMinutes - _mainBreakTime - _mainBreakTime - broadcast.TotalExtraTime + broadcast.ExtraTime} минута");
                                    break;

                                case _secondAdditionalHalf:
                                    response.Append(
                                        $"Игровое время: {currentTimeInMinutes - _mainBreakTime - _mainBreakTime - _additionalBreakTime - broadcast.TotalExtraTime + broadcast.ExtraTime} минута");
                                    break;
                            }

                            if (broadcast.ExtraTime != 0)
                            {
                                response.AppendLine($" + {broadcast.ExtraTime} дополнительного времени.");
                            }

                            response.AppendLine();

                            break;
                        }

                        case BroadcastStatus.Ended:
                            response.AppendLine($"Статус трансляции: {broadcast.Status}\n" +
                                                $"Счет: {broadcast.ScoreOfHomeTeam}-{broadcast.ScoreOfGuestTeam}");
                            break;

                        case BroadcastStatus.NotStarted:
                            response.AppendLine($"Статус трансляции: {broadcast.Status}\n" +
                                                $"Дата и время начала трансляции: {broadcast.StartTime}");
                            break;
                    }

                    response.AppendLine();
                }

                Console.WriteLine(response.ToString());
            }
            else
            {
                Console.WriteLine("Неверный формат даты. Пожалуйста, введите корректную дату.");
            }
        }

        private async Task<BroadcastResponse[]> SendRequestToGetBroadcasts(string dateBroadcasts)
        {
            Console.WriteLine("Начался поиск трансляций...\n");
            using (var client = new HttpClient())
            {
                dateBroadcasts = dateBroadcasts.Replace('.', '-');

                var response = await client.GetAsync($"{_getBroadcastUrl}{dateBroadcasts}");

                if (response.IsSuccessStatusCode == false)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ошибка: {errorContent}");

                    return null;
                }

                Console.WriteLine("Найденные трансляции:\n");
                return await response.Content.ReadFromJsonAsync<BroadcastResponse[]>();
            }
        }
    }
}