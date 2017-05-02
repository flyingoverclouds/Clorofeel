/*** CloRoFeel - (c) QuidMind 2012
 * Ce code est la propriété de QuidMind et ne peut être utilisé sans accord explicite.
 * 
 * ** DistanceMonitor.cs : Monitoring des capteurs de vide
 * 
 * 
 */

using System;
using Microsoft.SPOT;
using GHIElectronics.NETMF.Hardware;
using System.Threading;
using GHIElectronics.NETMF.FEZ;

namespace QuidMind.MFCloRoFeel
{
    class DistanceMonitor
    {
        int SeuilDetectionChuteAVG = 2000;
        int SeuilDetectionChuteARG = 2000;
        int SeuilDetectionChuteAVD = 2000;
        int SeuilDetectionChuteARD = 2000;


        const int NbCalibrationMesure = 100;

        const int nbDataAvg = 5;
        
        public bool AvantDroit { get; set; }
        public bool AvantGauche { get; set; }
        public bool ArriereDroit { get; set; }
        public bool ArriereGauche { get; set; }

        AnalogIn distAVD;
        AnalogIn distAVG;
        AnalogIn distARD;
        AnalogIn distARG;

        Thread thDistanceMonitor = null;

        public DistanceMonitor()
        {
            distAVD = new AnalogIn((AnalogIn.Pin)FEZ_Pin.AnalogIn.An0); distAVD.SetLinearScale(0, 3300);
            distAVG = new AnalogIn((AnalogIn.Pin)FEZ_Pin.AnalogIn.An4); distAVG.SetLinearScale(0, 3300);
            distARD = new AnalogIn((AnalogIn.Pin)FEZ_Pin.AnalogIn.An1); distARD.SetLinearScale(0, 3300);
            distARG = new AnalogIn((AnalogIn.Pin)FEZ_Pin.AnalogIn.An2); distARG.SetLinearScale(0, 3300);
            thDistanceMonitor = new Thread(DoRunDistanceScan);
        }

        public void Start()
        {
            thDistanceMonitor.Start();
        }

        public void Stop()
        {
            thDistanceMonitor.Abort();
        }

        void DoRunDistanceScan()
        {
            int nbMesureForCalibration = NbCalibrationMesure;
            int seuilDetectionCalibrationAVG =0, seuilDetectionCalibrationARG=0, seuilDetectionCalibrationAVD=0, seuilDetectionCalibrationARD = 0;

            int distanceAVD, distanceAVG, distanceARD, distanceARG;

            int[] argTampon = new int[nbDataAvg];
            int argPos = 0;
            int n;
            Program.WriteLine("[CALIBRATION]\n");
            while (true)
            {
                Thread.Sleep(50); // scan des capteur de distance tout les 50ms
                
                if (nbMesureForCalibration >= 0)
                {
                    Program.led.Write(!Program.led.Read());
                    seuilDetectionCalibrationAVD += distAVD.Read();
                    seuilDetectionCalibrationAVG += distAVG.Read();
                    seuilDetectionCalibrationARD += distARD.Read();
                    seuilDetectionCalibrationARG += distARG.Read();

                    
                    if (nbMesureForCalibration == 0)
                    {
                        SeuilDetectionChuteAVG = seuilDetectionCalibrationAVG / NbCalibrationMesure;
                        SeuilDetectionChuteAVD = seuilDetectionCalibrationAVD / NbCalibrationMesure;
                        SeuilDetectionChuteARG = seuilDetectionCalibrationARG / NbCalibrationMesure;
                        SeuilDetectionChuteARD = seuilDetectionCalibrationARD / NbCalibrationMesure;
                        Debug.Print("END OF CALIBRATION");
                        Debug.Print("   AVG Average Level is : " + SeuilDetectionChuteAVG);
                        Debug.Print("   AVD Average Level is : " + SeuilDetectionChuteAVD);
                        Debug.Print("   ARG Average Level is : " + SeuilDetectionChuteARG);
                        Debug.Print("   ARD Average Level is : " + SeuilDetectionChuteARD);

                        SeuilDetectionChuteAVG -= 300;
                        SeuilDetectionChuteAVD -= 300;
                        SeuilDetectionChuteARG -= 300;
                        SeuilDetectionChuteARD -= 300;
                        Program.led.Write(false);
                        Program.WriteLine("[SENSING]\n");
                    }
                    nbMesureForCalibration--;
                }

                

                distanceAVD = distAVD.Read();
                distanceAVG = distAVG.Read();
                distanceARG = distARG.Read();
                distanceARD = distARD.Read();
                //{
                //    argTampon[argPos] = distARG.Read();
                //    argPos = (argPos + 1) % nbDataAvg;
                //    distanceARG = 0;
                //    for (n = 0; n < nbDataAvg; n++)
                //        distanceARG += argTampon[n];
                //    distanceARG = distanceARG / nbDataAvg;
                //    //Debug.Print(distanceARG.ToString());
                //}
                this.AvantGauche = distanceAVG < SeuilDetectionChuteAVG;
                this.AvantDroit = distanceAVD < SeuilDetectionChuteAVD;
                this.ArriereGauche = distanceARG < SeuilDetectionChuteARG;
                this.ArriereDroit = distanceARD < SeuilDetectionChuteARD;

            }
        }
    }
}
