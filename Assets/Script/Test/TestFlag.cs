using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum TestFlag
{
    zero = 0,
    one = 1 << 0,
    two = 1 << 1,
    three = 1 << 2,
    four = 1 << 3,
    five = 1 << 4
}
