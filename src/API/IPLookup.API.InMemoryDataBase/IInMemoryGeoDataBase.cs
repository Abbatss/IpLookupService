using System.Collections.Generic;
using System.Threading.Tasks;

namespace IPLookup.API.InMemoryDataBase
{
    public interface IInMemoryGeoDataBase
    {
        Task<T> Get<T>(byte[] ip) where T : class, IByValueBinarySearchObject;
        Task<List<T>> GetItems<T>(int start, int count) where T : class, IByValueBinarySearchObject;
    }
}