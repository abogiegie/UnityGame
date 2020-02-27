using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalMaker : MonoBehaviour
{
    public List<GameObject> animalList;
    public int maxCount;
    private int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        MakeAnimal();
    }

    public void MakeAnimal()
    {
        while (count < maxCount)
        {
            int index = (int)(Random.value * animalList.Count);
            GameObject.Instantiate(animalList[index], this.transform);
            count++;
        }
    }

    public void AnimalDie(int deathCount = 1)
    {
        count -= deathCount;
    }
}
