namespace IPLookup.API.InMemoryDataBase
{
    public interface IRowObjectFactory
    {
        T CreateInstance<T>(byte[] db, uint index) where T : class, IByValueBinarySearchObject;
    }
}