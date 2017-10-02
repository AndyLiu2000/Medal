//
//  NBMedalUpComplete.cs
//
//  Created on 8/28/14.
//
//

using System.Collections.Generic;
using System;
using UnityEngine;

public class NBMedalUpComplete : MonoBehaviour
{

    GameObject m_pRootLayout;
	NBHero m_pNew;
    NBHero m_pOld;

    public NBMedalUpComplete()
    {
        m_pNew = new NBHero();
        m_pOld = new NBHero();
    }

    ~NBMedalUpComplete()
    {
        m_pNew = null;
        m_pOld = null;
    }

    void Start()
    {
        m_pRootLayout = GameObject.Find(NBUiHelper.NB_UI_MEDAL_UP_COMPLETE);
        NGUITools.AddChild(NBUiHelper.BaseLayer, m_pRootLayout);

        NBHero pCurrentHero = NBGameWorld.SharedInstance().GetMe().GetCurrentHero();

        m_pOld.Init(pCurrentHero.GetCharacterId());
        m_pNew.Init(pCurrentHero.GetCharacterId());

        m_pNew.SetExp(pCurrentHero.GetExp());
        m_pNew.SetQuality(pCurrentHero.GetQuality());
        m_pNew.SetStar(pCurrentHero.GetStar());
        m_pNew.SetPhaseLevel(pCurrentHero.GetPhaseLevel());

        m_pOld.SetExp(pCurrentHero.GetExp());
        m_pOld.SetQuality(pCurrentHero.GetQuality());
        m_pOld.SetStar(pCurrentHero.GetStar());
        m_pOld.SetPhaseLevel(pCurrentHero.GetPhaseLevel() - 1);

        m_pNew.calcAttr();
        m_pOld.calcAttr();

        if (!InitSkill())
        {
            return false;
        }

        Initleft();
        Initright();

        return true;
    }

    public void Initleft()
    {
        Dictionary<int, string> propmap;
        NBCharAttr charAtt;
        charAtt.Init(m_pOld);
        charAtt.GetPropMap(propmap, false);

        UILabel pOldNameLbl = m_pRootLayout.transform.Find("Label_name2").GetComponent<UILabel>();
        pOldNameLbl.text = m_pOld.GetName();

        UILabel pLevelLbl = m_pRootLayout.transform.FindChild("Panel_before").transform.FindChild("ImageView_attr1").transform.FindChild("Label_1").GetComponent<UILabel>();
        string level;
        level = string.Format("{0} {1}", NBGameWorld.SharedInstance().GetMe().GetCurrentHero().GetLevel(), NBGameWorld.SharedInstance().GetMe().GetLevel());
        pLevelLbl.text = level;

        string temp;
        for (int i = 2; i <= 9; i++)
        {
            temp = string.Format("ImageView_attr{0}", i);
            GameObject pAttriImgv = m_pRootLayout.transform.FindChild("Panel_before").transform.FindChild(temp).gameObject;
            pAttriImgv.SetActive(false);
        }

        int ipropseq = 1;
        for (int i = 0; i < propmap.Count; i++)
        {
            ipropseq++;
            if (ipropseq > 9)
            {
                break;
            }

            temp = string.Format("ImageView_attr{0}", ipropseq);
            GameObject pAttriImgv = m_pRootLayout.transform.FindChild("Panel_before").transform.FindChild(temp).gameObject;
            UILabel pLbl = pAttriImgv.transform.FindChild("Label_1").GetComponent<UILabel>();
            string strpropicon;
            strpropicon = string.Format("char_attr_icon_{0}", i);
            strpropicon = NBStringResource.GetText(strpropicon);
            string posheadframe = NBUiHelper.NB_UI_CONFIG_DIR + strpropicon;
            pAttriImgv.GetComponent<UISprite>().spriteName = posheadframe;
            pLbl.text = (i + 1).ToString();
            pAttriImgv.SetActive(true);
        }
    }

