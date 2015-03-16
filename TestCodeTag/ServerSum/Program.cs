using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ServerSum
{

    public class Program
    {
        private static SocketResultSum serverResulSum;
        private static SocketSum serverSum;
        private const int Iteraciones = 10;
        private static int interacionAcutal = 0;
        private static System.Timers.Timer aTimer; 

        static void Main(string[] args)
        {
            serverResulSum = new SocketResultSum(IPAddress.Loopback, 54322);
            serverResulSum.Start();

            serverSum = new SocketSum(IPAddress.Loopback, 54321, serverResulSum);
            serverSum.Start();

            ThreadStart childTimer = new ThreadStart(CallTimer);
            Thread childThread = new Thread(childTimer);
            childThread.Start();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

            serverSum.Stop();
            serverResulSum.Stop();

           

        }

        private static void CallTimer()
        {
            // Create a timer with a ten second interval.
            aTimer = new System.Timers.Timer(20000);

            // Hook up the Elapsed event for the timer.
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Enabled = true;
        } 

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            interacionAcutal++;
            if (interacionAcutal > Iteraciones) aTimer.Enabled = false;

            SocketSum.SendResultSum(interacionAcutal * 20);
        }
    }
}