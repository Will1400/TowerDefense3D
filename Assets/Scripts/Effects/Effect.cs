using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[System.Serializable]
public class Effect : IEquatable<Effect>
{
    public string AppliesTo;
    [Tooltip("Amount in percent to apply")]
    public float Amount;
    public float Duration;
    public EffectType EffectType;

    public Effect Clone()
    {
        return new Effect() { AppliesTo = this.AppliesTo, Amount = this.Amount, Duration = this.Duration, EffectType = this.EffectType };
    }

    public override bool Equals(object other)
    {
        if (other is Effect)
            return Equals(other);

        return base.Equals(other);
    }

    public bool Equals(Effect other)
    {
        return other != null &&
               AppliesTo == other.AppliesTo &&
               Amount == other.Amount;
    }

    public override int GetHashCode()
    {
        var hashCode = -786158230;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(AppliesTo);
        hashCode = hashCode * -1521134295 + Amount.GetHashCode();
        //hashCode = hashCode * -1521134295 + Duration.GetHashCode();
        return hashCode;
    }
}