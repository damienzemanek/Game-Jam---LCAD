using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Sirenix.OdinInspector;
using ReadOnlyAttribute = Sirenix.OdinInspector.ReadOnlyAttribute;
using Sirenix.Utilities;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] GameObject cam;
    [SerializeField, ReadOnly] bool moving;
    [SerializeField] float transitionTime = 1;
    [SerializeField] int currentPosIndex = 0;
    [SerializeField] Transform[] positions;
    [SerializeField] GameObject[] startButtons;

    private void Start()
    {
        EnableStart(true, 0);
    }

    public void AttemptMoveLeft()
    {
        if (moving) return;
        if (currentPosIndex > 0)
            currentPosIndex--;

        SetMoving(true);
        DisableAllStartButtons();

        cam.transform.Slerp(
            positions[currentPosIndex].position,
            transitionTime, this,
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

        SetMoving(true);
        DisableAllStartButtons();

        cam.transform.Slerp(
            positions[currentPosIndex].position,
            transitionTime, this, 
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

    public void DisableAllStartButtons() => startButtons.ForEach(b => b.SetActive(false));

    void SetMoving(bool val) => moving = val;
}
