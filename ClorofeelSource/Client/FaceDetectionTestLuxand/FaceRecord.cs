using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Luxand;

namespace FaceDetectionTestLuxand
{
    public class FaceRecord
    {
        public FaceRecord()
        {
            _facePosition = new FSDK.TFacePosition();
            _facialFeatures = new FSDK.TPoint[2];
            _template = new byte[FSDK.TemplateSize];

            for (int i = 0; i < FSDK.TemplateSize; i++) 
                _template[i] = 0; 
        }
        public byte[] _template; //Face Template;
        public FSDK.TFacePosition _facePosition;
        public FSDK.TPoint[] _facialFeatures; //Facial Features;
    }
}
