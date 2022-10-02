using Unity.Entities;

namespace TMG.LD51
{
    public readonly partial struct HarvesterAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRW<WeightHeld> _weightHeld;
        private readonly RefRO<HarvestProperties> _harvestProperties;

        public bool IsFull => _weightHeld.ValueRO.Value >= _harvestProperties.ValueRO.MaxCapacity;
        public bool IsEmpty => _weightHeld.ValueRO.Value <= 0f;
        
        public void Harvest(float deltaTime)
        {
            _weightHeld.ValueRW.Value += _harvestProperties.ValueRO.HarvestRate * deltaTime;
        }

        public void Unload(float deltaTime)
        {
            _weightHeld.ValueRW.Value -= _harvestProperties.ValueRO.UnloadRate * deltaTime;
        }
    }
}