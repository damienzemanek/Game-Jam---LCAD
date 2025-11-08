using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    
    [SerializeField] GameObject claimButton;

    private void Start()
    {
        EnableClaimScreen(false);
    }

    public void EnableClaimScreen(bool val)
    {
        claimButton.SetActive(val);
    }


}
