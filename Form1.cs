using Bluegrams.Application;
using Shojy.FF7.Elena.Converters;
using Shojy.FF7.Elena;
using System.Diagnostics;
using System.Drawing.Imaging;
using NeonStream.Constants;

namespace NeonStream
{
    public partial class Form1 : Form
    {
        private static Process FF7 { get; set; }
        private static NativeMemoryReader MemoryReader { get; set; }
        private static FF7SaveMap SaveMap { get; set; }
        public static FF7BattleMap BattleMap { get; set; }

        public Form1()
        {
            InitializeComponent();
            PortableSettingsProvider.ApplyProvider(Properties.Settings.Default);
            Starting.Text = ("starting...");
            Process ff7 = null;
            MemoryReader = new NativeMemoryReader(FF7);
            var saveMapByteData = MemoryReader.ReadMemory(new IntPtr(Addresses.SaveMapStart), 4342);
            var isBattle = MemoryReader.ReadMemory(new IntPtr(Addresses.ActiveBattleState), 1).First();
            var battleMapByteData = MemoryReader.ReadMemory(new IntPtr(Addresses.BattleMapStart), 0x750);
            var colors = MemoryReader.ReadMemory(new IntPtr(Addresses.WindowColorBlockStart), 16);
            SaveMap = new FF7SaveMap(saveMapByteData);
            BattleMap = new FF7BattleMap(battleMapByteData, isBattle);

            SaveMap.WindowColorTopLeft = $"{colors[0x2]:X2}{colors[0x1]:X2}{colors[0x0]:X2}";
            SaveMap.WindowColorBottomLeft = $"{colors[0x6]:X2}{colors[0x5]:X2}{colors[0x4]:X2}";
            SaveMap.WindowColorTopRight = $"{colors[0xA]:X2}{colors[0x9]:X2}{colors[0x8]:X2}";
            SaveMap.WindowColorBottomRight = $"{colors[0xE]:X2}{colors[0xD]:X2}{colors[0xC]:X2}";
        }
    }
}