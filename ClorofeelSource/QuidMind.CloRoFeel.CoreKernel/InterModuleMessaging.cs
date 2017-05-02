using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuidMind.CloRoFeel.CoreKernel
{

    public delegate void InterModuleMessageArrivedDelegate(InterModuleMessaging immContext);

    /// <summary>
    /// Classe d'interecnahe des modules
    /// </summary>
    public class InterModuleMessaging : MarshalByRefObject
    {

        #region Singleton
        static InterModuleMessaging _instance = null;
        static public InterModuleMessaging Current
        {
            get
            {
                if (_instance == null)
                    _instance = new InterModuleMessaging();
                return _instance;
            }
        }
        #endregion

        Queue<InterModuleMessage> messages = new Queue<InterModuleMessage>();

        public int MessageQueueCount { get { return messages.Count; } }

        public event InterModuleMessageArrivedDelegate MessageArrived;

        public void PushMessage(InterModuleMessage message)
        {
            try
            {
                if (message == null)
                    return;
                //Console.WriteLine("IMM message enqueued");
                messages.Enqueue(message);
                if (MessageArrived != null)
                    MessageArrived(this);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[InterModuleMessagin] : PushMessage() exception");
            }
        }

        public void PushMessage(string sourceModule,string targetModule,string body)
        {
            InterModuleMessage imm = new InterModuleMessage()
            {
                SourceModule = sourceModule,
                TargetModule = targetModule,
                Body = body
            };
            PushMessage(imm);
        }

        public InterModuleMessage ExtractMessage()
        {
            if (messages.Count == 0)
                return null;
            return messages.Dequeue();
        }
    }
}
