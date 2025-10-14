using System;
using System.Collections.Generic;
using System.ComponentModel;
using GasStation.Elements.ViewModels;
using GasStation.xml.Constant;
using ChartApplication.points;
using System.Windows;
using System.Linq;
using GasStation.UserControls.Elements;

namespace GasStation.ViewModels.Elements
{
    public class ClassViewDataClass: INotifyPropertyChanged
    {
        public ClassViewDataClass(XmlClassConst con)
        {

            FreqGeneratorView = new ClassFreqGeneratorView[con.ConstFreqGenerator.Length];
            for (int i = 0; i < FreqGeneratorView.Length; i++)
                FreqGeneratorView[i] = new ClassFreqGeneratorView(con.ConstFreqGenerator[i]);

            HydrogenBurnerView = new ClassHydrogenBurnerView[con.ConstBurner.Length];
            for (int i = 0; i < HydrogenBurnerView.Length; i++)
                HydrogenBurnerView[i] = new ClassHydrogenBurnerView(con.ConstBurner[i]);

            VacummetrView = new ClassVacuumetrView[con.ConstVacuumetr.Length];
            for (int i = 0; i < VacummetrView.Length; i++)
                VacummetrView[i] = new ClassVacuumetrView(con.ConstVacuumetr[i],"");

            SensorView = new ClassSensorView[con.ConstSensor.Length];
            for (int i = 0; i < SensorView.Length; i++)
                SensorView[i] = new ClassSensorView(con.ConstSensor[i]);

            ChamberView = new ClassChamberView[con.ConstCham.Length];
            for (int i = 0; i < ChamberView.Length; i++)
                ChamberView[i] = new ClassChamberView(con.ConstCham[i]);

            PumpSysView = new ClassPumpSysView[con.PumpSysConst.Length];
            for (var i = 0; i < con.PumpSysConst.Length; i++)
                PumpSysView[i] = new ClassPumpSysView(con.PumpSysConst[i]);
           
            RrgView = new ClassRrgView[con.ConstRrgs.Length];
            for (var i = 0; i < RrgView.Length; i++)
                RrgView[i] = new ClassRrgView(con.ConstRrgs[i]);

            BubblerView = new ClassBubblerView[con.ConstBubblers.Length];
            for (var i = 0; i < BubblerView.Length; i++)
                BubblerView[i] = new ClassBubblerView(con.ConstBubblers[i]);

            ShutterView = new ClassShutterView[con.ConstShutters.Length];
            for (var i = 0; i < ShutterView.Length; i++)
                ShutterView[i] = new ClassShutterView(con.ConstShutters[i]);

            FlapView = new ClassFlapView[con.ConstFlaps.Length];
            for (var i = 0; i < FlapView.Length; i++)
                FlapView[i] = new ClassFlapView();



            LoaderView = new ClassLoaderView[con.ConstLoader.Length];
            for (var i = 0; i < LoaderView.Length; i++)
                LoaderView[i] = new ClassLoaderView(con.ConstLoader[i]);

            GateView = new ClassGateView[con.GateConst.Length];
            for (var i = 0; i < GateView.Length; i++)
                GateView[i] = new ClassGateView(con.GateConst[i]);


            ViewControllers = new BaseClassViewControllers[con.ConstControllers.Length];
            for (int i = 0; i < ViewControllers.Length; i++)
            {
                switch(con.ConstControllers[i].NameController)
                {
                    case "IDAS 7018":
                    case "IDAS 87017":
                        ViewControllers[i] = new ViewModelControllerAcp();
                        break;

                    case "IDAS 87057":
                    case "IDAS 87053":
                        ViewControllers[i] = new ViewModelControllerDio();
                        break;

                    case "IDAS 87024":
                        ViewControllers[i] = new ViewModelControllerCap();
                        break;

                    case "TM SHIM":
                    case "TM 7042":
                    case "TM 7042P":
                        ViewControllers[i] = new ViewModelControllerTMPower();
                        break;
                    case "TM 7041":
                        ViewControllers[i] = new ViewModelControllerDio();
                        break;

                    default:
                        ViewControllers[i] = new BaseClassViewControllers();
                        break;
                }
            }
        }
        public void Clear()
        {
            foreach (var item in HydrogenBurnerView)
                item.Clear();


            foreach (var item in ChamberView)
                item.Clear();


            foreach (var item in RrgView)
                item.Clear();

            foreach (var item in PumpSysView)
                item.Clear();

            foreach (var item in VacummetrView)
                item.Clear();
        }
        public ClassChamberView[] ChamberView { get; set; }
        public ClassSensorView[] SensorView { get; set; }
        public ClassVacuumetrView[] VacummetrView { get; set; }
        public ClassGateView[] GateView { get; set; }
        public ClassPumpSysView[] PumpSysView { get; set; }
        public ClassRrgView[] RrgView { get; set; }
        public ClassBubblerView[] BubblerView { get; set; }
        public ClassShutterView[] ShutterView { get; set; }
        public ClassLoaderView[] LoaderView { get; set; }
        public ClassFlapView[] FlapView { get; set; }

        public ClassFreqGeneratorView[] FreqGeneratorView { get; set; }
        public ClassHydrogenBurnerView[] HydrogenBurnerView { get; set; }
        public BaseClassViewControllers[] ViewControllers{ get; set; }

        private string _messageOut = "";

        public event PropertyChangedEventHandler PropertyChanged;

        public PointsData<Point>[] MasGraphRrg
        {
            get
            {
                if (RrgView.Length == 0)
                    return null;
                var lst = new List<PointsData<Point>>();
                for (int i = 0; i < RrgView.Length; i++)
                    lst.AddRange(RrgView[i].MasGraph);
                return lst.ToArray();
            }
        }

        public PointsData<Point>[] MasGraphVacuumetr
        {
            get
            {
                if (VacummetrView.Length == 0)
                    return null;
                var lst = new List<PointsData<Point>>();
                for (int i = 0; i < VacummetrView.Length; i++)
                    lst.Add(VacummetrView[i].SeriesReadPress);
                return lst.ToArray();
            }
        }

        public PointsData<Point>[] MasGraphBubblers
        {
            get
            {
                if (BubblerView.Length == 0)
                    return null;
                var lst = new List<PointsData<Point>>();
                for (int i = 0; i < BubblerView.Length; i++)
                {
                    var graphs = BubblerView[i].MasGraph;
                    if (graphs != null)
                        lst.AddRange(graphs);
                }
                return lst.ToArray();
            }
        }

        public string MessageOut
        { get
            {
                return _messageOut;
            }
            set
            {
                _messageOut = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MessageOut"));
            }
        }

        private int _maxValueProgress = 0;
        public int MaxValueProgress
        {
            get
            {
                return _maxValueProgress;
            }
            set
            {
                _maxValueProgress = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MaxValueProgress"));
            }
        }

        private int _currValueProgress = 0;
        public int CurrValueProgress
        {
            get
            {
                return _maxValueProgress;
            }
            set
            {
                _currValueProgress = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrValueProgress"));
            }
        }
    }
}
