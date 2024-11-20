using SeverChitChat;
using System;
using System.Windows.Forms;

namespace ServerChitChat
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ServerForm()); // Khởi động form ServerChitChat
        }
    }
}
