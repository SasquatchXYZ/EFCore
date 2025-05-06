using FirstEFCoreApp;

Console.WriteLine(
    "Commands: l (List), u (Change Url), r (Reset DB) and e (Exit) - add `-l` to the first two for logs");

Console.Write(
    "Checking if database exists... ");

Console.WriteLine(Commands.WipeCreateSeed(true)
    ? "Created database and seeded it"
    : "Database exists.");

do
{
    Console.Write("> ");
    var command = Console.ReadLine();
    switch (command)
    {
        case "l":
            Commands.ListAll();
            break;
        case "u":
            Commands.ChangeWebUrl();
            break;
        case "l -l":
            Commands.ListAllWithLogs();
            break;
        case "u -l":
            Commands.ChangeWebUrlWithLogs();
            break;
        case "r":
            Commands.WipeCreateSeed(false);
            break;
        case "e":
            return;
        default:
            Console.WriteLine("Unknown command.");
            break;
    }
} while (true);
