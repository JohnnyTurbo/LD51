using Unity.Entities;
using UnityEngine;

namespace TMG.LD51
{
    public class GameControllerAuth : MonoBehaviour
    {
        public GameObject SelectionEntity;
    }

    public class GameControllerBaker : Baker<GameControllerAuth>
    {
        public override void Bake(GameControllerAuth authoring)
        {
            AddComponent(new SelectionUIPrefab { Value = GetEntity(authoring.SelectionEntity) });
        }
    }
}