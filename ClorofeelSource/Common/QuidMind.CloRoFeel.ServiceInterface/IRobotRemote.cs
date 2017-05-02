using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace QuidMind.CloRoFeel.Service
{

    [ServiceContract(
        Name = "IRobotRemote",
        Namespace="http://clorofeel.quidmind.com/ServiceModel/RobotRemote")]
    public interface IRobotRemote
    {
        /// <summary>
        /// Déclare un périphérique de pilotage.
        /// Cet appel est obligatoirement le premier
        /// <remarks>La déclaration ne vaut pas autorisation</remarks>
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns>True=le device a été accepté par le robot, sinon False</returns>
        [OperationContract]
        [WebGet(UriTemplate="/RegisterDevice?deviceId={deviceId}")]
        bool RegisterDevice(string deviceId);


        /// <summary>
        /// Retourne les informations sur la version interne du logiciel utilisé
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebGet(UriTemplate = "/GetRobotVersion?deviceId={deviceId}")]
        string GetRobotVersion(string deviceId);

        /// <summary>
        /// permet au device de pilotage de vérifier si il a le controle sur le robot
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebGet(UriTemplate = "/IsAuthorized?deviceId={deviceId}")]
        bool IsAuthorized(string deviceId);

        [OperationContract]
        [WebGet(UriTemplate = "/SetSpeed?deviceId={deviceId}&leftSpeed={leftSpeed}&rightSpeed={rightSpeed}")]
        RobotResult SetSpeed(string deviceId, int leftSpeed, int rightSpeed);

        [OperationContract]
        [WebGet(UriTemplate = "/SetCameraAbsolutePosition?deviceId={deviceId}&camIsActive={camIsActive}&orientationHorizontale={orientationHorizontale}&orientationVerticale={orientationVerticale}")]
        RobotResult SetCameraAbsolutePosition(string deviceId, bool camIsActive, uint orientationHorizontale, uint orientationVerticale);

        [OperationContract]
        [WebGet(UriTemplate = "/SetCameraPosition?deviceId={deviceId}&camIsActive={camIsActive}&orientationHorizontale={orientationHorizontale}&orientationVerticale={orientationVerticale}")]
        RobotResult SetCameraPosition(string deviceId, bool camIsActive, int orientationHorizontale, int orientationVerticale);

    }
}
