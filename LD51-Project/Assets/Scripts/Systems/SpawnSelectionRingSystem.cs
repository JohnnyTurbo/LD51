using Unity.Entities;
using Unity.Transforms;

namespace TMG.LD51
{
    public partial struct SpawnSelectionRingSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            
        }

        public void OnDestroy(ref SystemState state)
        {
        }

        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            if (SystemAPI.TryGetSingleton<SelectionUIPrefab>(out var selectionPrefab))
            {
                new AddSelectionRingJob
                {
                    ECB = ecb,
                    SelectionPrefab = selectionPrefab.Value
                }.Run();
            }
        }
    }
    
    [WithAll(typeof(SelectedEntityTag))]
    [WithNone(typeof(SelectionRingCleanup))]
    public partial struct AddSelectionRingJob : IJobEntity
    {
        public EntityCommandBuffer ECB;
        public Entity SelectionPrefab;
        
        private void Execute(Entity selectedEntity)
        {
            var selectionUI = ECB.Instantiate(SelectionPrefab);
            var newSelectionCleanup = new SelectionRingCleanup { SelectionUI = selectionUI };
            ECB.AddComponent(selectedEntity, newSelectionCleanup);
            ECB.AddComponent(selectionUI, new Parent { Value = selectedEntity });
            ECB.AddComponent<LocalToParent>(selectionUI);
        }
    }
}