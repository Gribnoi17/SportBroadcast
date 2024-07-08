using System.Collections;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Models;

namespace Sport.Broadcast.AppServices.UnitTests.Broadcasts.Data
{
    internal class InvalidBroadcastInternalRequestTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new BroadcastInternalRequest()
                {
                    HomeTeam = "",
                    GuestTeam = "Вороны",
                    StartTime = new DateTime(2024, 08, 08)
                }
            };
            
            yield return new object[]
            {
                new BroadcastInternalRequest()
                {
                    HomeTeam = "Быки",
                    GuestTeam = "",
                    StartTime = new DateTime(2024, 08, 08)
                }
            };

            yield return new object[]
            {
                new BroadcastInternalRequest()
                {
                    HomeTeam = "Быки",
                    GuestTeam = "Вороны",
                    StartTime = new DateTime(2001, 08, 08)
                }
            };

            yield return new object[]
            {
                new BroadcastInternalRequest()
                {
                    HomeTeam = "",
                    GuestTeam = "",
                    StartTime = new DateTime(2001, 08, 08)
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}