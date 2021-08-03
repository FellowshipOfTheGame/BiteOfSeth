using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectButtonOnMenu : MonoBehaviour
{

    private Animator anim;
    private Button firstButton;

    private void Start()
    {
        firstButton = gameObject.GetComponent<Button>();
    }

    public void Select()
    {
        //EventSystem.current.SetSelectedGameObject(gameObject); // desbugar o botão n estar sendo selecionado corretamente
        Selectable s = GetComponent<Selectable>();
        s.Select();
        anim = GetComponent<Animator>();
        anim.ResetTrigger("normal");
        Invoke("UpdateAnim", 0.5f);
        BlockMenuNav();
    }

    private void UpdateAnim()
    {
        UnblockMenuNav();
        anim.ResetTrigger("normal");
        anim.SetTrigger("select");
    }

    public void BlockMenuNav()
    {
        if (firstButton) {
            Navigation nav = new Navigation();
            nav.mode = Navigation.Mode.None;
            firstButton.navigation = nav;
        }
    }

    public void UnblockMenuNav()
    {
        if (firstButton) {
            Navigation nav = new Navigation();
            nav.mode = Navigation.Mode.Automatic;
            firstButton.navigation = nav;
        }
    }

}
