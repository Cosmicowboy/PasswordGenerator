using System.CommandLine;

namespace PasswordGenerator;

internal class Program
{
    static async Task<int> Main(string[] args)
    {
        //root command
        // generates basic password and returns to user
        var rootCommand = new RootCommand("Password Bank CLI");

        rootCommand.SetAction(parseResult =>
        {
            //if no arguments are given generate password 
            //min length 8 char
            //option for min char length 
            //option to save new password with given site
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
        ParseResult parseResult = rootCommand.Parse(args);

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