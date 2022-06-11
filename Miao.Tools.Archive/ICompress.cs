using System;
using System.Collections.Generic;

namespace Miao.Tools.Archive
{
    /// <summary>
    /// 压缩接口
    /// </summary>
    public interface ICompress
    {
        /// <summary>
        /// 文件列表
        /// </summary>
        List<string> Files { get; set; }

        /// <summary>
        /// 目录列表
        /// </summary>
        List<string> Directories { get; set; }

        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="compressFilePath">压缩文件路径</param>
        /// <param name="searchPattern">搜索模式,默认为'*'</param>
        /// <exception cref="Exception">不支持的压缩格式</exception>
        void CompressTo(string compressFilePath, string searchPattern = "*");
    }
}
