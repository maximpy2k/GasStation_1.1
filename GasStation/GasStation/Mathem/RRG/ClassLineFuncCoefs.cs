using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStation.Mathem.RRG
{
    public class ClassLineFuncCoefs
    {
        private readonly double _minX;
        private readonly double _maxX;
        private readonly double _minY;
        private readonly double _maxY;

        public ClassLineFuncCoefs(double minX, double maxX, double minY, double maxY)
        {
            _minX = minX;
            _maxX = maxX;
            _minY = minY;
            _maxY = maxY;


            GetKoefs();
        }

        private double k;

        private double b;
        private  void GetKoefs()
        {
            if ((_maxX-_minX) <= 0)
            {
                b = 0;
                k = 0;
                return;
            }

            k = (_minY - _maxY) / (_minX - _maxX);
            b = _minY - k * _minX;

            if (k > 0)
            {
                Up = true;
                Down = false;
            }

            if (k < 0)
            {
                Up = false;
                Down = true;
            }

        }

        public double GetRaise(double currTime)
        {
            return k*currTime+b;
        }

        public bool Up;

        public bool Down;
    }
}
