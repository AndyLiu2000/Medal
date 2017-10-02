//
//  NBMedal.cs
//
//  Created on 7/4/14.
//
//

using System.Collections.Generic;
using System;
using UnityEngine;
using System.Text;

public class NBMedal
{
    NBMedalConfInfo m_pMedalConfInfo;
    Color m_medalcolor;
    string m_medalId;
    string m_ownerroleid;    //所属角色ID
    int m_iswear;    // 是否穿戴
    string m_levelstr;
    string m_propstr;
    string m_starstr;
    int m_Atk_P;
    int m_Atk_M;
    int m_Def_P;
    int m_Def_M;
    int m_hP;
    int m_Dodge;
    int m_Cri;
    int m_AtkSpeed;

    public NBMedalConfInfo MedalConfInfo { get { return m_pMedalConfInfo; } set { m_pMedalConfInfo = value; } }
    public Color Medalcolor { get { return m_medalcolor; } set { m_medalcolor = value; } }
    public string MedalID {get {return m_medalId;} set {m_medalId = value;}}
    public string Ownerroleid {get {return m_ownerroleid;}set {m_ownerroleid = value;}}
    public int Iswear { get { return m_iswear; } set { m_iswear = value; } }
    public string Levelstr { get { return m_levelstr; } set { m_levelstr = value; } }
    public string Propstr { get { return m_propstr; } set { m_propstr = value; } }
    public string Starstr { get { return m_starstr; } set { m_starstr = value; } }
    public int Atk_P { get { return m_Atk_P; } set { m_Atk_P = value; } }
    public int Atk_M { get { return m_Atk_M; } set { m_Atk_M = value; } }
    public int Def_P { get { return m_Def_P; } set { m_Def_P = value; } }
    public int Def_M { get { return m_Def_M; } set { m_Def_M = value; } }
    public int HP { get { return m_hP; } set { m_hP = value; } }
    public int Dodge { get { return m_Dodge; } set { m_Dodge = value; } }
    public int Cri { get { return m_Cri; } set { m_Cri = value; } }
    public int AtkSpeed { get { return m_AtkSpeed; } set { m_AtkSpeed = value; } }

    public NBMedal()
    {
        m_medalId = "";
        m_ownerroleid = "";
        m_pMedalConfInfo = null;
        m_iswear = 0;
        m_levelstr = "";
        m_propstr = "";
        m_starstr = "";
        m_medalcolor = new Color(210, 210, 210);//gray
        m_Atk_P = 0;
        m_Atk_M = 0;
        m_Def_P = 0;
        m_Def_M = 0;
        m_hP = 0;
        m_Dodge = 0;
        m_Cri = 0;
        m_AtkSpeed = 0;
    }

    public bool Init(string pMedalId)
    {
        bool bResult = false;

        if (pMedalId == string.Empty || pMedalId == "0")
        {
            NBGame.NB_LOG_WARNING("MedalId = NULL");
            goto Exit0;
        }

        m_medalId = pMedalId;
        m_pMedalConfInfo = NBGameConfig.SharedConfig().GetMedalConfInfo(m_medalId);
        bResult = true;

        switch (m_pMedalConfInfo.QualityLevel)
        {
            case 1:
                m_medalcolor = new Color(210, 210, 210);//gray
                break;
            case 2:
                m_medalcolor = new Color(255, 255, 255);//white
                break;
            case 3:
                m_medalcolor = new Color(40, 245, 130);//green
                break;
            case 4:
                m_medalcolor = new Color(20, 143, 255);//blue
                break;
            case 5:
                m_medalcolor = new Color(182, 59, 233);//Purple
                break;
            case 6:
                m_medalcolor = new Color(255, 165, 50);//Orange
                break;
            case 7:
                m_medalcolor = new Color(255, 0, 0);//red
                break;
            default:
                m_medalcolor = new Color(96, 96, 96);//gray
                break;

        }
        m_propstr = GetCurrentAttri();
        NBLOG("Currentequip:\n\n {0}", m_propstr.ToString());
        Exit0:
        return bResult;
    }

    public int CalcAttriCommon(int level, int initAttrInParameter, int attrADInParameter, int attrAPInClass, int attrAPInequip)
    {
        double result = 0.0f;

        result = initAttrInParameter + level * attrADInParameter;
        result *= (100.0f + attrAPInClass) / 100.0f;
        result *= (attrAPInequip) / 100.0f;

        return (int)result;
    }

