using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectOnEnable : MonoBehaviour
{
    private void OnEnable() {
        EventSystem.current.SetSelectedGameObject(null); // desbugar o botão n estar sendo selecionado corretamente
        Selectable s = GetComponent<Selectable>();
        s.Select();
    }
}