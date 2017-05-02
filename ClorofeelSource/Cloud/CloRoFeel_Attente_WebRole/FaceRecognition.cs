using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Luxand;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Net;

namespace CloRoFeel_Attente_WebRole
{
    [DataContract]
    public class Coordinate
    {
        public Coordinate(FSDK.TPoint fsdkPoint)
        {
            this.X = fsdkPoint.x;
            this.Y = fsdkPoint.y;
        }
        [DataMember]
        public double X { get; set; }
        [DataMember]
        public double Y { get; set; }
    }

    [DataContract]
    public class FacePosition
    {
        public FacePosition(FSDK.TFacePosition fsdkFacePosition)
        {
            this.Angle = fsdkFacePosition.angle;
            this.X = fsdkFacePosition.xc;
            this.Y = fsdkFacePosition.yc;
            this.Width = fsdkFacePosition.w;
        }
        [DataMember]
        public double Angle { get; set; }
        [DataMember]
        public double X { get; set; }
        [DataMember]
        public double Y { get; set; }
        [DataMember]
        public double Width { get; set; }
        
        [DataMember]
        public Coordinate Eye1 { get; set; }
        [DataMember]
        public Coordinate Eye2 { get; set; }

        [DataMember]
        public bool TargetedFace { get; set; }

        [DataMember]
        public bool KnownFace { get; set; }

        [DataMember]
        public string PeopleName { get; set; }

        // not seriazed
        public FSDK.TFacePosition _facePosition;
    }

    [DataContract]
    public class FaceRecognitionResult
    {
        public FaceRecognitionResult()
        {
            this.FacePositions = new List<FacePosition>();
            this.EndCondition = string.Empty;
        }
        [DataMember]
        public DateTime StartTime { get; set; }
        [DataMember]
        public DateTime EndTime { get; set; }

        [DataMember]
        public string EndCondition { get; set; }

        [DataMember]
        public string TempFileName { get; set; }
        [DataMember]
        public int NbFaces { get; set; }

        [DataMember]
        public List<FacePosition> FacePositions { get; set; }

        [DataMember]
        public string ImageBase64 { get; set; }

        [DataMember]
        public string lastWebRequestError { get; set; }

    }

    /// <summary>
    /// Implementation of face recognition base on Luxand FaceSDK
    /// </summary>
    public class FaceRecognition
    {
        const float FaceDetectionThreshold = 3;
        const float FARValue = 100;
        //const string FSDK_ActivationKey = "6ECDDB0D383705026222D0F2C152F73183D0075260FCC30E561C62C11491C6B952FE8256C6CF615760A7D36113E27B2241E95FD832487A1AB04B7B1BACC9BEC5";
        const string FSDK_ActivationKey = "WKNEFJl3jw6oQMrgzk4yyzKovi5pgABmWxy0EqMfW4iteOHpgx9Qsp2Glkujrs061GsKrGMkELh8wcT9LEZMAGAfjNOVkJfSnxskItmDohkH48yrwPJm8BWP3PHEUSktIsJo1jFPXQ6lqbZ4boGSgrydC7n6XjZnAc5riG9iOaE=";

        static string lastWebRequestDeltaError = string.Empty;

        static  FaceRecognition()
        {
            Debug.WriteLine("Initializing Apllications ....");
            if (FSDK.FSDKE_OK == FSDK.ActivateLibrary(FSDK_ActivationKey))
            {
                if (FSDK.InitializeLibrary() != FSDK.FSDKE_OK)
                    Debug.WriteLine("ERROR initializing FaceSDK Library !!");
            }
            else
            {
                Debug.WriteLine("ERROR ActivateLibrary : check license key");
            }  
            FSDK.SetFaceDetectionParameters(false, true, 384);
            FSDK.SetFaceDetectionThreshold((int)FaceDetectionThreshold);

            WebRequest.DefaultCachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
        }

        public FaceRecognitionResult RecognitionResult { get; set; }

