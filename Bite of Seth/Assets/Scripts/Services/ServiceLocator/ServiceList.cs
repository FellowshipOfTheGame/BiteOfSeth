using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/ServiceList")]
public class ServiceList : ScriptableObject
{
    public List<GameService> services = new List<GameService>();
}
