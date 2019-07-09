using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SolverSeged
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NeuralNetwork nn;
        private List<double[]> learningInputs;
        private List<double[]> learningOutputs;

        private SolidColorBrush b_white;
        private SolidColorBrush b_gray;

        private List<Label> labels;

        public MainWindow()
        {
            InitializeComponent();

            labels = new List<Label>();
            labels.Add(this.lbl_AIS);
            labels.Add(this.lbl_BEJING);
            labels.Add(this.lbl_BLOCKWORLDS);
            labels.Add(this.lbl_BMC);
            labels.Add(this.lbl_BMS);
            labels.Add(this.lbl_CBS);
            labels.Add(this.lbl_GCP);
            labels.Add(this.lbl_LOGISTICS);
            labels.Add(this.lbl_QG);
            labels.Add(this.lbl_RND3SAT);
            labels.Add(this.lbl_SWGCP);
            labels.Add(this.lbl_AIM);
            labels.Add(this.lbl_BF);
            labels.Add(this.lbl_DUBOIS);
            labels.Add(this.lbl_HANOI);
            labels.Add(this.lbl_II);
            labels.Add(this.lbl_JNH);
            labels.Add(this.lbl_LRAN);
            labels.Add(this.lbl_PARITY);
            labels.Add(this.lbl_PHOLE);
            labels.Add(this.lbl_SSA);

            b_white = new SolidColorBrush(Colors.White);
            b_gray = new SolidColorBrush(Colors.Gray);
        }

        /// <summary>
        /// Lehetővé teszi az adafájl csatolássát
        /// Ha sikeres volt a csatolás, úgy a neurális háló véleménye meg is jelenik a felületen
        /// </summary>
        private void btn_browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                this.tb_path.Text = openFileDialog.SafeFileName;
                ShowResults(openFileDialog.FileName);
            }
            else
            {
                MessageBox.Show("Ismeretlen hiba!");
            }
        }

        private void ShowResults(string fileName)
        {
            float limit = 0.8f; //hardcode.
            double[] input = InputByTxt(fileName);

            if (input != null && input.Length == 48)
            {
                double[] results = nn.FeedForward(input);
                int maxi = 0;

                for (int i = 0; i < results.Length; i++)
                {
                    if (results[i] >= limit) labels[i].Foreground = b_white;
                    else labels[i].Foreground = b_gray;

                    if (results[i] > results[maxi]) maxi = i;
                }

                MessageBox.Show(labels[maxi].Content.ToString());
            }
            else
            {
                MessageBox.Show("Hibás a file felépítése");
            }
        }

        private double[] InputByTxt(string fileName)
        {
            StreamReader sr;

            try
            {
                sr = new StreamReader(fileName);
            }
            catch (Exception)
            {
                MessageBox.Show("Ismeretlen hiba");
                return null;
            }

            double[] temp = new double[48];

            string[] lineSplit = sr.ReadLine().Split(':')[0].Split(';');
            for (int i = 0; i < lineSplit.Length; i++)
            {
                try
                {
                    temp[i] = Convert.ToDouble(lineSplit[i]);
                }
                catch (Exception)
                {
                    MessageBox.Show("Hibás filefelépítés!");
                    return null;
                }
            }

            sr.Close();
            return temp;
        }

        private void CreateNetworkByTxt(string fileName)
        {
            StreamReader sr;
            try
            {
                sr = new StreamReader(fileName);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Nincs ilyen file!");
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("Ismeretlen hiba");
                return;
            }

            string[] atm = sr.ReadLine().Split(':'); //0-> Layers | 1 -> Activation Function

            nn = new NeuralNetwork(getLayersBuild(atm), getActi(atm));

            List<MLPLayer> atmLayers = new List<MLPLayer>();
            while (!sr.EndOfStream)
            {
                string[] atm2 = sr.ReadLine().Split(':');
                int atmNumberOfInputs = Convert.ToInt32(atm2[0].ToString());
                int atmNumberOfOutputs = Convert.ToInt32(atm2[1].ToString());

                MLPLayer atmLayer = new MLPLayer();
                atmLayer.NumberOfInput = atmNumberOfInputs;
                atmLayer.NumberOfOutput = atmNumberOfOutputs;
                atmLayer.ActivationStrategy = nn.Activation;

                double[] atmOutputs = new double[atmNumberOfOutputs];
                atm = sr.ReadLine().Split(':');
                for (int i = 0; i < atmOutputs.Length; i++)
                {
                    atmOutputs[i] = Convert.ToDouble(atm[i].ToString());
                }
                atmLayer.Output = atmOutputs;


                double[] atmInputs = new double[atmNumberOfInputs];
                atm = sr.ReadLine().Split(':');
                for (int i = 0; i < atmInputs.Length; i++)
                {
                    atmInputs[i] = Convert.ToDouble(atm[i].ToString());
                }
                atmLayer.Input = atmInputs;

                double[,] atmWeights = new double[atmNumberOfOutputs, atmNumberOfInputs];

                for (int i = 0; i < atmNumberOfOutputs; i++)
                {
                    atm = sr.ReadLine().Split(':');
                    for (int k = 0; k < atmNumberOfInputs; k++)
                    {
                        atmWeights[i, k] = Convert.ToDouble(atm[k].ToString());
                    }
                }
                atmLayer.Weights = atmWeights;

                //double[,] atmWeightsDelta = new double[atmNumberOfOutputs, atmNumberOfInputs];

                //for (int i = 0; i < atmNumberOfOutputs; i++)
                //{
                //    atm = sr.ReadLine().Split(':');
                //    for (int k = 0; k < atmNumberOfInputs; k++)
                //    {
                //        atmWeightsDelta[i, k] = Convert.ToDouble(atm[k].ToString());
                //    }
                //}
                //atmLayer.WeightsDelta = atmWeightsDelta;

                //double[] atmgamma = new double[atmNumberOfOutputs];
                //atm = sr.ReadLine().Split(':');
                //for (int i = 0; i < atmOutputs.Length; i++)
                //{
                //    atmgamma[i] = Convert.ToDouble(atm[i].ToString());
                //}
                //atmLayer.Gamma = atmgamma;

                //double[] atmerror = new double[atmNumberOfOutputs];
                //atm = sr.ReadLine().Split(':');
                //for (int i = 0; i < atmOutputs.Length; i++)
                //{
                //    atmerror[i] = Convert.ToDouble(atm[i].ToString());
                //}
                //atmLayer.Error = atmerror;

                atmLayers.Add(atmLayer);
            }

            nn.Layers = atmLayers.ToArray();


            sr.Close();
            //MessageBox.Show("Sikeres");
            //MessageBox.Show(nn.Activation.ToString());
        }

        private int[] getLayersBuild(string[] line)
        {
            int[] layers = new int[line.Length - 1];
            for (int i = 0; i < line.Length - 1; i++)
            {
                layers[i] = Convert.ToInt32(line[i]);
            }

            return layers;
        }

        private ActivationStrategy getActi(string[] line)
        {
            ActivationStrategy acti;
            try
            {
                if (line[line.Length - 1] == "Sigmoid") acti = new Sigmoid();
                else if (line[line.Length - 1] == "TanH") acti = new TanH();
                else if (line[line.Length - 1] == "ReLU") acti = new ReLU();
                else if (line[line.Length - 1] == "LReLU") acti = new LReLU();
                else
                {
                    MessageBox.Show("Nem támogatott aktivációs függvény!");
                    return null;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Hibás filefelépítés");
                return null;
            }

            return acti;
        }

        private void menu_setNetwork_Click(object sender, RoutedEventArgs e)
        {
            nn = null;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
                CreateNetworkByTxt(openFileDialog.FileName);

            if (nn != null) MessageBox.Show("Háló sikeresen betöltve!");
        }

        private void menu_teachNetwork_Click(object sender, RoutedEventArgs e)
        {
            LoadLearningDatas();

            BackgroundWorker worker = new BackgroundWorker();
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DoWork; ;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerAsync();
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.pbProcessing.Value = e.ProgressPercentage;
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            worker.ReportProgress(0, "Tanulás ...");

            for (int i = 0; i < 10000; i++)
            {
                if (i % 2 == 0)
                {
                    //NetworkProcessing.SaveNetToFile(net, net.acti + i + ".txt");

                }
                worker.ReportProgress(i / 100, "%");
                //NetworkProcessing.Learning(net, "NNInput.txt");
            }
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.pbProcessing.Value = 0;
        }

        /// <summary>
        /// Betölti a tanuláshoz szükséges adatokat, be illetve kimenetet
        /// </summary>
        private void LoadLearningDatas()
        {
            learningInputs = new List<double[]>();
            learningOutputs = new List<double[]>();

            StreamReader sr = null;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
                sr = new StreamReader(openFileDialog.FileName);

            if (sr != null)
            {
                while (!sr.EndOfStream)
                {
                    string[] atm = sr.ReadLine().Split(':');
                    double[] atmInput = new double[atm[0].Split(';').Length];
                    double[] atmOutput = new double[atm[1].Split(';').Length];

                    string[] atmInputLine = atm[0].Split(';');
                    for (int i = 0; i < atmInput.Length; i++)
                    {
                        atmInput[i] = Convert.ToDouble(atmInputLine[i]);
                    }

                    string[] atmOutputLine = atm[1].Split(';');
                    for (int i = 0; i < atmOutput.Length; i++)
                    {
                        atmOutput[i] = Convert.ToDouble(atmOutputLine[i]);
                    }

                    learningInputs.Add(atmInput);
                    learningOutputs.Add(atmOutput);
                }
                sr.Close();
            }
            else
            {
                MessageBox.Show("Ismeretlen hiba!");
            }
        }

    }
}