        public void ScanFace(string imgBase64)
        {
            FaceRecognitionResult frr = new FaceRecognitionResult() { StartTime = DateTime.UtcNow };
            frr.ImageBase64 = imgBase64;
            try
            {
                byte[] buffer = Convert.FromBase64String(imgBase64);
                MemoryStream ms = new MemoryStream(buffer);

                //save picture as JPG in temp folder
                frr.TempFileName = System.IO.Path.GetTempFileName() + ".jpg";
                FileStream picFile = File.Create(frr.TempFileName);
                ms.WriteTo(picFile);
                picFile.Close();
                ms.Close();
                picFile.Dispose();
                ms.Dispose();

                int fsdkRes = FSDK.FSDKE_OK;
                // load JPG in 
                Int32 hImg = 0;
                fsdkRes = FSDK.LoadImageFromFile(ref hImg, frr.TempFileName);
                if (fsdkRes != FSDK.FSDKE_OK)
                {
                    frr.EndCondition = "ERREUR FSDK.LoadImageFromFile() : " + GetFsdkErrorMessage(fsdkRes);
                    Debug.WriteLine("ERREUR FSDK.LoadImageFromFile() : " + GetFsdkErrorMessage(fsdkRes));
                    return;
                }

                // recherche des visages
                FSDK.TFacePosition[] facePositions;
                int nbFaces = 0;
                fsdkRes = FSDK.DetectMultipleFaces(hImg, ref nbFaces, out facePositions, 20000);
                if (fsdkRes != FSDK.FSDKE_OK)
                {
                    frr.EndCondition = "ERREUR dectecting faces : " + GetFsdkErrorMessage(fsdkRes);
                    Debug.WriteLine("ERREUR FSDK.DetectMultipleFaces() : " + GetFsdkErrorMessage(fsdkRes));
                    return;
                }
                frr.NbFaces = nbFaces;

                // recherche des yeux dans les visages et du visage le plus proche
                int targetedFace = -1;
                FSDK.TPoint[] targetedEyes = null;
                double targetedEyeDistance = -1;
                for (int n = 0; n < nbFaces; n++)
                {
                    FSDK.TPoint[] eyePositions = new FSDK.TPoint[2];
                    fsdkRes = FSDK.DetectEyesInRegion(hImg, ref facePositions[n], out eyePositions);
                    if (fsdkRes != FSDK.FSDKE_OK)
                        continue;
                    var fpos = new FacePosition(facePositions[n]);
                    fpos.Eye1 = new Coordinate(eyePositions[0]);
                    fpos.Eye2 = new Coordinate(eyePositions[1]);
                    frr.FacePositions.Add(fpos);
                    if (targetedFace == -1) // on préselectionne la 1ere paire d'yeux
                    {
                        targetedFace = n;
                        targetedEyes = eyePositions;
                        targetedEyeDistance =
                            Math.Sqrt((targetedEyes[1].x - targetedEyes[0].x) ^ 2 + (targetedEyes[1].y - targetedEyes[0].y) ^ 2);
                    }
                    else // si d'autres visage, on calcule la distance entre les 2 yeux. 
                    {
                        double newDistance = Math.Sqrt((eyePositions[1].x - eyePositions[0].x) ^ 2 + (eyePositions[1].y - eyePositions[0].y) ^ 2);
                        if (newDistance > targetedEyeDistance)
                        {
                            targetedFace = n;
                            targetedEyes = eyePositions;
                            targetedEyeDistance = newDistance;
                        }
                    }

                    // calcul du FaceTemplate puis recherche dans la base des templates connu
                    // TODO TODO 

                }
                frr.FacePositions[targetedFace].TargetedFace = true;

                // Calcul du delta par rapport au centre de l'image 
                double imgCenterX = 640 / 2.0; // bi.Width / 2;
                double imgCenterY = 480 / 2.0; // bi.Height / 2;

                double eyeCenterX = (targetedEyes[0].x + targetedEyes[1].x) / 2;
                double eyeCenterY = (targetedEyes[0].y + targetedEyes[1].y) / 2;

                // calcul du delta de déplacement de la tete pour recentrer le visage sélectionné
                const int leftExtreme = 1800;
                const int rightExtreme = 1200;
                const int bottomExtreme = 1725;
                const int upExtreme = 1275;

                double deltaPixelX = imgCenterX - eyeCenterX;
                double deltaPixelY = imgCenterY - eyeCenterY;

                int deltaServoX = (int)((leftExtreme - rightExtreme) * deltaPixelX / 640.0);
                int deltaServoY = -(int)((bottomExtreme - upExtreme) * deltaPixelY / 480.0);

                //txbDeltaServo.Text = string.Format(" {0} , {1}", deltaServoX, deltaServoY);
                string url =
                    string.Format("http://clorofeelv2.servicebus.windows.net/robotremote/RobotBoard/SetCameraPosition?deviceId=12345&camIsActive=true&orientationHorizontale={0}&orientationVerticale={1}&uidReq={2}",
                    deltaServoX, deltaServoY,
                    DateTime.Now.Ticks  // pour l'unicite de la requete
                    );
                var wreq = HttpWebRequest.Create(url);
                //wreq.BeginGetResponse((ar) => { 
                //}, null);
                var wresp = (HttpWebResponse)wreq.GetResponse();
                var body = new StreamReader(wresp.GetResponseStream()).ReadToEnd();
                frr.EndCondition = "No error : SetCameraPosition =" + body;
                //frr.EndCondition = "No error" ;
            }
            catch (Exception ex)
            {
                frr.EndCondition = "ERROR: " + ex.ToString();
            }
            finally
            {
                frr.EndTime = DateTime.UtcNow;
                this.RecognitionResult = frr;
            }
        }

