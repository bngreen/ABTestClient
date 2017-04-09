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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ABTestClient
{
    public class Client
    {
        public Client(string host)
        {
            Host = host;
        }
        public string Host { get; private set; }
        public async Task<bool> CreateExperiment(Experiment experiment)
        {
            var request = HttpWebRequest.Create($"{Host}/api/experiment") as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json";
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(experiment));
            request.ContentLength = data.Length;
            var dataStream = await request.GetRequestStreamAsync();
            dataStream.Write(data, 0, data.Length);
            dataStream.Close();
            using (var response = await request.GetResponseAsync() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    return false;
                return true;
            }
        }
        public async Task<UserState> ActivateExperiment(string name, UserState state)
        {
            var request = HttpWebRequest.Create($"{Host}/api/experiment/{name}/activate") as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json";
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(state));
            request.ContentLength = data.Length;
            var dataStream = await request.GetRequestStreamAsync();
            dataStream.Write(data, 0, data.Length);
            dataStream.Close();
            using (var response = await request.GetResponseAsync() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    return null;
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return JsonConvert.DeserializeObject<UserState>(await reader.ReadToEndAsync());
                }
            }
        }

        public async Task<bool> TrackEvent(string name, EventTrackData eventTrackData)
        {
            var request = HttpWebRequest.Create($"{Host}/api/event/{name}") as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json";
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventTrackData));
            request.ContentLength = data.Length;
            var dataStream = await request.GetRequestStreamAsync();
            dataStream.Write(data, 0, data.Length);
            dataStream.Close();
            using (var response = await request.GetResponseAsync() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    return false;
                return true;
            }
        }
    }
}
