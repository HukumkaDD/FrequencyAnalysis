using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FrequencyAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
                return;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            TripletDictionary tripletDictionary = new TripletDictionary(args[0]);
            tripletDictionary.AnalisStart();

            Console.WriteLine(tripletDictionary.WriteMaxTriplets());
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);

            return;
        }
    }
}
