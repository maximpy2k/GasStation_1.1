using StationImitation.Controllers;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using GasStation.Controllers;
using GasStation.Elements.ViewModels;
using GasStation.xml.Constant.XmlConst.Elements;
using StationImitation.ViewModel;

namespace StationImitation
{
    public class ClassImitator
    {
        private readonly ViewModelChannel _viewModel;
        string com;
        private List<ClassBaseController> controlers;
        private XmlClassControllerConst[] ViewControlers;// = new List<XmlClassControllerConst>();
        private List<BaseClassController> listControllers;
        public ClassImitator(ViewModelChannel viewModel)
        {
            _viewModel = viewModel;
            com = $"COM{_viewModel.ConstView.ClassXmlConst.ConstProgramm.Port}";
            ViewControlers = viewModel.ConstView.ClassXmlConst.ConstControllers;
            InitControllers(viewModel.ConstView.ViewData.ViewControllers);

          //this.com = com;
           //controlers.Add(new Class_IDAS7018(24));
           //controlers.Add(new Class_IDAS7042(10));
           //controlers.Add(new Class_IDAS7042(11));
           //controlers.Add(new Class_IDAS7042(12));
           //controlers.Add(new Class_IDAS7042(13));
           //controlers.Add(new Class_IDAS7041(14));
           //controlers.Add(new Class_IDAS87017(17));
           //controlers.Add(new Class_IDAS87024(29));
           //controlers.Add(new Class_IDAS87024(30));
           //controlers.Add(new Class_IDAS87024(31));
           //controlers.Add(new Class_IDAS87057(3));
           //controlers.Add(new Class_IDAS87053(65));
        }
        private void InitControllers(BaseClassViewControllers[] viewControllers)
        {
            controlers = new List<ClassBaseController>();
            ClassBaseController controller;

            for (int i = 0; i < _viewModel.ConstView.ClassXmlConst.ConstControllers.Length; i++)
            {
                switch (_viewModel.ConstView.ClassXmlConst.ConstControllers[i].NameController)
                {
                    case "IDAS 7018":
                        controller = new Class_IDAS7018(_viewModel.ConstView.ClassXmlConst.ConstControllers[i], viewControllers[i]);
                        break;
                    case "TM 7042":
                    case "TM 7042P":
                        controller = new Class_IDAS7042(_viewModel.ConstView.ClassXmlConst.ConstControllers[i], viewControllers[i]);
                        break;
                    case "TM 7041":
                        controller = new Class_IDAS7041(_viewModel.ConstView.ClassXmlConst.ConstControllers[i], viewControllers[i]);
                        break;
                    case "IDAS 87017":
                        controller = new Class_IDAS87017(_viewModel.ConstView.ClassXmlConst.ConstControllers[i], viewControllers[i]);
                        break;
                    case "IDAS 87024":
                        controller = new Class_IDAS87024(_viewModel.ConstView.ClassXmlConst.ConstControllers[i], viewControllers[i]);
                        break;
                    case "IDAS 87057":
                        controller = new Class_IDAS87057(_viewModel.ConstView.ClassXmlConst.ConstControllers[i], viewControllers[i]);
                        break;
                    case "IDAS 87053":
                        controller = new Class_IDAS87053(_viewModel.ConstView.ClassXmlConst.ConstControllers[i], viewControllers[i]);
                        break;
                    case "TM SHIM":
                        controller = new Class_Shim(_viewModel.ConstView.ClassXmlConst.ConstControllers[i], viewControllers[i]);
                        break;
                    default:
                        return;
                }
                controlers.Add(controller);
            }
        }
        public SerialPort sp;
        public void Start()
        {
            new Thread(ThreadFunc).Start();
        }

        public void Stop()
        {

        }
        private bool flagAbort;
        public void ThreadFunc()
        {
            sp = new SerialPort(com, 115200);
            sp.NewLine = "\r";
            sp.Open();
            flagAbort = false;

            //while (true)
            //{
            //    foreach (var ctrl in controlers)
            //        ctrl.InputQuest("#18");
            //}


            while (!flagAbort)
            {
                if (sp.BytesToRead != 0)
                {
                    try
                    {
                        var quest = sp.ReadLine();
                        Console.WriteLine($"Запрос {quest}");
                        if (quest == "~**")
                        {
                            foreach (var ctrl in controlers)
                                ctrl.InputQuest(quest);
                            continue;
                        }
                        Console.WriteLine();

                        var adr = Convert.ToInt32(quest.Substring(1, 2), 16);
                        var ctr = controlers.Where(dat => dat.addr == adr).First();

                        ctr.InputQuest(quest);
                        if (ctr.Answer != "")
                        {
                            Console.WriteLine($"Ответ {ctr.Answer}");
                            sp.WriteLine(ctr.Answer);
                        }
                    }
                    catch (Exception e)
                    {
                        var a = e;
                        Console.WriteLine("Err");
                    }

                }
            }
            sp.Close();
        }
    }
}
