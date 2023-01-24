using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.BLL.package
{
    internal interface IPackageManager
    {
        void NewPackage(Guid[] package);
        void AcquireNewPackage(string username);
    }
}
