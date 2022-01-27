#region usings

using System;
using System.Runtime.InteropServices;

#endregion

namespace InternetPLS;

public static class VisibilityManager
{
    #region Constants and Fields

    private const int SW_HIDE = 0;
    private const int SW_SHOW = 5;

    private static bool windowShown = true;

    #endregion

    public static void HideWindow()
    {
        IntPtr handle = GetConsoleWindow();
        ShowWindow(handle, SW_HIDE);
        windowShown = false;
    }

    public static void ShowWindow()
    {
        IntPtr handle = GetConsoleWindow();
        ShowWindow(handle, SW_SHOW);
        windowShown = true;
    }

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
    }

    [DllImport("kernel32.dll")]
    static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int sw);
}
