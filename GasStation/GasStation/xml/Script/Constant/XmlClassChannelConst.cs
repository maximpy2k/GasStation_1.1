using System;
using System.IO;
using System.Xml;

namespace GasStation.xml.Constant
{
    public class XmlClassChannelConst
    {
        public XmlNode _xmlNode;
        public XmlNode XmlNode => _xmlNode;

        public XmlClassChannelConst(XmlNode xmlNode)
        {
            _xmlNode = xmlNode;
        }

        public string NameChannel
        {
            get
            {
                return XmlNode.Attributes["nameChannel"].Value;
            }
        }

        private string LogPath
        {
            get { return XmlNode.Attributes["logPath"].Value; }
        }
        /// <summary>
        /// Путь к папке телеметрии для текущего запуска
        /// </summary>
        public string LogPathFull=> $"{LogPath}\\{StartupLogDir}";

        
        /// <summary>
        /// Название папки с логом на текущий запуск
        /// </summary>
        public static string StartupLogDir = "None";
        /// <summary>
        /// Определение имени папки лога
        /// </summary>
        /// <returns></returns>
        public void FindStartupLogDir()
        {
            var num = 1;
            StartupLogDir = $"{DateTime.Now.ToString("yyyy_MM_dd")}_{NameChannel}_{num:00}";
            var path = $"{LogPath}\\{StartupLogDir}";

            while (Directory.Exists(path))
            {
                StartupLogDir = $"{DateTime.Now.ToString("yyyy_MM_dd")}_{NameChannel}_{num++:00}";
                path = $"{LogPath}\\{StartupLogDir}";
            }
            Directory.CreateDirectory(path);
        }
    }
}