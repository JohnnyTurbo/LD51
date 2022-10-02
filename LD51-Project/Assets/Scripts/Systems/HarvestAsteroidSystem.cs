using System;
using Unity.Burst;
using Unity.Entities;

namespace TMG.LD51
{
    [BurstCompile]
    public partial struct HarvestAsteroidSystem : ISystem
    { 
        public void OnCreate(ref SystemState state) {}

        public void OnDestroy(ref SystemState state) {}

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            new HarvestAsteroidJob { DeltaTime = deltaTime }.Run();
        }
    }
    
    [BurstCompile]
    [WithAll(typeof(HarvestAsteroidsTag))]
    public partial struct HarvestAsteroidJob : IJobEntity
    {
        public float DeltaTime;
        
        private void Execute(DroneAspect drone, HarvesterAspect harvester)
        {
            switch (drone.DroneState)
            {
                case DroneState.MovingToTargetAsteroid:
                    drone.MoveToAsteroid(DeltaTime);
                    break;
                case DroneState.MovingToHomeBase:
                    drone.MoveToHomeBase(DeltaTime);
                    break;
                case DroneState.Harvesting:
                    harvester.Harvest(DeltaTime);
                    if (harvester.IsFull)
                    {
                        drone.DroneState = DroneState.MovingToHomeBase;
                    }
                    break;
                case DroneState.Unloading:
                    harvester.Unload(DeltaTime);
                    if (harvester.IsEmpty)
                    {
                        drone.DroneState = DroneState.MovingToTargetAsteroid;
                    }
                    break;
                default:
                    if (harvester.IsFull)
                    {
                        drone.DroneState = DroneState.MovingToHomeBase;
                    }
                    else
                    {
                        drone.DroneState = DroneState.MovingToTargetAsteroid;
                    }
                    break;
            }
        }
    }
}