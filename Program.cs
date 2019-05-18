using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using CommandLine;

namespace InfiniteKnightsSaveEditor
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                args = new string[] { "--help" };
            }

            return CommandLine.Parser.Default.ParseArguments<Options>(args)
                .MapResult(RunWithOptions, HandleParseError);
        }

        private static int RunWithOptions(Options options)
        {
            try
            {
                options.Validate();

                var data = SaveData.Load(options.Input);

                if (options.Gem != null)
                {
                    data.Gem = int.Parse(options.Gem);
                }

                if (options.AdsTicket != null)
                {
                    data.AdsTicket = int.Parse(options.AdsTicket);
                }

                if (options.ElfStone != null)
                {
                    data.ElfStone = BigInteger.Parse(options.ElfStone);
                }

                if (options.Coin != null)
                {
                    data.Coin = BigInteger.Parse(options.Coin);
                }

                if (options.Vip == "1")
                {
                    data.SetVip(true);
                }
                else if (options.Vip == "0")
                {
                    data.SetVip(false);
                }
                else if (Regex.IsMatch(options.Vip, @"^\d{8}$"))
                {
                    data.SetVip(true, DateTime.ParseExact(options.Vip, "yyyyMMdd", CultureInfo.CurrentCulture));
                }

                var output = options.Output ?? options.Input;

                if (File.Exists(output) && !options.Yes)
                {
                    Console.Write("Overwrite existing file? [y/N] ");

                    var answer = Console.ReadLine().Trim().ToLower();

                    if (answer != "y" && answer != "yes")
                    {
                        throw new AbortedException();
                    }
                }

                if (output.ToLower().EndsWith(".json"))
                {
                    File.WriteAllText(output, data.ToJson());
                }
                else
                {
                    data.Save(output);
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return 1;
            }
        }

        private static int HandleParseError(IEnumerable<Error> errors)
        {
            if (errors.Count() == 1 && errors.First() is HelpRequestedError)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}
