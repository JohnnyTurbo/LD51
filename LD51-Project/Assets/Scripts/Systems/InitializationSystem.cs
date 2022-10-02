using Unity.Entities;
using UnityEngine;

namespace TMG.LD51
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    public partial class InitializationSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var drone in SystemAPI.Query<DroneAspect>().WithAll<MoveToTargetTag, HarvestAsteroidsTag, InitializeDroneTag>())
            {
                EntityManager.SetComponentEnabled<MoveToTargetTag>(drone.Entity, false);
                EntityManager.SetComponentEnabled<HarvestAsteroidsTag>(drone.Entity, false);
            }
        }
    }
}