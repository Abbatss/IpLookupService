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
        /// /// <param name="value">what to find. First equal item in collection</param>
        /// <returns></returns>
        internal static T ValueBinarySearch<T>(this byte[] dataBase, int rowCount, string value, IRowObjectFactory objectFactory)
            where T : class, IByValueBinarySearchObject
        {
            if (rowCount == 0)
            {
                return null;
            }
            return BinarySearchRange<T>(dataBase, rowCount, value, objectFactory);
        }

        private static T BinarySearchRange<T>(byte[] dataBase, int rowCount, string value, IRowObjectFactory objectFactory)
            where T : class, IByValueBinarySearchObject
        {
            var minRow = 0;
            var maxRow = rowCount - 1;
            while (minRow <= maxRow)
            {
                var midRow = minRow + (maxRow - minRow) / 2;
                var ipRange = objectFactory.CreateInstance<T>(dataBase, midRow);
                bool isItemFound = ipRange.ContainsValue(value);

                //this if handler situation when we have duplicates in collections. Algorithm will find first item in collection equals to value
                if (isItemFound && (midRow == 0 || objectFactory.CreateInstance<T>(dataBase, midRow - 1).LessThan(value)))
                {
                    return ipRange;
                }

                else if (isItemFound || !ipRange.LessThan(value))
                {
                    maxRow = midRow - 1;
                }

                else if (ipRange.LessThan(value))
                {
                    minRow = midRow + 1;
                }
            }
            return null;
        }

    }
}