    //当前勋章属性计算 跟装备一样的计算
    public string GetCurrentAttri()
    {
        NBMedalConfInfo p_info = NBGameConfig.GetMedalConfInfo();
        if (p_info == null)
        {
            NB_ASSERT(false);
            return "";
        }

        NBEquipParametersConfInfo p_equipPara = NBGameConfig.SharedConfig().GetEquipParametersConfInfo(p_info.QualityLevel, p_info.StarLevel);
        if (p_equipPara == null)
        {
            NB_ASSERT(false);
            return "";
        }
        StringBuilder propstr = new StringBuilder();
        NBClassConfInfo pClassInfo = NBGameConfig.SharedConfig().getClassConfInfo(p_info.ClassID);
        if (p_equipPara != null)
        {
            m_Atk_P = CalcAttriCommon(p_info.AttriLevel, p_equipPara.initAtk_P, p_equipPara.atkAD_P, pClassInfo.AtkAP_P, p_info.AtkAP_P);
            if (m_Atk_P > 0)
            {
                propstr.Append(string.Format("{0} {1}\n\n", NBStringResource.GetText("equip_Atk_P"), m_Atk_P));
            }

            m_Atk_M = CalcAttriCommon(p_info.AttriLevel, p_equipPara.initAtk_M, p_equipPara.atkAD_M, pClassInfo.AtkAP_M, p_info.AtkAP_M);
            if (m_Atk_M > 0)
            {
                propstr.Append(string.Format("{0} {1}\n\n", NBStringResource.GetText("equip_Atk_M"), m_Atk_M));
            }


            m_Def_P = CalcAttriCommon(p_info.AttriLevel, p_equipPara.initDef_P, p_equipPara.defAD_P, pClassInfo.DefAP_P, p_info.DefAP_P);
            if (m_Def_P > 0)
            {
                propstr.Append(string.Format("{0} {1}\n\n", NBStringResource.GetText("equip_Def_P"), m_Def_P));
            }

            m_Def_M = CalcAttriCommon(p_info.AttriLevel, p_equipPara.initDef_M, p_equipPara.defAD_M, pClassInfo.DefAP_M, p_info.DefAP_M);
            if (m_Def_M > 0)
            {
                propstr.Append(string.Format("{0} {1}\n\n", NBStringResource.GetText("equip_Def_M"), m_Def_M));
            }

            m_hP = CalcAttriCommon(p_info.AttriLevel, p_equipPara.initHP, p_equipPara.hPAD, pClassInfo.HPAP, p_info.HPAP);
            if (m_hP > 0)
            {
                propstr.Append(string.Format("{0} {1}\n\n", NBStringResource.GetText("equip_HP"), m_hP));
            }

            m_Dodge = CalcAttriCommon(p_info.AttriLevel, p_equipPara.initDod, p_equipPara.DodgeAD, pClassInfo.DodgeAP, p_info.DodgeAP);
            if (m_Dodge > 0)
            {
                propstr.Append(string.Format("{0} {1}\n\n", NBStringResource.GetText("equip_Dodge"), m_Dodge));
            }

            m_Cri = CalcAttriCommon(p_info.AttriLevel, p_equipPara.initCri, p_equipPara.CriAD, pClassInfo.CritAP, p_info.CriAP);
            if (m_Cri > 0)
            {
                propstr.Append(string.Format("{0} {1}\n\n", NBStringResource.GetText("equip_Cri"), m_Cri));
            }

            m_AtkSpeed = CalcAttriCommon(p_info.AttriLevel, p_equipPara.initAtkSpeed, p_equipPara.AtkSpeedAD, pClassInfo.AtkSpeedAp, p_info.AtkSpeedAP);
            if (m_AtkSpeed > 0)
            {
                propstr.Append(string.Format("{0} {1}\n\n", NBStringResource.GetText("equip_AtkSpeed"), m_AtkSpeed));
            }
        }

        NBLOG("{0}", propstr.ToString());
        return propstr.ToString();
    }

    public int GetCount()
    {
        return NBGameWorld.SharedInstance().GetMe().GetMedalCount(m_medalId);
    }

    public NBMedal GetMedalInPlayer(NBEntityId_t heroentityid)
    {
        NBMedal pRet = this;
        NBPlayer pMe = NBGameWorld.SharedInstance().GetMe();
        IList<NBMedal> medalList = pMe.GetMedalList();
        foreach(NBMedal iter in medalList)
        {
            if (iter.MedalID == m_pMedalConfInfo.MedalID)
            {
                if (iter.Iswear == 0)
                {
                    pRet = iter;
                }
                if (iter.Ownerroleid == heroentityid)
                {
                    pRet = iter;
                    break;
                }

            }
        }
        return pRet;

    }
}

