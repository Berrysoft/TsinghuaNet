using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace TsinghuaNet.CLI
{
    class Program
    {
        public static Task<int> Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (Parser p = new Parser(settings =>
            {
                settings.HelpWriter = Console.Error;
                settings.CaseSensitive = false;
                settings.CaseInsensitiveEnumValues = true;
            }))
            {
                return p.ParseArguments<LoginVerb,
                                        LogoutVerb,
                                        StatusVerb,
                                        OnlineVerb,
                                        DropVerb,
                                        DetailVerb,
                                        SuggestionVerb,
                                        SaveCredentialVerb,
                                        DeleteCredentialVerb>(args).
                         MapResult(RunVerb, RunError);
            }
        }

        private static async Task<int> RunVerb(object opts)
        {
            try
            {
                await ((VerbBase)opts).RunAsync();
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return 1;
            }
        }

        private static Task<int> RunError(IEnumerable<Error> errs)
        {
            if (errs.Any(e => e.Tag == ErrorType.HelpRequestedError || e.Tag == ErrorType.HelpVerbRequestedError || e.Tag == ErrorType.VersionRequestedError))
                return Task.FromResult(0);
            else
                return Task.FromResult(1);
        }
    }
}
