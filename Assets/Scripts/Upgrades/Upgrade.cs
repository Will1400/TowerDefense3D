using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

[Serializable]
public class Upgrade
{
    public string AppliesTo;
    public float Amount;
}

[Serializable]
public class UpgradeInTier
{
    public int Tier;
    public List<Upgrade> Upgrades;
}