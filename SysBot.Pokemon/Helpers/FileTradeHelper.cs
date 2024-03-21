using PKHeX.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SysBot.Pokemon.Helpers
{
    /// <summary>
    /// 宝可梦文件交易帮助类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FileTradeHelper<T> where T : PKM, new()
    {
        /// <summary>
        /// 将二进制文件(箱子bin文件以及不同版本的PKM文件)转换成对应版本的PKM list
        /// 读取的是二进制件转化而成的byte数组
        /// </summary>
        /// <param name="bb"></param>
        /// <returns></returns>
        /// <summary>
        /// Converts a byte array from a bin file to a list of PKM objects.
        /// </summary>
        /// <param name="bb">The byte array to convert.</param>
        /// <returns>A list of PKM objects.</returns>
        public static List<T> Bin2List(byte[] bb)
        {
            // If the byte array length matches the size of the PKM type, attempt to convert the byte array to a PKM object
            if (pkmSize[typeof(T)] == bb.Length)
            {
                var tp = GetPKM(bb);
                // If the conversion is successful and the PKM object is valid, return a list containing the single PKM object
                if (tp != null && tp.Species > 0 && tp.Valid && tp is T pkm) return new List<T>() { pkm };
            }

            // Get the size of the PKM type in the .bin file
            int size = pkmSizeInBin[typeof(T)];
            // Calculate the number of PKM objects in the byte array
            int times = bb.Length % size == 0 ? (bb.Length / size) : (bb.Length / size + 1);
            // Initialize a list to store the PKM objects
            List<T> pkmBytes = new();
            // Loop through the byte array and convert each segment to a PKM object
            for (var i = 0; i < times; i++)
            {
                // Calculate the start and end indices for the current segment
                int start = i * size;
                int end = (start + size) > bb.Length ? bb.Length : (start + size);
                // Convert the current segment to a PKM object
                var tp = GetPKM(bb[start..end]);
                // If the conversion is successful and the PKM object is valid, add it to the list
                if (tp != null && tp.Species > 0 && tp.Valid && tp is T pkm) pkmBytes.Add(pkm);
            }

            // Return the list of PKM objects
            return pkmBytes;
        }

        /// <summary>
        /// 文件名称是否有效
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool ValidFileName(string fileName)
        {
            string ext = fileName?.Split('.').Last().ToLower() ?? "";
            return (ext == typeof(T).Name.ToLower()) || (ext == "bin");
        }

        /// <summary>
        /// 文件大小是否有效
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static bool ValidFileSize(long size) => ValidPKMFileSize(size) || ValidBinFileSize(size);

        public static bool ValidPKMFileSize(long size) => size == pkmSize[typeof(T)];

        public static bool ValidBinFileSize(long size) => (size > 0) &&
                                                          (size <= MaxCountInBin * pkmSizeInBin[typeof(T)]) &&
                                                          (size % pkmSizeInBin[typeof(T)] == 0);

        public static int MaxCountInBin => maxCountInBin[typeof(T)];

        static PKM? GetPKM(byte[] ba) => typeof(T) switch
        {
            Type t when t == typeof(PK8) => new PK8(ba),
            Type t when t == typeof(PB8) => new PB8(ba),
            Type t when t == typeof(PA8) => new PA8(ba),
            Type t when t == typeof(PK9) => new PK9(ba),
            _ => null
        };

        static readonly Dictionary<Type, int> pkmSize = new()
        {
            { typeof(PK8), 344 },
            { typeof(PB8), 344 },
            { typeof(PA8), 376 },
            { typeof(PK9), 344 }
        };

        static readonly Dictionary<Type, int> pkmSizeInBin = new()
        {
            { typeof(PK8), 344 },
            { typeof(PB8), 344 },
            { typeof(PA8), 360 },
            { typeof(PK9), 344 }
        };

        static readonly Dictionary<Type, int> maxCountInBin = new()
        {
            { typeof(PK8), 960 },
            { typeof(PB8), 1200 },
            { typeof(PA8), 960 },
            { typeof(PK9), 960 }
        };
    }
}