    public void Initright()
    {
        Dictionary<int, string> propmap;
        NBCharAttr charAtt;
        charAtt.Init(m_pNew);
        charAtt.GetPropMap(propmap, false);

        UILabel pOldNameLbl = m_pRootLayout.transform.FindChild("Label_name1").GetComponent<UILabel>();
        pOldNameLbl.text = m_pOld.GetName();

        UILabel pLevelLbl = m_pRootLayout.transform.FindChild("Panel_after").transform.FindChild("ImageView_attr1").transform.FindChild("Label_1").GetComponent<UILabel>();
        string level;
        level = string.Format("{0}{1}", NBGameWorld.SharedInstance().GetMe().GetCurrentHero().GetLevel(), NBGameWorld.SharedInstance().GetMe().GetLevel());
        pLevelLbl.text = level;

        string temp;
        for (int i = 2; i <= 9; i++)
        {
            
            temp = string.Format("ImageView_attr{0}", i);
            GameObject pAttriImgv = m_pRootLayout.transform.FindChild("Panel_before").transform.FindChild(temp).gameObject;
            pAttriImgv.SetActive(false);

            temp = string.Format("ImageView_Arrow{0}", i);
            GameObject pArrowImgv = m_pRootLayout.transform.FindChild("Panel_arrow").transform.FindChild(temp).gameObject;
            GameObject pArrowUpImgv = m_pRootLayout.transform.FindChild("Panel_arrowup").transform.FindChild(temp).gameObject;
            pAttriImgv.SetActive(false);
            pArrowImgv.SetActive(false);
            pArrowUpImgv.SetActive(false);
        }

        int ipropseq = 1;
        for (int i = 0; i < propmap.Count; i++)
        {
            ipropseq++;
            if (ipropseq > 9)
            {
                break;
            }

            temp = string.Format("ImageView_attr{0}", ipropseq);
            GameObject pAttriImgv = m_pRootLayout.transform.FindChild("Panel_after").transform.FindChild(temp).gameObject;
            UILabel pLbl = pAttriImgv.transform.FindChild("Label_1").GetComponent<UILabel>();
            temp = string.Format("ImageView_Arrow{0}", ipropseq);
            GameObject pArrowImgv = m_pRootLayout.transform.FindChild("Panel_arrow").transform.FindChild(temp).gameObject;
            GameObject pArrowUpImgv = m_pRootLayout.transform.FindChild("Panel_arrowup").transform.FindChild(temp).gameObject;

            string strpropicon;
            strpropicon = string.Format("char_attr_icon_{0}", i);
            strpropicon = NBStringResource.GetText(strpropicon);
            string posheadframe = NBUiHelper.NB_UI_CONFIG_DIR + strpropicon;
            pAttriImgv.GetComponent<UISprite>().spriteName = posheadframe;
            pLbl.text = (i + 1).ToString();
            pAttriImgv.SetActive(true);
            pArrowImgv.SetActive(true);
            pArrowUpImgv.SetActive(true);
        }
    }

