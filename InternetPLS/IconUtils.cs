using System.Drawing;
using System.IO;
using System.Reflection;

namespace InternetPLS;

public class IconUtils
{
    public static Icon GetIconFromResources(string name)
    {
        var assembly = Assembly.GetCallingAssembly();
        var resourceName = $"{assembly.GetName().Name}.icons.{name}.ico";
        Stream stream = assembly.GetManifestResourceStream(resourceName);

        return stream == null
            ? null
            : new Icon(stream);
    }
}
