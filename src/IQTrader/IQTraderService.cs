using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace IQTrader
{
    public partial class IQTraderService : ServiceBase
    {
        public IQTraderService()
        {
            InitializeComponent();
            ServiceName = MyServiceName;
        }

        public const string MyServiceName = "IQTrader";
        public const string MyDisplayName = "IQ Trader Service";

        protected override void OnStart(string[] args)
        {
            var thr = new Thread(delegate()
            {
                var frm = new MainForm();
                Application.Run(frm);
            });
            thr.SetApartmentState(ApartmentState.STA);
            thr.Start();
            while (!thr.IsAlive) ;
        }

        protected override void OnStop()
        {
            Application.Exit();
        }
    }
}
