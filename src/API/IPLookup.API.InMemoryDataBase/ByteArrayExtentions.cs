using System;

namespace IPLookup.API.InMemoryDataBase
{
    internal static class ByteArrayExtentions
    {
        /// <summary>
        /// Binary search row in array by specific row value.
        /// </summary>
        /// <param name="dataBase">where to search</param>
        /// <param name="startIndex">staart index of rows list</param>
        /// <param name="rowSize">size of row</param>
        /// <param name="rowCount">overall row count</param>
        /// <param name="rowOffset">where to find value in a row</param>
        /// /// <param name="value">what to find</param>
        /// <returns></returns>
        internal static T ValueBinarySearch<T>(this byte[] dataBase, uint rowCount, byte[] value, Func<byte[], uint, T> objectFactory)
            where T : class, IByValueBinarySearch
        {
            if (rowCount == 0)
            {
                return null;
            }
            return BinarySearchRange<T>(dataBase, rowCount, value, objectFactory);
        }

        private static T BinarySearchRange<T>(byte[] dataBase, uint rowCount, byte[] value, Func<byte[], uint, T> objectFactory)
            where T : class, IByValueBinarySearch
        {
            var minRow = 0u;
            var maxRow = rowCount - 1;
            while (minRow <= maxRow)
            {
                var midRow = (minRow + maxRow) / 2;
                var ipRange = objectFactory.Invoke(dataBase, midRow);
                if (ipRange.ContainsValue(value))
                {
                    return ipRange;
                }
                else if (ipRange.Less(value))
                {
                    maxRow = midRow - 1;
                }
                else
                {
                    minRow = midRow + 1;
                }
            }
            return default(T);
        }

    }
}
