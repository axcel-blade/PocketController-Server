namespace PocketControllerServer
{
    internal static class Program
    {
        /// <summary>Application entry point.</summary>
        [STAThread]
        static void Main()
        {
            if (!SingleInstance.TryAcquire())
            {
                // Already running (often hidden in the tray) — restore that window instead of starting another.
                SingleInstance.ActivateExisting();
                return;
            }

            try
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new MainForm());
            }
            finally
            {
                SingleInstance.Release();
            }
        }
    }
}
