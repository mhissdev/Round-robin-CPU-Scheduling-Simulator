using System;

namespace ProcessorSchedulingRR
{
    class Process
    {
        // Arrival time of process
        public int arrivalTime;

        // Burst time of process
        public int burstTime;

        // Remaining time until termination
        public int reaminingTime;

        // Process name
        public String processName;

        // Completion time
        public int completionTime;

        // Turnaround time
        public int turnaroundTime;

        // Waiting time
        public int waitingTime;

        /*****************************************************************
        * Constructor
        *****************************************************************/
        public Process(int arrivalTime, int burstTime, String processName)
        {
            // Set properties
            this.arrivalTime = arrivalTime;
            this.burstTime = burstTime;
            this.processName = processName;

            // Reset properties
            Reset();
        }


        /*****************************************************************
        * Reset the process to allow simulation to be run multiple times
        *****************************************************************/
        public void Reset()
        {
            // Reset properties to orginal values
            reaminingTime = burstTime;
            completionTime = 0;
            turnaroundTime = 0;
            waitingTime = 0;
        }


        /*****************************************************************
        * Calculate turnaround time and waiting time
        *****************************************************************/
        public void CalculateStats()
        {
            // Set turnaround time
            turnaroundTime = completionTime - arrivalTime;

            // Set waiting time
            waitingTime = turnaroundTime - burstTime;
        }

    }
}
