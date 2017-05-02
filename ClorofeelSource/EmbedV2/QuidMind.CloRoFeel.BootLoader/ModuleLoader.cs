using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using QuidMind.CloRoFeel.CoreKernel;

using System.Threading;

namespace QuidMind.CloRoFeel.BootLoader
{
    class ModuleRunContextContainer
    {
        public string ModuleName { get; set; }
        public ModuleRunContext Context { get; set; }
    }

    class ModuleLoader
    {
        //static List<AppDomain> moduleAppDomains = new List<AppDomain>();
        static Dictionary<string, ModuleRunContextContainer> modulesRunContextList = new Dictionary<string, ModuleRunContextContainer>();

        public ModuleLoader()
        {
            InterModuleMessaging.Current.MessageArrived += new InterModuleMessageArrivedDelegate(Current_MessageArrived);
        }

        void Current_MessageArrived(InterModuleMessaging immContext)
        {
            ThreadPool.QueueUserWorkItem((s) => {
                try
                {
                    var msg = immContext.ExtractMessage();
                    if (modulesRunContextList.Keys.Contains(msg.TargetModule))
                        modulesRunContextList[msg.TargetModule].Context.BootstrapperInstance.PropagateMessage(msg);
                    else
                    {
                        Console.WriteLine("[ModuleLoader] : unknow target module for message " + msg.ToString());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("[ModuleLoader] : ERROR WHILE FORWARDING MESSAGE : \r\n {0}",immContext.ToString()));
                }
            });
            
        }
        void LoadModule(string moduleAssemblyName, string moduleClassName,string moduleSettings)
        {
            AppDomainSetup setup = AppDomain.CurrentDomain.SetupInformation;
            //setup.PrivateBinPathProbe = setup.ApplicationBase + ";" + Path.GetDirectoryName(moduleAssemblyName);
            setup.PrivateBinPath = setup.ApplicationBase + ";" + Path.GetDirectoryName(moduleAssemblyName);
            //setup.ApplicationBase = Path.GetDirectoryName(moduleAssemblyName);
            setup.ConfigurationFile = moduleAssemblyName + ".config";
            setup.ShadowCopyFiles = "true";

            var moduleAppDomain = AppDomain.CreateDomain("MODULE_" + moduleClassName, null, setup);
            
            // Remplacer le module qui suit par une instanciation dynamique du module et son lancement dans l'appdomain.
            
            var moduleBootStrapper = (Bootstrapper)moduleAppDomain.CreateInstanceAndUnwrap(
                typeof(Bootstrapper).Assembly.FullName, typeof(Bootstrapper).FullName);

            var moduleName = moduleBootStrapper.Load(InterModuleMessaging.Current, moduleAssemblyName, moduleClassName,moduleSettings);

            IRobotModule loadedModule = null;
            moduleBootStrapper.Start();
            //var loadedModule = moduleBootStrapper.Start();
            //moduleAppDomains.Add(moduleAppDomain);
            ModuleRunContext mrc = new ModuleRunContext(moduleAppDomain, moduleBootStrapper, loadedModule);
            ModuleRunContextContainer container = new ModuleRunContextContainer() 
                {  ModuleName = moduleName, Context = mrc };
            modulesRunContextList.Add(container.ModuleName, container);
        }


        public void LoadModules()
        {
            if (!File.Exists("Bootloader.cfil")) 
            {
                Console.WriteLine("ERROR : configuraiton file [Bootloaded.cfil] missing. Stopping");
                return;
            }
            try
            {
                XElement xeBootloaderConfig = XElement.Load("Bootloader.cfil");
                XElement xeModules = xeBootloaderConfig.Element("Modules");
                string modulesRootFolder = xeModules.Attribute("folder").Value;
                foreach (var xeModule in xeModules.Elements("Module"))
                {
                    string name = xeModule.Attribute("name").Value;
                    string subFolder = xeModule.Attribute("folder").Value;
                    string assembly = xeModule.Attribute("assembly").Value;
                    string className = xeModule.Attribute("class").Value;
                    bool isPublic = bool.Parse(xeModule.Attribute("isPublic").Value);
                    string moduleSettings = null;
                    if (xeModule.Attribute("settings")!=null)
                        moduleSettings = xeModule.Attribute("settings").Value;

                    string assemblyFileName = Path.Combine(Environment.CurrentDirectory, modulesRootFolder, subFolder, assembly);

                    try
                    {
                        LoadModule(assemblyFileName, className,moduleSettings);
                    }
                    catch (Exception exmod)
                    {
                        Console.WriteLine("Error loading module " + name + " [" + exmod.GetType().Name + "] " + assemblyFileName + "/"  + className + "\r\n" + exmod.ToString());
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("*** ERROR WHILE LOADING MODULE : " + ex.ToString());
            }
        }


        public void TerminateAllModules()
        {
            foreach (var mrcc in modulesRunContextList.Values)
            {
                AppDomain.Unload(mrcc.Context.AppDomainInstance);
                mrcc.Context.Dispose();
            }
            modulesRunContextList.Clear();
        }
    }
}
