using Microsoft.WindowsAzure.Storage.Table;

namespace AzureTables
{
    public class Movie : TableEntity
    {
        public Movie()
        { }

        public Movie(string releaseDate, string title)
        {
            this.PartitionKey = releaseDate;
            this.RowKey = title;
        }

        public int Rating { get; set; }

        public string Description { get; set; }
    }
}
