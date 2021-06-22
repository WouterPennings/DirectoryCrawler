using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DirectoryCrawler.Logic
{
    public class Client
    {
        public string ClientId { get; set; }
        public List<string> Files { get; set; }
        
        public string GetId()
        {
            return ClientId;
        }

        public void MaybeSetId()
        {
            if (ClientId.Length == 0)
            {
                ClientId = Guid.NewGuid().ToString();
                string fileLocation = @"../../../Files/info.json";
                SaveClientId(fileLocation);
            }
        }

        private void SaveClientId(string fileLocation)
        {
            string jsonString = JsonSerializer.Serialize(this);
            File.WriteAllText(fileLocation, jsonString);
        }
    }
}