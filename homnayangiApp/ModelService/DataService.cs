using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace homnayangiApp.ModelService
{
    public class DataService
    {
        public const String DatabaseName = "HomNayAnGi";
        public const String UserCollection = "User";
        public const String LocationCollection = "Location";
        public const String TagsCollection = "Tags";
        //"mongodb://root:O2RP3WuoYxHt8lAl@ac-zpgptup-shard-00-00.st3asz1.mongodb.net:27017,ac-zpgptup-shard-00-01.st3asz1.mongodb.net:27017,ac-zpgptup-shard-00-02.st3asz1.mongodb.net:27017/HomNayAnGi?ssl=true&replicaSet=atlas-ogq50t-shard-0&authSource=admin&retryWrites=true&w=majority&appName=Cluster0"
        public const String ConnectString = "mongodb://root:O2RP3WuoYxHt8lAl@ac-zpgptup-shard-00-00.st3asz1.mongodb.net:27017,ac-zpgptup-shard-00-01.st3asz1.mongodb.net:27017,ac-zpgptup-shard-00-02.st3asz1.mongodb.net:27017/HomNayAnGi?ssl=true&replicaSet=atlas-ogq50t-shard-0&authSource=admin&retryWrites=true&w=majority&appName=Cluster0";
        public const String ConnectStringFirebase = "https://homnayangi-ed618-default-rtdb.firebaseio.com/";
        public const String ConnectStringFirebaseStorage = "homnayangi-ed618.appspot.com";
    }
}
