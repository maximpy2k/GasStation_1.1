using GasStation.ViewModels.Elements;
using GasStation.xml.Const.Elements;
using GasStation.xml.Constant;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using GasStation.xml.Script.EnumConst;
using GasStation.xml.Script.Security;

namespace GasStation.xml.Script
{
    /// <summary>
    /// Xml узел Flap (Клапан)
    /// </summary>
    public class XmlClassVacuumetr : XmlBaseClassElementScript
    {
        /// <summary>
        /// Константы вакуметра
        /// </summary>
        public XmlClassVacuumetrConst VacuumetrConst { get; set; }
        /// <summary>
        /// Класс для отображения
        /// </summary>
        public ClassVacuumetrView VacuumetrView { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="num">Номер клапана</param>
        /// <param name="flapConst">Константы клапана</param>
        /// <param name="flapView">Класс для отображения</param>
        public XmlClassVacuumetr(int num, XmlClassVacuumetrConst vacuumetrConst, ClassVacuumetrView vacuumetrView)
        {
            TypeElement  = TypeElement.Flap;

            VacuumetrConst = vacuumetrConst;
            VacuumetrView = vacuumetrView;
            var xmlDocument = new XmlDocument();
            _xmlNode = xmlDocument.CreateElement("dev");

            #region Создание атрибутов

            var atrName = xmlDocument.CreateAttribute("name");
            atrName.Value = "vacuumetr";

            var atrNum = xmlDocument.CreateAttribute("num");
            atrNum.Value = num.ToString();            
            #endregion                           

            #region Добавление атрибутов
            _xmlNode.Attributes.Append(atrName);
            _xmlNode.Attributes.Append(atrNum);
            #endregion
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="xmlFlapNode">XML узел с заданием для клапана для шага скрипта</param>
        /// <param name="flapConst">Константы клапана</param>
        /// <param name="flapView">Класс для отображения</param>
        public XmlClassVacuumetr(XmlNode xmlVacumetrNode, XmlClassVacuumetrConst vacuumetrConst, ClassVacuumetrView vacuumetrView) : base(xmlVacumetrNode)
        {
            TypeElement = TypeElement.Flap;

            VacuumetrConst = vacuumetrConst;
            VacuumetrView = vacuumetrView;
        }
    }
}
