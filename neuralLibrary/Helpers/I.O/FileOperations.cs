using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace neuralLibrary.Helpers.I.O
{
    public static class FileOperations
    {
            public static int findNthIndexOf(this string target, string value, int n)
            {
                Match m = Regex.Match(target, "((" + Regex.Escape(value) + ").*?){" + n + "}");

                if (m.Success)
                    return m.Groups[2].Captures[n - 1].Index;
                else
                    return -1;
            }
       
        public static int ReadNumericdata(Stream s)
        { // edited to improve handling of multiple spaces etc
            int b;
            // skip any preceeding
            while ((b = s.ReadByte()) >= 0 && (b < '0' || b > '9')) { }
            if (b < 0) throw new EndOfStreamException();

            int result = b - '0';
            while ((b = s.ReadByte()) >= '0' && b <= '9')
            {
                result = result * 10 + (b - '0');
            }
            return result;
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static List<string> ReadfromFile(string filename)
        {
            string[] splittedText = null;
            if (!string.IsNullOrEmpty(filename))
            {
                string text = System.IO.File.ReadAllText(filename);

               splittedText = text.Split(' ');
            }
            if (splittedText == null)
            {
                return new List<string>();
            }
            else
            {
                return splittedText.ToList<string>();
            }
            
        }

        private static double squareDiff(double average, double val)
        {
            double sqDiff = 0.0;
            sqDiff = (val - average) * (val - average);
            return sqDiff; 
        }

        public static List<int> parselist2Int(List<string> l)
        {
            List<int> newL = new List<int>();

            for (int i = 0; i < l.Count; i++)

            {
                    newL.Add(Int16.Parse(l[i]));
            }

            return newL;
        }
        public static List<double> parseList2Double(List<string> l)
        {
           
            List<double> newL = new List<double>();
            ///
            if (l.Count == 0)
            { return newL; }
            int count = 0;
            double sum = 0.0;
            double avg = 0.0;
            double dummy = 0.0;

            for (int i = 0; i < l.Count; i++)

            {
                if (string.IsNullOrEmpty(l[i]))
                {
                    dummy = Double.Parse(l[i]);
                    sum += dummy; count++;
                    avg = sum / count;

                    newL.Add(Double.Parse(l[i]));
                }
                else
                {
                    //Compensate with the average value [Optional]
                    newL.Add(avg);
                }

            }

            return newL;

        }
    }
}
