namespace IPLookup.API.InMemoryDataBase
{
    public interface IRowObjectFactory
    {
        T CreateInstance<T>(byte[] db, int index) where T : class;
    }
}