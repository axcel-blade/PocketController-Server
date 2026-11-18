using System.Runtime.InteropServices;

namespace PocketControllerServer;

/// <summary>
/// Ensures only one PocketController Server process runs. A second launch
/// asks the existing window (including when hidden in the tray) to show itself.
/// </summary>
internal static class SingleInstance
{
    private const string MutexName = @"Local\AxcelBlade.PocketControllerServer";

    /// <summary>Unique Win32 message used to ask a running instance to restore its window.</summary>
    public static readonly int WmShowInstance =
        NativeMethods.RegisterWindowMessage("AxcelBlade.PocketControllerServer.ShowInstance");

    private static Mutex? _mutex;

    /// <summary>
    /// Tries to become the sole running instance.
    /// Returns <c>false</c> when another instance already holds the lock.
    /// </summary>
    public static bool TryAcquire()
    {
        _mutex = new Mutex(initiallyOwned: false, name: MutexName);
        try
        {
            return _mutex.WaitOne(0, exitContext: false);
        }
        catch (AbandonedMutexException)
        {
            // Previous process crashed while holding the mutex; we own it now.
            return true;
        }
    }

    /// <summary>Releases the single-instance lock.</summary>
    public static void Release()
    {
        if (_mutex is null) return;
        try { _mutex.ReleaseMutex(); } catch (ApplicationException) { /* not owned */ }
        _mutex.Dispose();
        _mutex = null;
    }

    /// <summary>Asks the already-running instance to show its main window.</summary>
    public static void ActivateExisting()
    {
        _ = NativeMethods.PostMessage(
            (IntPtr)NativeMethods.HwndBroadcast,
            WmShowInstance,
            IntPtr.Zero,
            IntPtr.Zero);
    }

    private static class NativeMethods
    {
        public const uint HwndBroadcast = 0xffff;

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int RegisterWindowMessage(string lpString);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
    }
}
