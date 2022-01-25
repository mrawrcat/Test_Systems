using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectable
{
    string itemID { get; set; }
    void Collect();
}
