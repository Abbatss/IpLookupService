using System;
using System.Runtime.Serialization;

namespace IPLookup.API.InMemoryDataBase
{
    [Serializable]
    internal class GeoDataBaseException : Exception
    {
        public GeoDataBaseException()
        {
        }

        public GeoDataBaseException(string message) : base(message)
        {
        }

        public GeoDataBaseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GeoDataBaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}