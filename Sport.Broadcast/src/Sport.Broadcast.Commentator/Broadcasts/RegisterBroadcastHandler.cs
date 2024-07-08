using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
using Sport.Broadcast.Api.Contracts.Broadcasts.Requests;
using Sport.Broadcast.Api.Contracts.Broadcasts.Responses;

namespace Sport.Broadcast.Commentator.Broadcasts
{
    /// <summary>
    /// Обработчик для регистрации трансляции.
    /// </summary>
    public class RegisterBroadcastHandler
    {
        private const string _registerBroadcastUrl = "http://localhost:2023/broadcasts/register";
        
        /// <summary>
        /// Зарегистрировать трансляцию.
        /// </summary>
        /// <returns>Id трансляции.</returns>
        public async Task<long> Handle()
        {
            Console.WriteLine("Введите данные для регистрации трансляции.");
            
            Console.Write("Название команды, играющей дома: ");
            var homeTeam = Console.ReadLine();
            
            Console.Write("Название команды, играющей в гостях: ");
            var guestTeam = Console.ReadLine();

            Console.Write("Время начала трансляции (yyyy-MM-dd HH:mm:ss): ");

            if (DateTime.TryParse(Console.ReadLine(), out var startTime))
            {
                var registrationRequest = new BroadcastRegistrationRequest
                {
                    HomeTeam = homeTeam,
                    GuestTeam = guestTeam,
                    StartTime = startTime
                };
                
                if (!IsDataCorrect(homeTeam, guestTeam, startTime))
                {
                    return -1;
                }

                var broadcast = await SendRequestToRegisterBroadcast(registrationRequest);

                if (broadcast.Id != 0)
                {
                    Console.WriteLine($"Трансляция успешно зарегистрирована. Id трансляции: {broadcast.Id}");
                }
                else
                {
                    Console.WriteLine("Ошибка при регистрации трансляции.");
                    return -1;
                }

                return broadcast.Id;
            }
            
            Console.WriteLine("Неверный формат времени. Пожалуйста, введите корректную дату и время.");

            return -1;
        }
        
        private async Task<BroadcastResponse> SendRequestToRegisterBroadcast(BroadcastRegistrationRequest broadcastRegistrationRequest)
        {
            using (var client = new HttpClient())
            {
                var jsonContent = JsonConvert.SerializeObject(
                    broadcastRegistrationRequest,
                    Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        DateFormatString = "yyyy-MM-ddTHH:mm:ss.ff1Z"
                    });
                
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(_registerBroadcastUrl, content);

                return await response.Content.ReadFromJsonAsync<BroadcastResponse>();
            }
        }

        private bool IsDataCorrect(string homeTeam, string guestTeam, DateTime startTime)
        {
            var currentTime = DateTime.Now.ToUniversalTime();
            startTime = startTime.ToUniversalTime();

            if (string.IsNullOrWhiteSpace(homeTeam) || string.IsNullOrWhiteSpace(guestTeam))
            {
                Console.WriteLine("Не указана команда, которая играет дома.");
                return false;
            }
            
            if (string.IsNullOrWhiteSpace(guestTeam))
            {
                Console.WriteLine("Не указана команда, которая играет в гостях.");
                return false;
            }

            if (currentTime > startTime)
            {
                Console.WriteLine("Начала трансляции не может быть в прошлом.");
                return false;
            }

            return true;
        }
    }
}