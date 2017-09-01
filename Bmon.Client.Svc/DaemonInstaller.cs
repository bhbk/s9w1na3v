using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace Bmon.Client.Svc
{
    [RunInstaller(true)]
    public partial class DaemonInstaller : System.Configuration.Install.Installer
    {
        public DaemonInstaller()
        {
            InitializeComponent();
        }
    }
}
