using Unity.Entities;

namespace TMG.LD51
{
    [UpdateAfter(typeof(SpawnSelectionRingSystem))]
    public partial struct CleanupSelectionRingSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            
        }

        public void OnDestroy(ref SystemState state)
        {
            
        }

        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            new CleanupSelectionRingJob { ECB = ecb }.Run();
        }
    }
    
    [WithNone(typeof(SelectedEntityTag))]
    public partial struct CleanupSelectionRingJob : IJobEntity
    {
        public EntityCommandBuffer ECB;

        private void Execute(Entity deselectedEntity, in SelectionRingCleanup selectionRingCleanup)
        {
            ECB.DestroyEntity(selectionRingCleanup.SelectionUI);
            ECB.RemoveComponent<SelectionRingCleanup>(deselectedEntity);
        }
    }
}