using NeonStream.Constants;
using NeonStream.Models;

namespace NeonStream.GameData
{
    public class FF7BattleMap
    {
        #region Private Fields

        private readonly byte[] _map;

        #endregion Private Fields

        #region Public Constructors

        public FF7BattleMap(byte[] bytes, byte activeBattle)
        {
            IsActiveBattle = activeBattle == 0x01;
            _map = bytes;
        }

        #endregion Public Constructors

        #region Public Properties

        public bool IsActiveBattle { get; set; }

        public BattleActor[] Opponents => GetActors(BattleMapOffsets.EnemyActors, 6);
        public BattleActor[] Party => GetActors(BattleMapOffsets.PartyActors, 4);

        #endregion Public Properties

        #region Private Methods

        private BattleActor[] GetActors(int start, int count)
        {
            BattleActor[] acts = new BattleActor[count];

            for (int i = 0; i < count; ++i)
            {
                int offset = start + (i * BattleMapActorOffsets.ActorLength);
                BattleActor a = new()
                {
                    CurrentHp = BitConverter.ToInt32(_map, offset + BattleMapActorOffsets.CurrentHp),
                    MaxHp = BitConverter.ToInt32(_map, offset + BattleMapActorOffsets.MaxHp),
                    CurrentMp = BitConverter.ToInt16(_map, offset + BattleMapActorOffsets.CurrentMp),
                    MaxMp = BitConverter.ToInt16(_map, offset + BattleMapActorOffsets.MaxMp),
                    Level = _map[BattleMapActorOffsets.Level],
                    Status = (StatusEffect)BitConverter.ToUInt32(_map, offset + BattleMapActorOffsets.Status),
                    IsBackRow = (_map[offset + BattleMapActorOffsets.Row] & 0x40) == 0x40
                };
                acts[i] = a;
            }

            return acts;
        }

        #endregion Private Methods
    }
}