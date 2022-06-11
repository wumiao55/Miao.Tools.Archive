using System;
using System.IO;
using Miao.Tools.Archive.Compress;
using Miao.Tools.Archive.Decompress;

namespace Miao.Tools.Archive
{
    /// <summary>
    /// 文档压缩/解压缩工具
    /// </summary>
    public static class ArchiveUtil
    {
        /// <summary>
        /// 文件解压缩 - 支持zip,rar,7z格式
        /// </summary>
        /// <param name="compressFile">压缩文件</param>
        /// <param name="outputDir">解压输出目录,传空为解压到当前压缩文件所在目录</param>
        /// <param name="searchPattern">搜索模式,默认为'*'</param>
        public static void Decompress(string compressFile, string outputDir = null, string searchPattern = "*")
        {
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
            string fileExtension = Path.GetExtension(compressFile);
            IDecompress decompress;
            if (string.Equals(fileExtension, Path.GetExtension(".zip"), StringComparison.OrdinalIgnoreCase))
            {
                decompress = new ZipDecompress(compressFile);
            }
            else if (string.Equals(fileExtension, Path.GetExtension(".rar"), StringComparison.OrdinalIgnoreCase))
            {
                decompress = new RarDecompress(compressFile);
            }
            else if (string.Equals(fileExtension, Path.GetExtension(".7z"), StringComparison.OrdinalIgnoreCase))
            {
                decompress = new SevenZipDecompress(compressFile);
            }
            else
            {
                throw new Exception("不支持该文件格式的解压缩, Extension:" + fileExtension);
            }
            decompress.Decompress(outputDir, searchPattern);
        }

        /// <summary>
        /// 文件压缩 - 支持zip格式
        /// </summary>
        /// <param name="compressFile">压缩文件</param>
        /// <param name="toCompressFiles">待压缩文件列表</param>
        public static void CompressTo(string compressFile, params string[] toCompressFiles)
        {
            new ZipCompress(toCompressFiles).CompressTo(compressFile);
        }

        /// <summary>
        /// 文件夹压缩 - 支持zip格式
        /// </summary>
        /// <param name="compressFile">压缩文件</param>
        /// <param name="toCompressDirectory">待压缩文件夹</param>
        public static void CompressTo(string compressFile, string toCompressDirectory)
        {
            new ZipCompress(toCompressDirectory).CompressTo(compressFile);
        }

    }
}
