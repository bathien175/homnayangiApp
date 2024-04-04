using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homnayangiApp.Models
{
    public class Location
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        [BsonElement("name")]
        public string Name { get; set; } = String.Empty;

        [BsonElement("address")]
        public string Address { get; set; } = String.Empty;

        [BsonElement("province")]
        public string Province { get; set; } = String.Empty;

        [BsonElement("district")]
        public string District { get; set; } = String.Empty;

        [BsonElement("min_price")]
        public long MinPrice { get; set; }

        [BsonElement("max_price")]
        public long MaxPrice { get; set; }

        [BsonElement("open_time")]
        public TimeSpan? OpenTime { get; set; }

        [BsonElement("close_time")]
        public TimeSpan? CloseTime { get; set; }

        [BsonElement("is_open_24h")]
        public bool IsOpen24H { get; set; } = false;

        [BsonElement("hotline")]
        public string HotLine { get; set; } = String.Empty;

        [BsonElement("images")]
        public List<string>? Images { get; set; }

        [BsonElement("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [BsonElement("creator")]
        public string? Creator { get; set; }

        [BsonElement("is_share")]
        public bool IsShare { get; set; } = false;
    }
}
