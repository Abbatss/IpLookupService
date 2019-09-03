using System;

namespace IPLookup.API.InMemoryDataBase
{
    internal class GeoDataBaseRowObjectFactory : IRowObjectFactory
    {
        //We can use Activator.CreateInstance<T>(byte[] db, uint index) instead but it is much slower.
        //or we can have base class with a constructor. 
        //But the adia is that we can have one interface for DB client but different 
        // implementations. For example we can store data in AWS DynamoDB.
        T IRowObjectFactory.CreateInstance<T>(byte[] db, uint index)
        {
            if (typeof(T) == typeof(IPRange))
            {
               return new IPRange(db, index) as T;
            }
            throw new InvalidOperationException($"type {typeof(T)} is not supported by row factory");
        }
    }
}