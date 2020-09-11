﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character Object")]
public class CharacterInfo : ScriptableObject{
    [Header("Insert character info below")]
    public string characterName;
    public Sprite portrait, statue, spirit, eye;

    public AnimatorOverrideController art;

    public Color color = Color.white;
}
