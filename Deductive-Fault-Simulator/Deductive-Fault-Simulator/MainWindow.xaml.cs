using Deductive_Fault_Simulator_BusinessLogic;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
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
        
            if(ofd.ShowDialog().Value)
            {
                // Get the filePath, write it to text box
                string filePath = ofd.FileName;
                FileNameTextBox.Text = filePath;

                // Read the filecontents and display on the
                // file contents display box
                string fileContents = File.ReadAllText(filePath);
                FileContentsDisplayBox.Text = fileContents;
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

        }

        #endregion

    }
}
