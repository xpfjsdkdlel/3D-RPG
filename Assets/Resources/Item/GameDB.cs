using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class GameDB : ScriptableObject
{
	public List<ItemData> ItemData; // Replace 'EntityType' to an actual type that is serializable.
}
