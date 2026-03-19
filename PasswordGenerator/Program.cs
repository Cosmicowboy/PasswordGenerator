using System.CommandLine;

using Microsoft.Extensions.DependencyInjection;

using PasswordGenerator.Services;

namespace PasswordGenerator;

internal class Program
{
    static async Task<int> Main(string[] args)
    {
        ServiceCollection services = new();
        services.AddSingleton<IPasswordService, PasswordService>();

        using ServiceProvider provider = services.BuildServiceProvider();

        //root command
        // generates basic password and returns to user
        var rootCommand = new RootCommand("Password Bank CLI");

        Option<byte> minLength = new("--length", "--len", "-l")
        {
            Description = "Minimum length of the returned password (absolute min is 8 chars)"
        };

        rootCommand.Options.Add(minLength);

        ParseResult parseResult = rootCommand.Parse(args);
        
        rootCommand.SetAction(parseResult =>
        {
            var pWordService = provider.GetService<IPasswordService>()!;
            //if no arguments are given generate password 
            //min length 8 char
            //option for min char length 
            //option to save new password with given site
            var userRequestLength = parseResult.GetValue(minLength);

            Console.WriteLine(pWordService.GeneratePassword(userRequestLength));
            return 0;
        });

        //Get
        var getCmd = new Command("--get", "Retrieves a password")
        {
            new Argument<string>("site")
        };

        getCmd.SetAction((parseResult) =>
        {
            // lookup logic
            //var passWord = passwordServices.GetPassword();
            //write to console
            //write erros to Console.Error.WriteLine()
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