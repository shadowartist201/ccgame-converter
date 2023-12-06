using System.Text.RegularExpressions;

string input, output;
int fileCount;
string[] name_array, path_array;

if (args.Length == 0)
{
    Console.WriteLine("Usage: ConsoleApp1.exe <folder>");
    return -1;
}

Console.WriteLine("Looking for XCabInfo.resources...");
try
{
    input = File.ReadAllText(args[0] + "\\" + "XCabInfo.resources");
    input = Regex.Replace(input, @"\x06\x02", ".....");
    input = Regex.Replace(input, @"[\x00-\x20]", " ");
    output = input.Substring(input.IndexOf(".....") + 2);
}
catch 
{
    Console.WriteLine("Unable to find XCabInfo.resources. Exiting...");
    return -1;
}

fileCount = Directory.GetFiles(args[0]).Length - 1;
name_array = new string[fileCount];
path_array = new string[fileCount];

Console.WriteLine("XCabInfo.resources found. Finding file names...");

for (int i = 0; i < fileCount; i++)
{
    output = output.Remove(0, 7);
    name_array[i] = Regex.Match(output, @"^\w+").Value;
    output = output.Remove(0, Regex.Match(output, @"^\w+").Value.Length-1);
}

for  (int i = 0; i < fileCount; i++)
{
    output = output.Remove(0, 7);
    name_array[i] = Regex.Match(output, @"(\w|\\)+\s*\w+\.\w{0,4}").Value;
    if (!path_array.Contains(Regex.Match(output, @"(\w+\\)+").Value))
        path_array[i] = Regex.Match(output, @"(\w+\\)+").Value;
    output = output.Remove(0, Regex.Match(output, @"(\w|\\)+\s*\w+\.\w{0,4}").Value.Length - 1);
}

Console.WriteLine("Found file names. Renaming files...");

for (int i = 0; i < path_array.Length; i++)
{
    Directory.CreateDirectory(args[0] + "\\" + path_array[i]);
}

for (int i = 0; i < fileCount; i++)
{
    File.Move(args[0] + "\\" + i.ToString(), args[0] + "\\"  + name_array[i]);
}

Console.WriteLine("Files renamed.");
return 0;