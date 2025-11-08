using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DependencyInjection;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

public class RepeatWorld : RuntimeInjectableMonoBehaviour
{
    [Inject] Movement move;
    [SerializeField] float encounterSectionObjsMoveAmount = 50f;
    [SerializeField] float moveTime = 3f;
    [SerializeField] List<GameObject> encounterSectionObjs;
    [SerializeField] Transform nextEncounterPos;

    protected override void OnInstantiate()
    {
        base.OnInstantiate();
        if (encounterSectionObjs == null || encounterSectionObjs.Count == 0) this.Error("No encounter objs set");
    }

    private void OnEnable()
    {
        HookIntoMove();
    }


    [Button]
    void NextEncounterEnviorment() 
    { 
        encounterSectionObjs.ForEach(e => 
        { 
            Vector3 endPoint = e.transform.position.With(z: e.transform.position.z - encounterSectionObjsMoveAmount); 
            e.transform.Slerp(endPoint, 5f, this, SetLastUsedEncounterToNextUse);
        });
    }

    void SetLastUsedEncounterToNextUse()
    {
        print("go");
        encounterSectionObjs[0].transform.position = nextEncounterPos.position;
        encounterSectionObjs.Swap(0, 1);
        
    }

    void HookIntoMove()
    {
        move.onMoveHook.AddListener(NextEncounterEnviorment);
    }

    private void OnDisable()
    {
        move.onMoveHook.RemoveListener(NextEncounterEnviorment);
    }

}
