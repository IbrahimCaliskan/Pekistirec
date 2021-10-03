/*
 * Copyright (c) 2012 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except
 * in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under the License
 * is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
 * or implied. See the License for the specific language governing permissions and limitations under
 * the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace GoogleAPI
{
    public static class ClientCredentials
    {
        static private string GetAppSetting(string Key, ref string PrivateProp)
        {
            if (string.IsNullOrEmpty(PrivateProp))
                PrivateProp = WebConfigurationManager.AppSettings[Key];

            return PrivateProp;
        }

        private static string _CLIENT_ID;
        public static string CLIENT_ID { get { return GetAppSetting("GoogleAPI:ClientId", ref _CLIENT_ID); } }

        private static string _CLIENT_SECRET = "";
        public static string CLIENT_SECRET { get { return GetAppSetting("GoogleAPI:ClientSecret", ref _CLIENT_SECRET); } }

        private static string _SCOPES;
        public static string[] SCOPES { get { return GetAppSetting("GoogleAPI:Scopes", ref _SCOPES).Split(','); } }

        private static string _REDIRECT_URI;
        public static string REDIRECT_URI { get { return GetAppSetting("GoogleAPI:RedirectUri", ref _REDIRECT_URI); } }

        private static string _API_KEY;
        public static string API_KEY { get { return GetAppSetting("GoogleAPI:ApiKey", ref _API_KEY); } }
    }    
}
