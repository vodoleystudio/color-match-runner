using GameCore.Data;

namespace GameCore.Services
{
    public interface IGamePlayProgressService
    {
        BlockData GenerateBlockData();
        void Reset();
    }
}

