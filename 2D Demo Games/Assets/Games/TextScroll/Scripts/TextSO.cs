using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TextSO : ScriptableObject
{
    [TextArea(15, 20)]
    public string text;
}
