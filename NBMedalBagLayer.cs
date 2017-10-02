//
//  NBMedalBagLayer.cs
//
//  Created on 7/20/14.
//
//

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class NBMedalBagLayer : MonoBehaviour
{
    public GameObject m_pRootLayer;
    NBNoticeBar m_pNoticeBar;
    NBTop1 m_pTop;
    NBBottom1 m_pBottom;
    NBMiddleMedalBaglist m_pMiddle;

    void Start()
    {
        //////////////////////////////
        m_pNoticeBar = new NBNoticeBar();
        m_pTop = new NBTop1();
        m_pBottom = new NBBottom1();
        m_pMiddle = new NBMiddleMedalBaglist();

        NBUiHelper::addWidgetsToLayerTop(NBUiHelper.m_pUiLayer, m_pNoticeBar, m_pTop, m_pMiddle, null);
        NBUiHelper::addWidgetsToLayerBottom(NBUiHelper.m_pUiLayer, m_pBottom, null);
        NBUiHelper::addWidgetToLayerCenter(NBUiHelper.m_pUiLayer, m_pMiddle);

        return true;
    }
    public void onEnter()
    {
        NBBaseLayer.onEnter();
    }
}

