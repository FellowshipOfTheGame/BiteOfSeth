using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectButtonOnMenu : MonoBehaviour
{

    private Animator anim;

    public void Select()
    {
        //EventSystem.current.SetSelectedGameObject(gameObject); // desbugar o botão n estar sendo selecionado corretamente
        
        Selectable s = GetComponent<Selectable>();
        s.Select();
        anim = GetComponent<Animator>();
        anim.ResetTrigger("normal");
        Invoke("UpdateAnim", 0.5f);
        
    }

    private void UpdateAnim()
    {
        anim.ResetTrigger("normal");
        anim.SetTrigger("select");
    }

}
