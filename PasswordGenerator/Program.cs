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

        var savePassword = new Option<string>("--save", aliases: ["-s"])
        {
            Description = "Save the generated password for the given site"
        };

        root.Options.Add(minLength);
        root.Options.Add(savePassword);
        root.Subcommands.Add(GetPasswordCommand(provider));
        root.Subcommands.Add(AddPasswordCommand(provider));
        root.Subcommands.Add(UpdatePasswordCommand(provider));
        root.Subcommands.Add(DeletePasswordCommand(provider));

        root.SetAction(parseResult =>
        {
            var pWordService = provider.GetService<IPasswordService>()!;

            try
            {
                var userRequestLength = parseResult.GetValue(minLength);
                var siteToSave = parseResult.GetValue(savePassword);

                var generatedPassword = pWordService.GeneratePassword(userRequestLength);
                if (!string.IsNullOrWhiteSpace(siteToSave))
                {
                    pWordService.StorePassword(siteToSave, generatedPassword);
                    Console.WriteLine($"Saved password : {generatedPassword} for site '{siteToSave}'");
                }
                else
                {
                    Console.WriteLine($"Generated Password: {generatedPassword}");
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return 1;
            }
        });
        return root;
    }


    private static Command GetPasswordCommand(ServiceProvider provider)
    {
        var siteArg = new Argument<string>("site")
        {
            Description = "Identifier for site belonging to password"
        };
        var getCmd = new Command("--get")
        {
            Description = "Retrieves a password",
        };

        getCmd.Arguments.Add(siteArg);

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

                var returnedPassword = pWordService.GetStoredPassword(siteIdentifier!);
                Console.WriteLine($"returned password : {returnedPassword}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return 1;
            }

            return 0;
        });

        return getCmd;
    }
    private static Command AddPasswordCommand(ServiceProvider provider)
    {

        var siteArg = new Argument<string>("siteValue");
        var passwordArg = new Argument<string>("passwordValue");
        var addCmd = new Command("--add")
        {
            Description = "Adds password to test bank",
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
                if (string.IsNullOrWhiteSpace(siteId) || string.IsNullOrWhiteSpace(passwordId))
                {
                    throw new ArgumentException("Argument(s) for saving password were empty");
                }

                pWordService.StorePassword(siteId, passwordId);
                Console.WriteLine($"Password Saved to : {siteId}");

                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return 1;
            }

        });
        return addCmd;
    }
    private static Command UpdatePasswordCommand(ServiceProvider provider)
    {
        var updateArg = new Argument<string>("site")
        {
            Description = "identifier of the site stored with the password",
        };

        //Generate as option not default behavior
        //this can avoid accidental overwrites with generated passwords
        var generateOption = new Option<string>("--generate", aliases: ["-g", "-gen"]);

        var updateCmd = new Command("--update")
        {
            Description = "updates the specified sites password"
        };

        updateCmd.Arguments.Add(updateArg);
        updateCmd.Options.Add(generateOption);

        /*
         * can either pass in site and password 
         *     - check bank against passed in site then update value
         * or can generate new password and update site with generated password
         */

        //TODO: add in logic for option to generate password
        updateCmd.SetAction((parseResult) =>
        {
            try
            {
                var siteId = parseResult.GetValue(updateArg);

                var pWordService = provider.GetService<IPasswordService>()!;

                if (string.IsNullOrWhiteSpace(siteId))
                {
                    throw new ArgumentException($"Empty argument for {nameof(siteId)}");
                }

                Console.WriteLine($"Password for site: {siteId} was updated");
                return 0;
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return 1;
            }

        });


        return updateCmd;
    }

    private static Command DeletePasswordCommand(ServiceProvider provider)
    {
        var deleteArg = new Argument<string>("site")
        {
            Description = "Deletes the password for the given site"
        };
        var deleteCmd = new Command("--delete")
        {
            Description = "Deltes Password for related site"
        };

        deleteCmd.SetAction((parseResult) =>
        {
            var siteIdentifier = parseResult.GetValue(deleteArg);
            try
            {
                if (siteIdentifier == null)
                {
                    throw new ArgumentNullException(siteIdentifier);
                }
                var pWordService = provider.GetService<IPasswordService>()!;

                pWordService.DeleteStoredPassword(siteIdentifier);

                Console.WriteLine($"Delted: {siteIdentifier} from stored passwords");
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error removing {siteIdentifier} from stored passwords \n{ex.Message}");
                return 1;
            }
        });

        return deleteCmd;
    }
}