using System;
using System.Runtime.InteropServices;

namespace InternetPLS;

public static class VisibilityManager
{
    [DllImport("kernel32.dll")]
    static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int sw);

    private const int SW_HIDE = 0;
    private const int SW_SHOW = 5;

    private static bool windowShown = true;

    public static void Toggle()
    {
        if (windowShown)
        {
            HideWindow();
        }
        else
        {
            ShowWindow();
        }

        windowShown = !windowShown;
    }
    
    public static void HideWindow()
    {
        IntPtr handle = GetConsoleWindow();
        ShowWindow(handle, SW_HIDE);
    }

    public static void ShowWindow()
    {
        IntPtr handle = GetConsoleWindow();
        ShowWindow(handle, SW_SHOW);
    }
}
