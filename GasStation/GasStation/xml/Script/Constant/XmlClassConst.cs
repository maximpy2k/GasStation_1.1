using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using GasStation.ViewModels.Converters;
using GasStation.xml.Const.Elements;
using GasStation.xml.Constant.XmlConst.Elements;
using GasStation.xml.Script.Constant;

namespace GasStation.xml.Constant
{
    public class XmlClassConst
    {
        /// <summary>
        /// Путь к файлу
        /// </summary>
        private readonly string _path;

        public XmlClassConst(string path)
        {
            _path = path;
            XmlConst = new XmlDocument();
            XmlConst.Load(path);
        }

        

        /// <summary>
        /// Узел команды скрипта
        /// </summary>
        public XmlDocument XmlConst { get; private set; }
        /// <summary>
        /// Константы для канала
        /// </summary>
        public XmlClassChannelConst ChannelConsts
        {
            get
            {
                var xmlConst = XmlConst.SelectSingleNode("settings");
                var nameChannel = new XmlClassChannelConst(xmlConst);

                return nameChannel;
            }
        }
        
        public XmlClassSecuretyConst SecuretyConst
        {
            get
            {
                var xmlConst = XmlConst.SelectSingleNode("settings/securety");
                var securety = new XmlClassSecuretyConst(xmlConst);                
                return securety;
            }
        }

        public XmlClassChamberConst[] ConstCham
        {
            get
            {
                var xmlConst = XmlConst.SelectNodes("settings/dev[@name='chamber']");
                var masConst = new XmlClassChamberConst[xmlConst.Count];
                for (var i = 0; i < xmlConst.Count; i++)
                {
                    masConst[i] = new XmlClassChamberConst(xmlConst[i]);
                }
                return masConst;
            }
        }

        public XmlClassVacuumetrConst[] ConstVacuumetr
        {
            get
            {
                var xmlConst = XmlConst.SelectNodes("settings/dev[@name='vacuumetr']");

                var masConst = new XmlClassVacuumetrConst[xmlConst.Count];
                for (var i = 0; i < xmlConst.Count; i++)
                {
                    masConst[i] = new XmlClassVacuumetrConst(xmlConst[i]);
                }

                return masConst;
            }
        }

        public XmlClassSensorConst[] ConstSensor
        {
            get
            {
                var xmlConst = XmlConst.SelectNodes("settings/dev[@name='sensor']");

                var masConst = new XmlClassSensorConst[xmlConst.Count];
                for (var i = 0; i < xmlConst.Count; i++)
                    masConst[i] = new XmlClassSensorConst(xmlConst[i]);

                return masConst;
            }
        }

        public XmlClassPumpSysConst[] PumpSysConst
        {
            get
            {
                var xmlConst = XmlConst.SelectNodes("settings/dev[@name='pumpSystem']");

                var masConst = new XmlClassPumpSysConst[xmlConst.Count];
                for (var i = 0; i < xmlConst.Count; i++)
                {
                    masConst[i] = new XmlClassPumpSysConst(xmlConst[i]);
                }
                
                return masConst;
            }
        }

        public XmlClassGateConst[] GateConst
        {
            get
            {
                var xmlConst = XmlConst.SelectNodes("settings/dev[@name='gate']");

                var masConst = new XmlClassGateConst[xmlConst.Count];
                for (var i = 0; i < xmlConst.Count; i++)
                {
                    masConst[i] = new XmlClassGateConst(xmlConst[i]);
                }

                return masConst;
            }
        }

        public XmlClassHydrogenBurnerConst [] ConstBurner
        {
            get
            {
                var xmlConst = XmlConst.SelectNodes("settings/dev[@name='burner']");
                var masConst = new XmlClassHydrogenBurnerConst [xmlConst.Count];
                for (var i = 0; i < xmlConst.Count; i++)
                {
                    masConst[i] = new XmlClassHydrogenBurnerConst (xmlConst[i]);
                }
                return masConst;
            }
        }

        public XmlClassFreqGeneratorConst[] ConstFreqGenerator
        {
            get
            {
                var xmlConst = XmlConst.SelectNodes("settings/dev[@name='freqGenerator']");
                var masConst = new XmlClassFreqGeneratorConst[xmlConst.Count];
                for (var i = 0; i < xmlConst.Count; i++)
                {
                    masConst[i] = new XmlClassFreqGeneratorConst(xmlConst[i]);
                }
                return masConst;
            }
        }

