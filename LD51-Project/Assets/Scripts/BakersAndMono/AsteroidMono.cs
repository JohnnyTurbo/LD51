using Unity.Entities;
using UnityEngine;

namespace TMG.LD51
{
    public class AsteroidMono : MonoBehaviour
    {
        
    }

    public class AsteroidBaker : Baker<AsteroidMono>
    {
        public override void Bake(AsteroidMono authoring)
        {
            AddComponent<AsteroidTag>();
        }
    }
}