using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SugarReader
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

            if (!Regex.IsMatch(Properties.Settings.Default.UserAPIurl, @"^https:\/\/sugarmate.io\/api\/v1\/.{6}\/latest.json$"))
            {
                Properties.Settings.Default.UserAPIurl = Microsoft.VisualBasic.Interaction.InputBox("What is your SugarMate API url?", "Program Initialize");
                Properties.Settings.Default.Save();
            }

            if (Regex.IsMatch(Properties.Settings.Default.UserAPIurl, @"^https:\/\/sugarmate.io\/api\/v1\/.{6}\/latest.json$"))
            {
                Application.Run(new MainForm());
            }
        }
    }
}
