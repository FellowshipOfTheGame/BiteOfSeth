using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectButtonOnMenu : MonoBehaviour
{

    public void Select()
    {
        //EventSystem.current.SetSelectedGameObject(null); // desbugar o botão n estar sendo selecionado corretamente
        Selectable s = GetComponent<Selectable>();
        s.Select();
        Invoke("UpdateAnim", 0.5f);
    }

    private void UpdateAnim()
    {
        Animator a = GetComponent<Animator>();
        a.ResetTrigger("normal");
        a.SetTrigger("select");
    }

}
