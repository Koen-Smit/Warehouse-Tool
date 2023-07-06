namespace WMT.Commands
{
    internal class CommandHelp
    {
        // Shows a list of all commands.
        public static void Execute()
        {
            Console.WriteLine();
            Console.WriteLine("help: Shows all commands");
            Console.WriteLine("json.read: Reads a json file and sends it to the api");
            Console.WriteLine("json.write: Writes a json file from the api");
            Console.WriteLine("item.read: Reads a json file and sends it to the api for items");
            Console.WriteLine("item.write: Writes a json file from the api for items");

            Console.WriteLine();
        }
    }
}
