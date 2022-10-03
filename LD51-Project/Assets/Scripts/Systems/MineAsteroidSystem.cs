using Unity.Burst;
using Unity.Entities;
using Unity.Physics;

namespace TMG.LD51
{
    [BurstCompile]
    public partial struct MineAsteroidSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
        }

        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            new MineAsteroidJob { DeltaTime = deltaTime, ECB = ecb}.Run();
        }
    }

    [BurstCompile]
    public partial struct MineAsteroidJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer ECB;
        
        private void Execute(MiningAspect miner)
        {
            miner.Mine(DeltaTime, ECB, out var newChunk);
            if(newChunk == Entity.Null) return;
            var force = miner.GetSpitOutForce();
            ECB.SetComponent(newChunk, new PhysicsVelocity{Linear = force});
        }
    }
}