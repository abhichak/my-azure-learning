using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace AzureTables
{
    class Program
    {
        static void Main(string[] args)
        {
            var storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            var azureTableClient = storageAccount.CreateCloudTableClient();
            var cloudTable = azureTableClient.GetTableReference("Movies");

            cloudTable.CreateIfNotExists();

            var movie1 = new Movie("1990", "Driving Miss Daisy");
            movie1.Rating = 7;
            movie1.Description = "Nice movie!";

            var movie2 = new Movie("1990", "Dance with wolves");
            movie1.Rating = 7;
            movie1.Description = "Good watch!";

            var movie3 = cloudTable.Execute(TableOperation.Retrieve<Movie>("1990", "Silence of the lambs")).Result as Movie;
            movie3.Rating = 9;
            movie3.Description = "It's spine chilling!";

            var updateOperation = TableOperation.Replace(movie3);

            var tableOperation = new TableBatchOperation();
            tableOperation.Insert(movie1);
            tableOperation.Insert(movie2);
            tableOperation.Add(updateOperation);

            //var tableOperationResult = cloudTable.ExecuteBatch(tableOperation);

            var query = new TableQuery<Movie>().
                Where(TableQuery.GenerateFilterCondition("Rating", QueryComparisons.Equal, "9"));

            foreach(var result in cloudTable.ExecuteQuery(query))
            {
                Console.WriteLine(string.Concat(result.PartitionKey, ":", result.RowKey, ":",
                    result.Rating, ":", result.Description));
            }
        }
    }
}
