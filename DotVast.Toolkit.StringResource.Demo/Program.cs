using DotVast.Toolkit.StringResource.Demo.Strings;

namespace DotVast.Toolkit.StringResource.Demo;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(Resw.TestA);
        Console.WriteLine(Resw.TestB);
        Console.WriteLine(Resw.TestC);
        Console.WriteLine(Resw.TestD);
        //Console.WriteLine(Resw.TestE); // error
        Console.WriteLine(ReswDialog.TestDialog);
        Console.WriteLine(ReswEx.TestA);
        Console.WriteLine(ReswEx.TestB);
        Console.WriteLine(ReswEx.TestC);
        Console.WriteLine(ReswEx.TestD);
        //Console.WriteLine(ReswEx.TestE); // error
        Console.WriteLine(ReswDialogEx.TestDialog);
    }
}

// Outputs:
// TestA
// TestB
// TestC
// TestD
// TestDialog
// Localized TestA
// Localized TestB
// Localized TestC
// Localized TestD
// Localized TestDialog