    public bool InitSkill()
    {
        int nextphaselevel = m_pNew.GetPhaseLevel();
        if (nextphaselevel == 0)
        {
            return false;
        }
        NBMedalEvolutionConfInfo pNextMedalEv = NBGameConfig.SharedConfig().GetMedalEvolutionConfInfo(NBUiHelper.Make_pair(m_pNew.GetClassID(), nextphaselevel));
        if (pNextMedalEv == null)
        {
            return false;
        }
        string skillID = "";
        //待解锁的技能图片id
        string skilliconid = "";
        //待解锁的技能名字
        string nowSkillName = "";
        string descSkillName = "";
        //待解锁的条件
        string skillcond;
        string rowstr = "\n";
        NBCharacterConfInfo pCharacterConfInfo = NBGameConfig.SharedConfig().GetCharacterConfInfo(m_pNew.GetCharacterId());
        if (pCharacterConfInfo == null)
        {
            return false;
        }
        NBParameterConfInfo pParam = NBGameConfig.SharedConfig().GetParameterConfInfo();

        if (pNextMedalEv != null)
        {
            skillcond = string.Format(NBStringResource.GetText("medal_skillcond") + pNextMedalEv.phaselname);
        }

        //改变进阶顺序
        //2.1   1.5  5.2
        if (nextphaselevel == 1)
        {
            skillID = pCharacterConfInfo.autoSkill1Id;
            skilliconid = NBGameConfig.SharedConfig().GetSkillConfInfo(pCharacterConfInfo.autoSkill1Id).skillIconId;
            nowSkillName = NBGameConfig.SharedConfig().GetSkillConfInfo(pCharacterConfInfo.autoSkill1Id).skillName;
            descSkillName = nowSkillName;
        }
        else if (nextphaselevel == 2)
        {
            skillID = pCharacterConfInfo.autoSkill2Id;
            skilliconid = NBGameConfig.SharedConfig().GetSkillConfInfo(pCharacterConfInfo.autoSkill2Id).skillIconId;
            nowSkillName = NBGameConfig.SharedConfig().GetSkillConfInfo(pCharacterConfInfo.autoSkill2Id).skillName;
            descSkillName = nowSkillName;
        }
        else if (nextphaselevel == 3)
        {
            skillID = pCharacterConfInfo.autoSkill1Id;
            skilliconid = NBGameConfig.SharedConfig().GetSkillConfInfo(pCharacterConfInfo.autoSkill1Id).skillIconId;
            nowSkillName = NBGameConfig.SharedConfig().GetSkillConfInfo(pCharacterConfInfo.autoSkill1Id).skillName;
            descSkillName = nowSkillName;
        }
        else if (nextphaselevel == 4)
        {
            skillID = pCharacterConfInfo.autoSkill2Id;
            skilliconid = NBGameConfig.SharedConfig().GetSkillConfInfo(pCharacterConfInfo.autoSkill2Id).skillIconId;
            nowSkillName = NBGameConfig.SharedConfig().GetSkillConfInfo(pCharacterConfInfo.autoSkill2Id).skillName;
            descSkillName = nowSkillName;
        }
        else if (nextphaselevel == 5)
        {
            skillID = pCharacterConfInfo.activeSkillId;
            skilliconid = NBGameConfig.SharedConfig().GetSkillConfInfo(pCharacterConfInfo.activeSkillId).skillIconId;
            nowSkillName = NBGameConfig.SharedConfig().GetSkillConfInfo(pCharacterConfInfo.activeSkillId).skillName;
            descSkillName = nowSkillName;
        }
        else if (nextphaselevel == 6)
        {
            skillID = pCharacterConfInfo.passiveSkillId;
            skilliconid = NBGameConfig.SharedConfig().GetSkillConfInfo(pCharacterConfInfo.passiveSkillId).skillIconId;
            nowSkillName = NBGameConfig.SharedConfig().GetSkillConfInfo(pCharacterConfInfo.passiveSkillId).skillName;
            descSkillName = nowSkillName;
        }
        else if (nextphaselevel == 7)
        {
        }
        string finaldesc;
        string strconfphasex = string.Format("medal_phase{0}", nextphaselevel - 1);
        string strupdesc = string.Format("{0}", NBStringResource.GetText(strconfphasex));
        finaldesc = strupdesc + descSkillName;

        string skillname = nowSkillName;
        string desCription = NBStringResource.GetText("unlock_skill") + finaldesc;

        NBResConfInfo p_resInfo = NBGameConfig.SharedConfig().GetResConfInfo(skilliconid);
        UIButton skillBtn = m_pRootLayout.transform.FindChild("Button_skill").gameObject.GetComponent<UIButton>();
        string EquipHeadFrame = string.Format("{0}", NBUiHelper.NB_UI_HEAD_NO_FOUND_ICON);
        string EquipBorderFrame = string.Format("{0}", NBUiHelper.NB_UI_BORDER_NO_FOUND_ICON);
        if (p_resInfo != null)
        {
            EquipHeadFrame = string.Format("{0}", p_resInfo.headRes);
        }
        UISprite imageView_item = skillBtn.transform.FindChild("ImageView_backitem").gameObject.GetComponent<UISprite>();
        UISprite imageView_itemfront = skillBtn.transform.FindChild("ImageView_frontitem").gameObject.GetComponent<UISprite>();

        imageView_item.GetComponent<UISprite>().spriteName = EquipHeadFrame;
        imageView_itemfront.GetComponent<UISprite>().spriteName = EquipBorderFrame;

        UILabel pSkillNewLbl = m_pRootLayout.transform.FindChild("Label_skill").gameObject.GetComponent<UILabel>();
        pSkillNewLbl.text = desCription;
        if (skillID == "")
        {
            return true;
        }
        NBSkill pSkill = m_pNew.GetSkill(skillID);
        UILabel pSkillDesLbl = m_pRootLayout.transform.FindChild("Label_des").gameObject.GetComponent<UILabel>();
        pSkillDesLbl.text = pSkill.GetSkillConfInfo().tips;
        UILabel pSkillDesLbl1 = m_pRootLayout.transform.FindChild("Label_des1").gameObject.GetComponent<UILabel>();
        string propTextStr = NBSkillTips.Calc(skillID, pSkill.GetSkillLevel(), m_pNew);
        pSkillDesLbl1.text = propTextStr;
        return true;
    }
}

