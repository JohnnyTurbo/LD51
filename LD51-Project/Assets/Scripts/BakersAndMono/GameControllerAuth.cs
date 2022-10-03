using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TMG.LD51
{
    public class GameControllerAuth : MonoBehaviour
    {
        public float GridUnitSize;
        public float3 GridOriginOffset;
        public GameObject SelectionEntity;
    }

    public class GameControllerBaker : Baker<GameControllerAuth>
    {
        public override void Bake(GameControllerAuth authoring)
        {
            AddComponent(new SelectionUIPrefab { Value = GetEntity(authoring.SelectionEntity) });
            AddComponent(new GridProperties{UnitSize = authoring.GridUnitSize, Offset = authoring.GridOriginOffset});
        }
    }
}