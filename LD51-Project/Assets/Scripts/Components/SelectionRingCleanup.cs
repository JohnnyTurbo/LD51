using Unity.Entities;

namespace TMG.LD51
{
    public struct SelectionRingCleanup : ICleanupComponentData
    {
        public Entity SelectionUI;
    }
}