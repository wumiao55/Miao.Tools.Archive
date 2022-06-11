using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Readers;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Miao.Tools.Archive.Decompress
{
    /// <summary>
    /// zip解压缩
    /// </summary>
    public class ZipDecompress : IDecompress
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="zipFile"></param>
        public ZipDecompress(string zipFile)
        {
            CompressFile = zipFile;
            IsSupport = string.Equals(SupportExtension, Path.GetExtension(zipFile), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        public string CompressFile { get; set; }

        /// <summary>
        /// 支持的压缩文件扩展名
        /// </summary>
        public string SupportExtension { get; } = ".zip";

        /// <summary>
        /// 是否支持解压缩
        /// </summary>
        public bool IsSupport { get; }

        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="decompressDir">指定解压缩的目录,传空为解压到当前压缩文件所在目录</param>
        /// <param name="searchPattern">搜索模式,默认为'*'</param>
        public void Decompress(string decompressDir = null, string searchPattern = "*")
        {
            if (!IsSupport)
            {
                throw new Exception("不支持该文件的解压缩,文件:" + Path.GetFileName(CompressFile));
            }

            if (string.IsNullOrEmpty(decompressDir))
            {
                decompressDir = Path.GetDirectoryName(CompressFile);
            }

            var readerOptions = new ReaderOptions
            {
                ArchiveEncoding = new ArchiveEncoding() { Default = Encoding.GetEncoding("GB2312") }
            };
            using var archive = ArchiveFactory.Open(CompressFile, readerOptions);
            searchPattern = searchPattern == "*" ? ".*" : searchPattern;
            var regex = new Regex(searchPattern);
            foreach (var entry in archive.Entries)
            {
                if (!entry.IsDirectory && regex.IsMatch(entry.Key))
                {
                    entry.WriteToDirectory(decompressDir, new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
                }
            }
        }
    }
}
