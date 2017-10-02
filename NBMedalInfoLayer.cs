//
//  NBMedalInfoLayer.cs
//
//  Created on 7/3/14.
//
//

using System.Collections.Generic;
using System;
using UnityEngine;

public class NBMedalInfoLayer : MonoBehaviour
{
    public GameObject m_pBgLayout;
    NBMiddleMedalInfo m_pMiddle;

    void Start()
    {
        //∏∏¿‡≥ı ºªØ
        if (!NBBaseLayer.init())
        {
            return false;
        }

        m_pMiddle = new NBMiddleMedalInfo();
        NBUiHelper.addWidgetToLayerCenter(NBUiHelper.m_pUiLayer, m_pMiddle);

        m_pBgLayout.GetComponent<UISprite>().color = new Color(111,111,111);

        return true;
    }

    public void onEnter()
    {
        NBBaseLayer.onEnter();
    }

    public void SetParentOfMiddle(NBHome2Layer pLayer)
    {
        m_pMiddle.SetParentScene(pLayer);
    }
}

