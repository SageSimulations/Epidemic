using System;
using System.Windows.Forms;
using Core;

namespace DiseaseView
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new InitialUI(MapData.LoadFrom("Data/MapData.dat")));
        }
    }
}
