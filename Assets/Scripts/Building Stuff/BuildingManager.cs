using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private BuildingTypeSO buildingType;
    private BuildingTypeListSO buildingTypeList;
    // Start is called before the first frame update
    void Start()
    {
        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
        buildingType = buildingTypeList.bTypeList[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
