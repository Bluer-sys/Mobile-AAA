using CodeBase.Infrastructure.Factory;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentMoveToPlayer : Follow
    {
        private const float MinimalDistance = 1f;

        private NavMeshAgent _agent;
        private IGameFactory _gameFactory;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (HeroNotReached())
            {
                _agent.destination = HeroTransform.position;
            }
        }

        private bool HeroNotReached() => 
            Vector3.Distance(_agent.transform.position, HeroTransform.position) >= MinimalDistance;
    }
}
