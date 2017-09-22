using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace ProcessorSchedulingRR
{
    class RoundRobin
    {
        // Reference to a TextBox to output messages
        private TextBox textBoxRef;

        // Stores a list of processes
        private List<Process> processes = new List<Process>();

        // Stores a list of processes in runqueue
        private List<Process> runqueue = new List<Process>();

        // The time quantum for the scheduler
        private int timeQuantum;

        // Current time within the simulation
        private int currentTime = 0;

        // Current process running time
        private int currentProcessRunningTime = 0;

        // Number of complete porcesses
        private int numComplete = 0;


        /***************************************************************************
        * Constructor
        ***************************************************************************/
        public RoundRobin(TextBox textBoxRef)
        {
            // Set reference to text box
            this.textBoxRef = textBoxRef;
        }


        /***************************************************************************
        * Outputs text to UI
        ***************************************************************************/
        public void OutputText(String message)
        {
            // Add text to text box
            textBoxRef.AppendText(message);

            // Scroll to end of text
            textBoxRef.ScrollToEnd();
        }


        /***************************************************************************
        * Adds a process to the list
        ***************************************************************************/
        public void AddProcess(int arrivalTime, int burstTime)
        {
            // Create name for new process
            String name = "P" + processes.Count.ToString();

            // create new process
            Process process = new Process(arrivalTime, burstTime, name);

            // Add to list
            processes.Add(process);

            // Output to UI
            String output = "Process " + name + " has been added (Arrival: ";
            output += arrivalTime + "ms, Burst: " + burstTime + "ms)\n";
            OutputText(output);
        }


        /***************************************************************************
        * Removes all processes from list
        ***************************************************************************/
        public void RemoveAll()
        {
            // Clear processes list
            processes.Clear();
        }


        /***************************************************************************
        * Runs the simulation
        ***************************************************************************/
        public void RunSimulation(int timeQuantum)
        {
            // Check we have a list of processes
            if(processes.Count > 0)
            {
                // OK to proceed - Set time quantum
                this.timeQuantum = timeQuantum;

                // Prepare simulation to be run
                Reset();

                // Run the simulation
                Run();
            }
            else
            {
                // No processes in list
                OutputText("ERROR: No processes have been entered!\n");
            }
        }


        /***************************************************************************
        * Resets the simulation before running
        ***************************************************************************/
        private void Reset()
        {
            // Remove any previous processes from runqueue
            runqueue.Clear();

            // Reset number of complete processes
            numComplete = 0;

            // Reset time
            currentTime = 0;

            // Reset current process running time
            currentProcessRunningTime = 0;

            // Reset processes
            for(int i = 0; i < processes.Count; i++)
            {
                processes[i].Reset();
            }
        }


        /***************************************************************************
        * Runs the simulation until complete
        ***************************************************************************/
        private void Run()
        {
            OutputText("\n******** Simulation Started ********\n");

            // Run until all processes are complete
            while(numComplete != processes.Count)
            {
                // start the scheduler routine
                Tick();
            }

            OutputText("******** Simulation Finished ********\n\n");

            // Output turnaround and waiting time stats
            CalculateStats();
        }


        /***************************************************************************
        * Time slice of simulation
        ***************************************************************************/
        private void Tick()
        {
            // Add arriving processes to runqueue
            InsertToRunqueue();

            // Remove current process from runqueue if complete
            CheckComplete();

            // Perform context switching if necessary
            SwitchContext();

            // Run the current process
            RunCurrentProcess();

            // Increment current time
            currentTime++;
        }


        /***************************************************************************
        * Insert processes into runqueue on arrival
        ***************************************************************************/
        private void InsertToRunqueue()
        {
            // Loop through processes list
            for (int i = 0; i < processes.Count; i++)
            {
                // Check arrival time for process
                if (processes[i].arrivalTime == currentTime)
                {
                    // Add process to end of queue
                    this.runqueue.Add(processes[i]);

                    // Output event
                    String output = currentTime.ToString() + " ms: ";
                    output += processes[i].processName + " added to runqueue\n";
                    OutputText(output);
                }
            }
        }


        /***************************************************************************
        * Run the first process in the runque (Index: 0)
        ***************************************************************************/
        private void RunCurrentProcess()
        {
            // Check we have at least one process in runqueue
            if(runqueue.Count > 0)
            {
                // Run the process for 1 ms
                runqueue[0].reaminingTime--;

                // Check for start running condition
                if(currentProcessRunningTime == 0)
                {
                    // Output event
                    String output = currentTime.ToString() + " ms: ";
                    output += runqueue[0].processName + " has started running\n";
                    OutputText(output);
                }

                // Add one ms to current process running time
                currentProcessRunningTime++;
            }
        }


        /***************************************************************************
        * Check the current process for termination
        ***************************************************************************/
        private void CheckComplete()
        {
            // Check we have at least one process in runqueue
            if (runqueue.Count > 0)
            {
                // Check remaining time
                if(runqueue[0].reaminingTime == 0)
                {
                    // Get index for terminating process in processes list
                    int index = GetProcessIndexByName(runqueue[0].processName);

                    // Set completion time
                    processes[index].completionTime = currentTime;

                    // Output event
                    String output = currentTime.ToString() + " ms: ";
                    output += processes[index].processName + " has terminated\n";
                    OutputText(output);

                    // Terminate process by removing from runqueue
                    runqueue.RemoveAt(0);

                    // Increment number of complete processes
                    numComplete++;

                    // Reset current process running time
                    currentProcessRunningTime = 0;
                }
            }
        }


        /***************************************************************************
        * Returns the index of the process in the process list
        ***************************************************************************/
        private int GetProcessIndexByName(String processName)
        {
            // Loop through process list
            for(int i = 0; i < processes.Count; i++)
            {
                // Check for matching names
                if(processes[i].processName == processName)
                {
                    // Names match therefore return index
                    return i;
                }
            }

            // Opps something went wrong: TODO Throw error
            return -1;
        }


        /***************************************************************************
        * Performs the context switching for the round robin
        ***************************************************************************/
        private void SwitchContext()
        {
            // Only switch if more than one process is in runqueue
            if (runqueue.Count > 1 && currentProcessRunningTime == timeQuantum)
            {
                // Output event
                String output = currentTime.ToString() + " ms: ";
                output += runqueue[0].processName + " has been paused\n";
                OutputText(output);

                // Switch first process in runqueue to the end
                Process process = runqueue[0];
                runqueue.RemoveAt(0);
                runqueue.Add(process);

                // Reset current process running time
                currentProcessRunningTime = 0;
            }
        }


        /***************************************************************************
        * Calculate and output stats
        ***************************************************************************/
        private void CalculateStats()
        {
            // Total turnaround time
            int totalTurnaround = 0;

            // Total Waiting time
            int totalWaiting = 0;

            // Output string for results
            String output = "";

            for (int i = 0; i < processes.Count; i++)
            {
                // Perform calculations
                processes[i].CalculateStats();

                // Add to total times
                totalTurnaround += processes[i].turnaroundTime;
                totalWaiting += processes[i].waitingTime;

                // Output stats for this process
                output += processes[i].processName + ": Turnaround Time = " + processes[i].turnaroundTime.ToString();
                output += "ms, Waiting Time = " + processes[i].waitingTime.ToString() + "ms\n";
            }

            // Calculate average times
            decimal averageTurnaround = (decimal)totalTurnaround / (decimal)processes.Count;
            decimal averageWaiting = (decimal)totalWaiting / (decimal)processes.Count;

            // Round to 2 decimal places
            averageTurnaround = Math.Round(averageTurnaround, 2);
            averageWaiting = Math.Round(averageWaiting, 2);

            // Add to output string
            output += "Average Turnaround Time = " + averageTurnaround.ToString() + "ms\n";
            output += "Average Waiting Time = " + averageWaiting.ToString() + "ms\n";

            // Output to UI
            OutputText(output);
        }
    }
}
