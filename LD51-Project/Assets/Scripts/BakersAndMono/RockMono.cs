using Unity.Entities;
using UnityEngine;

namespace TMG.LD51
{
    public class RockMono : MonoBehaviour
    {
        
    }
    
    public class RockBaker : Baker<RockMono>
    {
        public override void Bake(RockMono authoring)
        {
            AddComponent<RockTag>();
        }
    }
}