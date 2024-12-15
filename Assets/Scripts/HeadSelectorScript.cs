using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadSelectorScript : MonoBehaviour
{
    [Header("Left Dragon")]
    [SerializeField] OnButtonPress lHeadFunction;
    [SerializeField] List<MeshRenderer> lHeadRender;
    [SerializeField] SkinnedMeshRenderer lBodyRender;
    [SerializeField] Toggle lToggle;
    
    [Header("Right Dragon")]
    [SerializeField] OnButtonPress rHeadFunction;
    [SerializeField] List<MeshRenderer> rHeadRender;
    [SerializeField] SkinnedMeshRenderer rBodyRender;
    [SerializeField] Toggle rToggle;



    public void ToggleLeftDragon()
    {
        if (lToggle.isOn)
        {
            if (!rHeadFunction.enabled)
            {
                SetRightDragon(true);
                rToggle.isOn = true;
            }

            SetLeftDragon(false);
            lToggle.isOn = false;
        }
        else
        {
            SetLeftDragon(true);
            lToggle.isOn = true;
        }
    }

    public void ToggleRightDragon()
    {
        if (rToggle.isOn)
        {
            if (!lHeadFunction.enabled)
            {
                SetLeftDragon(true);
                lToggle.isOn = true;
            }
            SetRightDragon(false);
            rToggle.isOn = false;
        }
        else
        {
            SetRightDragon(true);
            rToggle.isOn = true;
        }
    }

    public void SetLeftDragon(bool value)
    {
        lHeadFunction.enabled = value;
        foreach (MeshRenderer lhr in lHeadRender)
        {
            lhr.enabled = value;
        }
        lBodyRender.enabled = value;
    }
    public void SetRightDragon(bool value)
    {
        rHeadFunction.enabled = value;
        foreach (MeshRenderer rhr in rHeadRender)
        {
            rhr.enabled = value;
        }
        rBodyRender.enabled = value;
    }
}
