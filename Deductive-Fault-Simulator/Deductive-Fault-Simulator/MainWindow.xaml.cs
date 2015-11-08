using Deductive_Fault_Simulator_BusinessLogic;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Deductive_Fault_Simulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables

        public List<int> inputs;
        public List<int> outputs;
        public List<LogicRecord> logicRecords;
        public List<IntermediateGateFault> faultList;

        public List<IntermediateGateFault> outputFinalList;

        public Dictionary<int, int> outputMasterDictionary;
        public List<string> outputFormula;

        Dictionary<int, int> inputSignalsDict;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Helper Methods

        private void ReadLogicRecords()
        {
            logicRecords = FileReader.GetLogicRecords(FileNameTextBox.Text);

            inputs = FileReader.GetInputs(FileNameTextBox.Text);
            outputs = FileReader.GetOutputs(FileNameTextBox.Text);
        }

        private void CreateFaultList()
        {
            faultList = new List<IntermediateGateFault>();

            foreach (int input in inputs)
            {
                IntermediateGateFault fault_0 = new IntermediateGateFault();
                IntermediateGateFault fault_1 = new IntermediateGateFault();
                //Stuck at 0
                fault_0.net_no = input;
                fault_0.stuck_at_fault = 0;

                //Stuck at 1
                fault_1.net_no = input;
                fault_1.stuck_at_fault = 1;

                faultList.Add(fault_0);
                faultList.Add(fault_1);
            }

            foreach (LogicRecord logicRecord in logicRecords)
            {
                int signalNumber = logicRecord._output;

                if ((faultList.Find(obj => obj.net_no == signalNumber) == null))
                {
                    IntermediateGateFault fault_0 = new IntermediateGateFault();
                    IntermediateGateFault fault_1 = new IntermediateGateFault();
                    //Stuck at 0
                    fault_0.net_no = signalNumber;
                    fault_0.stuck_at_fault = 0;

                    //Stuck at 1
                    fault_1.net_no = signalNumber;
                    fault_1.stuck_at_fault = 1;

                    faultList.Add(fault_0);
                    faultList.Add(fault_1);

                }

            }
        }

        private void CreateInputStringDictionary()
        {
            inputSignalsDict = new Dictionary<int, int>();
            string inputString = InputStringTextBox.Text;

            for (int i = 0; i < inputString.Length; i++)
            {
                inputSignalsDict.Add(inputs.ElementAt(i), int.Parse(inputString.ElementAt(i).ToString()));
            }

        }

        private int GetSimplExp(int output)
        {
            LogicRecord logicRecord = logicRecords.Find(obj => obj._output == output);
            int outputValue = -1;

            bool success = inputSignalsDict.TryGetValue(output, out outputValue);
            if (success)
                return outputValue;

            switch (logicRecord._operator)
            {
                case Operators.INV:
                    return (GetSimplExp(logicRecord._inputs.ElementAt(0)) == 1) ? 0 : 1;
                case Operators.BUF:
                    return (GetSimplExp(logicRecord._inputs.ElementAt(0)));

                case Operators.AND:
                    return GetSimplExp(logicRecord._inputs.ElementAt(0)) & GetSimplExp(logicRecord._inputs.ElementAt(1));

                case Operators.OR:
                    return GetSimplExp(logicRecord._inputs.ElementAt(0)) | GetSimplExp(logicRecord._inputs.ElementAt(1));

                case Operators.NAND:
                    return ((GetSimplExp(logicRecord._inputs.ElementAt(0)) & GetSimplExp(logicRecord._inputs.ElementAt(1))) == 1) ? 0 : 1;

                case Operators.NOR:
                    return ((GetSimplExp(logicRecord._inputs.ElementAt(0)) | GetSimplExp(logicRecord._inputs.ElementAt(1))) == 1) ? 0 : 1;

                default:
                    return -1;
            }

        }


        private bool CompareWithMasterDictionary(Dictionary<int, int> compareList)
        {

            foreach (KeyValuePair<int, int> kvp in compareList)
            {
                int originVal;
                outputMasterDictionary.TryGetValue(kvp.Key, out originVal);
                if (originVal != kvp.Value) return false;
            }

            return true;
        }

        private void WriteToOutputDisplay()
        {
            string outputText = "Net_No" + "\t" + "Stuck-at-value" + "\n";

            foreach (IntermediateGateFault fault in outputFinalList)
            {
                string faultLine = fault.net_no + "\t" + fault.stuck_at_fault;
                outputText = outputText + faultLine + "\n";
            }

            FileOutputDisplayBox.Text = outputText;
        }

        #endregion

        #region Public Methods

        #endregion

        #region Event Handlers

        /// <summary>
        /// Event Handler for when browse for input 
        /// file is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            // Open a new File Dialog
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog().Value)
            {
                // Get the filePath, write it to text box
                string filePath = ofd.FileName;
                FileNameTextBox.Text = filePath;

                // Read the filecontents and display on the
                // file contents display box
                string fileContents = File.ReadAllText(filePath);
                FileContentsDisplayBox.Text = fileContents;

                ReadLogicRecords();
                CreateFaultList();

                string faultFilePath = Path.GetFileNameWithoutExtension(filePath) + "_IntermediateFaultList.txt";
                IntermediateFaultFileDisplay.Text = FileReader.WriteGateFaultsToFile(faultFilePath, faultList);
            }
        }

        /// <summary>
        /// Event Handler for when the RUN button is clicked to
        /// run the input string on the logic ciruit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            CreateInputStringDictionary();

            outputMasterDictionary = new Dictionary<int, int>();

            foreach (int output in outputs)
            {
                outputMasterDictionary.Add(output, GetSimplExp(output));
            }

            outputFinalList = new List<IntermediateGateFault>();
            foreach (IntermediateGateFault fault in faultList)
            {
                inputSignalsDict = null;
                CreateInputStringDictionary();

                if (inputSignalsDict.ContainsKey(fault.net_no))
                {
                    inputSignalsDict.Remove(fault.net_no);
                }

                inputSignalsDict.Add(fault.net_no, fault.stuck_at_fault);

                Dictionary<int, int> intermediateOutputs = new Dictionary<int, int>();
                foreach (int output in outputs)
                {
                    intermediateOutputs.Add(output, GetSimplExp(output));
                }

                bool success = CompareWithMasterDictionary(intermediateOutputs);
                if (success == false)
                {
                    IntermediateGateFault faultWrite = new IntermediateGateFault();
                    faultWrite.net_no = fault.net_no;
                    faultWrite.stuck_at_fault = fault.stuck_at_fault;
                    outputFinalList.Add(faultWrite);

                }

            }

            outputFinalList.Sort((a, b) => a.net_no.CompareTo(b.net_no));
            WriteToOutputDisplay();
        }
    }
    #endregion
}

