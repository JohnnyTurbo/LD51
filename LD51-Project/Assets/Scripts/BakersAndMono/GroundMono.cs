using Unity.Entities;
using UnityEngine;

namespace TMG.LD51
{
    public class GroundMono : MonoBehaviour
    {
        
    }
    
    public class GroundBaker : Baker<GroundMono>
    {
        public override void Bake(GroundMono authoring)
        {
            AddComponent<GroundTag>();
        }
    }
}