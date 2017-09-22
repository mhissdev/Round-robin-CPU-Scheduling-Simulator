using System;
using System.Windows;

namespace ProcessorSchedulingRR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Round robin scheduler object
        private RoundRobin roundRobin;

        /***************************************************************************
        * Constructor
        ***************************************************************************/
        public MainWindow()
        {
            InitializeComponent();

            // Set default values for UI components
            TextBoxArrival.Text = "0";
            TextBoxBurst.Text = "0";
            TextBoxQuantum.Text = "5";

            // Create new round robin object
            roundRobin = new RoundRobin(TextBoxOutput);

            // Set Welcome message
            TextBoxOutput.AppendText("Please submit processes to be simulated...\n");
        }


        /***************************************************************************
        * Handles click event for add process button
        ***************************************************************************/
        private void ButtonAddProcess_Click(object sender, RoutedEventArgs e)
        {
            // Check for valid user input
            if(ValidateArrivalTime() == true && ValidateBurstTime() == true)
            {
                // Validation OK
                int arrivalTime = Int32.Parse(TextBoxArrival.Text);
                int burstTime = Int32.Parse(TextBoxBurst.Text);

                // Add new process
                roundRobin.AddProcess(arrivalTime, burstTime);

                // Reset text boxes
                TextBoxArrival.Text = "0";
                TextBoxBurst.Text = "0";
            }
        }


        /**************************************************************************
        * Validate arrival time user input
        **************************************************************************/
        private bool ValidateArrivalTime()
        {
            // Validation of user input
            bool isValid = true;

            // Check for valid arrival time
            int arrivalTime;

            if (Int32.TryParse(TextBoxArrival.Text, out arrivalTime) == false)
            {
                // Not a number
                isValid = false;
                TextBoxOutput.AppendText("ERROR: Arrival time must be a number!\n");
                TextBoxOutput.ScrollToEnd();
            }
            else if (arrivalTime < 0)
            {
                // Number cannot be negative
                isValid = false;
                TextBoxOutput.AppendText("ERROR: Arrival time value cannot be negative!\n");
                TextBoxOutput.ScrollToEnd();
            }

            // Return result
            return isValid;
        }


        /**************************************************************************
        * Validate arrival time user input
        **************************************************************************/
        private bool ValidateBurstTime()
        {
            // Validation of user input
            bool isValid = true;

            // Check for valid burst time
            int burstTime;

            if (Int32.TryParse(TextBoxBurst.Text, out burstTime) == false)
            {
                // Not a number
                isValid = false;
                TextBoxOutput.AppendText("ERROR: Burst time must be a number!\n");
                TextBoxOutput.ScrollToEnd();
            }
            else if (burstTime < 1)
            {
                // Burst must be greater than zero
                isValid = false;
                TextBoxOutput.AppendText("ERROR: Burst time value must be greater than zero!\n");
                TextBoxOutput.ScrollToEnd();
            }

            // Return result
            return isValid;
        }



        /***************************************************************************
        * Validate time quantum user input
        ***************************************************************************/
        private bool ValidateTimeQuantumInput()
        {
            // Validation of user input
            bool isValid = true;

            // Check for valid arrival time
            int userTimeQuantum;

            if (Int32.TryParse(TextBoxQuantum.Text, out userTimeQuantum) == false)
            {
                isValid = false;
                TextBoxOutput.AppendText("ERROR: Time quantum must be a number!\n");
                TextBoxOutput.ScrollToEnd();
            }
            else if(userTimeQuantum < 1)
            {
                isValid = false;
                TextBoxOutput.AppendText("ERROR: Time quantum must be greater than zero!\n");
                TextBoxOutput.ScrollToEnd();
            }

            // Return result
            return isValid;
        }


        /***************************************************************************
        * Handles click event for run simulation button
        ***************************************************************************/
        private void ButtonRunSimulation_Click(object sender, RoutedEventArgs e)
        {
            if(ValidateTimeQuantumInput() == true)
            {
                int timeQuantum = Int32.Parse(TextBoxQuantum.Text);
                roundRobin.RunSimulation(timeQuantum);
            }
        }


        /***************************************************************************
        * Handles click event for remove all button
        ***************************************************************************/
        private void ButtonRemoveAll_Click(object sender, RoutedEventArgs e)
        {
            // Remove all processes
            roundRobin.RemoveAll();

            // Clear text box content
            TextBoxOutput.Text = "";

            // Output message
            TextBoxOutput.AppendText("All processes have been removed.\n");
            TextBoxOutput.AppendText("Please submit processes to be simulated...\n");
        }
    }
}
