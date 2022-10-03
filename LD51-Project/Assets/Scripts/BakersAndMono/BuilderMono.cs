using Unity.Entities;
using UnityEngine;

namespace TMG.LD51
{
    public class BuilderMono : MonoBehaviour
    {
        public GameObject MinerBuild;
        public GameObject MinerFinal;
        public GameObject ConveyorStraightBuild;
        public GameObject ConveyorStraightFinal;
        public GameObject ConveyorTurnCWBuild;
        public GameObject ConveyorTurnCWFinal;
        public GameObject ConveyorTurnCCWBuild;
        public GameObject ConveyorTurnCCWFinal;
    }
    
    public class BuilderBaker : Baker<BuilderMono>
    {
        public override void Bake(BuilderMono authoring)
        {
            AddComponent(new BuildingPrefabs
            {
                MinerBuild = GetEntity(authoring.MinerBuild),
                MinerFinal = GetEntity(authoring.MinerFinal),
                ConveyorStraightBuild = GetEntity(authoring.ConveyorStraightBuild),
                ConveyorStraightFinal = GetEntity(authoring.ConveyorStraightFinal),
                ConveyorTurnCWBuild = GetEntity(authoring.ConveyorTurnCWBuild),
                ConveyorTurnCWFinal = GetEntity(authoring.ConveyorTurnCWFinal),
                ConveyorTurnCCWBuild = GetEntity(authoring.ConveyorTurnCCWBuild),
                ConveyorTurnCCWFinal = GetEntity(authoring.ConveyorTurnCCWFinal)
            });
        }
    }
}