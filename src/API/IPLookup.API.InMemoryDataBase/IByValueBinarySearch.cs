namespace IPLookup.API.InMemoryDataBase
{
    public interface IByValueBinarySearchObject
    {
        bool ContainsValue(string value);
        bool LessThan(string value);
    }
}