        public XmlClassRrgConst[] ConstRrgs
        {
            get
            {
                var xmlConst = XmlConst.SelectNodes("settings/dev[@name='rrg']");
                
                var masConst = new XmlClassRrgConst[xmlConst.Count];
                for (var i = 0; i < xmlConst.Count; i++)
                {
                    masConst[i] = new XmlClassRrgConst(xmlConst[i]);
                }
                return masConst;
            }
        }

        public XmlClassBubblerConst[] ConstBubblers
        {
            get
            {
                var xmlConst = XmlConst.SelectNodes("settings/dev[@name='bubbler']");

                var masConst = new XmlClassBubblerConst[xmlConst.Count];
                for (var i = 0; i < xmlConst.Count; i++)
                {
                    masConst[i] = new XmlClassBubblerConst(xmlConst[i]);
                }
                return masConst;
            }
        }

        public XmlClassFlapConst[] ConstFlaps
        {
            get
            {
                var xmlConst = XmlConst.SelectNodes("settings/dev[@name='flap']");
                var masConst = new XmlClassFlapConst[xmlConst.Count];
                for (var i = 0; i < xmlConst.Count; i++)
                {
                    masConst[i] = new XmlClassFlapConst(xmlConst[i]);
                }
                return masConst;
            }
        }

        public XmlClassShutterConst[] ConstShutters
        {
            get
            {
                var xmlConst = XmlConst.SelectNodes("settings/dev[@name='shutter']");
                var masConst = new XmlClassShutterConst[xmlConst.Count];
                for (var i = 0; i < xmlConst.Count; i++)
                {
                    masConst[i] = new XmlClassShutterConst(xmlConst[i]);
                }
                return masConst;
            }
        }

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

        /// <summary>
        /// Массив контант загрузчиков
        /// </summary>
        public XmlClassLoaderConst[] ConstLoader
        {
            get
            {
                var xmlConst = XmlConst.SelectNodes("settings/dev[@name='loader']");
                var masConst = new XmlClassLoaderConst[xmlConst.Count];
                for (var i = 0; i < xmlConst.Count; i++)
                {
                    masConst[i] = new XmlClassLoaderConst(xmlConst[i]);
                }

                return masConst;
            }
        }

        public XmlClassProgrammConst ConstProgramm
        {
            get
            {
                var xmlConst = XmlConst.SelectSingleNode("settings/dev[@name='ComPort']");                               
                return new XmlClassProgrammConst(xmlConst);
            }
        }

        public void Save(string path=null)
        {
            if (path == null)
                path = _path; 
            XmlConst.Save(path);
        }

        public void Open(string path)
        {
            XmlConst = new XmlDocument();

            XmlConst.Load(path);
        }

