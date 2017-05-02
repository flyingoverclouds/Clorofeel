/*** CloRoFeel - (c) QuidMind 2012
 * Ce code est la propriété de QuidMind et ne peut être utilisé sans accord explicite.
 * 
 * ** ChocMonitor.cs : monitoring des capteurs de choc 
 * 
 * 
 */


using System;
using Microsoft.SPOT;
using System.Threading;
using GHIElectronics.NETMF.Hardware;

namespace QuidMind.MFCloRoFeel
{
    class ChocMonitor
    {
        public bool AvantDroit { get; set; }
        public bool AvantGauche { get; set; }
        public bool ArriereDroit { get; set; }
        public bool ArriereGauche { get; set; }

        Thread thChocMonitor = null;

        public ChocMonitor()
        {
            thChocMonitor = new Thread(DoRunDistanceScan);
        }

        public void Start()
        {
            thChocMonitor.Start();
        }

        public void Stop()
        {
            thChocMonitor.Abort();
        }

        void DoRunDistanceScan()
        {
            while (true)
            {
                Thread.Sleep(100); // scan des capteur de distance tout les 150ms
               
            }
        }
    }
}
