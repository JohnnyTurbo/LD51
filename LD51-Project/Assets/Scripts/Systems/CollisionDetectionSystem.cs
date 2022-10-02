using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

namespace TMG.LD51
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateBefore(typeof(PhysicsSystemGroup))]
    [BurstCompile]
    public partial struct CollisionDetectionSystem : ISystem
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
            var simSingleton = SystemAPI.GetSingleton<SimulationSingleton>();
            state.Dependency = new DetectCollisionJob
            {
                AccelerationLookup = SystemAPI.GetComponentLookup<AccelerationSpeed>(true),
                RockLookup = SystemAPI.GetComponentLookup<RockTag>(true),
                LocalToWorldLookup = SystemAPI.GetComponentLookup<LocalToWorld>(true),
                VelocityLookup = SystemAPI.GetComponentLookup<PhysicsVelocity>(),
                DeltaTime = SystemAPI.Time.DeltaTime
            }.Schedule(simSingleton, state.Dependency);
        }
    }
    
    [BurstCompile]
    public struct DetectCollisionJob : ICollisionEventsJob
    {
        public ComponentLookup<PhysicsVelocity> VelocityLookup;
        [ReadOnly] public ComponentLookup<AccelerationSpeed> AccelerationLookup;
        [ReadOnly] public ComponentLookup<RockTag> RockLookup;
        [ReadOnly] public ComponentLookup<LocalToWorld> LocalToWorldLookup;
        
        public float DeltaTime;

        [BurstCompile]
        public void Execute(CollisionEvent collisionEvent)
        {
            var entityA = collisionEvent.EntityA;
            var entityB = collisionEvent.EntityB;

            var isEntityAConveyor = AccelerationLookup.HasComponent(entityA);
            var isEntityBConveyor = AccelerationLookup.HasComponent(entityB);

            if ((isEntityAConveyor && isEntityBConveyor) || (!isEntityAConveyor && !isEntityBConveyor)) return;

            var isEntityARock = RockLookup.HasComponent(entityA);
            var isEntityBRock = RockLookup.HasComponent(entityB);

            if (isEntityARock && isEntityBRock) return;

            var conveyorEntity = isEntityAConveyor ? entityA : entityB;
            var rockEntity = isEntityARock ? entityA : entityB;

            var accelerationStrength = AccelerationLookup[conveyorEntity].Value;
            var accelerationDirection = LocalToWorldLookup[conveyorEntity].Forward;
            var acceleration = accelerationStrength * accelerationDirection * DeltaTime;

            var velocity = VelocityLookup[rockEntity].Linear;
            velocity += acceleration;
            VelocityLookup[rockEntity] = new PhysicsVelocity { Linear = velocity };
        }
    }
}





















