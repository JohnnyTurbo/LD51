using Unity.Entities;
using UnityEngine;

namespace TMG.LD51
{
    public class ConveyorMono : MonoBehaviour
    {
        public float AccelerationSpeed;
    }
    
    public class ConveyorBaker : Baker<ConveyorMono>
    {
        public override void Bake(ConveyorMono authoring)
        {
            AddComponent(new AccelerationSpeed { Value = authoring.AccelerationSpeed });
        }
    }
}