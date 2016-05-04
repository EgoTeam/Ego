using UnityEngine;
using System.Collections;

public class EnemyState : CharacterState {
    [SerializeField] protected AIState _actionState = AIState.Patrol;

    protected NavMeshAgent _navigation;

    protected NavMeshAgent Navigation
    {
        get { return _navigation; }
        set { _navigation = value; }
    }

    public AIState ActionState {
        get { return _actionState; }
        set { _actionState = value; }
    }

    protected ControllableCharacterController PlayerController = null;

    [SerializeField]protected Transform PlayerTransform = null;

    public float ChaseDistance = 10.0f;

    public float AttackDistance = 0.1f;

    public float PatrolDistance = 30.0f;

    virtual protected void Start() {
        Navigation = GetComponent<NavMeshAgent>();
        GameObject PlayerObject = GameObject.FindGameObjectWithTag("Player");
        PlayerController = PlayerObject.GetComponent<ControllableCharacterController>();
        PlayerTransform = PlayerObject.GetComponent<Transform>();
        ChangeState(ActionState);
    }

    public void ChangeState(AIState State) {
        StopAllCoroutines();

        ActionState = State;

        switch(ActionState) {
            case AIState.Attack:
                StartCoroutine(Attack());
                return;
            case AIState.Chase:
                StartCoroutine(Chase());
                EventManager.PatrolEvent(this.gameObject.GetInstanceID());
                return;
            case AIState.Patrol:
                StartCoroutine(Patrol());
                EventManager.ChaseEvent(this.gameObject.GetInstanceID());
                return;
            case AIState.Dead:
                StartCoroutine(Dead());
                return;
        }
    }

    private IEnumerator Patrol() {
        Navigation.Stop();
        while(ActionState == AIState.Patrol) {
            if(IsDying || IsDead)
            {
                ChangeState(AIState.Dead);
            }
            
            Vector3 randomPosition = Random.insideUnitSphere * PatrolDistance;

            randomPosition += this.transform.position;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPosition, out hit, PatrolDistance, 1);

            Navigation.SetDestination(hit.position);
            Navigation.Resume();

            float ArrivalDistance = 2.0f;

            float TimeOut = 5.0f;

            float ElapsedTime = 0;

            while(Vector3.Distance(this.transform.position, hit.position) > ArrivalDistance && ElapsedTime < TimeOut) {
                
                ElapsedTime += Time.deltaTime;
                if (Vector3.Distance(this.transform.position, PlayerTransform.position) < ChaseDistance) {
                    ChangeState(AIState.Chase);
                    yield break;
                }
                yield return null;
            }
        }
    }

    private IEnumerator Chase() {
        Navigation.Stop();

        while(ActionState == AIState.Chase) {
            if (IsDying || IsDead)
            {
                ChangeState(AIState.Dead);
            }
            Navigation.SetDestination(PlayerTransform.position);
            Navigation.Resume();
            float DistanceFromPlayer = Vector3.Distance(this.transform.position, PlayerTransform.position);
            if(DistanceFromPlayer < AttackDistance) {
                ChangeState(AIState.Attack);
                yield break;
            }
            if(DistanceFromPlayer > ChaseDistance) {
                ChangeState(AIState.Patrol);
                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator Attack() {
        Navigation.Stop();

        while(ActionState == AIState.Attack) {
            if (IsDying || IsDead)
            {
                ChangeState(AIState.Dead);
            }
            float DistanceFromPlayer = Vector3.Distance(this.transform.position, PlayerTransform.position);
            if(DistanceFromPlayer > ChaseDistance) {
                ChangeState(AIState.Patrol);
                yield break;
            }
            if(DistanceFromPlayer > AttackDistance) {
                ChangeState(AIState.Chase);
                yield break;
            }
            EventManager.AttackEvent(gameObject.GetInstanceID(), false);
            yield return null;
        }

    }
    private IEnumerator Dead() {
        while(ActionState == AIState.Dead)
        {
            Navigation.Stop();
            yield return null;
        }
    }
}