using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using SharpCompress.Common;
using SharpCompress.Writers;

namespace Miao.Tools.Archive.Compress
{
    /// <summary>
    /// zip压缩
    /// </summary>
    public class ZipCompress : ICompress
    {
        /// <summary>
        /// 支持的压缩格式对应字典
        /// </summary>
        private static readonly Dictionary<string, ArchiveType> SupportCompressTypeDics = new Dictionary<string, ArchiveType>
        {
            { ".zip", ArchiveType.Zip  },
        };

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="filesOrDirectories"></param>
        public ZipCompress(params string[] filesOrDirectories)
        {
            foreach (var item in filesOrDirectories)
            {
                if (File.Exists(item))
                {
                    Files.Add(item);
                }
                else if (Directory.Exists(item))
                {
                    Directories.Add(item);
                }
            }
        }

        /// <summary>
        /// 文件集合
        /// </summary>
        public List<string> Files { get; set; } = new List<string>();

        /// <summary>
        /// 目录集合
        /// </summary>
        public List<string> Directories { get; set; } = new List<string>();

        /// <summary>
        /// 压缩 - 支持zip格式
        /// </summary>
        /// <param name="compressFilePath">压缩文件路径</param>
        /// <param name="searchPattern">搜索模式,默认为'*'</param>
        /// <exception cref="Exception">不支持的压缩格式</exception>
        public void CompressTo(string compressFilePath, string searchPattern = "*")
        {
            string compressFileDir = Path.GetDirectoryName(compressFilePath);
            if (string.IsNullOrEmpty(compressFilePath))
            {
                compressFileDir = Environment.CurrentDirectory;
            }
            if (!Directory.Exists(compressFileDir))
            {
                Directory.CreateDirectory(compressFileDir);
            }
            using var compressFileStream = File.Create(compressFilePath);
            //设置编码格式防止乱码
            var option = new WriterOptions(CompressionType.Deflate)
            {
                ArchiveEncoding = new ArchiveEncoding()
                {
                    Default = Encoding.UTF8
                }
            };
            string fileExtension = Path.GetExtension(compressFilePath).ToLower();
            if (!SupportCompressTypeDics.TryGetValue(fileExtension, out ArchiveType archiveType))
            {
                throw new Exception("不支持的压缩格式:" + fileExtension);
            }
            using var writer = WriterFactory.Open(compressFileStream, archiveType, option);

            //压缩文件
            if (Files != null && Files.Count > 0)
            {
                searchPattern = searchPattern == "*" ? ".*" : searchPattern;
                var regex = new Regex(searchPattern);
                foreach (var file in Files)
                {
                    if (File.Exists(file) && regex.IsMatch(Path.GetFileName(file)))
                    {
                        writer.Write(Path.GetFileName(file), file);
                    }
                }
            }

            //压缩文件夹
            if (Directories != null && Directories.Count > 0)
            {
                foreach (var directory in Directories)
                {
                    writer.WriteAll(directory, searchPattern, SearchOption.AllDirectories);
                }
            }
        }
    }
}
