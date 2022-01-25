using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace InternetPLS;

public static class NotifyIconManager
{
    private static NotifyIcon notifyIconInstance;

    public static readonly Icon AppIcon;
    public static readonly Icon SuccessIcon;
    public static readonly Icon ErrorIcon;
    
    static NotifyIconManager()
    {
        SuccessIcon = IconUtils.GetIconFromResources("success");
        ErrorIcon = IconUtils.GetIconFromResources("error");
        AppIcon = AppIcon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);

    }
    
    public static void Setup()
    {
        notifyIconInstance ??= new NotifyIcon();
        notifyIconInstance.Text = "InternetPLS";
        notifyIconInstance.Icon = AppIcon;
        
        notifyIconInstance.Click += NotifyIconInstance_OnClick;
        notifyIconInstance.Visible = true;
    }

    private static void NotifyIconInstance_OnClick(object? sender, EventArgs e)
    {
        Console.WriteLine("Clicked");
    }

    public static void SetIcon(Icon icon)
    {
        notifyIconInstance.Icon = icon;
    }
}
