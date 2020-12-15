using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="New video",menuName = "Video")]
public class MovieDB : ScriptableObject
{
    public new string title;
    public string description;

    public string pathLink;
    public Sprite videoImage;
}
