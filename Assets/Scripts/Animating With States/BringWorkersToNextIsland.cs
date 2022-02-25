using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringWorkersToNextIsland : MonoBehaviour
{
    private enum testUnits
    {
        hobo,
        villager,
        worker,
    }
    private int numOfUnits;
    private Dictionary<testUnits, int> unitTypes = new Dictionary<testUnits, int>();
    Vector3 offset, savedoffset;
    private void Start()
    {
        //numOfUnits = 4;
        unitTypes.Add(testUnits.hobo, 2);
        unitTypes.Add(testUnits.villager, 4);
        
        /*
        if(unitTypes.TryGetValue(testUnits.hobo, out numOfUnits))
        {
            Debug.Log("dictionary value of hobo "+ unitTypes[testUnits.hobo]);
        }
        unitTypes.Clear();
        if (unitTypes.TryGetValue(testUnits.hobo, out numOfUnits))
        {
            Debug.Log("dictionary value of hobo " + unitTypes[testUnits.hobo]);
        }
        */
        
        offset = new Vector3(Random.Range(-1, 1), 0);
        savedoffset = offset;
        for (int i = 0; i < unitTypes[testUnits.villager]; i++)
        {
            while(savedoffset == offset)
            {
                offset = new Vector3(Random.Range(-1f, 1f), 0);
            }
            BaseUnit.Create_BaseUnit(transform.position + offset, transform.position, BaseUnit.UnitType.Villager);
            savedoffset = offset;
        }
    }
}
