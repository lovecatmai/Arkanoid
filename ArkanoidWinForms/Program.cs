using System;
using System.Windows.Forms;

namespace ArkanoidWinForms
{
    /// <summary>
    /// Точка входа в приложение.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Запускает WinForms-приложение с главной игровой формой.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GameForm());
        }
    }
}
