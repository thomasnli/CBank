using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoBank.DataModels
{
    public class TimeTable
    {

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "createdAt")]
        public Double CreatedAt { get; set; }

        [JsonProperty(PropertyName = "updatedAt")]
        public string UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }

        [JsonProperty(PropertyName = "ticketnum")]
        public string Ticketnum { get; set; }

        [JsonProperty(PropertyName = "meettime")]
        public string Meettime { get; set; }

        [JsonProperty(PropertyName = "deleted")]
        public bool Deleted { get; set; }
        

    }
}