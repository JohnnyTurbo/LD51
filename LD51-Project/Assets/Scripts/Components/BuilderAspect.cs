using Unity.Entities;

namespace TMG.LD51
{
    public readonly partial struct BuilderAspect : IAspect
    {
        public readonly Entity Entity;

        public readonly RefRO<BuildingPrefabs> _buildingPrefabs;
        
        public Entity MinerBuild => _buildingPrefabs.ValueRO.MinerBuild;
        public Entity MinerFinal => _buildingPrefabs.ValueRO.MinerFinal;
        public Entity ConveyorStraightBuild => _buildingPrefabs.ValueRO.ConveyorStraightBuild;
        public Entity ConveyorStraightFinal => _buildingPrefabs.ValueRO.ConveyorStraightFinal;
        public Entity ConveyorTurnCWBuild => _buildingPrefabs.ValueRO.ConveyorTurnCWBuild;
        public Entity ConveyorTurnCWFinal => _buildingPrefabs.ValueRO.ConveyorTurnCWFinal;
        public Entity ConveyorTurnCCWBuild => _buildingPrefabs.ValueRO.ConveyorTurnCCWBuild;
        public Entity ConveyorTurnCCWFinal => _buildingPrefabs.ValueRO.ConveyorTurnCCWFinal;
    }

    public struct BuildingPrefabs : IComponentData
    {
        public Entity MinerBuild;
        public Entity MinerFinal;
        public Entity ConveyorStraightBuild;
        public Entity ConveyorStraightFinal;
        public Entity ConveyorTurnCWBuild;
        public Entity ConveyorTurnCWFinal;
        public Entity ConveyorTurnCCWBuild;
        public Entity ConveyorTurnCCWFinal;
    }
}