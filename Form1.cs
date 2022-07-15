using Bluegrams.Application;
using NeonStream.Constants;
using NeonStream.GameData;
using NeonStream.Models;
using Shojy.FF7.Elena;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Timers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Accessory = NeonStream.Models.Accessory;
using Character = NeonStream.GameData.Character;
using Timer = System.Timers.Timer;
using Weapon = NeonStream.Models.Weapon;

namespace NeonStream
{
    public partial class Form1 : Form
    {
        #region Public Properties
        private static Process FF7 { get; set; }
        private static NativeMemoryReader MemoryReader { get; set; }
        private static FF7SaveMap SaveMap { get; set; }
        public static FF7BattleMap BattleMap { get; set; }
        private static Timer Timer { get; set; }
        public static GameStatus PartyStatus { get; set; }
        public static List<Materia> MateriaDatabase { get; set; } = new List<Materia>();
        public static List<Weapon> WeaponDatabase { get; set; } = new List<Weapon>();
        public static List<Accessory> AccessoryDatabase { get; set; } = new List<Accessory>();
        public static List<Armlet> ArmletDatabase { get; set; } = new List<Armlet>();


        #endregion Public Properties
        public static GameStatus ExtractStatusFromMap(FF7SaveMap map, FF7BattleMap battleMap)
        {
            var time = map.LiveTotalSeconds;

            var t = $"{(time / 3600):00}:{((time % 3600) / 60):00}:{(time % 60):00}";

            var status = new GameStatus()
            {
                Gil = map.LiveGil,
                Location = map.LiveMapName,
                Party = new Models.Character[3],
                ActiveBattle = battleMap.IsActiveBattle,
                ColorTopLeft = map.WindowColorTopLeft,
                ColorBottomLeft = map.WindowColorBottomLeft,
                ColorBottomRight = map.WindowColorBottomRight,
                ColorTopRight = map.WindowColorTopRight,
                TimeActive = t
            };
            var party = battleMap.Party;

            var chars = map.LiveParty;

            for (var index = 0; index < chars.Length; ++index)
            {
                // Skip empty party
                if (chars[index].Id == 0xFF) continue;

                var chr = new Models.Character()
                {
                    MaxHp = chars[index].MaxHp,
                    MaxMp = chars[index].MaxMp,
                    CurrentHp = chars[index].CurrentHp,
                    CurrentMp = chars[index].CurrentMp,
                    Name = chars[index].Name,
                    Level = chars[index].Level,
                    Weapon = WeaponDatabase.FirstOrDefault(w => w.Id == chars[index].Weapon),
                    Armlet = ArmletDatabase.FirstOrDefault(a => a.Id == chars[index].Armor),
                    Accessory = AccessoryDatabase.FirstOrDefault(a => a.Id == chars[index].Accessory),
                    WeaponMateria = new Materia[8],
                    ArmletMateria = new Materia[8],
                    Face = GetFaceForCharacter(chars[index]),
                    BackRow = !chars[index].AtFront,
                };


                for (var m = 0; m < chars[index].WeaponMateria.Length; ++m)
                {
                    chr.WeaponMateria[m] = MateriaDatabase.FirstOrDefault(x => x.Id == chars[index].WeaponMateria[m]);
                }
                for (var m = 0; m < chars[index].ArmorMateria.Length; ++m)
                {
                    chr.ArmletMateria[m] = MateriaDatabase.FirstOrDefault(x => x.Id == chars[index].ArmorMateria[m]);
                }

                var effect = (StatusEffect)chars[index].Flags;

                if (battleMap.IsActiveBattle)
                {
                    chr.CurrentHp = party[index].CurrentHp;
                    chr.MaxHp = party[index].MaxHp;
                    chr.CurrentMp = party[index].CurrentMp;
                    chr.MaxMp = party[index].MaxMp;
                    chr.Level = party[index].Level;
                    effect = party[index].Status;
                    chr.BackRow = party[index].IsBackRow;
                }

                var effs = effect.ToString()
                    .Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
                effs.RemoveAll(x => new[] { "None", "Death" }.Contains(x));
                chr.StatusEffects = effs.ToArray();
                status.Party[index] = chr;
            }

            return status;
        }

        public static string GetFaceForCharacter(CharacterRecord chr)
        {
            // TODO: Abstract magic string names behind variable set that's also used for image extraction
            switch (chr.Character)
            {
                case Character.Cloud:
                    return "cloud";

                case Character.Barret:
                    return "barret";

                case Character.Tifa:
                    return "tifa";

                case Character.Aeris:
                    return "aeris";

                case Character.RedXIII:
                    return "red-xiii";

                case Character.Yuffie:
                    return "yuffie";

                case Character.CaitSith:
                    return "cait-sith";

                case Character.Vincent:
                    return "vincent";

                case Character.Cid:
                    return "cid";

                case Character.YoungCloud:
                    return "young-cloud";

                case Character.Sephiroth:
                    return "sephiroth";

                default:
                    return "";
            }
        }

        private static void StartMonitoringGame()
        {
            if (Timer is null)
            {
                Timer = new Timer(500);
                Timer.Elapsed += Timer_Elapsed;
                Timer.AutoReset = true;

                Timer_Elapsed(null, null);
                Timer.Start();
            }
        }
        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
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
                PartyStatus = ExtractStatusFromMap(SaveMap, BattleMap);
            }
            catch (Exception ex)
            {
                MessageBox.Show("done b0rk3d");
            }
        }

        public Form1()
        {
            InitializeComponent();
            PortableSettingsProvider.ApplyProvider(Properties.Settings.Default);
            if (FF7 is null) FF7 = Process.GetProcessesByName("ff7_en").FirstOrDefault();
            if (FF7 is null) FF7 = Process.GetProcessesByName("ff7").FirstOrDefault();
            StartMonitoringGame();
            int mainloop = 0;
            while ( mainloop == 1)
            {
                GameStatus game = ExtractStatusFromMap(SaveMap, BattleMap);
                NameOutputlabel1.Text = game.Party[0].Name;
            }

            FF7?.Dispose();


        }


        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}