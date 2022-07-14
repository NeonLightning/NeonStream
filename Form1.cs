using Bluegrams.Application;
using Shojy.FF7.Elena.Converters;
using Shojy.FF7.Elena;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace NeonStream
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            PortableSettingsProvider.ApplyProvider(Properties.Settings.Default);
            Starting.Text = ("starting...");
            Process ff7 = null;
            do
            {
                try
                {
                    ff7 ??= Process.GetProcessesByName("ff7_en").FirstOrDefault() ?? Process.GetProcessesByName("ff7").FirstOrDefault();
                }
                catch
                {
                    // Wait and try again
                    Thread.Sleep(250);
                }

            } 
            while (ff7?.MainModule is null) ;

            var ff7Folder = Path.GetDirectoryName(ff7.MainModule.FileName);

            var path = Path.Combine(ff7Folder, "data", "lang-en", "kernel");
            var minigame = Path.Combine(ff7Folder, "data", "minigame");

            // Read Kernel Data
            var reader = new KernelReader(Path.Combine(path, "KERNEL.BIN"))
                .MergeKernel2Data(Path.Combine(path, "kernel2.bin"));

            var weapons = reader.WeaponData.Weapons;

            int temp = 0;
            foreach (var wpn in weapons)
            {
                temp++;
                //Console.WriteLine($"Found a {wpn.Name}!");
                if (temp == 0)
                {
                    var wep1text = wpn.Name;
                    wep1label.Text = wep1text;
                }
                if (temp == 1)
                {
                    wep2label.Text = ("Weapon");
                }
                if (temp == 2)
                {
                    wep3label.Text = ("Weapon");
                }
            }
            temp = 0;
            var mem = new MemoryKernelReader(ff7);

        }
    }
}