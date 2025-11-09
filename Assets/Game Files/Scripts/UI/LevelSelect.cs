using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using ReadOnlyAttribute = Sirenix.OdinInspector.ReadOnlyAttribute;
using UnityEngine.Events;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] GameObject cam;
    [SerializeField, ReadOnly] bool moving;
    [SerializeField] float transitionTime = 1;
    [SerializeField] int currentPosIndex = 0;
    [SerializeField] Transform[] positions;
    [SerializeField] GameObject[] startButtons;
    [SerializeField] UnityEvent selectNextHook;

    private void Awake()
    {
        if (selectNextHook == null) selectNextHook = new UnityEvent();
    }

    private void Start()
    {
        EnableStart(true, 0);
    }

    public void AttemptMoveLeft()
    {
        if (moving) return;
        if (currentPosIndex > 0)
            currentPosIndex--;

        selectNextHook?.Invoke();
        SetMoving(true);
        DisableAllStartButtons();

        cam.transform.Lerp(
            positions[currentPosIndex].position,
            transitionTime,
            this,
            () =>
                {
                    SetMoving(false);
                    EnableStart(true, currentPosIndex);
                }
            );
    }

    public void AttemptMoveRight()
    {
        if (moving) return;
        if (currentPosIndex < positions.Length - 1)
            currentPosIndex++;

        selectNextHook?.Invoke();
        SetMoving(true);
        DisableAllStartButtons();

        cam.transform.Lerp(
            positions[currentPosIndex].position,
            transitionTime,
            this, 
            () =>
                {
                    SetMoving(false);
                    EnableStart(true, currentPosIndex);
                }
            );
    }

    public void EnableStart(bool val, int index)
    {
        DisableAllStartButtons();
        startButtons[index].SetActive(true);
    }

    public void DisableAllStartButtons() => startButtons.ToList().ForEach(b => b.SetActive(false));

    void SetMoving(bool val) => moving = val;
}
