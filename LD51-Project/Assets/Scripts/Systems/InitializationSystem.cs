using Unity.Entities;

namespace TMG.LD51
{
    [UpdateInGroup(typeof(SimulationSystemGroup),OrderLast = true)]
    public partial class InitializationSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Enabled = false;
            Entities
                .WithAll<MoveSpeed>()
                .WithNone<TargetPosition>()
                .ForEach((Entity e) =>
                {
                    EntityManager.AddComponent<TargetPosition>(e);
                }).WithStructuralChanges().Run();
        }
    }
}