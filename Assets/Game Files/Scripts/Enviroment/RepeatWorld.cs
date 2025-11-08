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
    [SerializeField] GameObject[] encounterSectionObjs;

    private void OnInstantiate()
    {
        base.OnInstantiate();
        if (encounterSectionObjs == null || encounterSectionObjs.Length == 0) this.Error("No encounter objs set");
    }

    private void OnEnable()
    {
        HookIntoMove();
    }

    [Button]
    void NextEncounterEnviorment()
    {
        for(int i = 0; i < encounterSectionObjs.Length; i++)
        {
            GameObject e = encounterSectionObjs[i];
            Vector3 endPoint = e.transform.position.With(z: e.transform.position.y - encounterSectionObjsMoveAmount);
            e.transform.Slerp(endPoint, moveTime, this);
        }
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
