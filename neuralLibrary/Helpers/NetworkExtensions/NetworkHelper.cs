using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using neuralLibrary.Neural_Models;

namespace neuralLibrary.Helpers.NetworkExtensions
{
    public class NetworkHelper
    {
        public static void ConverttoTreeView(TreeView t, Neural_Models.NeuralNetwork nn)
        {
            //reset Previous Drawing
            t.Nodes.Clear();

            TreeNode root = new TreeNode("NeuralNetwork");

            nn.Layers.ForEach((layer) =>
            {

                //Define Layer Node

                TreeNode Layernode = new TreeNode("Layer");

                layer.Neurons.ForEach((neuron) =>
                {
                    //Define Neuron Node
                    TreeNode nnode = new TreeNode("Neuron");
                    nnode.Nodes.Add("Bias: "+ neuron.Bias.ToString());
                    nnode.Nodes.Add("Delta: " + neuron.Delta.ToString());
                    nnode.Nodes.Add("Traning Value: "+neuron.TrainingValue.ToString());

                    //but before Augmenting the neuron
                    //define neuron's subobjects now:

                    //FOREACH Neuron, Define Synapses 

                    neuron.Synapses.ForEach((synapse) =>
                    {
                        //FOREACH Synapse, define the weight!
                        
                        //define a synapse on the treeView
                        TreeNode synnode = new TreeNode("Synapse");
                        
                        //Add weight on the synapse
                        synnode.Nodes.Add("Weight: "+synapse.Weight.ToString());
                        
                        //Finally, Augment the synapse object to the tree
                        synnode.Nodes.Add(synnode);
                    });
                    
                    //Finally, Augment Neuron to the Layer !
                    Layernode.Nodes.Add(nnode);
                });
               
                //Finally, Augment the Layer to the root !
                root.Nodes.Add(Layernode);
            });

            //Finally, Augment the root to the Treeview!

            t.Nodes.Add(root);
        }

        public static void ToPictureBox(PictureBox p,Neural_Models.NeuralNetwork nn, int X, int Y)
        {
            int neuronWidth = 30;
            int neuronDistance = 50;
            int layerDistance = 50;
            int fontSize = 8;

            Bitmap b = new Bitmap(p.Width, p.Height);
            Graphics g = Graphics.FromImage(b);

            g.FillRectangle(Brushes.White, g.ClipBounds);

            int y = Y;

            for (int l = 0; l < nn.Layers.Count; l++)
            {
                Layer layer = nn.Layers[l];

                int x = X - (neuronDistance * (layer.Neurons.Count / 2));

                for (int n = 0; n < layer.Neurons.Count; n++)
                {
                    Neuron neuron = layer.Neurons[n];
                    // TODO: optionally draw Synapsess between neurons
                    //for (int d = 0; d < neuron.Synapses.Count; d++)
                    //{
                       
                    //};

                    g.FillEllipse(Brushes.WhiteSmoke, x, y, neuronWidth, neuronWidth);
                    g.DrawEllipse(Pens.Gray, x, y, neuronWidth, neuronWidth);
                    g.DrawString(neuron.TrainingValue.ToString("0.00"), new Font("Arial", fontSize), Brushes.Black, x + 2, y + (neuronWidth / 2) - 5);

                    x += neuronDistance;
                };

                y += layerDistance;
            };

            p.Image = b;
        }
    }
}
