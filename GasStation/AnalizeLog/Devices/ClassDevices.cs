using System;
using System.Collections.Generic;
using System.Linq;
using GasStation.xml;
using GasStation.Controllers;
using GasStation.xml.Script;
using GasStation.Devices;
using GasStation.Devices.EventDevParams;
using GasStation.Status;

namespace GasStation.Devices
{
    public class ClassDevices
    {
        /// <summary>
        /// Событие обработки условного перехода
        /// </summary>
        public EventHandler StateError;

        /// <summary>
        /// Событие обработки тревоги
        /// </summary>
        public EventHandler AlarmError;

        public ClassDevices(List<BaseClassController> listControllers,ClassScript clsScript)
        {
            var con = clsScript.Consts;

            for (int i = 0; i < con.ConstFlaps.Length; i++)
            {
                Flaps.Add(new ClassControlFlap(clsScript.ClsDataTime));
                Flaps.Last().StateError += CheckState;
                Flaps.Last().AlarmError += Alarm;
                Flaps.Last().LstContr = listControllers;
            }

            for (int i = 0; i < con.ConstRrgs.Length; i++)
            {
                Rrgs.Add(new ClassControlRRG(clsScript.ClsDataTime));
                Rrgs.Last().StateError += CheckState;
                Rrgs.Last().AlarmError += Alarm;
                Rrgs.Last().LstContr = listControllers;
            }

            for (int i = 0; i < con.ConstBubblers.Length; i++)
            {
                Bublers.Add(new ClassControlBubbler(clsScript.ClsDataTime));
                Bublers.Last().StateError += CheckState;
                Bublers.Last().AlarmError += Alarm;
                Bublers.Last().LstContr = listControllers;

            }
            for (int i = 0; i < con.ConstVacuumetr.Length; i++)
            {
                Vacumetrs.Add(new ClassControlVacuumetr(clsScript.ClsDataTime));
                Vacumetrs.Last().StateError += CheckState;
                Vacumetrs.Last().AlarmError += Alarm;
                Vacumetrs.Last().LstContr = listControllers;
            }

            for (int i = 0; i < con.ConstSensor.Length; i++)
            {
               Sensors.Add(new ClassControlSensor(clsScript.ClsDataTime));
               Sensors.Last().StateError += CheckState;
               Sensors.Last().AlarmError += Alarm;
               Sensors.Last().LstContr = listControllers;
            }


            for (int i = 0; i < con.PumpSysConst.Length; i++)
            {
                PumpSystems.Add(new ClassControlPumpSystem(clsScript.ClsDataTime));
                PumpSystems.Last().StateError += CheckState;
                PumpSystems.Last().AlarmError += Alarm;
                PumpSystems.Last().LstContr = listControllers;
            }
            for (int i = 0; i < con.ConstFreqGenerator.Length; i++)
            {
                GenFreq.Add(new ClassControlGenFrequency(clsScript.ClsDataTime));
                GenFreq.Last().StateError += CheckState;
                GenFreq.Last().AlarmError += Alarm;
                GenFreq.Last().LstContr = listControllers;
            }

            for (int i = 0; i < con.ConstBurner.Length; i++)
            {
                Burners.Add(new ClassControlHydrogenBurner(clsScript.ClsDataTime));
                Burners.Last().StateError += CheckState;
                Burners.Last().AlarmError += Alarm;
                Burners.Last().LstContr = listControllers;
            }

            for (int i = 0; i < con.ConstCham.Length; i++)
            {
                ThermoChambers.Add(new ClassControlTermoChamber(clsScript.ClsDataTime));
                ThermoChambers.Last().StateError += CheckState;
                ThermoChambers.Last().AlarmError += Alarm;
                ThermoChambers.Last().LstContr = listControllers;
            }

            for (int i = 0; i < con.ConstLoader.Length; i++)
            {
                Loaders.Add(new ClassControlLoader(clsScript.ClsDataTime));
                Loaders.Last().StateError += CheckState;
                Loaders.Last().AlarmError += Alarm;
                Loaders.Last().LstContr = listControllers;
            }

            for (int i = 0; i < con.ConstShutters.Length; i++)
            {
                Shutters.Add(new ClassControlShutter(clsScript.ClsDataTime));
                Shutters.Last().StateError += CheckState;
                Shutters.Last().AlarmError += Alarm;
                Shutters.Last().LstContr = listControllers;
            }

            for (int i = 0; i < con.GateConst.Length; i++)
            {
                Gates.Add(new ClassControlGate(clsScript.ClsDataTime));
                Gates.Last().StateError += CheckState;
                Gates.Last().AlarmError += Alarm;
                Gates.Last().LstContr = listControllers;
            }
        }


