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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABTestClient
{
    public class EventTrackData
    {
        public UserState userstate { get; private set; }
        public Int64 timestamp { get; set; }
        public IDictionary<String, String> metrics { get; set; }
        public EventTrackData()
        {
            metrics = new Dictionary<String, String>();
        }
        public EventTrackData(UserState userState, Int64 timestamp):this()
        {
            userstate = userState;
            this.timestamp = timestamp;
        }
    }
}
