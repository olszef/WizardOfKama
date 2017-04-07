using System;

namespace TheWizardOfKama
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            do
            {
                Restart.RestartGame = false;
                using (var game = new Game1())
                    game.Run();
            } while (Restart.RestartGame);

        }
    }
#endif
}
