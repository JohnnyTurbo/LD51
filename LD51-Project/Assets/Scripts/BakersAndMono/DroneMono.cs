using Unity.Entities;
using UnityEngine;

namespace TMG.LD51
{
    public class DroneMono : MonoBehaviour
    {
        public float MoveSpeed;
        public float RotationSpeed;
    }
    
    public class DroneBaker : Baker<DroneMono>
    {
        public override void Bake(DroneMono authoring)
        {
            AddComponent(new MoveSpeed { Value = authoring.MoveSpeed });
            AddComponent(new RotationSpeed { Value = authoring.RotationSpeed });
            AddComponent<SelectableUnitTag>();
            //AddComponent<TargetPosition>();
        }
    }
}