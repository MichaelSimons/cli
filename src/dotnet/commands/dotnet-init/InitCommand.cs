using System;
using Microsoft.DotNet.Cli;
using Microsoft.DotNet.Cli.CommandLine;
using Microsoft.DotNet.Cli.Utils;
using Microsoft.DotNet.Configurer;

namespace Microsoft.DotNet.Tools.Init
{
    public class InitCommand
    {

        public static int Run(string[] args)
        {
            DebugHelper.HandleDebugSwitch(ref args);

            var app = new CommandLineApplication();
            app.Name = "dotnet init";
            app.FullName = ".NET Initializer";
            app.Description = "Initializes the dotnet CLI";
            app.HelpOption("-h|--help");

            //var output = app.Option("-o|--output <OUTPUT_DIR>", "Directory in which to place outputs", CommandOptionType.SingleValue);
            //var path = app.Argument("<PROJECT>", "The project to compile, defaults to the current directory. Can be a path to a project.json or a project directory");

            app.OnExecute(() =>
            {
                using (INuGetCacheSentinel nugetCacheSentinel = new NuGetCacheSentinel())
                {
                    ConfigureDotNetForFirstTimeUse(nugetCacheSentinel);
                }

                return 0;
            });

            try
            {
                return app.Execute(args);
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.Error.WriteLine(ex);
#else
                Console.Error.WriteLine(ex.Message);
#endif
                return 1;
            }
        }

        public static void ConfigureDotNetForFirstTimeUse(INuGetCacheSentinel nugetCacheSentinel)
        {
            using (PerfTrace.Current.CaptureTiming())
            {
                using (var nugetPackagesArchiver = new NuGetPackagesArchiver())
                {
                    var environmentProvider = new EnvironmentProvider();
                    var commandFactory = new DotNetCommandFactory();
                    var nugetCachePrimer =
                        new NuGetCachePrimer(commandFactory, nugetPackagesArchiver, nugetCacheSentinel);
                    var dotnetConfigurer = new DotnetFirstTimeUseConfigurer(
                        nugetCachePrimer,
                        nugetCacheSentinel,
                        environmentProvider);

                    dotnetConfigurer.Configure();
                }
            }
        }
    }
}