        /// <summary>
        /// Устройства для условных переходов
        /// </summary>
        public List<string> AvalibleDevices
        {
            get
            {
                var conv = new ClassConverterConditionDevices();
                
                //РРГ
                var mas = ConstRrgs.Select(baseClassRrgConst => $"{conv.Convert(baseClassRrgConst.DevName, baseClassRrgConst.DevNum)}").ToList();

                //Термосекции в термокамере
                mas.AddRange(ConstCham[0].ThermoSectionConst.Select(baseClassThermoSectionConst => $"{conv.Convert(baseClassThermoSectionConst.DevName, baseClassThermoSectionConst.DevNum)}"));

                //Клапан №
                //mas.AddRange(ConstFlaps.Select(baseClassFlapConst => $"{conv.Convert(baseClassFlapConst.DevName, baseClassFlapConst.DevNum)}"));

                //mas.AddRange(ConstShutters.Select(shutter=>$"{conv.Convert(shutter.StatusConst.)}"));

                //Вакууметры в РРГ
                mas.AddRange(from rrgConst in ConstRrgs where rrgConst.VacuumetrConst != null select $"{rrgConst.RusName} {rrgConst.DevNum} {rrgConst.VacuumetrConst.RusName} {rrgConst.VacuumetrConst.DevNum}");

                //Вакууметры в насосах
                mas.AddRange(from pumpSysConst in PumpSysConst where pumpSysConst.VacuumetrConst != null select $"{pumpSysConst.RusName} {pumpSysConst.DevNum} {pumpSysConst.VacuumetrConst.RusName} {pumpSysConst.VacuumetrConst.DevNum}");

                //Горелка
                foreach (var baseClassHydrogenBurner in ConstBurner)
                {
                    //Температура горелки
                    var xml = baseClassHydrogenBurner;
                    mas.Add($"{conv.Convert(xml.TdBurner.DevName, xml.TdBurner.DevNum)}");

                    //Температура горения
                    mas.Add($"{conv.Convert(xml.TdFire.DevName, xml.TdFire.DevNum)}");

                    //Датчик воды горелки
                    mas.Add($"{conv.Convert(xml.DioWaterConst.DevName, xml.DioWaterConst.DevNum,@"горелки")}");
                    //Датчик пламени горелки
                    mas.Add($"{conv.Convert(xml.DioFireConst.DevName, xml.DioFireConst.DevNum)}");
                }

                //Датчик воды камеры
                mas.Add($"{conv.Convert(ConstCham[0].DioWaterConst.DevName, ConstCham[0].DioWaterConst.DevNum, @"камеры")}");

                if (ConstCham[0].DioWaterForwardConst != null)
                    mas.Add($"{conv.Convert(ConstCham[0].DioWaterForwardConst.DevName, ConstCham[0].DioWaterForwardConst.DevNum, @"камеры")}");                    

                //Датчик воды камеры
                if (ConstCham[0].DioWaterBackwardConst != null)
                    mas.Add($"{conv.Convert(ConstCham[0].DioWaterBackwardConst.DevName, ConstCham[0].DioWaterBackwardConst.DevNum, @"камеры")}");

                if (ConstLoader.Length > 0)
                {
                    //Заслонка открыта
                    if (ConstLoader[0].StatusConst.DumperOpen != null)
                        mas.Add($"{conv.Convert(ConstLoader[0].StatusConst.DumperOpen.DevName, ConstLoader[0].StatusConst.DumperOpen.DevNum)}");
                    //Заслонка закрыта
                    if (ConstLoader[0].StatusConst.DumperClosed != null)
                        mas.Add($"{conv.Convert(ConstLoader[0].StatusConst.DumperClosed.DevName, ConstLoader[0].StatusConst.DumperClosed.DevNum)}");
                    //Загружен ОК
                    if (ConstLoader[0].StatusConst.LoadComplete != null)
                        mas.Add($"{conv.Convert(ConstLoader[0].StatusConst.LoadComplete.DevName, ConstLoader[0].StatusConst.LoadComplete.DevNum)}");
                    //Выгружен ОК
                    if (ConstLoader[0].StatusConst.UnLoadComplete != null)
                        mas.Add($"{conv.Convert(ConstLoader[0].StatusConst.UnLoadComplete.DevName, ConstLoader[0].StatusConst.UnLoadComplete.DevNum)}");
                }

                //if (ConstShutters.Length > 0)
                //{
                //    //Затвор открыт
                //    mas.Add($"{conv.Convert(ConstShutters[0].StatusConst.GateOpen.DevName, ConstShutters[0].StatusConst.GateOpen.DevNum)}");
                //    //Затвор закрыт
                //    mas.Add($"{conv.Convert(ConstShutters[0].StatusConst.GateClosed.DevName, ConstShutters[0].StatusConst.GateClosed.DevNum)}");
                //}

                return mas;
            }
        }


        /// <summary>
        /// Устройства для отображения графиков
        /// </summary>
        public List<string> AvalibleDevicesGraph
        {
            get
            {
                var mas = new List<string>();

                if (ConstRrgs.Length > 0)
                {
                    mas.Add(@"РРГ");
                }

               
                mas.AddRange(ConstCham.Select(xmlClassChamberConst => $"{xmlClassChamberConst.RusName}"));

                if (ConstBubblers.Length > 0 && ConstBubblers.Where(dat => dat.Td != null).Count() > 0)
                    mas.Add(@"Барботер");
                
                if (ConstVacuumetr.Length > 0)
                    mas.Add(@"Вакууметр");

                if (PumpSysConst.Length > 0)
                    mas.Add(@"Насосная система");

                mas.AddRange(ConstBurner.Select(xml => $"{xml.RusName} {xml.DevNum}"));

                return mas;
            }
        } 
    }
}
