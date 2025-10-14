using System.Xml;
using GasStation.ViewModels.Elements;
using GasStation.xml.Constant;
using GasStation.xml.Script.XmlScript;
using GasStation.xml.Script.XmlScript.Elements;

namespace GasStation.xml.Script
{
    public class XmlClassStep
    {
        /// <summary>
        /// Константы
        /// </summary>
        private XmlClassConst _consts;
        /// <summary>
        /// Класс для отображения
        /// </summary>
        private ClassViewDataClass _viewData { get; set; }

        /// <summary>
        /// Узел Xml step
        /// </summary>
        /// <param name="numStep">номер шага</param>
        /// <param name="chamSecCnt">колличество камер</param>
        /// <param name="flapsCnt">колличество клапанов</param>
        /// <param name="rrgCnt">колличество РРГ</param>
        public XmlClassStep(XmlNode xmlStepNode, XmlClassConst consts, ClassViewDataClass viewData)
        {
            _consts = consts;
            _viewData = viewData;
            this.xmlStepNode = xmlStepNode;
            var xmlDocument = new XmlDocument();
            xmlStepNode = xmlDocument.CreateElement("step");            
        }

        /// <summary>
        /// Узел Xml step
        /// </summary>
        /// <param name="numStep">номер шага</param>
        /// <param name="chamSecCnt">колличество камер</param>
        /// <param name="flapsCnt">колличество клапанов</param>
        /// <param name="rrgCnt">колличество РРГ</param>
        public XmlClassStep(int num, XmlNode xmlStepNode, XmlClassConst consts, ClassViewDataClass viewData):this(xmlStepNode,consts,viewData)
        {
            xmlStepNode.SelectSingleNode("params").Attributes["numStep"].Value = num.ToString();
        }

