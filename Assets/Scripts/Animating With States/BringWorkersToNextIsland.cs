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
    [SerializeField]private LayerMask AllyUnits;
    private Vector2 detectSize;
    private void Start()
    {
        //numOfUnits = 4;
        detectSize = new Vector3(4, 3);
        AllyUnits = LayerMask.GetMask("Villager");
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
        
        offset = new Vector3(Random.Range(-2f, 0f), 0);
        savedoffset = offset;
        for (int i = 0; i < unitTypes[testUnits.villager]; i++)
        {
            while(savedoffset == offset)
            {
                offset = new Vector3(Random.Range(-2f, 0f), 0);
            }
            BaseUnit.Create_BaseUnit(transform.position + offset, transform.position, BaseUnit.UnitType.Villager, new Vector3(-10,-3));
            savedoffset = offset;
        }

        
    }
}
