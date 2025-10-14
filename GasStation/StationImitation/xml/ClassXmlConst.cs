using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using GasStation.xml.Constant.XmlConst.Elements;


namespace StationImitation.xml
{
    public class ClassXmlConst
    {

        private int idx=0;
        public int Idx;
        public String Path;

        public XmlClassControllerConst[] ConstControllers
        {
            get
            {
                var xmlConst = XmlConst.SelectNodes("settings/dev[@name='Controllers']/dev[@name='Controller']");
                var masConst = new XmlClassControllerConst[xmlConst.Count];
                for (var i = 0; i < xmlConst.Count; i++)
                {
                    masConst[i] = new XmlClassControllerConst(xmlConst[i]);
                }
                return masConst;

            }
        }

        public XmlClassProgrammConst[] ConstProgramm
        {
            get
            {
                var xmlConst = XmlConst.SelectNodes("settings/dev[@name='ComPort']");
                var masConst = new XmlClassProgrammConst[xmlConst.Count];
                for (var i = 0; i < xmlConst.Count; i++)
                {
                    masConst[i] = new XmlClassProgrammConst(xmlConst[i]);
                }
                return masConst;
            }
        }

        /// <summary>
        /// Узел команды скрипта
        /// </summary>
        public XmlDocument XmlConst { get; private set; }

        public string _path = "";
        public ClassXmlConst(string pathConst)
        {
            _path = pathConst;
            if (pathConst == null)
                return;
            XmlConst = new XmlDocument();
            XmlConst.Load(pathConst);
        }

    }

}
