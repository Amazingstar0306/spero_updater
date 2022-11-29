using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPERO_Updater
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!AllRequiredFilesAvailable())
                Environment.Exit(0);

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(LoadFromResourcesFolder);

            string folderPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Resources";
            Directory.SetCurrentDirectory(folderPath);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        static System.Reflection.Assembly LoadFromResourcesFolder(object sender, ResolveEventArgs args)
        {
            AssemblyName MissingAssembly = new AssemblyName(args.Name);

            string folderPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Resources";
            string assemblyPath = Path.Combine(folderPath, MissingAssembly.Name + ".dll");
            if (File.Exists(assemblyPath))
            {
                return Assembly.LoadFrom(assemblyPath);
            }

            CultureInfo ci = MissingAssembly.CultureInfo;
            folderPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Lang\\"+ ci.TwoLetterISOLanguageName;
            assemblyPath = Path.Combine(folderPath, MissingAssembly.Name + ".dll");
            if (File.Exists(assemblyPath))
            {
                return Assembly.LoadFrom(assemblyPath);
            }


            return null;
        }

        private static bool IsFileAvailable(string fileName)
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath) +
              Path.DirectorySeparatorChar + "Resources" + Path.DirectorySeparatorChar;

            if (!File.Exists(path + fileName))
            {
                /*
                MessageBox.Show("The following file could not be found in Resources folder: " + fileName +
                  ".\nPlease copy " + fileName + " file into Resources folder.", "SperoUpdater",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                */
                return false;
            }
            return true;
        }

        private static bool AllRequiredFilesAvailable()
        {
            if (!IsFileAvailable("HidDfu.dll"))
                return false;


            return true;
        }
    }
}
