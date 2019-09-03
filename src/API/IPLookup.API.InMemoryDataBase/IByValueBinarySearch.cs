namespace IPLookup.API.InMemoryDataBase
{
    public interface IByValueBinarySearchObject
    {
        bool ContainsValue(byte[] value);
        bool Less(byte[] value);
    }
}