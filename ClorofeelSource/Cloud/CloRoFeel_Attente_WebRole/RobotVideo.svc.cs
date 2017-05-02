using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Diagnostics;
using QuidMind.CloRoFeel.Service;
using Luxand;
using System.Net;


namespace CloRoFeel_Attente_WebRole
{
    //[ServiceContract(Namespace = "http://clorofeel.quidmind.com/ServiceModel/RobotVideo")]
    //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    //[ServiceBehavior(Name = "RobotVideoService",
    //    InstanceContextMode = InstanceContextMode.Single,
    //    ConcurrencyMode = ConcurrencyMode.Multiple
    //    )]

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceContract(Namespace = "http://clorofeel.quidmind.com/ServiceModel/RobotVideo")]
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.Single,
        ConcurrencyMode = ConcurrencyMode.Multiple
        )]
    public class RobotVideo
    {
        int nbPicWithoutRecognition = 0;

        object lockPicture = new object();
        string lastPicture = string.Empty;
        FaceRecognitionResult lastFaceRecognitionResult = null;

        int compassValue = 0;

        [OperationContract]
        public void SendLastPicture(string secureToken, int positionBoussolle, string pictureBase64)
        {
            Trace.Listeners["AzureDiagnostics"].WriteLine("SendLastPicture");
            if (secureToken != "XX123456789XX") // seule les possesseurs du token de securité peuvent poster les images
                return;
            lock (lockPicture)
            {
                compassValue = positionBoussolle;
                this.lastPicture = pictureBase64;
                //Debug.WriteLine(pictureBase64);
                var fr = new FaceRecognition();
                fr.ScanFace(pictureBase64);
                lastFaceRecognitionResult = fr.RecognitionResult;
                if (lastFaceRecognitionResult.FacePositions.Count == 0) // si pas de visage -> incrémenter le compteur
                    nbPicWithoutRecognition++;
                if (nbPicWithoutRecognition > 20)
                {
                    nbPicWithoutRecognition = 0;
                    CenterClorofeelCam();
                }
            }
            return;
        }

        void CenterClorofeelCam()
        {
            string url =
                   string.Format("http://clorofeel.servicebus.windows.net/robotremote/RobotBoard/SetCameraAbsolutePosition?deviceId=12345&camIsActive=true&orientationHorizontale={0}&orientationVerticale={1}&uidReq={2}",
                   1500, 1250,
                   DateTime.Now.Ticks  // pour l'unicite de la requete
                   );
            var wreq = HttpWebRequest.Create(url);
            var wresp = (HttpWebResponse)wreq.GetResponse();
        }

        [OperationContract]
        public RobotResult GetLastPicture()
        {
            Trace.Listeners["AzureDiagnostics"].WriteLine("GetLastPicture()");
            RobotResult rr = new RobotResult()
            {
                Compass = compassValue,
                StatusOk = true,
                TimeStamp = DateTime.Now.ToUniversalTime(),
                Message = lastPicture
            };
            //compassValue += 10;
            //if (compassValue >= 360)
            //    compassValue = 0;
            return rr;
        }

        [OperationContract]
        public FaceRecognitionResult GetLastFaceRecognitionResult()
        {
            Trace.Listeners["AzureDiagnostics"].WriteLine("GetLastFaceRecognitionResult()");
            return this.lastFaceRecognitionResult;
        }

        [OperationContract]
        public string TestMethod()
        {
            Trace.Listeners["AzureDiagnostics"].WriteLine("TestMethod called");
            return "TEST " + DateTime.Now.ToString();
        }

        [OperationContract]
        public string AddFaceToDatabase(string peopleName,string picWithFaceBase64)
        {
            Trace.Listeners["AzureDiagnostics"].WriteLine("AddFaceToDatabase");

            return FaceTemplateEngine.Current.AddFace(peopleName, picWithFaceBase64);
        }

    }
}
