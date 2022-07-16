using Bluegrams.Application;
using NeonStream.Constants;
using NeonStream.GameData;
using NeonStream.Models;
using Shojy.FF7.Elena;
using System.Diagnostics;
using System.Timers;
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
        private static string ProcessName { get; set; }
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
            int time = map.LiveTotalSeconds;

            string t = $"{time / 3600:00}:{time % 3600 / 60:00}:{time % 60:00}";

            GameStatus status = new()
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
            BattleActor[] party = battleMap.Party;

            CharacterRecord[] chars = map.LiveParty;

            for (int index = 0; index < chars.Length; ++index)
            {
                // Skip empty party
                if (chars[index].Id == 0xFF)
                {
                    continue;
                }

                Models.Character chr = new()
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


                for (int m = 0; m < chars[index].WeaponMateria.Length; ++m)
                {
                    chr.WeaponMateria[m] = MateriaDatabase.FirstOrDefault(x => x.Id == chars[index].WeaponMateria[m]);
                }
                for (int m = 0; m < chars[index].ArmorMateria.Length; ++m)
                {
                    chr.ArmletMateria[m] = MateriaDatabase.FirstOrDefault(x => x.Id == chars[index].ArmorMateria[m]);
                }

                StatusEffect effect = (StatusEffect)chars[index].Flags;

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

                List<string> effs = effect.ToString()
                    .Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
                _ = effs.RemoveAll(x => new[] { "None", "Death" }.Contains(x));
                chr.StatusEffects = effs.ToArray();
                status.Party[index] = chr;
            }

            return status;
        }

        public static string GetFaceForCharacter(CharacterRecord chr)
        {
            // TODO: Abstract magic string names behind variable set that's also used for image extraction
            return chr.Character switch
            {
                Character.Cloud => "cloud",
                Character.Barret => "barret",
                Character.Tifa => "tifa",
                Character.Aeris => "aeris",
                Character.RedXIII => "red-xiii",
                Character.Yuffie => "yuffie",
                Character.CaitSith => "cait-sith",
                Character.Vincent => "vincent",
                Character.Cid => "cid",
                Character.YoungCloud => "young-cloud",
                Character.Sephiroth => "sephiroth",
                _ => "",
            };
        }
        private static void LocateGameProcess()
        {
            var firstRun = true;
            while (FF7 is null)
            {
                if (!firstRun)
                {
                    MessageBox.Show("Could not locate FF7. Is the game running?");
                    if (!string.IsNullOrWhiteSpace(ProcessName))
                    {
                        FF7 = Process.GetProcessesByName(ProcessName).FirstOrDefault();
                    }

                    if (FF7 is null)
                    {
                        SearchForProcess(ProcessName);
                    }
                }

                if (FF7 is null) FF7 = Process.GetProcessesByName("ff7_en").FirstOrDefault();
                if (FF7 is null) FF7 = Process.GetProcessesByName("ff7").FirstOrDefault();
                firstRun = false;
            }
        }
        private static void SearchForProcess(string processName)
        {
            if (Timer is null)
            {
                Timer = new Timer(300);
                Timer.Elapsed += Timer_Elapsed;
                Timer.AutoReset = true;

                Timer_Elapsed(null, null);
                Timer.Start();
            }
            lock (Timer)
            {
                if (null != Timer)
                {
                    Timer.Enabled = false;
                }

                FF7 = null;
                while (FF7 is null)
                {
                    try
                    {
                        if (FF7 is null) FF7 = Process.GetProcessesByName("ff7_en").FirstOrDefault();
                        if (FF7 is null) FF7 = Process.GetProcessesByName("ff7").FirstOrDefault();
                        if (FF7 is null && !string.IsNullOrWhiteSpace(processName))
                            FF7 = Process.GetProcessesByName(processName).FirstOrDefault();
                    }
                    catch (Exception e)
                    {
                    }

                    Thread.Sleep(250);
                }

                MemoryReader = new NativeMemoryReader(FF7);
                if (null != Timer)
                {
                    Timer.Enabled = true;
                }
            }
        }
        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                byte[] saveMapByteData = MemoryReader.ReadMemory(new IntPtr(Addresses.SaveMapStart), 4342);
                byte isBattle = MemoryReader.ReadMemory(new IntPtr(Addresses.ActiveBattleState), 1).First();
                byte[] battleMapByteData = MemoryReader.ReadMemory(new IntPtr(Addresses.BattleMapStart), 0x750);
                byte[] colors = MemoryReader.ReadMemory(new IntPtr(Addresses.WindowColorBlockStart), 16);

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
                SearchForProcess(ProcessName);
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

        public Form1()
        {
            PortableSettingsProvider.ApplyProvider(Properties.Settings.Default);
            LocateGameProcess();
            InitializeComponent();
            StartMonitoringGame();

            int mainloop = 1;
            while (mainloop == 1)
            {
                GameStatus game = ExtractStatusFromMap(SaveMap, BattleMap);
                NameOutputlabel1.Text = game.Party[0].Name;
            }

            FF7?.Dispose();


        }



    }
}