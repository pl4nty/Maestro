﻿using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Maestro
{
    internal class IntuneExecAppCommand : ExecutedCommand
    {
        public IntuneExecAppCommand(Dictionary<string, string> arguments) : base(arguments) { }
        public async Task Execute(Dictionary<string, string> arguments, LiteDBHandler database, bool reauth)
        {
            if (arguments.TryGetValue("--id", out string groupId) && arguments.TryGetValue("--name", out string appName) 
                && arguments.TryGetValue("--path", out string installationPath))
            {
                // Run as system by default
                string runAsAccount = "system";

                // Run as logged in user if specified
                if (arguments.ContainsKey("--run-as-user"))
                {
                    runAsAccount = "user";
                }

                var intuneClient = new IntuneClient();
                intuneClient = await IntuneClient.InitAndGetAccessToken(database, reauth: reauth);

                //var entraClient = new EntraClient();
                //var entraClient = await EntraClient.InitAndGetAccessToken(this);

                //EntraGroup group = await entraClient.GetGroup(this);
                //if (group is null) return;

                if (!await intuneClient.NewWin32App(groupId, appName, installationPath, runAsAccount)) return;

                Logger.Info("App assignment created, waiting 10 seconds before requesting device sync");
                Thread.Sleep(10000);
                await intuneClient.SyncDevice(groupId, database, skipDeviceLookup: true);
            }
            else
            {
                Logger.Error("Missing arguments for \"app\" command");
                CommandLine.PrintUsage("app");
            }
        }
    }
}
