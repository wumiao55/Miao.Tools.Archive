using System;
using System.Collections.Generic;

namespace Miao.Tools.Archive
{
    /// <summary>
    /// 解压缩接口
    /// </summary>
    public interface IDecompress
    {
        /// <summary>
        /// 压缩文件
        /// </summary>
        string CompressFile { get; set; }

        /// <summary>
        /// 支持的压缩文件扩展名
        /// </summary>
        string SupportExtension { get; }

        /// <summary>
        /// 是否支持解压缩
        /// </summary>
        bool IsSupport { get; }

        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="decompressDir">指定解压缩的目录,传空为解压到当前压缩文件所在目录</param>
        /// <param name="searchPattern">搜索模式,默认为'.*'</param>
        void Decompress(string decompressDir = "", string searchPattern = ".*");
    }
}