        public string GetFsdkErrorMessage(int fsdkError)
        {
            if (fsdkError == FSDK.FSDK_FACIAL_FEATURE_COUNT)
            { return "Facial Feature Cound"; }
            else if (fsdkError == FSDK.FSDKE_BAD_FILE_FORMAT)
            { return "Bad file format"; }
            else if (fsdkError == FSDK.FSDKE_CANNOT_CREATE_FILE)
            { return "Cannot create file"; }
            else if (fsdkError == FSDK.FSDKE_CANNOT_OPEN_FILE)
            { return "Cannot open file"; }
            else if (fsdkError == FSDK.FSDKE_FACE_NOT_FOUND)
            { return "Face not found"; }
            else if (fsdkError == FSDK.FSDKE_FAILED)
            { return "Failed"; }
            else if (fsdkError == FSDK.FSDKE_FILE_NOT_FOUND)
            { return "File not found"; }
            else if (fsdkError == FSDK.FSDKE_IMAGE_TOO_SMALL)
            { return "Image too small"; }
            else if (fsdkError == FSDK.FSDKE_INSUFFICIENT_BUFFER_SIZE)
            { return "Insufficient buffer size"; }
            else if (fsdkError == FSDK.FSDKE_INVALID_ARGUMENT)
            { return "Invalid argument"; }
            else if (fsdkError == FSDK.FSDKE_IO_ERROR)
            { return "IO Error"; }
            else if (fsdkError == FSDK.FSDKE_NOT_ACTIVATED)
            { return "Not activated"; }
            else if (fsdkError == FSDK.FSDKE_OK)
            { return "Ok (no error)"; }
            else if (fsdkError == FSDK.FSDKE_OUT_OF_MEMORY)
            { return "Out of memory"; }
            else if (fsdkError == FSDK.FSDKE_UNSUPPORTED_IMAGE_EXTENSION)
            { return "Unsupported Image extension"; }
            else
            {
                return "Unknow error code : " + fsdkError.ToString();
            }
        }

        public byte[] GetTemplateForFace(string imgBase64)
        {
            try
            {
                byte[] buffer = Convert.FromBase64String(imgBase64);
                MemoryStream ms = new MemoryStream(buffer);

                //save picture as JPG in temp folder
                string tempFileName = System.IO.Path.GetTempFileName() + ".jpg";
                FileStream picFile = File.Create(tempFileName);
                ms.WriteTo(picFile);
                picFile.Close();
                ms.Close();
                picFile.Dispose();
                ms.Dispose();

                int fsdkRes = FSDK.FSDKE_OK;
                Int32 hImg = 0;
                fsdkRes = FSDK.LoadImageFromFile(ref hImg, tempFileName);
                if (fsdkRes != FSDK.FSDKE_OK)
                {
                    Debug.WriteLine("ERREUR FSDK.LoadImageFromFile() : " + GetFsdkErrorMessage(fsdkRes));
                    return null;
                }

                FSDK.TFacePosition facePosition = new FSDK.TFacePosition();
                if (FSDK.FSDKE_OK == FSDK.DetectFace(hImg, ref facePosition))
                {
                    byte[] templateData = new byte[FSDK.TemplateSize];
                    
                    if (FSDK.FSDKE_OK == FSDK.GetFaceTemplateInRegion(hImg, ref facePosition, out templateData))
                    {
                        return templateData;   
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }
    }
}
