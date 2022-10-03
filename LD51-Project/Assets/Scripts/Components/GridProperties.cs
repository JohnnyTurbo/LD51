using Unity.Entities;
using Unity.Mathematics;

namespace TMG.LD51
{
    public struct GridProperties : IComponentData
    {
        public float UnitSize;
        public float3 Offset;
    }
}