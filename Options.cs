using System;
using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using CommandLine;

namespace InfiniteKnightsSaveEditor
{
    public class Options
    {
        [Option("gem", HelpText = "Set number of gems.")]
        public string Gem { get; set; }

        [Option("ads-ticket", HelpText = "Set number of ads tickets.")]
        public string AdsTicket { get; set; }

        [Option("elf-stone", HelpText = "Set number of elf stones.")]
        public string ElfStone { get; set; }

        [Option("coin", HelpText = "Set number of coins.")]
        public string Coin { get; set; }

        [Option("vip", HelpText = "Enable or disable VIP. Could be 1, 0 or a date string in yyyyMMdd format (e.g. 20190518) indicating when VIP is purchased. If 1 is provided, purchase date is set to today. VIP is valid for 30 days since purchase date.")]
        public string Vip { get; set; }

        [Option('i', "input", HelpText = "Path of input save data file.")]
        public string Input { get; set; }

        [Option('o', "output", HelpText = "Path of output data file. Defaults to input file path.")]
        public string Output { get; set; }

        [Option('y', "yes", HelpText = "Overwrite existing file without prompt.")]
        public bool Yes { get; set; }

        public void Validate()
        {
            if (Gem != null)
            {
                ValidateInt("gem", Gem, min: 0);
            }

            if (AdsTicket != null)
            {
                ValidateInt("ads ticket", AdsTicket, min: 0);
            }

            if (ElfStone != null)
            {
                ValidateBigInt("elf stone", ElfStone, min: 0);
            }

            if (Coin != null)
            {
                ValidateBigInt("coin", Coin, min: 0);
            }

            if (Vip != null)
            {
                if (!Regex.IsMatch(Vip, @"^(1|0|\d{8})$"))
                {
                    throw new InvalidOptionException("Invalid vip value. Should be 1, 0 or a date string like 20190518.");
                }
                if (Regex.IsMatch(Vip, @"^\d{8}$"))
                {
                    ValidateDate("vip", Vip, "yyyyMMdd");
                }
            }

            if (Input == null)
            {
                throw new InvalidOptionException("Input file is required.");
            }
        }

        private static void ValidateInt(string name, string value, int min = int.MinValue, int max = int.MaxValue)
        {
            int intVal;

            if (!int.TryParse(value, out intVal))
            {
                throw new InvalidOptionException($"Could not parse {name} value. Should be an int32 value.");
            }

            if (intVal < min)
            {
                throw new InvalidOptionException($"Invalid {name} value. Should be greater than or equal to {min}.");
            }

            if (intVal > max)
            {
                throw new InvalidOptionException($"Invalid {name} value. Should be less than or equal to {max}.");
            }
        }

        private static void ValidateBigInt(string name, string value, BigInteger? min = null, BigInteger? max = null)
        {
            BigInteger intVal;

            if (!BigInteger.TryParse(value, out intVal))
            {
                throw new InvalidOptionException($"Could not parse {name} value. Should be an integer value.");
            }

            if (min != null && intVal < min)
            {
                throw new InvalidOptionException($"Invalid {name} value. Should be greater than or equal to {min}.");
            }

            if (max != null && intVal > max)
            {
                throw new InvalidOptionException($"Invalid {name} value. Should be less than or equal to {max}.");
            }
        }

        private static void ValidateDate(string name, string value, string format)
        {
            DateTime dateVal;

            if (!DateTime.TryParseExact(value, format, CultureInfo.CurrentCulture, DateTimeStyles.None, out dateVal))
            {
                throw new InvalidOptionException($"Invalid {name} value. Should be a valid date string in {format} format.");
            }
        }
    }
}
