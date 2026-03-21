using System.CommandLine;

using Microsoft.Extensions.DependencyInjection;

using PasswordGenerator.Services;

namespace PasswordGenerator;

internal class Program
{
    static async Task<int> Main(string[] args)
    {
        ServiceCollection services = new();

        services.AddScoped<IPasswordService, PasswordService>();
        services.AddSingleton<IPasswordBank, PasswordBank>();

        using ServiceProvider provider = services.BuildServiceProvider();

        var rootCommand = new RootCommand("Password Bank CLI");
        ParseResult parseResult = rootCommand.Parse(args);


        var minLength = new Option<byte>("--length", "--len", "-l")
        {
            Description = "Minimum length of the returned password (absolute min is 8 chars)",
            DefaultValueFactory = parseResult => 8
        };

        //var saveArg = new Argument<string>("");
        var savePassWord = new Option<string>("--save", "-s")
        {
            Description = "Option to save the generated password",
            //need to pass argument for site here
        };

        rootCommand.Options.Add(minLength);
        var siteArg = new Argument<string>("site")
        {
            Description = "Identifier for site belonging to password"
        };

        var getCmd = new Command("--get", "Retrieves a password")
        {
            
        };

        getCmd.Arguments.Add(siteArg);

        rootCommand.Subcommands.Add(getCmd);

        rootCommand.SetAction(parseResult =>
        {
            var pWordService = provider.GetService<IPasswordService>()!;

            //option to save new password with given site
            var userRequestLength = parseResult.GetValue(minLength);

            Console.WriteLine(pWordService.GeneratePassword(userRequestLength));
            return 0;
        });


        getCmd.SetAction((parseResult) =>
        {
            try
            {
                var siteIdentifier = parseResult.GetValue(siteArg);
                if(siteIdentifier == null)
                {
                    throw new ArgumentNullException(siteIdentifier);
                }


                var pWordService = provider.GetService<IPasswordService>()!;
                

                pWordService.GetStoredPassword(siteIdentifier!);
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return 1;
            }

            return 0;
        });


        //Add
        var addCmd = new Command("--add", "Adds password to test bank")
        {
            new Argument<(string,string)>("siteValue")
        };

        addCmd.SetAction((parsedResult) =>
        {

        });
        //update 
        //remove

        return parseResult.Invoke();
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