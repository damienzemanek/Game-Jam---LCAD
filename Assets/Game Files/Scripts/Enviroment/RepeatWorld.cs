using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DependencyInjection;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class RepeatWorld : MonoBehaviour, IDependencyProvider
{
    [Provide] RepeatWorld Provide() => this;
    [SerializeField] int amountOfEncounters = 2;
    [SerializeField, ReadOnly] int encountersLeft = 2;
    [SerializeField] float encounterSectionObjsMoveAmount = 50f;
    [SerializeField] float moveTime = 3f;
    [SerializeField] List<Encounter> encounters;
    [SerializeField] Transform nextEncounterPos;

    [SerializeField] public UnityEventPlus startCombatHook;
    [SerializeField] UnityEventPlus dungeonCompleteHook;

    [ShowInInspector] public Encounter currentEncounter { get => encounters == null? null : encounters[0]; }

    private void Awake()
    {
        if (startCombatHook == null) startCombatHook = new UnityEventPlus();
    }

    void Start()
    {
        if (encounters == null || encounters.Count == 0) this.Error("No encounter objs set");
        encountersLeft = amountOfEncounters;
        TransitionToNextEncounter();
    }

    [Button]
    public void TransitionToNextEncounter()
    {
        encountersLeft--;

        if (encountersLeft > 0)
        {
            TransitionEnviroment();
            this.DelayedCall(() => InitNextEncounter(false), moveTime);
        }
        else
            CompleteDungeon();

    }

    void CompleteDungeon()
    {
        TransitionEnviroment();
        InitNextEncounter(true);
        dungeonCompleteHook?.InvokeWithDelay(this);
    }


    void TransitionEnviroment() 
    {
        encounters.ForEach(e => 
        { 
            Vector3 endPoint = e.transform.position.With(z: e.transform.position.z - encounterSectionObjsMoveAmount);
            e.transform.Slerp(
                endPoint,
                moveTime, this
            );
        });
    }

    void InitNextEncounter(bool dungeonComplete)
    {
        SetLastUsedEncounterToNextUse();


        if (!dungeonComplete)
        {
            currentEncounter.SpawnRandomEnemy();
            startCombatHook?.get?.Invoke();
        }
        else
            currentEncounter.SpawnDungeonCompleteItem();




        void SetLastUsedEncounterToNextUse()
        {
            print("go");
            encounters[0].transform.position = nextEncounterPos.position;
            encounters.Swap(0, 1);

        }
    }



}
