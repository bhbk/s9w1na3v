using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bmon.Client.Lib.Models
{
    internal abstract class BaseDevourConfig
    {

    }

    public enum FileAction
    {
        AbortIfExist,
        OverwriteIfExist
    }
}
