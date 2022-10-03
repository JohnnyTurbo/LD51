using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace TMG.LD51
{
    public readonly partial struct MiningAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly TransformAspect _transform;
        private readonly RefRO<MinerProperties> _minerProperties;
        private readonly RefRW<MiningTimer> _miningTimer;
        
        private float MiningTimer
        {
            get => _miningTimer.ValueRO.Value;
            set => _miningTimer.ValueRW.Value = value;
        }

        private int NumberSpawned
        {
            get => _miningTimer.ValueRO.NumberSpawned;
            set => _miningTimer.ValueRW.NumberSpawned = value;
        }
        
        private float MiningRate => _minerProperties.ValueRO.MiningRate;
        private bool ShouldSpawnChunk => MiningTimer >= MiningRate;
        //private bool NoAsteroidToMine => !SystemAPI.Exists(_minerProperties.ValueRO.AsteroidToMine);
        private Entity ChunkPrefab => _minerProperties.ValueRO.ChunkPrefab;
        private float3 SpawnPoint => _minerProperties.ValueRO.SpawnPoint;
        private float SpitOutForce => _minerProperties.ValueRO.SpitOutForce;
        
        public float3 GetSpitOutForce()
        {
            var randomDegree = Random.CreateFromIndex((uint)NumberSpawned).NextFloat(-5f, 5f);
            var myRot = _transform.Rotation;
            //var randRot = math.rotate(myRot, new float3(0, randomDegree, 0));
            var randRot = math.mul(myRot, quaternion.EulerXYZ(0, randomDegree, 0));
            randRot = math.normalize(randRot);
            return math.mul(randRot, SpitOutForce);
        }
        
        public void Mine(float deltaTime, EntityCommandBuffer ecb, out Entity newChunk)
        {
            newChunk = Entity.Null;
            //if (NoAsteroidToMine) return;
            MiningTimer += deltaTime;
            if (ShouldSpawnChunk)
            {
                NumberSpawned++;
                MiningTimer = 0f;
                newChunk = ecb.Instantiate(ChunkPrefab);
                ecb.SetComponent(newChunk, new Translation { Value = SpawnPoint });
            }
        }
    }
}