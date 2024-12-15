using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalEnemyDesigner : MonoBehaviour
{
    [SerializeField] List<GameObject> heads;
    [SerializeField] List<GameObject> hairs;
    [SerializeField] List<GameObject> beards;

    [SerializeField] List<GameObject> bodies;
    [SerializeField] List<GameObject> boots;
    [SerializeField] List<GameObject> gauntlets;
    [SerializeField] List<GameObject> helmets;
    [SerializeField] List<GameObject> legs;

    public void GenerateRandomFace()
    {
        //Pick head
        int headChoice = Random.Range(0, heads.Count);
        for(int i = 0; i < heads.Count; i++)
        {
            if(i == headChoice) { heads[i].SetActive(true); }
        }

        //Make hair
        foreach(GameObject hair in hairs)
        {
            if(Random.value >= 0.5f)
            {
                hair.SetActive(true);
            }
        }

        //Make beard
        foreach (GameObject beard in beards)
        {
            if (Random.value >= 0.5f)
            {
                beard.SetActive(true);
            }
        }
    }

    public void SetStyle(int difficultyLevel)
    {
        int styleLevel = difficultyLevel;

        if (styleLevel <= 0) { return; }
        if (styleLevel >= 3) { styleLevel++; }
        if (styleLevel >= 7) { styleLevel++; }

        int setLevel = styleLevel / 4;
        int difference = styleLevel % 4;

        int bodyLevel = setLevel;
        int bootGauntletLevel = setLevel;
        int helmetLevel = setLevel;
        int legLevel = setLevel;

        while(difference > 0)
        {
            switch (difference)
            {
                //boots and gauntlets
                case 1:
                    bootGauntletLevel++;
                    break;
                //legs
                case 2:
                    legLevel++;
                    break;
                //helmet
                case 3:
                    helmetLevel++;
                    break;
            }
            difference--;
        }

        bodies[0].SetActive(false);
        boots[0].SetActive(false);
        gauntlets[0].SetActive(false);
        helmets[0].SetActive(false);
        legs[0].SetActive(false);

        if(bodyLevel >= bodies.Count) { bodyLevel = bodies.Count - 1; }
        bodies[bodyLevel].SetActive(true);

        if (bootGauntletLevel >= boots.Count) { bootGauntletLevel = boots.Count - 1; }
        boots[bootGauntletLevel].SetActive(true);
        gauntlets[bootGauntletLevel].SetActive(true);

        if (helmetLevel >= helmets.Count) { helmetLevel = helmets.Count - 1; }
        helmets[helmetLevel].SetActive(true);

        if (legLevel >= legs.Count) { legLevel = legs.Count - 1; }
        legs[legLevel].SetActive(true);
    }
}
