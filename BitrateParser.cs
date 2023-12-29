/*
*   Author: Francisco Gomes
*   .NET Version: 6.0.101
*
*   Arguments:  None -> Example JSON is used
*               JSON string -> Given string is used
*
*   Parses a given JSON string, calculates and displays the communication bitrates
*   using the converter method BitrateOf(long), assuming a 2Hz polling rate.
* 
*   Run example:    No args -> dotnet run
*                   JSON in args -> dotnet run -- [INSERT JSON HERE]
*/ 

namespace BitrateParser 
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;


    public class BitrateParser { 


        // Polling Rate

        private const int POLLING_RATE = 2;


        // JSON Example Message

        private const string SERIALIZED_JSON = 
        @"{
            ""Device"": ""Arista"",
            ""Model"": ""X-Video"",
            ""NIC"": [{
                ""Description"": ""Linksys ABR"",
                ""MAC"": ""14:91:82:3C:D6:7D"",
                ""Timestamp"": ""2020-03-23T18:25:43.511Z"",
                ""Rx"": ""3698574500"",
                ""Tx"": ""122558800""
            }]
        }";


        // JSON Message Class

        private class DeviceMessage {

            [JsonProperty("Device")]
            public string Device { get; set; } = string.Empty;
            
            [JsonProperty("Model")]
            public string Model { get; set; } = string.Empty;

            [JsonProperty("NIC")]
            public NICMessage[] NIC { get; set; } = new NICMessage[0];


            public static DeviceMessage FromJson(string json) => JsonConvert.DeserializeObject<DeviceMessage>(json);
            public static string ToJson(DeviceMessage msg) => JsonConvert.SerializeObject(msg);

        }

        private class NICMessage {

            [JsonProperty("Description")]
            public string Description { get; set; } = string.Empty;
            
            [JsonProperty("MAC")]
            public string MAC { get; set; } = string.Empty;
            
            [JsonProperty("Timestamp")]
            public string Timestamp { get; set; } = string.Empty;

            [JsonProperty("Rx")]
            public ulong Rx { get; set; } = 0;
            
            [JsonProperty("Tx")]
            public ulong Tx { get; set; } = 0;

        }




        public static ulong BitrateOf(ulong numberOfOctets) => numberOfOctets * 8 * POLLING_RATE;




        static void Main(string[] args)
        {

            string json;
            DeviceMessage msg;

            // JSON picker
            if(args.Length == 0)
                json = SERIALIZED_JSON;
            else
                json = args[0];


            // Parse JSON
            try {
                
                msg = DeviceMessage.FromJson(json);

            } catch (Exception e) {
                System.Console.WriteLine($"Error while parsing JSON! Exiting...\nFull Error: {e}");
                return;
            }


            // Get Bitrates
            ulong bitrateRx = BitrateOf(msg.NIC[0].Rx);
            ulong bitrateTx = BitrateOf(msg.NIC[0].Tx);


            // Print Result
            System.Console.WriteLine($"\nJSON Used: {json}\n\nCalculated Bitrates:\n   Rx Bitrate -> {bitrateRx}\n   Tx Bitrate -> {bitrateTx}\n");
        }


    }

}