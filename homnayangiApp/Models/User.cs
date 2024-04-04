using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homnayangiApp.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        [BsonElement("iduser")]
        public string IDUser { get; set; } = String.Empty;

        [BsonElement("phone")]
        public string Phone { get; set; } = String.Empty;

        [BsonElement("name")]
        public string Name { get; set; } = String.Empty;

        [BsonElement("gender")]
        public string Gender { get; set; } = String.Empty;

        [BsonElement("datebirth")]
        public string DateBirth { get; set; } = DateTime.Now.Date.ToString("yyyy-MM-dd");

        [BsonElement("city")]
        public string City { get; set; } = String.Empty;

        [BsonElement("district")]
        public String Dictrict { get; set; } = String.Empty;

        [BsonElement("image")]
        public string? ImageData { get; set; } = String.Empty;

        [BsonElement("savestore")]
        public List<string>? SaveStore { get; set; }

        [BsonElement("password")]
        public string Password { get; set; } = String.Empty;

        [BsonElement("tags")]
        public List<string> Tags { get; set; } = new List<string>();

    }
}
