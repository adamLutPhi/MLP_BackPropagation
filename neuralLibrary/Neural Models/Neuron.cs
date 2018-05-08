using neuralLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralLibrary.Neural_Models
{
   public class Neuron
    {
        #region Properties
        public List<Synapse> Synapses { get; set; }
        public int synapseCount { get { return Synapses.Count; } }

        public double TrainingValue { get; set; }

        //for the Recalibration Process
        public double Bias { get; set; }
        public double Delta { get; set; }

        #endregion

        #region Constructor
        public Neuron()
        {
            this.Synapses = new List<Synapse>();

            //Initialize Synaptic Weights Randomly, on [0,1.0]
            TrueRandom n = new TrueRandom();
            this.Bias = n.RandomValue;

        }


        #endregion

    }
}
