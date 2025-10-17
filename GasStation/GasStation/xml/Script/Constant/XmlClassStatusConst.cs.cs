using GasStation.xml.Script.Constant;
using System.Xml;

namespace GasStation.xml.Constant
{
    public class XmlClassStatusConst:XmlBaseConst
    {
        /// <summary>
        /// Узел Xml
        /// </summary>
        public XmlClassStatusConst(XmlNode xmlNode):base(xmlNode)
        {
        }
        
        /// <summary>
        /// Заслонка открыта
        /// </summary>
        public XmlClassSensorConst DumperOpen
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='dumperOpen']");
                return node == null ? null : new XmlClassSensorConst(node);
            }
        }
        /// <summary>
        /// Заслонка открыта
        /// </summary>
        public XmlClassSensorConst RegWork
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='regWork']");
                return node == null ? null : new XmlClassSensorConst(node);
            }
        }

        /// <summary>
        /// Заслонка закрыта
        /// </summary>
        public XmlClassSensorConst DumperClosed
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='dumperClosed']");
                return node == null ? null : new XmlClassSensorConst(node);
            }
        }
        /// <summary>
        /// Загружен ОК
        /// </summary>
        public XmlClassSensorConst LoadComplete
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='loadComplete']");
                return node == null ? null : new XmlClassSensorConst(node);
            }
        }
        /// <summary>
        /// Выгружен ОК
        /// </summary>
        public XmlClassSensorConst UnLoadComplete
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='unLoadComplete']");
                return node == null ? null : new XmlClassSensorConst(node);
            }
        }
        /// <summary>
        /// Затвор открыт
        /// </summary>
        public XmlClassSensorConst GateOpen
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='dumperOpen']");
                return node == null ? null : new XmlClassSensorConst(node);
            }
        }
        /// <summary>
        /// Затвор закрыт
        /// </summary>
        public XmlClassSensorConst GateClosed
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='dumperClosed']");
                return node == null ? null : new XmlClassSensorConst(node);
            }
        }

        /// <summary>
        /// Авария загрузки
        /// </summary>
        public XmlClassSensorConst ErrorLoad
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='errorLoad']");
                return node == null ? null : new XmlClassSensorConst(node);
            }
        }
        /// <summary>
        /// Авария выгрузки
        /// </summary>
        public XmlClassSensorConst ErrorUnLoad
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='errorUnLoad']");
                return node == null ? null : new XmlClassSensorConst(node);
            }
        }

        /// <summary>
        /// Работа вакуумного насоса
        /// </summary>
        public XmlClassSensorConst VacuumPumpLamp
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='VacuumPumpLamp']");
                return node == null ? null : new XmlClassSensorConst(node);
            }
        }
        /// <summary>
        /// Работа насоста Рудса
        /// </summary>
        public XmlClassSensorConst RoughingPumpLamp
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='RoughingPumpLamp']");
                return node == null ? null : new XmlClassSensorConst(node);
            }
        }

        ///// <summary>
        ///// Загрузка платформы
        ///// </summary>
        //public XmlClassDioPortConst LoadingPlatform
        //{
        //    get
        //    {
        //        var node = XmlNode.SelectSingleNode("dev[@name='loadingPlatform']");
        //        return node == null ? null : new XmlClassDioPortConst(node);
        //    }
        //}
        ///// <summary>
        ///// Выгрузка платформы
        ///// </summary>
        //public XmlClassDioPortConst UnLoadingPlatform
        //{
        //    get
        //    {
        //        var node = XmlNode.SelectSingleNode("dev[@name='unloadingPlatform']");
        //        return node == null ? null : new XmlClassDioPortConst(node);
        //    }
        //}


        //public XmlClassDioPortConst[] MasDioConst
        //{
        //    get
        //    {
        //        var nodes = XmlNode.SelectNodes("dev[@name='dio']");
        //        if (nodes.Count == 0)
        //            return null;

        //        var masDioConst = new XmlClassDioPortConst[nodes.Count];

        //        for (var idx = 0; idx < nodes.Count; idx++)
        //        {
        //            masDioConst[idx] = new XmlClassDioPortConst(nodes[idx]);
        //        }

        //        return masDioConst;
        //    }
        //}
    }
}
