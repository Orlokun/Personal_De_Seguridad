using System;

namespace GamePlayManagement
{
    public interface IGameBasedPlayerData
    {
        public int GameDifficulty { get; }
        public DateTime GameCreationDate { get; }
        public Guid GameId { get; }
    }
}