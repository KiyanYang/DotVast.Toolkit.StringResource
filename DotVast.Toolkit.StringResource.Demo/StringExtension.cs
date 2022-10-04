namespace DotVast.Toolkit.StringResource.Demo;

internal static class StringExtension
{
    public static string GetLocalized(this string str)
    {
        var val = VirtualGetLocalized(str);
        return val;
    }

    private static string VirtualGetLocalized(string str) =>
        $"Localized {str}";
}
