using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentMoveToPlayer : Follow
    {
        private const float MinimalDistance = 1f;

        private NavMeshAgent _agent;

        private Transform _heroTransform;
        private IGameFactory _gameFactory;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();

            if (_gameFactory.HeroGameObject != null)
            {
                InitializeHeroTransform();
            }
            else
            {
                _gameFactory.HeroCreated += OnHeroCreated;
            }
        }

        private void Update()
        {
            if (Initialized() && HeroNotReached())
            {
                _agent.destination = _heroTransform.position;
            }
        }

        private void OnDestroy()
        {
            if (_gameFactory != null)
                _gameFactory.HeroCreated -= OnHeroCreated;
        }

        private void OnHeroCreated()
        {
            InitializeHeroTransform();
        }

        private bool Initialized() =>
            _heroTransform != null;

        private void InitializeHeroTransform() => 
            _heroTransform = _gameFactory.HeroGameObject.transform;

        private bool HeroNotReached() => 
            Vector3.Distance(_agent.transform.position, _heroTransform.position) >= MinimalDistance;
    }
}
