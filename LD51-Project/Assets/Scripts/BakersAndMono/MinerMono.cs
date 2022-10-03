using Unity.Entities;
using UnityEngine;

namespace TMG.LD51
{
    public class MinerMono : MonoBehaviour
    {
        public float MiningRate;
        public Transform SpawnPoint;
        public float SpitOutForce;
        public GameObject ChunkPrefab;
    }

    public class MinerBaker : Baker<MinerMono>
    {
        public override void Bake(MinerMono authoring)
        {
            AddComponent(new MinerProperties
            {
                MiningRate = authoring.MiningRate,
                SpawnPoint = authoring.SpawnPoint.position,
                SpitOutForce = authoring.SpitOutForce,
                ChunkPrefab = GetEntity(authoring.ChunkPrefab),
                AsteroidToMine = Entity.Null
            });
            AddComponent<MiningTimer>();
        }
    }
}