        /// <summary>
        /// Узел Xml step
        /// </summary>
        /// <param name="numStep">номер шага</param>
        /// <param name="chamSecCnt">колличество камер</param>
        /// <param name="flapsCnt">колличество клапанов</param>
        /// <param name="rrgCnt">колличество РРГ</param>
        public XmlClassStep(int num, XmlClassConst consts, ClassViewDataClass viewData)
        {
            _consts = consts;
            _viewData = viewData;

            var xmlDocument = new XmlDocument();
            xmlStepNode = xmlDocument.CreateElement("step");

            var classStepParams = new XmlClassStepParams(num, consts);
            var importNode = xmlStepNode.OwnerDocument.ImportNode(classStepParams.XmlNode, false);
            xmlStepNode.AppendChild(importNode);


            var conditionalScript = new XmlConditionalScript(consts);
            importNode = xmlStepNode.OwnerDocument.ImportNode(conditionalScript.XmlNode, true);
            xmlStepNode.AppendChild(importNode);
            
            for (int idx = 0; idx < _consts.ConstCham.Length; idx++)
            {
                var cham = new XmlClassChamber(1, consts.ConstCham[idx], _viewData.ChamberView[idx]);
                importNode = xmlStepNode.OwnerDocument.ImportNode(cham.XmlNode, true);
                xmlStepNode.AppendChild(importNode);
            }

            for (var idx = 0; idx < _consts.ConstFlaps.Length; idx++)
            {
                var flap = new XmlClassFlap(idx + 1, consts.ConstFlaps[idx],_viewData.FlapView[idx]);
                importNode = xmlStepNode.OwnerDocument.ImportNode(flap.XmlNode, false);
                xmlStepNode.AppendChild(importNode);
            }

            for (var idx = 0; idx < _consts.ConstFreqGenerator.Length; idx++)
            {
                var shim = new XmlClassFreqGenerator(idx + 1, _consts.ConstFreqGenerator[idx], _viewData.FreqGeneratorView[idx]);
                importNode = xmlStepNode.OwnerDocument.ImportNode(shim.XmlNode, false);
                xmlStepNode.AppendChild(importNode);
            }

            for (var idx = 0; idx < _consts.ConstBurner.Length; idx++)
            {
                var burn = new XmlClassHydrogenBurning(idx+1, _consts.ConstBurner[idx],_viewData.HydrogenBurnerView[idx]);
                importNode = xmlStepNode.OwnerDocument.ImportNode(burn.XmlNode, false);
                xmlStepNode.AppendChild(importNode);
            }
                        
            for (var idx = 0; idx < _consts.ConstRrgs.Length; idx++)
            {
                var rrg = new XmlClassRrg(idx + 1, consts.ConstRrgs[idx],_viewData.RrgView[idx]);
                importNode = xmlStepNode.OwnerDocument.ImportNode(rrg.XmlNode, true);
                xmlStepNode.AppendChild(importNode);
            }
            for (var idx = 0; idx < _consts.ConstVacuumetr.Length; idx++)
            {
                var vacuumetr = new XmlClassVacuumetr(idx + 1, consts.ConstVacuumetr[idx], _viewData.VacummetrView[idx]);
                importNode = xmlStepNode.OwnerDocument.ImportNode(vacuumetr.XmlNode, true);
                xmlStepNode.AppendChild(importNode);
            }

            for (var idx = 0; idx < _consts.ConstSensor.Length; idx++)
            {
                var classSensor = new XmlClassSensor(idx + 1, consts.ConstSensor[idx], _viewData.SensorView[idx]);
                importNode = xmlStepNode.OwnerDocument.ImportNode(classSensor.XmlNode, true);
                xmlStepNode.AppendChild(importNode);
            }

            for (var idx = 0; idx < _consts.PumpSysConst.Length; idx++)
            {
                var pumpSys = new XmlClassPumpSys(idx + 1, consts.PumpSysConst[idx], _viewData.PumpSysView[idx]);
                importNode = xmlStepNode.OwnerDocument.ImportNode(pumpSys.XmlNode, true);
                xmlStepNode.AppendChild(importNode);
            }

            for (var idx = 0; idx < _consts.ConstShutters.Length; idx++)
            {
                var shutter = new XmlClassShutter(idx + 1, consts.ConstShutters[idx], _viewData.ShutterView[idx]);
                importNode = xmlStepNode.OwnerDocument.ImportNode(shutter.XmlNode, true);
                xmlStepNode.AppendChild(importNode);
            }


            for (var idx = 0; idx < _consts.ConstBubblers.Length; idx++)
            {
                var bubbler = new XmlClassBubbler(idx + 1, consts.ConstBubblers[idx], _viewData.BubblerView[idx]);
                importNode = xmlStepNode.OwnerDocument.ImportNode(bubbler.XmlNode, true);
                xmlStepNode.AppendChild(importNode);
            }

            for (var idx = 0; idx < _consts.ConstLoader.Length; idx++)
            {
                var loaders = new XmlClassLoader(idx + 1, consts.ConstLoader[idx], _viewData.LoaderView[idx]);
                importNode = xmlStepNode.OwnerDocument.ImportNode(loaders.XmlNode, true);
                xmlStepNode.AppendChild(importNode);
            }

            for (var idx = 0; idx < _consts.GateConst.Length; idx++)
            {
                var classGate = new XmlClassGate (idx + 1, consts.GateConst[idx], _viewData.GateView[idx]);
                importNode = xmlStepNode.OwnerDocument.ImportNode(classGate.XmlNode, true);
                xmlStepNode.AppendChild(importNode);
            }

        }

        /// <summary>
        /// Узел Xml step
        /// </summary>
        /// <param name="xmlStepNode"></param>
        public XmlClassStep(XmlNode xmlStepNode)
        {
            this.xmlStepNode = xmlStepNode;
        }

        private XmlNode xmlStepNode;        

        /// <summary>
        /// Узел команды скрипта
        /// </summary>
        public XmlNode XmlStepNode => xmlStepNode;


