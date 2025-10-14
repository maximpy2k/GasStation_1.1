using GasStation.xml.Constant.XmlConst.Elements;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStation
{
    public class ClassSerialParams
    {
        XmlClassProgrammConst _xmlComSettings;
        public ClassSerialParams(XmlClassProgrammConst xmlComSettings)
        {
            _xmlComSettings = xmlComSettings;
            SP = new SerialPort(ComPort, 115200, Parity.None, 8, StopBits.One) { Handshake = Handshake.None, NewLine = "\r", ReadTimeout = 1000 };
        }

        //private SerialPort sP;

        public SerialPort SP;

        public String ComPort
        {
            get
            {
                return $"COM{_xmlComSettings.Port}";
            }
        }

        public void Connect()
        {
            if (SP != null)
            {
                try
                {
                    SP.Open();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
                
            }
        }

        public void SendCMDString(String value)
        {
            SP.Write(value);
        }

        public String ReadCMDString()
        {
            return SP.ReadLine();
        }

        public void Disconnect()
        {
            if (SP != null)
                SP.Close();
        }
    }
}
