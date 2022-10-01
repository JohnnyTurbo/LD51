using Unity.Entities;
using UnityEngine;

namespace TMG.LD51
{
    public class DroneMono : MonoBehaviour
    {
        public float MoveSpeed;
    }
    
    public class DroneBaker : Baker<DroneMono>
    {
        public override void Bake(DroneMono authoring)
        {
            AddComponent(new MoveSpeed { Value = authoring.MoveSpeed });
            AddComponent<SelectableUnitTag>();
        }
    }
}