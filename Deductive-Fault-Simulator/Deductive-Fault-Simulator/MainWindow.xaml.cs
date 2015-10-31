using Microsoft.Win32;
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

        #endregion
    }
}
