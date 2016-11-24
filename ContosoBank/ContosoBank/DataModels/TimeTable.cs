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

        [JsonProperty(PropertyName = "Id")]
        public string id { get; set; }

        [JsonProperty(PropertyName = "CreatedAt")]
        public Double createdAt { get; set; }

        [JsonProperty(PropertyName = "UpdatedAt")]
        public string updatedAt { get; set; }

        [JsonProperty(PropertyName = "Version")]
        public string version { get; set; }

        [JsonProperty(PropertyName = "Ticketnum")]
        public string ticketnum { get; set; }

        [JsonProperty(PropertyName = "Meettime")]
        public string meettime { get; set; }

        [JsonProperty(PropertyName = "Deleted")]
        public bool deleted { get; set; }
        

    }
}