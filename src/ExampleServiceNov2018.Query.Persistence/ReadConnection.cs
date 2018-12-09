namespace ExampleServiceNov2018.Query.Persistense
{
    public class ReadConnection
    {
        public readonly string SqlConnectionString;

        public ReadConnection(string sqlConnectionString)
        {
            SqlConnectionString = sqlConnectionString;
        }
    }
}