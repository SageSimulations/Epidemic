using System;
using System.IO;
using Highpoint.XMILE;
using Highpoint.XMILE.ModelGenerator;
using Highpoint.Sage.SystemDynamics.Design;

namespace DataFusion
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            xmile xmile = XMILEReader.ReadModel(@"../../Generated/EpidemicNode.xml");
            IModelSource modelSource = new XMILEWrapper(xmile);
            ModelGenerator mg = new ModelGenerator(modelSource);
            string @class = mg.Class;
            File.WriteAllText(@"../../Generated/EpidemicNode.cs", @class);
        }
    }
}
