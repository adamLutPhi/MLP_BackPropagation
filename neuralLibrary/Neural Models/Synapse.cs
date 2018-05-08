using neuralLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralLibrary.Neural_Models
{
    public class Synapse
    {
        public double Weight { get; set; }

        public Synapse()
        {
            TrueRandom n = new TrueRandom();
            this.Weight = n.RandomValue; 
        }
    }
}
