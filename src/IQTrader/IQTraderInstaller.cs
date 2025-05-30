using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using Microsoft.Win32;

namespace IQTrader
{
    [RunInstaller(true)]
    class IQTraderInstaller : Installer
    {
        private ServiceProcessInstaller processInstaller;
        private ServiceInstaller serviceInstaller;

        public IQTraderInstaller()
        {
            processInstaller = new ServiceProcessInstaller();
            processInstaller.Account = ServiceAccount.LocalSystem;
            processInstaller.Username = null;
            processInstaller.Password = null;

            serviceInstaller = new ServiceInstaller();
            serviceInstaller.ServiceName = IQTraderService.MyServiceName;
            serviceInstaller.DisplayName = IQTraderService.MyDisplayName;
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.Description = "IQ Trader is a self trading system based on selected strategy";

            Installers.AddRange(new Installer[] { serviceInstaller, processInstaller });
        }

        protected override void OnAfterInstall(System.Collections.IDictionary savedState)
        {
            using (var key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\services\" + IQTraderService.MyServiceName, true))
            {
                if (key == null)
                    throw new Exception("Failed to open register key");

                key.SetValue("ImagePath", key.GetValue("ImagePath") + " service");
                base.OnAfterInstall(savedState);
            }
        }
    }
}