        public List<ClassBaseDevices> Base
        {
            get
            {
                var lst = new List<ClassBaseDevices>();
                
                lst.AddRange(Flaps);
                lst.AddRange(Rrgs);
                lst.AddRange(Bublers);
                lst.AddRange(Shutters);
                lst.AddRange(Vacumetrs);
                lst.AddRange(Sensors);
                lst.AddRange(PumpSystems);
                lst.AddRange(ThermoChambers);
                lst.AddRange(GenFreq);
                lst.AddRange(Burners);
                lst.AddRange(Loaders);
                lst.AddRange(Gates);
                return lst;
            }
        }
        public List<ClassControlFlap> Flaps=new List<ClassControlFlap>();
        public List<ClassControlShutter> Shutters = new List<ClassControlShutter>();
        public List<ClassControlRRG> Rrgs = new List<ClassControlRRG>();
        public List<ClassControlBubbler> Bublers = new List<ClassControlBubbler>();
        public List<ClassControlVacuumetr> Vacumetrs = new List<ClassControlVacuumetr>();
        public List<ClassControlSensor> Sensors = new List<ClassControlSensor>();

        public List<ClassControlPumpSystem> PumpSystems = new List<ClassControlPumpSystem>();
        public List<ClassControlTermoChamber> ThermoChambers = new List<ClassControlTermoChamber>();
        public List<ClassControlLoader> Loaders = new List<ClassControlLoader>();
        public List<ClassControlGate> Gates = new List<ClassControlGate>();
        public List<ClassControlGenFrequency> GenFreq = new List<ClassControlGenFrequency>();
        public List<ClassControlHydrogenBurner> Burners = new List<ClassControlHydrogenBurner>();


        public void StepScript(XmlClassStep classStep)
        {
            for (int i = 0; i < classStep.Flaps.Length; i++)
                Flaps[i].DataStep(classStep.Flaps[i], classStep.ConditionalScript.StateScripts, classStep.StepParams);

            for (int i = 0; i < classStep.Rrgs.Length; i++)
                Rrgs[i].DataStep(classStep.Rrgs[i], classStep.ConditionalScript.StateScripts, classStep.StepParams);

            for (int i = 0; i < classStep.Bubblers.Length; i++)
                Bublers[i].DataStep(classStep.Bubblers[i], classStep.ConditionalScript.StateScripts, classStep.StepParams);

            for (int i = 0; i < classStep.Shutters.Length; i++)
                Shutters[i].DataStep(classStep.Shutters[i], classStep.ConditionalScript.StateScripts, classStep.StepParams);

            for (int i = 0; i < classStep.Vacuumetrs.Length; i++)
                Vacumetrs[i].DataStep(classStep.Vacuumetrs[i], classStep.ConditionalScript.StateScripts, classStep.StepParams);

            for (int i = 0; i < classStep.Sensors.Length; i++)
                Sensors[i].DataStep(classStep.Sensors[i], classStep.ConditionalScript.StateScripts, classStep.StepParams);

            for (int i = 0; i < classStep.PumpSys.Length; i++)
                PumpSystems[i].DataStep(classStep.PumpSys[i], classStep.ConditionalScript.StateScripts, classStep.StepParams);

            for (int i = 0; i < classStep.Chambers.Length; i++)
                ThermoChambers[i].DataStep(classStep.Chambers[0], classStep.ConditionalScript.StateScripts, classStep.StepParams);

            for (int i = 0; i < classStep.FreqGenerators.Length; i++)
                GenFreq[i].DataStep(classStep.FreqGenerators[i], classStep.ConditionalScript.StateScripts, classStep.StepParams);

            for (int i = 0; i < classStep.Burners.Length; i++)
                Burners[i].DataStep(classStep.Burners[i], classStep.ConditionalScript.StateScripts, classStep.StepParams);

            for (int i = 0; i < classStep.Gates.Length; i++)
                Gates[i].DataStep(classStep.Gates[i], classStep.ConditionalScript.StateScripts, classStep.StepParams);

            for (int i = 0; i < classStep.Loaders.Length; i++)
                Loaders[i].DataStep(classStep.Loaders[i], classStep.ConditionalScript.StateScripts);

        }

        public void CheckState(object a, EventArgs o)
        {

            var par = (ConJumpArgs)o;
            Console.WriteLine($"{par.TimeStep} {par.TextError}");

            foreach (var dev in Base)
            {
                dev.FailSave();
            }
            StateError(this, o);
        }

        public void Alarm(object a, EventArgs o)
        {
            AlarmError(this, o);
        }

    }
}
