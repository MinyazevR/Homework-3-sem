
if (args.Length != 1)
{
    Console.WriteLine("Wrong number of argumnets");
    return;
}

if (!Directory.Exists(args[0])) {
    Console.WriteLine("Directory not exist");
    return;
}

MyNUnit.MyNUnit.Run(args[0]);
MyNUnit.MyNUnit.OutputInformationAboutTheTests();