        XmlClassVacuumetr[] _vacuumetrs;
        /// <summary>
        /// Массив вакуметров
        /// </summary>
        public XmlClassVacuumetr[] Vacuumetrs
        {
            get
            {
                if (_vacuumetrs != null)
                    return _vacuumetrs;

                var nodes = xmlStepNode.SelectNodes("dev[@name='vacuumetr']");


                _vacuumetrs = new XmlClassVacuumetr[nodes.Count];
                for (var idx = 0; idx < nodes.Count; idx++)
                    _vacuumetrs[idx] = new XmlClassVacuumetr(nodes[idx], _consts.ConstVacuumetr[idx], _viewData.VacummetrView[idx]);
                return _vacuumetrs;
            }
        }

        XmlClassSensor[] _sensors;
        /// <summary>
        /// Массив датчиков
        /// </summary>
        public XmlClassSensor[] Sensors
        {
            get
            {
                if (_sensors != null)
                    return _sensors;

                var nodes = xmlStepNode.SelectNodes("dev[@name='sensor']");
                if (nodes == null)
                    return null;

                _sensors = new XmlClassSensor[nodes.Count];
                for (var idx = 0; idx < nodes.Count; idx++)
                    _sensors[idx] = new XmlClassSensor(nodes[idx], _consts.ConstSensor[idx], _viewData.SensorView[idx]);
                return _sensors;
            }
        }

        XmlClassPumpSys[] _pumpSys;
        /// <summary>
        /// Массив систем создания вакуума
        /// </summary>
        public XmlClassPumpSys[] PumpSys
        {
            get
            {
                if (_pumpSys != null)
                    return _pumpSys;

                var nodes = xmlStepNode.SelectNodes("dev[@name='pumpSys']");


                _pumpSys = new XmlClassPumpSys[nodes.Count];
                for (var idx = 0; idx < nodes.Count; idx++)
                    _pumpSys[idx] = new XmlClassPumpSys(nodes[idx], _consts.PumpSysConst[idx], _viewData.PumpSysView[idx]);
                return _pumpSys;
            }
        }

        XmlClassGate[] _gates;
        /// <summary>
        /// Массив систем создания вакуума
        /// </summary>
        public XmlClassGate[] Gates
        {
            get
            {
                if (_gates != null)
                    return _gates;

                var nodes = xmlStepNode.SelectNodes("dev[@name='gate']");


                _gates = new XmlClassGate[nodes.Count];
                for (var idx = 0; idx < nodes.Count; idx++)
                    _gates[idx] = new XmlClassGate(nodes[idx], _consts.GateConst[idx], _viewData.GateView[idx]);
                return _gates;
            }
        }


        XmlClassFreqGenerator[] _freqGenerators;
        /// <summary>
        /// Массив частотных генераторов
        /// </summary>
        public XmlClassFreqGenerator[] FreqGenerators
        {
            get
            {
                if (_freqGenerators != null)
                    return _freqGenerators;

                var nodes = xmlStepNode.SelectNodes("dev[@name='freqGenerator']");
                _freqGenerators = new XmlClassFreqGenerator[nodes.Count];
                for (var idx = 0; idx < nodes.Count; idx++)
                    _freqGenerators[idx] = new XmlClassFreqGenerator(nodes[idx], _consts.ConstFreqGenerator[idx], _viewData.FreqGeneratorView[idx]);
                return _freqGenerators;
            }
        }

        XmlClassHydrogenBurning[] _burners;
        /// <summary>
        /// Массив горелок
        /// </summary>
        public XmlClassHydrogenBurning[] Burners
        {
            get
            {
                if (_burners != null)
                    return _burners;

                var nodes = xmlStepNode.SelectNodes("dev[@name='hydrogenBurning']");


                _burners = new XmlClassHydrogenBurning[nodes.Count];
                for (var idx = 0; idx < nodes.Count; idx++)
                    _burners[idx] = new XmlClassHydrogenBurning(nodes[idx],_consts.ConstBurner[idx], _viewData.HydrogenBurnerView[idx]);

                return _burners;
            }
        }

