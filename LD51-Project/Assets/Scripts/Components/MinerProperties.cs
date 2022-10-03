using Unity.Entities;
using Unity.Mathematics;

namespace TMG.LD51
{
    public struct MinerProperties : IComponentData
    {
        public float MiningRate;
        public float3 SpawnPoint;
        public float SpitOutForce;
        public Entity ChunkPrefab;
        public Entity AsteroidToMine;
    }

    public struct MiningTimer : IComponentData
    {
        public float Value;
        public int NumberSpawned;
    }
}