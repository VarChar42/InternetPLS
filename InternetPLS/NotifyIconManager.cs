#region usings

using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

#endregion

namespace InternetPLS;

public static class NotifyIconManager
{
    #region Constants and Fields

    public static readonly Icon AppIcon;
    public static readonly Icon ErrorIcon;
    private static NotifyIcon notifyIconInstance;
    public static readonly Icon SuccessIcon;

    #endregion

    static NotifyIconManager()
    {
        SuccessIcon = IconUtils.GetIconFromResources("success");
        ErrorIcon = IconUtils.GetIconFromResources("error");
        string executingAssemblyExe = Assembly.GetExecutingAssembly()?.GetName().Name + ".exe";

        AppIcon = AppIcon = Icon.ExtractAssociatedIcon(executingAssemblyExe);
    }

    public static void SetIcon(Icon icon)
    {
        notifyIconInstance.Icon = icon;
    }

    public static void Setup()
    {
        notifyIconInstance ??= new NotifyIcon();
        notifyIconInstance.Text = "InternetPLS";
        notifyIconInstance.Icon = AppIcon;

        notifyIconInstance.Click += NotifyIconInstance_OnClick;
        notifyIconInstance.Visible = true;
    }

    #region Event Handlers

    private static void NotifyIconInstance_OnClick(object? sender, EventArgs e)
    {
        VisibilityManager.Toggle();
    }

    #endregion
}
