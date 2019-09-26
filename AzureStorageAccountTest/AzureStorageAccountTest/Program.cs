using System;
using System.IO;
using System.Threading.Tasks;
using AzureStorageAccountTest.Model;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureStorageAccountTest
{
    class Program
    {
        public static void Main()
        {
            Console.WriteLine("Azure Table Storage - Demo\n");

            // Run the examples asynchronously, wait for the results before proceeding
            ProcessAsync().GetAwaiter().GetResult();

            Console.WriteLine("Press any key to exit the application.");
            Console.ReadLine();
        }

        private static async Task ProcessAsync()
        {
            var storageAccount = new CloudStorageAccount(new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials("mattsteststorageaccount", Passwords.StorageAccountKey), true);
            var tableClient = storageAccount.CreateCloudTableClient();

            // Get a reference to a table named "peopleTable" - you need to do this even if table does not exist
            var peopleTable = tableClient.GetTableReference("peopleTable");

            await CreatePeopleTableAsync(peopleTable);
            await AddSingleEntity(peopleTable);
            await AddEntityBatch(peopleTable);
            await GetEntitiesInPartition(peopleTable);
            await GetSingleEntity(peopleTable);
            await DeleteAnEntity(peopleTable);
        }

        private static async Task DeleteAnEntity(CloudTable peopleTable)
        {
            // Create a retrieve operation that expects a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<CustomerEntity>("Smith", "Ben");

            // Execute the operation.
            TableResult retrievedResult = await peopleTable.ExecuteAsync(retrieveOperation);

            // Assign the result to a CustomerEntity object.
            CustomerEntity deleteEntity = (CustomerEntity)retrievedResult.Result;

            // Create the Delete TableOperation and then execute it.
            if (deleteEntity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);

                // Execute the operation.
                await peopleTable.ExecuteAsync(deleteOperation);

                Console.WriteLine("Entity deleted.");
            }

            else
                Console.WriteLine("Couldn't delete the entity.");
        }

        private static async Task GetSingleEntity(CloudTable peopleTable)
        {
            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<CustomerEntity>("Smith", "Ben");

            // Execute the retrieve operation.
            TableResult retrievedResult = await peopleTable.ExecuteAsync(retrieveOperation);

            // Print the phone number of the result.
            if (retrievedResult.Result != null)
                Console.WriteLine(((CustomerEntity)retrievedResult.Result).PhoneNumber);
            else
                Console.WriteLine("The phone number could not be retrieved.");
        }

        private static async Task GetEntitiesInPartition(CloudTable peopleTable)
        {
            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<CustomerEntity> query = new TableQuery<CustomerEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Smith"));

            // Print the fields for each customer.
            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<CustomerEntity> resultSegment = await peopleTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                foreach (CustomerEntity entity in resultSegment.Results)
                {
                    Console.WriteLine("{0}, {1}\t{2}\t{3}", entity.PartitionKey, entity.RowKey,
                    entity.Email, entity.PhoneNumber);
                }
            } while (token != null);
        }

        private static async Task AddEntityBatch(CloudTable peopleTable)
        {
            // Create the batch operation.
            TableBatchOperation batchOperation = new TableBatchOperation();

            // Create a customer entity and add it to the table.
            CustomerEntity customer1 = new CustomerEntity("Smith", "Jeff");
            customer1.Email = "Jeff@contoso.com";
            customer1.PhoneNumber = "425-555-0104";

            // Create another customer entity and add it to the table.
            CustomerEntity customer2 = new CustomerEntity("Smith", "Ben");
            customer2.Email = "Ben@contoso.com";
            customer2.PhoneNumber = "425-555-0102";

            // Add both customer entities to the batch insert operation.
            batchOperation.InsertOrMerge(customer1);
            batchOperation.InsertOrMerge(customer2);

            // Execute the batch operation.
            await peopleTable.ExecuteBatchAsync(batchOperation);
        }

        private static async Task AddSingleEntity(CloudTable peopleTable)
        {
            // Create a new customer entity.
            CustomerEntity customer1 = new CustomerEntity("Harp", "Walter");
            customer1.Email = "Walter@contoso.com";
            customer1.PhoneNumber = "425-555-0101";

            // Create the TableOperation that inserts the customer entity.
            TableOperation insertOperation = TableOperation.InsertOrMerge(customer1);

            // Execute the insert operation.
            await peopleTable.ExecuteAsync(insertOperation);
        }

        async static Task CreatePeopleTableAsync(CloudTable peopleTable)
        {
            // Create the CloudTable if it does not exist
            try
            {
                await peopleTable.CreateIfNotExistsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("There was a problem creating the table: " + ex.Message);
            }
        }
    }
}
