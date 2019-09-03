namespace IPLookup.API.InMemoryDataBase
{
    public interface IByValueBinarySearch
    {
        bool ContainsValue(byte[] value);
        bool Less(byte[] value);
    }
}