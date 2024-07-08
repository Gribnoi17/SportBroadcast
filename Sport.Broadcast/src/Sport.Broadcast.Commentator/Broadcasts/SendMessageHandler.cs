using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.SignalR.Client;
using Sport.Broadcast.Api.Contracts.Broadcasts.Responses;
using Sport.Broadcast.Api.Contracts.Broadcasts.SignalR.Enums;
using Sport.Broadcast.Api.Contracts.Broadcasts.SignalR.Requests;

namespace Sport.Broadcast.Commentator.Broadcasts
{
    /// <summary>
    /// Обработчик отправки сообщения.
    /// </summary>
    public class SendMessageHandler
    {
        private readonly Stopwatch _gameTime = new();
        private bool _isBreak;
        private bool _isExtraTime;
        private int _extraTime;
        private int _totalExtraTime;
        private double _timePassed;
        
        /// <summary>
        /// Отправить сообщения.
        /// </summary>
        /// <param name="hubConnection">Подключение к Хабу.</param>
        /// <param name="broadcast">Трансляция, на которую отправляются сообщения.</param>
        public async Task SendMessage(HubConnection hubConnection, BroadcastResponse broadcast)
        {
            _gameTime.Start();
            _timePassed = Math.Floor((DateTime.Now - broadcast.StartTime).TotalMinutes);

            Console.WriteLine("Можете отправлять сообщения.\n" +
                              "Минута игры добавляется автоматчиески, если оставить поле пустым.\n" +
                              "Если не указать событие в игре, то оно автоматически будет пропущено.\n" +
                              "Серия пенальти считается как 5 тайм, учтите это.\n" +
                              "Для остановки трансляции напишите в блоке информации о событии 'exit'.\n");
            while (true)
            {
                var message = new MessageRequest();

                while (true)
                {
                    Console.Write("Введите минуту: ");
                    message.Minute = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(message.Minute))
                    {
                        break;
                    }

                    if (message.Minute.All(char.IsDigit))
                    {
                        break;
                    }
                    
                    Console.WriteLine("Минута может состоять только из цифр без пробелов!");
                }
                
                Console.WriteLine("Возможные события:");

                if (!_isBreak)
                {
                    Console.WriteLine("0. Гол.\n" +
                                      "1. VAR.\n" +
                                      "2. VAR - Отмена гола.\n" +
                                      "3. Замена.\n" +
                                      "4. Желтая карточка.\n" +
                                      "5. Вторая желтая карточка.\n" +
                                      "6. Красная карточка.\n" +
                                      "7. Дополнительное время.\n" +
                                      "8. Перерыв.\n" +
                                      "9. Конец перерыва.");
                }
                else
                {
                    Console.WriteLine("9. Конец перерыва - будет выбрано автоматически. Нажмите Enter.");
                }

                Console.Write("Выберите событие: ");
                message.Event = ChoiceEventIngame();

                if (message.Event == EventInGame.ExtraTime)
                {
                    while (true)
                    {
                        Console.Write("Введите добавленное дополнительное время: ");
                        var extraTime = Console.ReadLine();

                        if (!string.IsNullOrWhiteSpace(extraTime) && char.IsDigit(extraTime[0]) && extraTime.Length == 1)
                        {
                            message.ExtraTime = Convert.ToInt32(extraTime);
                            _extraTime = message.ExtraTime.Value;
                            _totalExtraTime += _extraTime;
                            break;
                        }
                        Console.WriteLine("Не было введено добавленное время.");
                    }
                }
                else if (message.Event == EventInGame.Goal || message.Event == EventInGame.VarGoalCancellation)
                {
                    while (true)
                    {
                        Console.Write("Введите новый счет матча через '-' : ");
                        var score = Console.ReadLine().Trim();
                        
                        if (IsCorrectScore(score))
                        {
                            message.Score = score;
                            break;
                        }
                        
                        Console.WriteLine("Не был корректно введен счет матча.");
                    }
                }
                else if (message.Event == EventInGame.BeginningHalf)
                {
                    while (true)
                    {
                        Console.Write("Введите номер начатого тайма: ");
                        var half = Console.ReadLine().Trim();
                        
                        if (!string.IsNullOrWhiteSpace(half) && char.IsDigit(half[0]) && half.Length == 1)
                        {
                            message.HalfNumberAfterBreak = Convert.ToInt32(half);
                            break;
                        }
                        
                        Console.WriteLine("Номер тайма введен не коректно.");
                    }
                }

                Console.Write("Введите имя игрока: ");
                message.PlayerName = Console.ReadLine();

                Console.Write("Введите информацию о событии: ");
                message.Text = Console.ReadLine();

                if (message.Text == "exit")
                {
                    await StopBroadcast(hubConnection, broadcast.Id);
                    break;
                }

                if (string.IsNullOrEmpty(message.Minute))
                {
                    SetMinuteOfGame(message, broadcast);
                }

                await hubConnection.SendAsync("SendMessage", broadcast.Id, message);
                Console.WriteLine();
            }
        }

        private EventInGame? ChoiceEventIngame()
        {
            var choice = Console.ReadLine();

            if (_isBreak)
            {
                _isBreak = false;
                _isExtraTime = false;
                return EventInGame.BeginningHalf;
            }

            return choice switch
            {
                "0" => EventInGame.Goal,
                "1" => EventInGame.Var,
                "2" => EventInGame.VarGoalCancellation,
                "3" => EventInGame.Substitution,
                "4" => EventInGame.YellowCard,
                "5" => EventInGame.SecondYellowCard,
                "6" => EventInGame.RedCard,
                "7" => EventInGame.ExtraTime,
                "8" => EventInGame.Break,
                "9" => EventInGame.BeginningHalf,
                _ => null
            };
        }

        private void SetMinuteOfGame(MessageRequest messageRequest, BroadcastResponse broadcast)
        {
            if (!_gameTime.IsRunning)
            {
                _gameTime.Start();
            }

            var activeTime = new TimeSpan(_gameTime.ElapsedTicks);

            if (messageRequest.Event is EventInGame.Break or EventInGame.ExtraTime)
            {
                messageRequest.Minute = Math.Floor((activeTime.TotalMinutes + _timePassed) - (_totalExtraTime - _extraTime)).ToString(CultureInfo.InvariantCulture);
                
                switch (messageRequest.Event)
                {
                    case EventInGame.ExtraTime:
                        _isExtraTime = true;
                        break;
                    case EventInGame.Break:
                        _isBreak = true;
                        _extraTime = 0;
                        _gameTime.Stop();
                        break;
                }

                return;
            }

            messageRequest.Minute = _isExtraTime
                ? Math.Floor((activeTime.TotalMinutes + _timePassed) - (_totalExtraTime - _extraTime)).ToString(CultureInfo.InvariantCulture)
                : Math.Floor((activeTime.TotalMinutes + _timePassed) - _totalExtraTime).ToString(CultureInfo.InvariantCulture);
        }

        private bool IsCorrectScore(string score)
        {
            if (string.IsNullOrWhiteSpace(score) || !score.Contains('-'))
            {
                return false;
            }
                    
            foreach (var item in score)
            {
                if (item == '-')
                {
                    continue;
                }

                if (!char.IsDigit(item))
                {
                    return false;;
                }
            }

            return true;
        }

        private async Task StopBroadcast(HubConnection hubConnection, long broadcastId)
        {
            await hubConnection.SendAsync("StopBroadcast", broadcastId);
        }
    }
}
