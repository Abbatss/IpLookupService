using System.Collections.Generic;
using System.Threading.Tasks;

namespace IPLookup.API.InMemoryDataBase
{
    public interface IInMemoryGeoDataBase
    {
        Task<T> SearchFirstItemByValue<T>(string value) where T : class, IByValueBinarySearchObject;
        Task<List<T>> GetItems<T>(int start, int count) where T : class, IByValueBinarySearchObject;
        Task<T> Get<T>(int index) where T : class;
        Task<T> Scan<T>(string value) where T : class, IByValueBinarySearchObject;
    }
}