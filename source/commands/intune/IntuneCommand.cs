﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maestro
{
    public class IntuneCommand : ExecutedCommand
    {
        public IntuneCommand(Dictionary<string, string> arguments) : base(arguments) { }

        public static async Task Execute(Dictionary<string, string> arguments, LiteDBHandler database, bool databaseOnly, bool reauth, int prtMethod)
        {
            if (arguments.TryGetValue("subcommand1", out string subcommandName))
            {
                if (subcommandName == "devices")
                {
                    await IntuneDevicesCommand.Execute(arguments, database, databaseOnly, reauth, prtMethod);
                }
                else if (subcommandName == "exec")
                {
                    //await IntuneExecCommand.Execute(arguments, database, reauth);
                }
                else if (subcommandName == "scripts")
                {
                    await IntuneScriptsCommand.Execute(arguments, database, databaseOnly, reauth, prtMethod);
                }
                else if (subcommandName == "sync")
                {
                    await IntuneSyncCommand.Execute(arguments, database, reauth, prtMethod);
                }
            }
            else
            {
                Logger.Error("Missing arguments for \"intune\" command");
                CommandLine.PrintUsage("intune");
            }
        }
    }
}