        private XmlClassBubbler[] bubblers;
        /// <summary>
        /// Массив барботеров
        /// </summary>
        public XmlClassBubbler[] Bubblers
        {
            get
            {
                if (bubblers != null)
                    return bubblers;
                var nodes = xmlStepNode.SelectNodes("dev[@name='bubbler']");
                bubblers = new XmlClassBubbler[nodes.Count];
                for (var idx = 0; idx < nodes.Count; idx++)
                    bubblers[idx] = new XmlClassBubbler(nodes[idx], _consts.ConstBubblers[idx], _viewData.BubblerView[idx]);

                return bubblers;
            }
        }

        private XmlClassRrg[] rrgs;
        /// <summary>
        /// Массив РРГ
        /// </summary>
        public XmlClassRrg[] Rrgs
        {
            get
            {
                if (rrgs != null)
                    return rrgs;
                var nodes = xmlStepNode.SelectNodes("dev[@name='rrg']");
                rrgs = new XmlClassRrg[nodes.Count];
                for (var idx = 0; idx < nodes.Count; idx++)
                    rrgs[idx] = new XmlClassRrg(nodes[idx], _consts.ConstRrgs[idx],_viewData.RrgView[idx]);

                return rrgs;
            }
        }

        private XmlClassShutter[] shutters;
        /// <summary>
        /// Массив РРГ
        /// </summary>
        public XmlClassShutter[] Shutters
        {
            get
            {
                if (shutters != null)
                    return shutters;
                var nodes = xmlStepNode.SelectNodes("dev[@name='shutter']");
                shutters = new XmlClassShutter[nodes.Count];
                for (var idx = 0; idx < nodes.Count; idx++)
                    shutters[idx] = new XmlClassShutter(nodes[idx], _consts.ConstShutters[idx], _viewData.ShutterView[idx]);

                return shutters;
            }
        }

        private XmlClassFlap[] flaps;
        /// <summary>
        /// Массив Flap (клапанов)
        /// </summary>
        public XmlClassFlap[] Flaps
        {
            get
            {
                if (flaps != null)
                    return flaps;
                var nodes = xmlStepNode.SelectNodes("dev[@name='flap']");
                flaps = new XmlClassFlap[nodes.Count];
                for (int idx = 0; idx < nodes.Count; idx++)
                    flaps[idx] = new XmlClassFlap(nodes[idx], _consts.ConstFlaps[idx], _viewData.FlapView[idx]);

                return flaps;
            }
        }
        
        public XmlClassChamber[] chambers;
        /// <summary>
        /// Массив Термокамер
        /// </summary>
        public XmlClassChamber[] Chambers
        {
            get
            {
                if (chambers != null)
                    return chambers;
                var nodes = xmlStepNode.SelectNodes("dev[@name='cham']");
                chambers = new XmlClassChamber[nodes.Count];
                for (int idx = 0; idx < nodes.Count; idx++)
                    chambers[idx] = new XmlClassChamber(nodes[idx], _consts.ConstCham[idx], _viewData.ChamberView[idx]);
                return chambers;
            }
        }

        public XmlClassLoader[] loaders;
        /// <summary>
        /// Массив загрузчиков
        /// </summary>
        public XmlClassLoader[] Loaders
        {
            get
            {
                if (loaders != null)
                    return loaders;
                var nodes = xmlStepNode.SelectNodes("dev[@name='loader']");
                loaders = new XmlClassLoader[nodes.Count];
                for (int idx = 0; idx < nodes.Count; idx++)
                    loaders[idx] = new XmlClassLoader(nodes[idx], _consts.ConstLoader[idx], _viewData.LoaderView[idx]);
                return loaders;
            }
        }


        private XmlClassStepParams _stepParams;
        /// <summary>
        /// Параметры шага
        /// </summary>
        public XmlClassStepParams StepParams
        {
            get
            {
                if (_stepParams != null)
                    return _stepParams;
                var node = xmlStepNode.SelectSingleNode("params");
                _stepParams= new XmlClassStepParams(node,_consts);
                return _stepParams;
            }           
        }

        public XmlConditionalScript ConditionalScript
        {
            get
            {
                var node = xmlStepNode.SelectSingleNode("conditional");
                return new XmlConditionalScript(node, _consts);
            }
        }
    }
}
