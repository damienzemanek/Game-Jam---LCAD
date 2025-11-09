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

    [SerializeField] GameObject lockObj;

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

        lockObj.SetActive(false);
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

        lockObj.SetActive(false);
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

        bool isLast = index == positions.Length - 1;

        if (isLast)
        {
            bool hasAll = GroceryList.Instance.hasAllItems();

            lockObj.SetActive(!hasAll);

            if (hasAll)
                CheckProgress.Instance.Unlock();
            else
                CheckProgress.Instance.Lock();
        }
        else
        {
            lockObj.SetActive(false);
            CheckProgress.Instance.Unlock(); // Ensures button isn't stuck locked on earlier levels
        }
    }

    public void DisableAllStartButtons() => startButtons.ToList().ForEach(b => b.SetActive(false));

    void SetMoving(bool val) => moving = val;
}
