using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
        //Feladatok:
        //Adatbeolvasás a hálóra tekintve jó legyen

        //Másik program: Tanítsa be a neurális hálót, tegye lementhetővé

        //Opciók: A határ legyen csúszka?
        
        private NeuralNetwork nn;

        private SolidColorBrush b_white = new SolidColorBrush(Colors.White);
        private SolidColorBrush b_gray = new SolidColorBrush(Colors.Gray);

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
        }

        /// <summary>
        /// Lehetővé teszi az adafájl csatolássát
        /// Ha sikeres volt a csatolás, úgy a neurális háló véleménye meg is jelenik a felületen
        /// </summary>
        private void btn_browse_Click(object sender, RoutedEventArgs e)
        {
            //File kiválasztása
            string fileName = ""; //ahol van a feldolgozandó file
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                fileName = openFileDialog.FileName;
                this.tb_path.Text = openFileDialog.SafeFileName;
            }

            //Eredmények meghatározása
            float hatar = 0.8f;
            double[] input = InputByTxt(fileName);

            if (input != null && input.Length == 48)
            {
                double[] eredmenyek = nn.FeedForward(input);

                //Felület frissítése az eredmények alapján
                for (int i = 0; i < eredmenyek.Length; i++)
                {
                    if (eredmenyek[i] >= hatar) labels[i].Foreground = b_white;
                    else labels[i].Foreground = b_gray;
                }
            }
            else
            {
                MessageBox.Show("Hibás a file felépítése");
            }

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

            //első sor alapján a háló létrehozása
            string[] atm = sr.ReadLine().Split(':'); //0-> Rétegek | 1 -> Acti

            ActivationStrategy acti;
            try
            {
                if (atm[atm.Length-1] == "Sigmoid") acti = new Sigmoid();
                else if (atm[atm.Length - 1] == "TanH") acti = new TanH();
                else if (atm[atm.Length - 1] == "ReLU") acti = new ReLU();
                else if (atm[atm.Length - 1] == "LReLU") acti = new LReLU();
                else
                {
                    MessageBox.Show("Nem támogatott aktivációs függvény!");
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Hibás filefelépítés");
                return;
            }

            int[] layers = new int[atm.Length-1];
            for (int i = 0; i < atm.Length-1; i++)
            {
                layers[i] = Convert.ToInt32(atm[i]);
            }

            nn = new NeuralNetwork(layers, acti);

            //Minden réteg adatainak beolvasása
            List<MLPLayer> atmLayers = new List<MLPLayer>();
            while (!sr.EndOfStream)
            {
                string[] atm2 = sr.ReadLine().Split(':');
                int atmNumberOfInputs = Convert.ToInt32(atm2[0].ToString());
                int atmNumberOfOutputs = Convert.ToInt32(atm2[1].ToString());

                MLPLayer atmLayer = new MLPLayer();
                atmLayer.NumberOfInput = atmNumberOfInputs;
                atmLayer.NumberOfOutput = atmNumberOfOutputs;
                atmLayer.ActivationStrategy = acti;

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

                double[,] atmWeightsDelta = new double[atmNumberOfOutputs, atmNumberOfInputs];

                for (int i = 0; i < atmNumberOfOutputs; i++)
                {
                    atm = sr.ReadLine().Split(':');
                    for (int k = 0; k < atmNumberOfInputs; k++)
                    {
                        atmWeightsDelta[i, k] = Convert.ToDouble(atm[k].ToString());
                    }
                }
                atmLayer.WeightsDelta = atmWeightsDelta;

                double[] atmgamma = new double[atmNumberOfOutputs];
                atm = sr.ReadLine().Split(':');
                for (int i = 0; i < atmOutputs.Length; i++)
                {
                    atmgamma[i] = Convert.ToDouble(atm[i].ToString());
                }
                atmLayer.Gamma = atmgamma;

                double[] atmerror = new double[atmNumberOfOutputs];
                atm = sr.ReadLine().Split(':');
                for (int i = 0; i < atmOutputs.Length; i++)
                {
                    atmerror[i] = Convert.ToDouble(atm[i].ToString());
                }
                atmLayer.Error = atmerror;

                atmLayers.Add(atmLayer);
            }
            
            nn.Layers = atmLayers.ToArray();

            //vége
            if (sr != null)
            {
                sr.Close();
                MessageBox.Show("Sikeres");
                MessageBox.Show(nn.Activation.ToString());
            }
        }

        private double[] InputByTxt(string fileName)
        {
            StreamReader sr;

            try
            {
                sr = new StreamReader(fileName);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Nincs ilyen file!");
                return null;
            }
            catch (Exception)
            {
                MessageBox.Show("Ismeretlen hiba");
                return null;
            }

            double[] temp = new double[48];

            try
            {
                string[] lineSplit = sr.ReadLine().Split(':')[0].Split(';');
                for (int i = 0; i < lineSplit.Length; i++)
                {
                    temp[i] = Convert.ToDouble(lineSplit[i]);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Hibás filefelépítés!");
                return null;
            }

            sr.Close();
            return temp;
        }

        private void menu_setNetwork_Click(object sender, RoutedEventArgs e)
        {
            //Mehetne a progressbarba

            //File kiválasztása
            string fileName = ""; //ahol van a neurális háló
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
                fileName = openFileDialog.FileName;

            //File alapján a háló létrehozása
            CreateNetworkByTxt(fileName);

            if (nn != null) MessageBox.Show("Háló sikeresen betöltve!");
        }
    }
}
