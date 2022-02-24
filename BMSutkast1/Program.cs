using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace BMS
{
    internal class Program
    {

        static async Task Main(string[] args)
        {
            var display = new Display();
            await display.MainMenu();
        }

    }
}
