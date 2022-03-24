using System;
using System.Collections.Generic;

namespace GestureChartSample
{
    public class DataCreator
    {
        static Random rnd = new Random();
        static string[] coef = new string[]
        {
          "AMTMNQQXUYGA",
          "CVQKGHQTPHTE",
          "FIRCDERRPVLD",
          "GIIETPIQRRUL",
          "GLXOESFTTPSV",
          "GXQSNSKEECTX",
        };

        public static List<DataPoint> Create(int ptsCount)
        {
            var result = new List<DataPoint>();

            double[] x = new double[ptsCount];
            double[] y = new double[ptsCount];
            Random rnd = new Random();
            double[] c = StringToCoeff(coef[rnd.Next(coef.Length)]);

            for (int i = 1; i < ptsCount; i++)
            {
                DataPoint pt = Iterate(x[i - 1], y[i - 1], c);
                result.Add(pt);
                x[i] = pt.XVals; y[i] = pt.YVals;
            }

            return result;
        }

        static DataPoint Iterate(double x, double y, double[] c)
        {
            double x1 = c[0] + c[1] * x + c[2] * x * x + c[3] * x * y + c[4] * y + c[5] * y * y;
            double y1 = c[6] + c[7] * x + c[8] * x * x + c[9] * x * y + c[10] * y + c[11] * y * y;

            return new DataPoint() { XVals = x1, YVals = y1 };
        }

        static double[] StringToCoeff(string s)
        {
            int len = s.Length;
            double[] c = new double[len];

            for (int i = 0; i < len; i++)
            {
                c[i] = -1.2 + 0.1 * (s[i] - 'A');
            }

            return c;
        }
    }
}
