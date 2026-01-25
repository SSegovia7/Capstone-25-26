using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class uiHandler : MonoBehaviour
{
    private bool _PAUSEMENUOPEN = false;
    [SerializeField] private GameObject _PAUSEMENU;
    [SerializeField] private TMP_Text _HEALTHDISPLAY;
    // Start is called before the first frame update
    void Start()
    {
        //
    }
    public void UpdateHealthDisplay(int hp)
    {
        if (hp < 10)
        {
            _HEALTHDISPLAY.text =  hp.ToString();
        }
        else
        {
            _HEALTHDISPLAY.text = hp.ToString();
        }
        
    }
    public void TogglePauseMenu()
    {
        _PAUSEMENUOPEN = !_PAUSEMENUOPEN;
        _PAUSEMENU.SetActive(_PAUSEMENUOPEN);
    }
}
