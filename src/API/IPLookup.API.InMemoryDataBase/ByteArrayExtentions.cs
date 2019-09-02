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
        internal static T ValueBinarySearch<T>(this byte[] dataBase, uint startIndex, int rowCount, byte[] value)
            where T : new()
        {
            if (rowCount == 0)
            {
                default(T);
            }
            return BinarySearchRange<T>(dataBase, startIndex, rowCount, value);
        }

        private static T BinarySearchRange<T>(byte[] dataBase, uint startIndex, int rowCount, byte[] value)
            where T:new()
        {
            var minRow = 0;
            var maxRow = rowCount - 1;
            while (minRow <= maxRow)
            {
                int midRow = (minRow + maxRow) / 2;
                var ipRange = new T(dataBase, startIndex, midRow);
                if (ipRange.ContainsIp(value))
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
