namespace DotVast.Toolkit.StringResource.Demo;

internal static class StringExtension
{
    public static string GetLocalized(this string str)
    {
        var val = VirtualGetLocalized(str);
        return val;
    }

    public static string GetLocalized(this string str, string arg)
    {
        var val = VirtualGetLocalized(str);
        return $"Val: {val}, Arg: {arg}";
    }

    private static string VirtualGetLocalized(string str) =>
        $"Localized {str}";
}
