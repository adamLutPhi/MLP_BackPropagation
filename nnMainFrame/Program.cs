using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using neuralLibrary.Helpers.I.O;
using neuralLibrary.Neural_Models;
using System.Windows.Forms;
using System.Threading;

namespace nnMainFrame
{
    class Program
    {
        public static double LearningRate = 0.01;
        public static string xFilename = "";
        public static string yFilename = "";

        public static string UseFileDlg()
        {
            string directoryPath = "";
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Multiselect = false;
            openFileDialog1.Title = "Choose your Input text File";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(openFileDialog1.FileName))
                {
                    //
                    try
                    {
                        if ((myStream = openFileDialog1.OpenFile()) != null)
                        {
                            using (myStream)
                            {
                                // Insert code to read the stream here.
                                directoryPath = openFileDialog1.FileName;
                                //   string directoryPath = Path.GetDirectoryName(filePath);

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    }
                    //
                }
            }
            return directoryPath;
        }

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Eng. Ahmad Lutfi -- All Rights Reserved " + DateTime.Now.Year.ToString());
            Console.WriteLine("Welcome to our Neural Network Test Drive...");
            //  Console.WriteLine("How many Layers");

            //  int num1 =  Convert.ToInt16(Console.ReadLine());
            Console.WriteLine("please enter number of neurons that you like, with a space delimiter ' ' please!\n i.e.  2 5 1 ");
            string s = Console.ReadLine();

            var l = s.Split(' ').ToList<string>(); //get list of strings
            var neosLayer = neuralLibrary.Helpers.I.O.FileOperations.parselist2Int(l); //list : { 3 , 4 , 5}
            NeuralNetwork n = new NeuralNetwork(LearningRate, neosLayer.ToArray());   //2, 2, 1
            string key = "";
            List<double> Xvals = new List<double>();
            List<double> Yvals = new List<double>();
            //  do
            // {
            Console.WriteLine("would you like to browse for your inputs file? Y/N");
            if (Console.ReadLine().ToUpper() == "Y")
            {
                ///
                //OpenFileDialog fileDialog = new OpenFileDialog();
                //OpenFileDialog openFileDialog1 = new OpenFileDialog();


                xFilename = UseFileDlg();
                Xvals = FileOperations.parseList2Double(FileOperations.ReadfromFile(xFilename));
                Console.WriteLine("Inputs' completely Loaded!"); waitaSec();
                Console.WriteLine("we also need the output file, you can choose it now.");
                yFilename = UseFileDlg();
                Yvals = FileOperations.parseList2Double(FileOperations.ReadfromFile(yFilename));

                Console.WriteLine("Output was loaded Successfully!\nWARNING: it is obvious that user is free to choose the output of his desire, hence," +
                    "for this reason, Normalization would be skipped to protect the integrity of the desired Outputs");
                waitaSec(); waitaSec(); waitaSec();

            }

            if (!string.IsNullOrEmpty(xFilename) && !string.IsNullOrEmpty(yFilename)) //Only start the computation if both X & Y values were loaded successfully!
            {
                if (neuralLibrary.Helpers.Data_Operations.NormalizeData.NormalizeInput(Xvals)) // If it got Normalized
                {
                    Console.WriteLine("Great!");
                    waitaSec();
                    Console.WriteLine("Training your Neural Network will now Commence...");

                    if (n.TrainwEpochs(Xvals, Yvals))
                    {
                        //write back the results to its each corresponding file 

                        string weightsString = n.DisplayWeights(n.RetrieveWeights());

                        //
                        Console.WriteLine("Weights after Training for " + n.Epochs + " Epochs are:" + "\n" + weightsString);
                        waitaSec();
                        Console.WriteLine("Please Choose the desired Path to store the generated Weights Matrix");
                        waitaSec();
                        storetoFile(weightsString);
                    }
                    else
                    {
                        //something bad happened
                        Console.WriteLine("Unexpected error Occured while training the dataset\nplease try again later!");
                    }


                }
            }

            // } while (key.ToUpper() != "N");
            else
            {
                Console.WriteLine("Operation HALTED\nPress Any Key to Exit...");
                Console.ReadKey();
            }


        }
        #region Misc.

        public static bool  SaveStream(string filename, string content)
        {
            bool flag = false;
            using (System.IO.StreamWriter file =
      new System.IO.StreamWriter(filename, true))
            {
                file.Write(content);// WriteLine(content);
                flag = true;
                
            }
            return flag;
        }
        public static bool storetoFile(string content) //what do you wanna save
        {
            bool flag = false;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
          //  saveFileDialog1.InitialDirectory = @"C:\";
            saveFileDialog1.Title = "Save the Weights Matrix";
            saveFileDialog1.CheckFileExists = true;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "txt";
            saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (SaveStream(saveFileDialog1.FileName, content))
                {
                    flag = true;
                }
             
            }
            return flag;
        }


        public static void waitaSec()
        {
            ThreadPool.QueueUserWorkItem((state) =>
            {
                Thread.Sleep(1000);

                // do your work here
                // CAUTION: use Invoke where necessary
            });
        }
        public string readfromStream(Stream s)
        {
            StreamReader reader = new StreamReader(s);
            string text = reader.ReadToEnd();
            return text;
        }
        #endregion
    }
}
