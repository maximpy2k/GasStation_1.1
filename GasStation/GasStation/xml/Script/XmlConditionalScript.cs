using System.Collections.Generic;
using System.Linq;
using System.Xml;
using GasStation.xml.Constant;
using GasStation.xml.Script.Security;

namespace GasStation.xml.Script
{
    public class XmlConditionalScript
    {
        
        public XmlConditionalScript(XmlClassConst consts)
        {
            _consts = consts;
            xmlDocument = new XmlDocument();
            xmlNode = xmlDocument.CreateElement("conditional");

            //_stateScripts = new List<XmlStateConditionScript>();

            //var nodes = XmlNode.SelectNodes("state");

            //for (var idx = 0; idx < nodes.Count; idx++)
            //    _stateScripts.Add(new XmlStateConditionScript(nodes[idx]));

            //var stateConditionScript = new XmlStateConditionScript();
            //var importNode = xmlDocument.ImportNode(stateConditionScript.XmlNode, true);
            //xmlNode.AppendChild(importNode);

            //StateScripts.Add(stateConditionScript);

        }



        public XmlConditionalScript(XmlNode xmlNode, XmlClassConst consts)
        {
            this.xmlNode = xmlNode;

            _consts = consts;

            //var nodes = XmlNode.SelectNodes("state");

            //_stateScripts = new List<XmlStateConditionScript>();

            //for (var idx = 0; idx < nodes.Count; idx++)
            //    _stateScripts.Add(new XmlStateConditionScript(nodes[idx]));
        }

        private readonly XmlNode xmlNode;
        /// <summary>
        /// Узел Xml
        /// </summary>
        public XmlNode XmlNode => xmlNode;

        //public XmlNode XmlNode
        //{
        //    get
        //    {
        //        xmlNode = new 

        //        foreach (var xmlStateConditionScript in StateScripts)
        //        {
        //            var importNode = xmlNode.OwnerDocument.ImportNode(xmlStateConditionScript.XmlNode, true);
        //            xmlNode.AppendChild(importNode);
        //        }

        //        return xmlNode;
        //    }
        //}

        List<XmlStateConditionScript> _stateScripts;
        public List<XmlStateConditionScript> StateScripts
        {
            get
            {
                //if ((_stateScripts == null) || (_stateScripts.Count == 0))
                {
                    _stateScripts = new List<XmlStateConditionScript>();

                    //var importNode = xmlNode.OwnerDocument.ImportNode(currState.XmlNode, true);
                    //xmlNode.AppendChild(importNode);

                    var nodes = XmlNode.SelectNodes("state");

                    //_stateScripts = new List<XmlStateConditionScript>();

                    for (var idx = 0; idx < nodes.Count; idx++)
                        _stateScripts.Add(new XmlStateConditionScript(nodes[idx],_consts));
                }




                return _stateScripts;
            }
            set { _stateScripts = value; }
        }

        private XmlDocument xmlDocument;
        private XmlClassConst _consts;

        /// <summary>
        /// Узел команды скрипта
        /// </summary>

        //public void AddNode()
        //{
        //    var stateConditionScript = new XmlStateConditionScript();

        //    //if (_stateConditionScripts==null)
        //    //{
        //    //    _stateConditionScripts = new List<XmlStateConditionScript>();
        //    //}

        //    //_stateConditionScripts.Add(stateConditionScript);



        //    var importNode = xmlNode.OwnerDocument.ImportNode(stateConditionScript.XmlNode, true);
        //    xmlNode.AppendChild(importNode);
        //}

        public void AddState(XmlStateConditionScript currState)
        {
            if (currState == null)
            {
                currState = new XmlStateConditionScript(_consts);
            }
            currState.NumState = StateScripts.Count;



            StateScripts.Add(currState);
        }

        public void RemoveState(XmlStateConditionScript currState)
        {
            StateScripts.Remove(currState);
        }

        internal void RemoveNode(XmlStateConditionScript currState)
        {
            var node = xmlNode.SelectNodes("state");
            xmlNode.RemoveChild(node[currState.NumState]);
            var steps = StateScripts;
            for (int i = 0; i < steps.Count; i++)
            {
                steps[i].NumState = i ;
            }
        }

        internal void RemoveNodeIdx(int currIdx)
        {
            var node = xmlNode.SelectNodes("state");
            xmlNode.RemoveChild(node[currIdx]);
            var steps = StateScripts;
            for (int i = 0; i < steps.Count; i++)
            {
                steps[i].NumState = i;
            }
        }

        internal void AddNode(XmlStateConditionScript currState)
        {
            if (currState==null)
            {
                currState = new XmlStateConditionScript(_consts);
            }
            currState.NumState = StateScripts.Count;

            var importNode = xmlNode.OwnerDocument.ImportNode(currState.XmlNode, true);
            xmlNode.AppendChild(importNode);
        }


        /// <summary>
        /// Удаление узла из скрипта
        /// </summary>
        public void RemoveNode(int idx)
        {
            //_stateConditionScripts.RemoveAt(idx);

            var node = xmlNode.SelectNodes("state");
            xmlNode.RemoveChild(node[idx]);
            var steps = StateScripts;
            for (int i = 0; i < steps.Count; i++)
            {
                steps[i].NumState = i + 1;
            }
        }

        /// <summary>
        /// Возможность изменнеия скрипта
        /// </summary>
        public bool EnabledChangedScript
        {
            get { return UsePriv || _consts.SecuretyConst.CurrUser.Privs.Contains(EnumPriv.ChangeConditionalScript); }
        }

        public virtual bool UsePriv
        {
            get
            {
                return false;
            }
            set
            {

            }
        }
    }
}
