/*
Copyright (C) 2017  Bruno Naspolini Green. All rights reserved.

This file is part of ABTestClient.

ABTestClient is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

ABTestClient is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with ABTestClient.  If not, see <http://www.gnu.org/licenses/>.
*/

using ABTestClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Example
{
    class ClientExample
    {
        private static long getUnixTimeNow()
        {
            return new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
        }

        public int VariantACount=0;

        public void Run(string experimentName, string host, int clients, int operations)
        {
            var experiment = new Experiment(experimentName, "This is a test experiment", getUnixTimeNow(), new Variation[] { new Variation("A", "Variation A", 0.5), new Variation("B", "Variation B", 0.5) });
            var client = new Client(host);
            var random = new Random();
            var places = new string[] { "Amsterdam", "Berlin", "Dublin", "Vancouver" };
            var semaphore = new Semaphore(0, clients);
            foreach (var ci in Enumerable.Range(0, clients))
            {
                semaphore.WaitOne();
                Task.Run(async () =>
                {
                    Console.WriteLine($"Started client_{ci}");
                    ActivationResponse response;
                    do
                    {
                        response = await client.ActivateExperiment(experimentName, new UserState($"user_{ci}"));
                        if (response == null)
                        {
                            Console.WriteLine($"Error Activating Variant, user_{ci}");
                            await Task.Delay(10);
                        }
                    } while (response == null);
                    if (response.Variant == "A")
                        Interlocked.Increment(ref VariantACount);
                    var bookingProbability = response.Variant == "A" ? 0.15 : 0.22;
                    for (int i = 0; i < operations; i++)
                    {
                        var trackData = new EventTrackData(response.UserState, getUnixTimeNow());
                        trackData.metrics.Add("price", (random.NextDouble() * 50000 + 10000).ToString());
                        trackData.metrics.Add("bedrooms", random.Next(1, 4).ToString());
                        trackData.metrics.Add("location", places[random.Next(0, places.Length)]);

                        await TrackEvent(client, "view_property", ci, trackData);

                        if(random.NextDouble() < bookingProbability)
                            await TrackEvent(client, "property_booked", ci, trackData);
                    }
                    Console.WriteLine($"Ended client_{ci}");
                    semaphore.Release();

                });
            }
            semaphore.WaitOne();
        }

        private static async Task TrackEvent(Client client, string name, int ci, EventTrackData trackData)
        {
            bool result;
            do
            {
                result = await client.TrackEvent(name, trackData);
                if (!result)
                {
                    Console.WriteLine($"Error tracking event, user_{ci}");
                    await Task.Delay(10);
                }
            } while (result == false);
        }
    }
}
