using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralLibrary.Neural_Models
{
    public class Layer
    {
        #region Properties
        public List<Neuron> Neurons { get; set; }
        public int NeuronCount { get { return Neurons.Count; } }

        #endregion
        public Layer(int numNeurons)
        {
            Neurons = new List<Neuron>(numNeurons);
        }
    }
}
