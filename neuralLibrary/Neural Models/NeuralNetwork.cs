using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralLibrary.Neural_Models
{
    public class NeuralNetwork
    {
        #region Properties
        public List<Layer> Layers { get; set; }
        public double LearningRate { get; set; }
        public int LayerCout { get { return Layers.Count; } }
        public int Epochs { get; set; }
        #endregion
        #region Constructor
        public NeuralNetwork(double learningRate, int[] layers)
        {
            Epochs = 1000;
            if (layers.Length < 2) return;

            this.LearningRate = learningRate;
            this.Layers = new List<Layer>();

            for (int i = 0; i < layers.Length; i++)
            {
                Layer layer = new Layer(layers[i]);
                this.Layers.Add(layer);

                for (int j = 0; j < layers[i]; j++)
                {
                    layer.Neurons.Add(new Neuron());

                    layer.Neurons.ForEach((mm)
                       =>
                   {
                       if (i == 0)
                       { mm.Bias = 0; }
                       else
                       {
                           for (int d = 0; d < layers[i - 1]; d++)
                           {
                               mm.Synapses.Add(new Synapse());
                           }
                       }
                   });
                }

            }
        }
        #endregion

        #region Methods
        private double Sigmoid(double x) //TODO: add other Afs as well
        {
            return 1 / (1 + Math.Exp(-x));
        }

        private double[] Run(List<double> input)
        {
            if (input.Count != this.Layers[0].NeuronCount)
            {
                return null;
            }
            for (int l = 0; l < Layers.Count; l++)
            {
                Layer layer = Layers[l];

                for (int n = 0; n < layer.Neurons.Count; n++)
                {
                    Neuron neuron = layer.Neurons[n];

                    if (l == 0) //first stage
                    {
                        neuron.TrainingValue = input[n];
                    }
                    else
                    {
                        neuron.TrainingValue = 0;
                        for (int m = 0; m < this.Layers[l - 1].Neurons.Count; m++)
                        {
                            neuron.TrainingValue = neuron.TrainingValue + this.Layers[l - 1].Neurons[n].TrainingValue * neuron.Synapses[m].Weight; //calc x input
                        }
                        neuron.TrainingValue = Sigmoid(neuron.TrainingValue + neuron.Bias); //calc f(x) output

                    }

                }

            }
            Layer last = this.Layers[this.Layers.Count - 1];

            int numOutput = last.Neurons.Count;
            double[] output = new double[numOutput];

            for (int i = 0; i < last.Neurons.Count; i++)
            {
                output[i] = last.Neurons[i].TrainingValue;
            }
            return output;
        }
        private void Feedforward(double[] output)
        {
            for (int i = 0; i < this.Layers[this.Layers.Count - 1].Neurons.Count; i++)
            {
                Neuron neuron = this.Layers[this.Layers.Count - 1].Neurons[i];

                neuron.Delta = neuron.TrainingValue * (1 - neuron.TrainingValue) * (output[i] - neuron.TrainingValue);

                for (int j = this.Layers.Count - 2; j > 2; j--)
                {
                    for (int k = 0; k < this.Layers[j].Neurons.Count; k++)
                    {
                        Neuron n = this.Layers[j].Neurons[k];

                        n.Delta = n.TrainingValue *
                                  (1 - n.TrainingValue) *
                                  this.Layers[j + 1].Neurons[i].Synapses[k].Weight *
                                  this.Layers[j + 1].Neurons[i].Delta;
                    }
                }
            }
        }
        private void Backpropagate()
        {
            for (int i = this.Layers.Count - 1; i > 1; i--)
            {
                for (int j = 0; j < this.Layers[i].Neurons.Count; j++)
                {
                    Neuron n = this.Layers[i].Neurons[j];
                    n.Bias = n.Bias + (this.LearningRate * n.Delta);

                    for (int k = 0; k < n.Synapses.Count; k++)
                        n.Synapses[k].Weight = n.Synapses[k].Weight + (this.LearningRate * this.Layers[i - 1].Neurons[k].TrainingValue * n.Delta);
                }
            }
        }
        private bool Train(List<double> input, List<double> output)
        {
            if ((input.Count != this.Layers[0].Neurons.Count) ||
                (output.Count != this.Layers[this.Layers.Count - 1].Neurons.Count))
            {
                return false;
            }

            Run(input);

            for (int i = 0; i < this.Layers[this.Layers.Count - 1].Neurons.Count; i++)
            {
                Neuron neuron = this.Layers[this.Layers.Count - 1].Neurons[i];

                neuron.Delta = neuron.TrainingValue * (1 - neuron.TrainingValue) * (output[i] - neuron.TrainingValue);

                for (int j = this.Layers.Count - 2; j > 2; j--)
                {
                    for (int k = 0; k < this.Layers[j].Neurons.Count; k++)
                    {
                        Neuron n = this.Layers[j].Neurons[k];

                        n.Delta = n.TrainingValue *
                                  (1 - n.TrainingValue) *
                                  this.Layers[j + 1].Neurons[i].Synapses[k].Weight *
                                  this.Layers[j + 1].Neurons[i].Delta;
                    }
                }
            }
          

            for (int i = this.Layers.Count - 1; i > 1; i--)
            {
                for (int j = 0; j < this.Layers[i].Neurons.Count; j++)
                {
                    Neuron n = this.Layers[i].Neurons[j];
                    n.Bias = n.Bias + (this.LearningRate * n.Delta);

                    for (int k = 0; k < n.Synapses.Count; k++)
                        n.Synapses[k].Weight = n.Synapses[k].Weight + (this.LearningRate * this.Layers[i - 1].Neurons[k].TrainingValue * n.Delta);
                }
            }

            return true;
        }

        public string DisplayWeights(List<List<double>> Weights)
        {
            string weightRep = "";
            for (int i = 0; i < Weights.Count; i++) //2
            {
                for (int j = 0; j < Weights[i].Count; j++) //3
                {
                    if (i != 0)
                    {
                        // find 1st \n
                       int StartingIndex = neuralLibrary.Helpers.I.O.FileOperations.findNthIndexOf("\n", weightRep, j) -1;
                        weightRep.Insert(StartingIndex, " "+Weights[i][j]);// .insert(StartingIndex,);

                    }

                    weightRep += Weights[i][j];
                    weightRep +="\n";
                    //1   4
                    //2   5
                    //3   6
                }
                weightRep.TrimEnd('\n');
            }
            return weightRep;
        }


        public List<List<double>>  RetrieveWeights()
        {
            List<List<double>> Weights = new List<List<double>>();
            for (int i = this.Layers.Count - 1; i > 1; i--)//for each Layer
            {
                for (int j = 0; j < this.Layers[i].Neurons.Count; j++) //for each Neuron
                {
                    //Access the neurons
                     Neuron n = this.Layers[i].Neurons[j];

                    List<double> L = new List<double>();

                    for (int k = 0; k < n.Synapses.Count; k++) //for each Synapse
                    {
                        n.Synapses[k].Weight = n.Synapses[k].Weight + (this.LearningRate * this.Layers[i - 1].Neurons[k].TrainingValue * n.Delta);
                        L.Add(n.Synapses[k].Weight);
                    } //L got filled with Valuable Data

                     //now add L back to its uppeer Jagged List Weights

                    Weights.Add(L);
                    //empty L for memory's sake!
                    L = null;
                }
            }
            return Weights;

        }

        public bool TrainwEpochs(List<double> input, List<double> output, int epochs=1000)
        {
            bool flag = false;
            try
            {
                for (int i = 0; i < epochs; i++)
                {
                    Train(input, output);
                }
                flag = true;
            }
            catch(Exception)
            { }

            return flag;
        }

        #endregion
    }
}
