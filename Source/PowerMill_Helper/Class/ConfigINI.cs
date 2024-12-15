using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PowerMill_Helper
{
    internal class ConfigINI
    {
        private const int VALUE_MAX_LEN = 1024; //值最大长度

        /// <summary>
        /// 写配置到ini文件
        /// </summary>
        /// <param name="appName">节点名</param>
        /// <param name="keyName">要写入的key</param>
        /// <param name="value">要写入的值</param>
        /// <param name="fileName">要写入ini配置文件路径</param>
        /// <returns>成功返回非0，失败返回0</returns>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string appName, string keyName,
            string value, string fileName);

        /// <summary>
        /// 从ini配置文件读配置
        /// </summary>
        /// <param name="appName">节点名</param>
        /// <param name="keyName">目标key</param>
        /// <param name="defaultValue">如果找不到目标key就返回这个默认值</param>
        /// <param name="returnedString">返回目标key对应的值</param>
        /// <param name="size">returnedString的大小</param>
        /// <param name="fileName">要读取的ini配置文件路径</param>
        /// <returns>返回返回字符串的长度</returns>

        [DllImport("kernel32")]
        private static extern long GetPrivateProfileString(string appName, string keyName,
            string defaultValue, StringBuilder returnedString, int size, string fileName);

        /// <summary>
        /// 外部接口，写配置
        /// </summary>
        /// <param name="iniFilePath">ini配置文件路径</param>
        /// <param name="appName">节点名</param>
        /// <param name="keyName">key</param>
        /// <param name="value">值</param>
        /// <returns>成功返回true，失败返回false</returns>
        public static bool WriteSetting(string iniFilePath, string appName, string keyName, string value)
        {
            return (0 == WritePrivateProfileString(appName, keyName, value, iniFilePath) ? false : true);
        }

        /// <summary>
        /// 读配置
        /// </summary>
        /// <param name="iniFilePath">ini配置文件路径</param>
        /// <param name="appName">节点名</param>
        /// <param name="keyName">目标key</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>返回key对应的value</returns>
        public static string ReadSetting(string iniFilePath, string appName, string keyName, string defaultValue)
        {
            StringBuilder result = new StringBuilder(VALUE_MAX_LEN + 1);
            GetPrivateProfileString(appName, keyName, defaultValue, result, VALUE_MAX_LEN, iniFilePath);
            return result.ToString();
        }

    }
}
