using ChartApplication.points;
using GasStation.Elements.Data;
using System.Windows;
using GasStation.ViewModels.Elements;
using GasStation.xml.Constant;
using GasStation.xml.Script;
using GasStation.xml.Script.EnumConst;
using GasStation.xml.Script.Security;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace GasStation.xml
{
    public class ClassScript:INotifyPropertyChanged
    {
        /// <summary>
        /// Путь к xml файлу паспортных констант
        /// </summary>
        public static string DefaultConstPath = @"xml\consts.xml";

        #region Реализация интерфейса INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        

        #endregion
        public  XmlClassConst Consts { get; set; }

        
        /// <summary>
        /// Класс отображения данных
        /// </summary>
        ClassViewDataClass _viewData { get; set; }

        public ClassViewDataClass ViewData
        {
            get
            {
                if (_viewData != null)
                    return _viewData;
                _viewData = new ClassViewDataClass(Consts);
                return _viewData;
            }
            set { _viewData = value; }
        }

        public void RefreshData()
        {
            //ClsDataTime = new ClassDataTime(Steps);
            ViewData.Clear();
            SelectedGraph = "Термокамера";
        }
        
        private ClassDataTime _clsDataTime;
        /// <summary>
        /// Класс данных по времени
        /// </summary>
        public ClassDataTime ClsDataTime
        {
            get
            {
                if (_clsDataTime== null)
                {
                    ClsDataTime = new ClassDataTime(Steps);
                }
                return _clsDataTime;
            }
            set
            {
                _clsDataTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ClsDataTime"));
            }
        }

        public ClassScript(string pathConst)
        {
            Consts = new XmlClassConst(pathConst);            
            Steps = new ObservableCollection<XmlClassStep>();            
            CreateNewStep();
            CurrStep = Steps[0];

            if (ViewData.ChamberView.Length == 0)
                return;
            MasGraph = ViewData.ChamberView[0].MasGraph;

            FileName = @"Техпроцесс не загружен";
        }

        public ClassScript(string pathScript, string pathConst,string fileName)
        {
            FileName = fileName;

            Consts = new XmlClassConst(pathConst);

            Steps = new ObservableCollection<XmlClassStep>();   

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(pathScript);
            var nodes = xmlDocument.SelectNodes("script/step");

            for (int i = 0; i < nodes.Count; i++)
                Steps.Add(new XmlClassStep(nodes[i], Consts, ViewData));

            currStep = Steps[0];
            MasGraph = ViewData.ChamberView[0].MasGraph;
        }

        public void CreateNewStep()
        {           
            Steps.Add(new XmlClassStep(Steps.Count + 1, Consts, ViewData));
        }

        public void CopyStep()
        {
            if (Steps.Count == 0)
                return;
            if (CurrStep == null)
                return;
            Steps.Add(new XmlClassStep(Steps.Count + 1, CurrStep.XmlStepNode.Clone(), Consts, _viewData));
        }

        public void DeleteStep()
        {
            if (CurrStep == null)
                return;
            if (CurrStep.StepParams.NumStep == 1)
            {
                System.Windows.MessageBox.Show("Невозможно удалить шаг Ожидания");
                return;
            }
            if (Steps.Count == 0)
            {
                return;
            }
            Steps.Remove(CurrStep);
            for(int i=0;i < Steps.Count;i++)
            {
                Steps[i].StepParams.NumStep = i + 1;
            }
        }

        public void GoToStep(int stepNum)
        {
            if (stepNum < 0)
                throw new Exception("Номер шага меньше нуля");

            if (stepNum >= Steps.Count)
                throw new Exception("Нет шага с таким номером");

            CurrStep=Steps[stepNum];
        }
        
        public void Save(string path)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);

            var xmlNode = xmlDocument.CreateElement("script");
            
            foreach (var step in Steps)
            {
                var importNode = xmlNode.OwnerDocument.ImportNode(step.XmlStepNode, true);
                xmlNode.AppendChild(importNode);
            }
            xmlDocument.AppendChild(xmlNode);
            xmlDocument.Save(path);
        }


        private XmlClassStep currStep;
        /// <summary>
        /// Шаги скрипта
        /// </summary>
        public XmlClassStep CurrStep
        {
            get
            {                
                return currStep;
            }
            set
            {
                currStep = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrStep"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EnableStartTechProcess"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EnableStopTechProcess"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EnablePauseTechProcess"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StatusSystem"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentStepType"));
            }
        }

        public int _сurrentStepIdx;
        /// <summary>
        /// Текущий рабочий шаг скрипта
        /// </summary>
        public int CurrentStepIdx
        {
            get
            {
                return _сurrentStepIdx;
            }
            set
            {
                _сurrentStepIdx = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentStepName"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RunningStepIdx"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentStepIdx"));
            }
        }
        /// <summary>
        /// Название текущего шага
        /// </summary>
        public string CurrentStepName
        {
            get
            {
                if (CurrentStepIdx >= Steps.Count)
                    return "";

                if (CurrentStepIdx < 0)
                {
                }
                if (Steps.Count < CurrentStepIdx)
                {
                }
                return Steps[CurrentStepIdx].StepParams.NameStep;
            }
        }

        public int RunningStepIdx
        {
            get
            {

                Console.WriteLine($"CurrStep= {CurrentStepIdx}");
                if (CurrentStepIdx >= Steps.Count)
                    return 0;

                if (CurrentStepIdx < 0)
                {
                }
                if (Steps.Count < CurrentStepIdx)
                {
                }
                return Steps[CurrentStepIdx].StepParams.NumStep-1;
            }
        }

        public RegimsStep CurrentStepType
        {
            get
            {
                if (CurrentStepIdx < 0)
                {
                }
                if (Steps.Count < CurrentStepIdx)
                {
                }
                return Steps[CurrentStepIdx].StepParams.TypeStep;
            }
        }

        public string StatusSystem
        {
            get
            {
                if(!IsViewScriptStart)
                    return "Подключено";
                if(IsViewScriptStart)
                    return "Отключено";
                return "Отключено";
            }
        }

        private string selectedGraph;
        public string SelectedGraph
        {
            get { return selectedGraph; }
            set
            {
                selectedGraph = value;

                switch(selectedGraph)
                {
                    case "Термокамера":
                        if(ViewData.ChamberView.Length>0)
                            MasGraph = ViewData.ChamberView[0].MasGraph;
                        break;
                    case "Горелка":
                        MasGraph = ViewData.HydrogenBurnerView[0].MasGraph;
                        break;
                    case "РРГ":
                        MasGraph = ViewData.MasGraphRrg;
                        break;
                    case "Барботер":
                        MasGraph = ViewData.MasGraphBubblers;
                        break;
                    case "Вакууметр":
                        MasGraph = ViewData.MasGraphVacuumetr;
                        break;
                    case "Насосная система":
                        MasGraph = ViewData.PumpSysView[0].MasGraph;
                        break;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedGraph"));
            }
        }
        

        public ObservableCollection<XmlClassStep> Steps { get; set; }

        public PointsData<Point>[] _masGraph;
        public PointsData<Point>[] MasGraph
        {
            get
            {
                return _masGraph;
            }
            set
            {
                _masGraph = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MasGraph"));
            }
        }

        
        public string FileName { get; set; }

        private bool enableStartTechProcess = false;
        public bool EnableStartTechProcess
        {
            get
            {
                return enableStartTechProcess && (CurrentStepIdx == 0);
            }
            set
            {
                enableStartTechProcess = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EnableStartTechProcess"));
            }
        }

        public bool EnableStopTechProcess => (!EnableStartTechProcess && !IsViewScriptStart);

        /// <summary>
        /// Обновление связанных полей
        /// </summary>
        public void Update()
        {

            var step = CurrStep;
            CurrStep = null;
            CurrStep = step;
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrStep"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StatusSystem"));
        }

        /// <summary>
        /// Разрешение на изменение списка шагов
        /// </summary>
        public bool IsChangeScript
        {
            get
            {
                var result = Consts.SecuretyConst.CurrUser.Privs.Contains(EnumPriv.ChangeScript);

                return result;
            }
            set
            {
                if (!Consts.SecuretyConst.CurrUser.Privs.Contains(EnumPriv.MegaBoss))
                {
                    System.Windows.MessageBox.Show(@"У вас нет привилегии MegaBoss");
                    return;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsChangeScript"));
            }
        }

        private bool enablePauseTechProcess=true;

        public bool EnablePauseTechProcess
        {
            get 
            {
                enablePauseTechProcess = false;
                if (!EnableStartTechProcess && !IsViewScriptStart)
                {
                    if (Consts.SecuretyConst.CurrUser.UserName == "Admin")
                    {
                        enablePauseTechProcess = true;
                    }
                }
                return enablePauseTechProcess;
            }
            set 
            {
                value = enablePauseTechProcess;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EnablePauseTechProcess"));
            }
        }

       
        /// <summary>
        /// Разрешение на переход по списку шагов
        /// </summary>
        public bool IsGoScript
        {
            get
            {
                var result = Consts.SecuretyConst.CurrUser.Privs.Contains(EnumPriv.GoStepScript); 

                return result;
            }
            set
            {
                if (!Consts.SecuretyConst.CurrUser.Privs.Contains(EnumPriv.MegaBoss))
                {
                    System.Windows.MessageBox.Show(@"У вас нет привилегии MegaBoss");
                    return;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsGoScript"));
            }
        }          

        #region Из ViewMod
        private bool _isViewScriptStart = true;
        public bool IsViewScriptStart
        {
            get { return _isViewScriptStart; }
            set
            {
                _isViewScriptStart = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsViewScriptStart"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsViewScriptStop"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StatusSystem"));
            }
        }

        private bool _isViewScriptNext = true;
        public bool IsViewScriptNext
        {
            get { return _isViewScriptNext; }
            set
            {
                _isViewScriptNext = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsViewScriptNext"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StatusSystem"));
            }
        }


        private bool _isViewScriptStop = false;
        public bool IsViewScriptStop
        {
            get { return _isViewScriptStop; }
            set
            {
                _isViewScriptStop = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsViewScriptStop"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StatusSystem"));
            }
        }
        #endregion
    }
}
