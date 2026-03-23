using System.CommandLine;

using Microsoft.Extensions.DependencyInjection;

using PasswordGenerator.Services;

namespace PasswordGenerator;

internal class Program
{
    static async Task<int> Main(string[] args)
    {
        using var provider = BuildServices();

        var rootcmd = BuildRootCommand(provider);

        ParseResult parseResult = rootcmd.Parse(args);

        return parseResult.Invoke();
    }

    private static ServiceProvider BuildServices()
    {
        ServiceCollection services = new();

        services.AddScoped<IPasswordService, PasswordService>();
        services.AddSingleton<IPasswordBank, PasswordBank>();

        return services.BuildServiceProvider();
    }

    private static RootCommand BuildRootCommand(ServiceProvider provider)
    {
        var root = new RootCommand("Password Bank CLI");

        var minLength = new Option<byte>("--length", aliases: ["--len", "-l"])
        {
            Description = "Minimum length of the returned password (absolute min is 8 chars)",
            DefaultValueFactory = parseResult => 8
        };

        var savePassWord = new Option<string>("--save", "-s")
        {
            Description = "Save the generated password for the given site"
        };

        root.Options.Add(minLength);
        root.Options.Add(savePassWord);
        root.Subcommands.Add(BuildGetCommand(provider));
        root.Subcommands.Add(BuildAddCommand(provider));
        root.Subcommands.Add(BuildUpdateCommand(provider));
        root.Subcommands.Add(BuildDeleteCommand(provider));

        root.SetAction(parseResult =>
        {
            var pWordService = provider.GetService<IPasswordService>()!;

            try
            {
                var userRequestLength = parseResult.GetValue(minLength);
                var siteToSave = parseResult.GetValue(savePassword);

                var generated = pWordService.GeneratePassword(userRequestLength);
                if (siteToSave is not null)
                {
                    pWordService.StorePassword(siteToSave, generated);
                    Console.WriteLine($"Saved password for site '{siteToSave}'.");
                }
                else
                {
                    Console.WriteLine($"Generated Password: {generated}");
                }
                return 0;
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return 1;
            }
        });
        return root;
    }


    private static Command BuildGetCommand(ServiceProvider provider)
    {
        var siteArg = new Argument<string>("site")
        {
            Description = "Identifier for site belonging to password"
        };
        var getCmd = new Command("--get", "Retrieves a password")
        {

        };
        getCmd.SetAction((parseResult) =>
        {
            try
            {
                var siteIdentifier = parseResult.GetValue(siteArg);
                if (siteIdentifier == null)
                {
                    throw new ArgumentNullException(siteIdentifier);
                }


                var pWordService = provider.GetService<IPasswordService>()!;


                Console.WriteLine(pWordService.GetStoredPassword(siteIdentifier!));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return 1;
            }

            return 0;
        });
        getCmd.Arguments.Add(siteArg);

        return getCmd;
    }
    private static Command BuildAddCommand(ServiceProvider provider)
    {

        var siteArg = new Argument<string>("siteValue");
        var passwordArg = new Argument<string>("passwordValue");
        var addCmd = new Command("--add", "Adds password to test bank")
        {
            
        };

        addCmd.Arguments.Add(siteArg);
        addCmd.Arguments.Add(passwordArg);

        addCmd.SetAction((parsedResult) =>
        {
            try
            {


                var siteId = parsedResult.GetValue(siteArg);
                var passwordId = parsedResult.GetValue(passwordArg);

                var pWordService = provider.GetService<IPasswordService>()!;
                if (!string.IsNullOrWhiteSpace(siteId) && !string.IsNullOrWhiteSpace(passwordId))
                {
                    pWordService.StorePassword(siteId, passwordId);

                    Console.WriteLine($"Password Saved to : {siteId}");
                    return 0;
                }
                throw new ArgumentNullException("Argument for saving password was left empty");
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return 1;
            }
            
        });
        return addCmd;
    }
    private static Command BuildUpdateCommand(ServiceProvider provider)
    {
        throw new NotImplementedException();
        /*
         * 
         * can either pass in site and password 
         *     - check bank against passed in site then update value
         * or can generate new password and update site with generated password
         * 
         * 
         */
    }
    private static Command BuildDeleteCommand(ServiceProvider provider)
    {
        throw new NotImplementedException();
        /*
         * 
         * pass in site and delte if available
         * 
         */
    }
}

/*
 * 
 * command line password bank
 * create (either insert or generate new)
 *      -option to save generated password
 *      -argument for how long it needs to be
 * read (give site --> get password)
 * update (give site --> new password)
 * delete (give site --> delete)
 */