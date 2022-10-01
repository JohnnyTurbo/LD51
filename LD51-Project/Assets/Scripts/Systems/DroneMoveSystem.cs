using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace TMG.LD51
{
    [BurstCompile]
    public partial struct DroneMoveSystem : ISystem
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
            foreach (var drone in SystemAPI.Query<DroneAspect>())
            {
                drone.RotateTowardsTarget(deltaTime);
                if(!drone.ShouldMoveToTarget) continue;
                drone.Position += drone.Forward * drone.MoveSpeed * deltaTime;
            }
        }
    }
    
    public partial struct DroneMoveJob : IJobEntity
    {
        public float DeltaTime;
        
        private void Execute(TransformAspect transform, in MoveSpeed moveSpeed)
        {
            transform.Position += transform.Forward * moveSpeed.Value * DeltaTime;
        }
    }
}