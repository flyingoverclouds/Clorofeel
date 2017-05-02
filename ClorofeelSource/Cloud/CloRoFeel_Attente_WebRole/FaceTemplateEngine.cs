using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Luxand;

namespace CloRoFeel_Attente_WebRole
{
    public class FaceTemplateData
    {
        public string Name { get; set; }
        public byte[] FaceTemplate { get; set; }
        public DateTime AddedDate { get; set; }
    }

    public class FaceTemplateEngine
    {
        List<FaceTemplateData> lstKnownTemplate = new List<FaceTemplateData>();


        #region Singleton
        static FaceTemplateEngine _instance=null;

        static public FaceTemplateEngine Current
        {
            get
            {
                if (_instance == null)
                    _instance = new FaceTemplateEngine();
                return _instance;
            }
        }
        #endregion


        /// <summary>
        /// cette methode charge en cache mémoire les template des visages à partir de la base SQL Azure
        /// </summary>
        void FillCacheFromDataBase()
        {
            lstKnownTemplate.Clear();
        }


        public string AddFace(string peopleName, string picWithFaceBase64)
        {
            FaceRecognition fr = new FaceRecognition();
            byte[] faceTemplate = fr.GetTemplateForFace(picWithFaceBase64);

            if (faceTemplate == null)
                return "ERROR: Pas de Visage Détecté";

            FaceTemplateData ftd = new FaceTemplateData()
            {
                Name = peopleName,
                AddedDate = DateTime.Now,
                FaceTemplate = faceTemplate
            };
            lstKnownTemplate.Add(ftd);

            return string.Format("OK: Visage ajouté ({0} visages dans la base)",lstKnownTemplate.Count);
        }

        public string GetNameForFaceTemplate(byte[] searchedTemplateData)
        {

            foreach (var knowTemplateDate in lstKnownTemplate)
            {
                float similarity = 0.0f;
                byte[] sft = knowTemplateDate.FaceTemplate;
                FSDK.MatchFaces(ref searchedTemplateData, ref sft, ref similarity);
                float threshold = 0.0f;
                FSDK.GetMatchingThresholdAtFAR(0.01f, ref threshold); // set FAR to 1%
                if (similarity > threshold)
                {
                    return knowTemplateDate.Name;
                }
            }
            return null;
        }
    }
}