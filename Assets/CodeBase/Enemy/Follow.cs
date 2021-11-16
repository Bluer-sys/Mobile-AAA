using UnityEngine;

namespace CodeBase.Enemy
{
    public abstract class Follow : MonoBehaviour
    {
        protected Transform HeroTransform;

        public void Construct(Transform heroTransform)
        {
            HeroTransform = heroTransform;
        }
    }
}
