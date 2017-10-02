//
//  NBHomeMedalLayer.cs
//
//  Created on 7/9/14.
//
//

using System.Collections.Generic;
using System;
using UnityEngine;
using System.Text;

public class NBHomeMedalLayer : MonoBehaviour
{
    public static NBHomeMedalLayer ms_this;
    public static NBHomeMedalLayer SharedHomeMedalLayer() {return ms_this; }
    public GameObject m_pRootLayer;
    NBNoticeBar m_pNoticeBar;
    GameObject m_pTop;
    NBBottom1 m_pBottom;
    NBMiddleMedalList m_pMiddle;
    NBMedalInfoLayer m_pMedalinfoLayer;			//勋章信息
    NBMedal ms_pMedalInfo = null;

    void Start()
    {
        //////////////////////////////
        // 1. super init first
        ms_this = this;
        if (!NBBaseLayer.Init())
        {
            return false;
        }
        m_pNoticeBar = new NBNoticeBar();
        m_pTop = GameObject.Find(NBUiHelper.NB_UI_CONFIG_DIR);
        m_pBottom = new NBBottom1();
        m_pMiddle = new NBMiddleMedalList();

        InitTitle(false);
        NBUiHelper.addWidGetsToLayerTop(NBUiHelper.m_pUiLayer, m_pTop, m_pMiddle, null);
        NBUiHelper.addWidGetsToLayerBottom(NBUiHelper.m_pUiLayer, m_pBottom, m_pNoticeBar, null);
        NBUiHelper.setNodePosUsingAnchorPoint(NBUiHelper.m_pNoticeBar, new Vector2(0, m_pBottom.GetContentSize().height - 6), new Vector2(0, 0));
        NBUiHelper.setNodePosUsingAnchorPoint(NBUiHelper.m_pMiddle, new Vector2(0, m_pBottom.GetContentSize().height + m_pNoticeBar.GetContentSize().height), new Vector2(0, 0));
        return true;
    }

    public void onEnter()
    {
        NBBaseLayer.onEnter();
        NBUiHelper.moveTop(m_pTop);
        NBUiHelper.moveBottom(m_pBottom);
    }

    public void PushMedalInfo(NBMedal pCurrentInfo)
    {
        //加载勋章信息界面
        //初始化时要用到当前勋章信息
        m_pMedalinfoLayer = new NBMedalInfoLayer();
        ms_pMedalInfo = pCurrentInfo;
    }

    public void InitTitle(bool flag)
    {
        NBPlayer pPlayer = NBGameWorld.SharedInstance().GetMe();
        string temp;

        UILabel pPlayerGoldLblInTop1 = GetChildComponent<UILabel>(m_pTop, "Label_gold");
        UILabel pPlayerCrystalLblInTop1 = GetChildComponent<UILabel>(m_pTop, "Label_crystal");
        UILabel pPlayerStaminaLblInTop1 = GetChildComponent<UILabel>(m_pTop, "Label_stamina");
        UILabel pPlayerVitalityLblInTop1 = GetChildComponent<UILabel>(m_pTop, "Label_vitality");
        UIProgressBar pStaminaLBar = GetChildComponent<UIProgressBar>(m_pTop, "LoadingBar_endurance");
        UIProgressBar pVitalityLBar = GetChildComponent<UIProgressBar>(m_pTop, "LoadingBar_hp");

        temp = string.Format("{0}", pPlayer.GetGold());
        pPlayerGoldLblInTop1.text = temp;

        temp = string.Format("{0}", pPlayer.GetCrystal());
        pPlayerCrystalLblInTop1.text = temp;

        temp = string.Format("{0}/{1}", pPlayer.GetStamina(), pPlayer.GetMaxStamina());
        pPlayerStaminaLblInTop1.text = temp;

        temp = string.Format("{0}/{1}", pPlayer.GetVitality(), pPlayer.GetMaxVitality());
        pPlayerVitalityLblInTop1.text = temp;

        NBUserConfInfo pUserInfo = NBGameConfig.SharedConfig().GetUserInfo(pPlayer.GetLevel());
        float nNum = (pPlayer.GetStamina() * 100.0f) / pUserInfo.maxStamina;
        pStaminaLBar.value = nNum;

        nNum = (pPlayer.GetVitality() * 100.0f) / pUserInfo.maxVitality;
        pVitalityLBar.value = nNum;

        UILabel pBagLbl = GetChildComponent<UILabel>(m_pTop, "Label_bagsize");
        if (flag)
        {
            temp = string.Format("{0}/{1}", pPlayer.m_medalFragmentMap.size(), pPlayer.GetMedalFrgBagSize());
        }
        else
        {
            temp = string.Format("{0}/{1}", NBGameWorld.SharedInstance().GetMe().GetMedalCount(),
                NBGameWorld.SharedInstance().GetMe().GetMedalBagSize());
        }

        pBagLbl.text = temp;

        UILabel pLvLbl = GetChildComponent<UILabel>(m_pTop, "Label_level");
        temp = string.Format("{0}", pPlayer.GetLevel());
        pLvLbl.text = temp;
    }

    static T GetChildComponent<T>(GameObject parent,string componentName)
    {
        return parent.transform.FindChild(componentName).GetComponent<T>();
    }
}
