namespace IPLookup.API.InMemoryDataBase
{
    public interface IByValueBinarySearchObject
    {
        bool ContainsValue(string value);
        bool Less(string value);
    }
}