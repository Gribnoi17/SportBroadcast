using System.Collections;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Models;

namespace Sport.Broadcast.AppServices.UnitTests.Broadcasts.Data
{
    internal class ValidBroadcastInternalRequestTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new BroadcastInternalRequest()
                {
                    HomeTeam = "Быки",
                    GuestTeam = "Вороны",
                    StartTime = DateTime.Today + TimeSpan.FromDays(3)
                }
            };
            
            yield return new object[]
            {
                new BroadcastInternalRequest()
                {
                    HomeTeam = "Киты",
                    GuestTeam = "Цска",
                    StartTime = DateTime.Today + TimeSpan.FromDays(2)
                }
            };

            yield return new object[]
            {
                new BroadcastInternalRequest()
                {
                    HomeTeam = "Кошки",
                    GuestTeam = "Собачки",
                    StartTime = DateTime.Today + TimeSpan.FromDays(5)
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}