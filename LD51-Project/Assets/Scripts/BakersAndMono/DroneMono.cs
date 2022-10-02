using Unity.Entities;
using UnityEngine;

namespace TMG.LD51
{
    public class DroneMono : MonoBehaviour
    {
        public float MoveSpeed;
        public float RotationSpeed;
        public float MaxCapacity;
        public float HarvestRate;
        public float UnloadRate;
        public Transform HomeBase;
    }
    
    public class DroneBaker : Baker<DroneMono>
    {
        public override void Bake(DroneMono authoring)
        {
            AddComponent(new MoveSpeed { Value = authoring.MoveSpeed });
            AddComponent(new RotationSpeed { Value = authoring.RotationSpeed });
            AddComponent(new HomeBasePosition {Value = authoring.HomeBase.position});
            AddComponent(new HarvestProperties
            {
                MaxCapacity = authoring.MaxCapacity,
                HarvestRate = authoring.HarvestRate,
                UnloadRate = authoring.UnloadRate
            });
            AddComponent<SelectableUnitTag>();
            AddComponent<TargetPosition>();
            AddComponent<TargetAsteroidPosition>();
            AddComponent<MoveToTargetTag>();
            AddComponent<HarvestAsteroidsTag>();
            AddComponent<CurrentDroneState>();
            AddComponent<WeightHeld>();
            AddComponent<InitializeDroneTag>();
        }
    }
}