using GameCore.Data;

namespace GameCore.Services
{
    public interface IGamePlayProgressService
    {
        void Setup();

        BlockData GenerateBlockData();

        void Reset();
    }
}