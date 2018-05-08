using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace neuralLibrary.Helpers
{
    public class TrueRandom
    {
        public double RandomValue { get; set; }
       // public bool IsMin1Max1 { get; set; }

        #region Constructor
        public TrueRandom() //Double Random between 0.0 and 1.0
        {
            using (RNGCryptoServiceProvider p = new RNGCryptoServiceProvider())
            {
                Random myRnd = new Random(p.GetHashCode());
                this.RandomValue = myRnd.NextDouble();
            }
        }
        public TrueRandom(bool flag = true) //Double Random between -1.0 and 1.0
        {
            if (flag == true)
            {
                using (RNGCryptoServiceProvider p = new RNGCryptoServiceProvider())
                {
                    Random myRnd = new Random(p.GetHashCode());
                    this.RandomValue = myRnd.NextDouble() - myRnd.NextDouble();
                }
            }
            //TODO:Implement other Ranges of Randomness
            //if (flag == false)
            //{

            //}
        }
        #endregion
    }
}
