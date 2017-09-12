using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bmon.Client.Lib.Models
{
    public abstract class DevourConfig
    {
        DevourConfig() { }
    }

    public enum FileAction
    {
        AbortIfExist,
        OverwriteIfExist
    }
}
