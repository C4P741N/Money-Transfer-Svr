using money_transfer_server_side.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net;

namespace money_transfer_server_side.DataServer
{
    public class DS_MongoCollector(IConfiguration config) : IAuth
    {
        [Obsolete]
        public void CheckConnection()
        {
            string username = "danconawm";
            string password = "password";
            string mongoDbAuthMechanism = "SCRAM-SHA-1";

            MongoInternalIdentity internalIdentity = new ("admin", username);
            PasswordEvidence passwordEvidence = new(password);
            MongoCredential mongoCredential = new (mongoDbAuthMechanism,internalIdentity, passwordEvidence);
            List<MongoCredential> credentials = [mongoCredential];
            MongoClientSettings settings = new();
            // comment this line below if your mongo doesn't run on secured mode
            settings.Credentials = credentials;
            string mongoHost = "102.167.223.167/32";
            MongoServerAddress address = new (mongoHost);
            settings.Server = address;

            MongoClient client = new MongoClient(settings);

            var mongoServer = client.GetDatabase("sample_weatherdata");
            var coll = mongoServer.GetCollection<Weather>("data");

            string userId = "5553a998e4b02cf7151190b8";

            ObjectId objectId;
            if (!ObjectId.TryParse(userId, out objectId))
            {

            }
            // Create a filter to find a document with the specified userId
            var filter = Builders<Weather>.Filter.Eq("elevation", objectId);

            // Use the filter with the Find method to retrieve the document
            var user = coll.Find(filter).FirstOrDefault();
        }
        public void CheckConnection_III()
        {
            const string connectionUri = "mongodb+srv://danconawm:<password>@cluster-mwanthi.dtherj1.mongodb.net/?authSource=admin";
            var settings = MongoClientSettings.FromConnectionString(connectionUri);
            // Set the ServerApi field of the settings object to Stable API version 1
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            // Create a new client and connect to the server
            var client = new MongoClient(settings);
            // Send a ping to confirm a successful connection
            try
            {
                var result = client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public IMongoCollection<Weather> WeatherCollection { get; private set; }
        public IMongoDatabase db { get; private set; }
        public string DbName { get; private set; }
        public void CheckConnection_II()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json").Build();

            var client = new MongoClient(config.GetConnectionString("MongoDB"));
            DbName = config["Demo"];
            db = client.GetDatabase(DbName);

            WeatherCollection = db.GetCollection<Weather>("");
            
        }
        public void CheckConnection_I()
        {
            var config = WebApplication.CreateBuilder().Configuration;

            var dbName = "Demo";

            var client = new MongoClient(config.GetConnectionString("MongoDB"));

            try
            {
                var result = client.GetDatabase(dbName).RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
            }
            catch (MongoException ex)
            {
                Console.WriteLine($"Error connecting to MongoDB: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex}");
            }
        }
        public void GetValues()
        {
            // TODO:
            // Replace the placeholder connection string below with your
            // Altas cluster specifics. Be sure it includes
            // a valid username and password! Note that in a production environment,
            // you do not want to store your password in plain-text here.

            var mongoUri = "mongodb+srv://danconawm:<password>@cluster-mwanthi.dtherj1.mongodb.net/?retryWrites=true&w=majority";

            // The IMongoClient is the object that defines the connection to our
            // datastore (Atlas, for example)
            IMongoClient client;

            // An IMongoCollection defines a connection to a specific MongoDB
            // collection. Your app may have one or many different IMongoCollection
            // objects.
            IMongoCollection<Weather> collection;

            // Note that you must define the *type* of data stored in the
            // IMongoCollection. We have created a class called Recipe at
            // the bottom of this file that serves as a "mapping class" -- the 
            // driver maps the C# class to the BSON stored in MongoDB.

            // Using mapping classes is strongly advised, but if you
            // don't create them, you can always use the more generic BsonDocument
            // type.

            try
            {
                client = new MongoClient(mongoUri);
            }
            catch (Exception e)
            {
                Console.WriteLine("There was a problem connecting to your " +
                    "Atlas cluster. Check that the URI includes a valid " +
                    "username and password, and that your IP address is " +
                    $"in the Access List. Message: {e.Message}");
                Console.WriteLine(e);
                Console.WriteLine();
                return;
            }

            // Provide the name of the database and collection you want to use.
            // If they don't already exist, the driver and Atlas will create them
            // automatically when you first write data.
            var dbName = "sample_weatherdata";
            var collectionName = "data";

            collection = client.GetDatabase(dbName)
               .GetCollection<Weather>(collectionName);

            string userId = "5553a998e4b02cf7151190b8";

            ObjectId objectId;
            if (!ObjectId.TryParse(userId, out objectId))
            {
                // Handle invalid ObjectId format
                //return null;
            }

            // Create a filter to find a document with the specified userId
            var filter = Builders<Weather>.Filter.Eq("_id", objectId);

            // Use the filter with the Find method to retrieve the document
            var user = collection.Find(filter).FirstOrDefault();

            //var filter = Builders<Weather>.Filter.Eq("_id", ObjectId.Parse(userId));

            /*      *** INSERT DOCUMENTS ***
             * 
             * You can insert individual documents using collection.Insert(). 
             * In this example, we're going to create 4 documents and then 
             * insert them all in one call with InsertMany().
             */

            //var docs = Weather.GetRecipes();

            //try
            //{
            //    collection.InsertMany(docs);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine($"Something went wrong trying to insert the new documents." +
            //        $" Message: {e.Message}");
            //    Console.WriteLine(e);
            //    Console.WriteLine();
            //    return;
            //}

            /*      *** FIND DOCUMENTS ***
             * 
             * Now that we have data in Atlas, we can read it. To retrieve all of
             * the data in a collection, we call Find() with an empty filter. 
             * The Builders class is very helpful when building complex 
             * filters, and is used here to show its most basic use.
             */

            var allDocs = collection.Find(Builders<Weather>.Filter.Empty)
                .ToList();

            //foreach (Weather recipe in allDocs)
            //{
            //    Console.WriteLine($"{recipe.Name} has {recipe.Ingredients.Count} ingredients " +
            //        $"and takes {recipe.PrepTimeInMinutes} minutes to make");
            //    Console.WriteLine();
            //}

            // We can also find a single document. Let's find the first document
            // that has the string "potato" in the Ingredients list. Again we
            // use the Builders class to create the filter, and a LINQ
            // statement to define the property and value we're after:

            //var findFilter = Builders<Weather>
            //    .Filter.AnyEq(t => t.elevation,
            //    9999);

            //var findResult = collection.Find(findFilter).FirstOrDefault();

            //if (findResult == null)
            //{
            //    Console.WriteLine(
            //        "I didn't find any recipes that contain 'potato' as an ingredient.");
            //    Console.WriteLine();
            //    return;
            //}
            //Console.WriteLine("We've retrieved the document:");
            //Console.WriteLine(findResult.ToString());
            //Console.WriteLine();

            /*      *** UPDATE A DOCUMENT ***
             * 
             * You can update a single document or multiple documents in a single call.
             * 
             * Here we update the PrepTimeInMinutes value on the document we 
             * just found.
             */

            var updateFilter = Builders<Weather>.Update.Set(t => t.elevation, 72);

            // The following FindOneAndUpdateOptions specify that we want the *updated* document
            // to be returned to us. By default, we get the document as it was *before*
            // the update.

            var options = new FindOneAndUpdateOptions<Weather, Weather>()
            {
                ReturnDocument = ReturnDocument.After
            };

            // The updatedDocument object is a Recipe object that reflects the
            // changes we just made.
            //var updatedDocument = collection.FindOneAndUpdate(findFilter,
            //    updateFilter, options);

            Console.WriteLine("Here's the updated document:");
            //Console.WriteLine(updatedDocument.ToString());
            Console.WriteLine();
            /*      *** DELETE DOCUMENTS ***
             *      
             *      As with other CRUD methods, you can delete a single document 
             *      or all documents that match a specified filter. To delete all 
             *      of the documents in a collection, pass an empty filter to 
             *      the DeleteMany() method. In this example, we'll delete 2 of 
             *      the recipes.
             */

            var deleteResult = collection
                .DeleteMany(Builders<Weather>.Filter.In(r => r.st, new string[] { "elotes", "fried rice" }));

            Console.WriteLine($"I deleted {deleteResult.DeletedCount} records.");

            Console.Read();
        }

        public HttpStatusCode Authenticate(UserLogin userDetails)
        {
            throw new NotImplementedException();
        }

        public HttpStatusCode Register(UserLogin userDetails)
        {
            throw new NotImplementedException();
        }

        public HttpStatusCode Unregister(UserLogin userDetails)
        {
            throw new NotImplementedException();
        }

        public HttpStatusCode Withdraw(TransactionsModel transactions)
        {
            throw new NotImplementedException();
        }

        public HttpStatusCode Deposit(TransactionsModel transactions)
        {
            throw new NotImplementedException();
        }

        public HttpStatusCode GetBalance(TransactionsModel transactions)
        {
            throw new NotImplementedException();
        }

        public HttpStatusCode CreditTransfer(TransactionsModel transactions)
        {
            throw new NotImplementedException();
        }
    }
    public class Weather
    {
        public string st { get; set; }
        public int elevation { get; set; }

        public Weather(string st, int elevation)
        {
            this.st = st;
            this.elevation = elevation;
        }

        /// <summary>
        /// This static method is just here so we have a convenient way
        /// to generate sample recipe data.
        /// </summary>
        /// <returns>A list of Recipes</returns>       
        public static List<Weather> GetRecipes()
        {
            return new List<Weather>()
            {
                //new Weather("elotes", new List<string>(){"corn", "mayonnaise", "cotija cheese", "sour cream", "lime" }, 35),
                //new Weather("loco moco", new List<string>(){"ground beef", "butter", "onion", "egg", "bread bun", "mushrooms" }, 54),
                //new Weather("patatas bravas", new List<string>(){"potato", "tomato", "olive oil", "onion", "garlic", "paprika" }, 80),
                //new Weather("fried rice", new List<string>(){"rice", "soy sauce", "egg", "onion", "pea", "carrot", "sesame oil" }, 40),
            };
        }
    }
}
