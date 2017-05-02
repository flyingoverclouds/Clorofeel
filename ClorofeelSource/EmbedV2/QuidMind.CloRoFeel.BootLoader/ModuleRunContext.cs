using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuidMind.CloRoFeel.CoreKernel;

namespace QuidMind.CloRoFeel.BootLoader
{
    class ModuleRunContext : IDisposable
    {
        public ModuleRunContext(AppDomain appd,Bootstrapper bs, IRobotModule module)
        {
            this._appDomain = appd;
            this._bootstrapperInstance = bs;
            this._loadedModule = module;
        }
        AppDomain _appDomain = null;
        public AppDomain AppDomainInstance
        {
            get { return _appDomain; }
        }
        Bootstrapper _bootstrapperInstance = null;
        public Bootstrapper BootstrapperInstance
        {
            get { return _bootstrapperInstance; }
        }
        IRobotModule _loadedModule = null;
        public IRobotModule LoadedModule
        {
            get { return _loadedModule; }
            set { _loadedModule = value; }
        }

        public void Dispose()
        {
            this._appDomain = null;
            this._bootstrapperInstance = null;
            this._loadedModule = null;
        }

        //public void NewMessage(InterModuleMessage immsg)
        //{
        //    //Console.WriteLine("[ModuleRunContext] Message message arrvice in module " + immsg.Body);
            
        //    if (_bootstrapperInstance != null)
        //        _loadedModule.SendData(immsg);
        //}

        void UnloadAppDomain()
        {
            
        }
    }
}
