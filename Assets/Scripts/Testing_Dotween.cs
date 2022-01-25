using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Testing_Dotween : MonoBehaviour
{
    
    public RectTransform slideTab;
    public RectTransform ShopTab;
    public GameObject Shop_Bg;
    public bool tab_hidden;
    public bool shop_hidden;
    private bool show_hide_shop;
    // Start is called before the first frame update
    void Start()
    {
        
        slideTab.DOAnchorPos(new Vector2(-250, 0), .25f);
    }

    // Update is called once per frame
    void Update()
    {
        if(slideTab.anchoredPosition == new Vector2(250, 0))
        {
            tab_hidden = true;
        }
        else
        {
            tab_hidden = false;
        }

        if(ShopTab.anchoredPosition == new Vector2(0, 500))
        {
            shop_hidden = true;
        }
        else
        {
            shop_hidden = false;
        }
        
    }


    public void Hide_Panel()
    {
        slideTab.DOAnchorPos(new Vector2(250, 0), .25f);
    }

    public void Show_Panel()
    {
        slideTab.DOAnchorPos(new Vector2(-250, 0), .25f);
    }

    public void Show_Shop()
    {
        slideTab.DOAnchorPos(new Vector2(250, 0), .25f);
        ShopTab.DOLocalMoveX(0, .25f).SetDelay(.25f);

    }

    public void Hide_Shop()
    {
        //ShopTab.DOAnchorPos(new Vector2(0, 500), .25f);
        ShopTab.DOLocalMoveX(1500, .25f);
        slideTab.DOLocalMoveX(710, .25f).SetDelay(.25f);
    }

    public void Hide_And_Disable_Panel()
    {
        slideTab.DOAnchorPos(new Vector2(250, 0), .25f);
        Hide_Panel();
        StartCoroutine(check_if_hidden_disable());
    }
   
    private IEnumerator check_if_hidden_disable()
    {
        while (!tab_hidden)
        {
            Debug.Log("not hidden");
            yield return null;
        }
        if (tab_hidden)
        {
            Debug.Log("finished hiding");
            gameObject.SetActive(false);
        }
    }

    /*
    private IEnumerator check_if_hidden_show_shop()
    {
        while (!tab_hidden)
        {
            Debug.Log("not hidden");
            yield return null;
        }
        if (tab_hidden)
        {
            Debug.Log("finished hiding");
            if (show_hide_shop)
            {
                Shop_Bg.SetActive(true);
                ShopTab.DOLocalMoveY(0, .25f);
                //ShopTab.DOAnchorPos(new Vector2(0, -540), .25f).SetDelay(.25f);
            }
            
        }
    }
    */
}
