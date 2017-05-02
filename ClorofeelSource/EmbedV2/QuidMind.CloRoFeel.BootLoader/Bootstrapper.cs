using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuidMind.CloRoFeel.CoreKernel;
using System.Reflection;
using System.Threading;

namespace QuidMind.CloRoFeel.BootLoader
{
    // Assembly de chargement de l'appdomain
    public class Bootstrapper : MarshalByRefObject
    {
        RobotModuleAttribute moduleinfo = null;
        IRobotModule moduleInstance = null;


        string _moduleSettings = null;

        string _moduleAssemblyName;
        string _moduleClass;

        InterModuleMessaging immContext = null;

        public Bootstrapper()
        {
            
        }


        Assembly moduleAssembly = null;
        Type moduleType = null;
        MethodInfo runMethod = null;


        public string Load(InterModuleMessaging immContext, string moduleAssemblyName, string moduleClass, string settings)
        {
            this.immContext = immContext;
            this._moduleAssemblyName = moduleAssemblyName;
            this._moduleClass = moduleClass;
            this._moduleSettings = settings;
            moduleAssembly = Assembly.LoadFrom(_moduleAssemblyName);
            moduleType = moduleAssembly.GetType(_moduleClass);
            moduleinfo = (RobotModuleAttribute)moduleType.GetCustomAttributes(typeof(RobotModuleAttribute), false).FirstOrDefault();
            Console.WriteLine("loaded module [" + moduleinfo.Name + "]");
            //runMethod = moduleType.GetMethod("Run");
            return moduleinfo.Name;
        }

        Thread th = null;

        public void Start()
        {
            Console.WriteLine("starting module [" + moduleinfo.Name + "]");
            var instance = moduleAssembly.CreateInstance(_moduleClass, true);
            moduleInstance = instance as IRobotModule;
            if (moduleInstance == null)
            {
                Console.WriteLine("MODULE " + moduleinfo.Name + " MUST IMPLEMENT IROBOTMODULE");
                return;// null;
            }
            else
            {
                //runMethod.Invoke(instance, null);
                moduleInstance.Initialize(immContext, _moduleSettings);
                moduleInstance.Run();
                return; // moduleInstance;
            }
        }

        public void Stop()
        {
            if (moduleInstance != null)
                moduleInstance.Terminate();
        }


        internal void PropagateMessage(InterModuleMessage msg)
        {
            try
            {
                if (moduleInstance != null)
                    moduleInstance.SendData(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("[BootStrapper] : ERROR while sending message to module : \r\n   {0}",msg.ToString()));
            }
        }
    }
}
