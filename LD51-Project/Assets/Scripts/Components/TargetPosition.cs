using Unity.Entities;
using Unity.Mathematics;

namespace TMG.LD51
{
    public struct MoveToTargetTag : IComponentData, IEnableableComponent {}
    public struct HarvestAsteroidsTag : IComponentData, IEnableableComponent {}
    public struct InitializeDroneTag : IComponentData {}
    
    public struct TargetPosition : IComponentData
    {
        public float3 Value;
    }
    
    public struct HomeBasePosition : IComponentData
    {
        public float3 Value;
        public float BaseRadiusSq;
    }
    
    public struct TargetAsteroidPosition : IComponentData
    {
        public float3 Value;
    }

    public struct HarvestProperties : IComponentData
    {
        public float MaxCapacity;
        public float HarvestRate;
        public float UnloadRate;
    }

    public struct WeightHeld : IComponentData
    {
        public float Value;
    }

    public enum DroneState
    {
        Stopped,
        MovingToTargetPosition,
        MovingToTargetAsteroid,
        MovingToHomeBase,
        MovingToBuildPosition,
        Harvesting,
        Unloading,
        Building,
    }

    public struct CurrentDroneState : IComponentData
    {
        public DroneState Value;
    }
}