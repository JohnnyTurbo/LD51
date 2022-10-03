using Unity.Entities;
using UnityEngine;

namespace TMG.LD51
{
    public class BuildPrefabMono : MonoBehaviour
    {
        
    }

    public class BuildPrefabBaker : Baker<BuildPrefabMono>
    {
        public override void Bake(BuildPrefabMono authoring)
        {
            //AddComponent<curr>();
        }
    }
}