using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Configuration.Install;
using System.Collections;

namespace IQTrader
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                var thr = new Thread(delegate()
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    var frm = new MainForm();
                    Application.Run(frm);
                });
                thr.SetApartmentState(ApartmentState.STA);
                thr.Start();
            }
            else
            {
                switch (args[0].ToLower())
                {
                    case "service":
                        var services = new ServiceBase[] { new IQTraderService() };
                        ServiceBase.Run(services);
                        break;

                    case "start":
                        var starter = new ServiceController(IQTraderService.MyServiceName);
                        starter.Start();
                        break;

                    case "stopper":
                        var stopper = new ServiceController(IQTraderService.MyServiceName);
                        stopper.Stop();
                        break;

                    case "reg":
                    case "register":
                        var installer = new AssemblyInstaller(typeof(IQTraderService).Assembly, null);
                        installer.UseNewContext = true;
                        var state = new Hashtable();
                        try
                        {
                            installer.Install(state);
                            installer.Commit(state);
                        }
                        catch
                        {
                            installer.Rollback(state);
                        }
                        break;

                    case "unreg":
                    case "unregister":
                        var remover = new AssemblyInstaller(typeof(IQTraderService).Assembly, null);
                        remover.UseNewContext = true;
                        remover.Uninstall(null);
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
