using Unity.Entities;
using Unity.Mathematics;

namespace TMG.LD51
{
    public struct TargetPosition : IComponentData
    {
        public float3 Value;
    }
}