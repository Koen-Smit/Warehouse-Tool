using System.Net.Http.Headers;
using WMT.Commands;

namespace WMT
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0 || args[0] != "--no-menu")
            {
                await RunMenu();
            }
            else
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("Invalid command. Use 'help'");
                    return;
                }

                await RunCommand(args[1]);
            }
        }

        static async Task RunMenu()
        {
            while (true)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Read JSON");
                Console.WriteLine("2. Write JSON");
                Console.WriteLine("3. Read JSON for items");
                Console.WriteLine("4. Write JSON for items");
                Console.WriteLine("0. Exit");

                string choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        await Json.Read();
                        break;
                    case "2":
                        await Json.Write();
                        break;
                    case "3":
                        await Json.ItemRead();
                        break;
                    case "4":
                        await Json.ItemWrite();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please choose again.");
                        break;
                }

                Console.WriteLine("Press enter to continue.");
                Console.ReadLine();
                Console.Clear();
            }
        }

        static async Task RunCommand(string command)
        {
            var validCommands = new List<string> { "help", "json.read", "json.write", "item.read", "item.write" };

            // Checks if the command is valid.
            if (!validCommands.Contains(command))
            {
                Console.WriteLine("Invalid command. Use 'help'");
                return;
            }

            // Executes the chosen command.
            switch (command)
            {
                case "help":
                    // Shows a list of all commands.
                    CommandHelp.Execute();
                    break;
                case "json.read":
                    // reads a json file and sends it to the api
                    await Json.Read();
                    break;
                case "json.write":
                    // Writes a json file from the api
                    await Json.Write();
                    break;
                case "item.read":
                    // reads a json file and sends it to the api for items
                    await Json.ItemRead();
                    break;
                case "item.write":
                    // Writes a json file from the api for items
                    await Json.ItemWrite();
                    break;
                default:
                    Console.WriteLine("Invalid command. Use 'help'");
                    break;
            }

            // Waits for the user to press enter before clearing the screen.
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
}
