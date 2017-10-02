//
//  NBMedalData.cs
//
//  Created on 4/8/14.
//
//
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Text;

public class NBMedalData
{
    int m_boxnum = -1;
    ArrayList m_medalArray = new ArrayList();
    ArrayList m_medalRoleArray = new ArrayList();
    NBMedalDataInfo m_pCurrentInfo;
    int bagnum = 0;
    int bagtotalcount = 0;
    string m_current_roleid = "";
    int m_current_parent;
    string gray_suitpropstr;

    NBMedalData ms_pSharedMedalData = null;

    ~NBMedalData()
    {
        if (m_medalArray != null)
        {
            m_medalArray.Clear();
        }
        if (m_medalRoleArray != null)
        {
            m_medalRoleArray.Clear();
        }

        NBGameWorld.SharedInstance().removeEventDelegate(this);
    }

    void SetData()
    {

    }

    public bool InitData()
    {
        string data;
        BOH.MsgGetUserEquipInfoReq req;
        uint uid = NBGameWorld.SharedInstance().GetMe().GetUid();
        uint areaid = NBGameWorld.SharedInstance().GetMe().GetAreaId();
        req.set_uid(NBGameWorld.SharedInstance().GetMe().GetUid());
        req.set_areaid(NBGameWorld.SharedInstance().GetMe().GetAreaId());

        NBLOG("NBMedalData.initData send packet  uid {0}", uid);
        req.SerializeToString(data);
        NBGameWorld.SharedInstance().GetGameClient().Send(CMD_ENTRY_Get_USER_EQUIP_INFO, data, data.Length);
        return true;
    }

    public bool Init()
    {
        if (!UILayer.init())
        {
            return false;
        }
        NBGameWorld.SharedInstance().addEventDelegate(this);
        InitData();
        SetData();
        return true;
    }

    public int CalcAttrCommon(int level, int initAttrInParameter, int attrADInParameter, int attrAPInClass, int attrAPInequip)
    {
        double result = 0.0f;

        result = initAttrInParameter + level * attrADInParameter;
        result *= (100.0f + attrAPInClass) / 100.0f;
        result *= (attrAPInequip) / 100.0f;

        return (int)result;
    }

    public ArrayList GetMedalRoleArray()
    {
        m_medalRoleArray.Clear();

        for (int index = 0; index < m_medalArray.Count; index++)
        {
            NBMedalDataInfo p_Info = m_medalArray[index] as NBMedalDataInfo;

            if (p_Info.Iswear == 1)
            {
                m_medalRoleArray.Add(p_Info);
            }
        }

        return m_medalRoleArray;
    }

    public void ChangeMedalArray(BOH.DataChange dataChangeResp)
    {
	    for (int i = 0; i<dataChangeResp.equipadd_size(); i++)
	    {
		    const MsgUserEquipmentInfo medalInforesp = dataChangeResp.equipadd(i);
		    NBMedalDataInfo p_info = new NBMedalDataInfo();
            FullMedalDataInfo(p_info, medalInforesp);

            NBLOG("add medalid = {0}", p_info.Entityid);
            ////////////////////////////
            m_medalArray.Add(p_info);
	    }

	    for (int i = 0; i<dataChangeResp.equipdelete_size(); i++)
	    {
		    const MsgUserEquipmentInfo medalInforesp = dataChangeResp.equipdelete(i);
		    NBMedalDataInfo p_info = new NBMedalDataInfo();

		    ////////////////////////////
		    for (int index = m_medalArray.Count - 1; index >= 0; index--)
		    {
			    NBMedalDataInfo p_oldInfo = m_medalArray[index] as NBMedalDataInfo;
			    if (p_oldInfo.Entityid == medalInforesp.Entityid())
			    {
                    NBLOG("remove medalid = {0}", p_info.Entityid);
                    m_medalArray.RemoveAt(index);
				    break;
			    }
		    }

	    }
    }

    public void FullMedalDataInfo(NBMedalDataInfo p_info, BOH.MsgUserEquipmentInfo medalInforesp)
    {
	    NBMedalConfInfo p_medalConfInfo = null;

        p_medalConfInfo = NBGameConfig.SharedConfig().GetMedalConfInfo(medalInforesp.equipid());

        NBEquipParametersConfInfo p_medalPara = NBGameConfig.SharedConfig().GetEquipParametersConfInfo(medalInforesp.quality(), medalInforesp.star());

        NBClassConfInfo pClassInfo = NBGameConfig.SharedConfig().GetClassConfInfo(p_medalConfInfo.ClassID);

        p_info.Entityid = medalInforesp.Entityid;
	    p_info.Iswear = medalInforesp.Iswear;
	    p_info.Experience = medalInforesp.Experience;
	    p_info.Level = medalInforesp.Level;
	    p_info.Quality = medalInforesp.Quality;
	    p_info.Star = medalInforesp.Star;
	    p_info.Ownerroleid = medalInforesp.Ownerroleid;
	    p_info.MedalID = p_medalConfInfo.MedalID;
	    p_info.Name = p_medalConfInfo.Name;
	    p_info.InitLevel = p_medalConfInfo.AttriLevel;
	    p_info.QualityLevel = p_medalConfInfo.QualityLevel;
	    p_info.ClassID = p_medalConfInfo.ClassID;
	    p_info.SlotID = p_medalConfInfo.SlotID;
	    p_info.AtkAP_P = p_medalConfInfo.AtkAP_P;
	    p_info.AtkAP_M = p_medalConfInfo.AtkAP_M;
	    p_info.DefAP_P = p_medalConfInfo.DefAP_P;
	    p_info.DefAP_M = p_medalConfInfo.DefAP_M;
	    p_info.HPAP = p_medalConfInfo.HPAP;
	    p_info.DodgeAP = p_medalConfInfo.DodgeAP;
	    p_info.CriAP = p_medalConfInfo.CriAP;
	    p_info.AtkSpeedAP = p_medalConfInfo.AtkSpeedAP;
	    p_info.ResID = p_medalConfInfo.ResID;
        /////////////////////////////
        string propstr = "";
        string temp;

        temp = string.Format("{0}: {1}/{2} \n", NBStringResource.GetText("medal_level"), p_info.Level, NBGameWorld.SharedInstance().GetMe().GetLevel());
	    p_info.Levelstr = temp;

	    if (p_info.Star > 0)
	    {
		    temp = string.Format("+ {0} \n", p_info.Star);
		    p_info.Starstr = temp;
	    }
	    else
	    {
		    p_info.Starstr = "";
	    }

        p_info.Serverpropstr = propstr.ToString();
	    propstr = GetCurrentmedal(p_info, p_medalPara);

        NBLOG("Currentmedal:\n {0}", propstr);
        p_info.Propstr.Append(propstr);
    }

    public string GetSuitProp(string tierid, int num)
    {
        string propstr = "";

        return propstr;
    }

    public void plusSuit(ref NBMedalSuit medalsuit, NBEquipTierConfInfo p_medalTier)
    {
        medalsuit.Atk_P += p_medalTier.AtkAD_P;
        medalsuit.Atk_M += p_medalTier.AtkAD_M;
        medalsuit.Def_P += p_medalTier.DefAD_P;
        medalsuit.Def_M += p_medalTier.DefAD_M;
        medalsuit.HP += p_medalTier.HPAD;
        medalsuit.Dodge += p_medalTier.DodgeAD;
        medalsuit.Cri += p_medalTier.CriAD;
        medalsuit.AtkSpeed += p_medalTier.AtkSpeedAD;
    }

    public string GetSuitPropStr(NBMedalSuit medalsuit)
    {
        StringBuilder propstr = new StringBuilder();
        string temp;
        int rownum = 0;
        string rowstr = "\n";
        if (rownum % 2 == 0)
            rowstr = "      ";
        else
            rowstr = "\n";

        if (medalsuit.Atk_P > 0)
        {
            temp = string.Format("{0}: +{1} {2}", NBStringResource.GetText("medal_Atk_P"), medalsuit.Atk_P, rowstr);
            propstr.Append(temp);
            rownum++;
        }
        if (rownum % 2 == 0)
            rowstr = "      ";
        else
            rowstr = "\n";

        if (medalsuit.Atk_M > 0)
        {
            temp = string.Format("{0}: +{1} {2}", NBStringResource.GetText("medal_Atk_M"), medalsuit.Atk_M, rowstr);
            propstr.Append(temp);
            rownum++;
        }
        if (rownum % 2 == 0)
            rowstr = "      ";
        else
            rowstr = "\n";

        if (medalsuit.Def_P > 0)
        {
            temp = string.Format("{0}: +{1} {2}", NBStringResource.GetText("medal_Def_P"), medalsuit.Def_P, rowstr);
            propstr.Append(temp);
            rownum++;
        }
        if (rownum % 2 == 0)
            rowstr = "      ";
        else
            rowstr = "\n";

        if (medalsuit.Def_M > 0)
        {
            temp = string.Format("{0}: +{1} {2}", NBStringResource.GetText("medal_Def_M"), medalsuit.Def_M, rowstr);
            propstr.Append(temp);
            rownum++;
        }
        if (rownum % 2 == 0)
            rowstr = "      ";
        else
            rowstr = "\n";

        if (medalsuit.hP > 0)
        {
            temp = string.Format("{0}: +{1} {2}", NBStringResource.GetText("medal_HP"), medalsuit.hP, rowstr);
            propstr.Append(temp);
            rownum++;
        }
        if (rownum % 2 == 0)
            rowstr = "      ";
        else
            rowstr = "\n";

        if (medalsuit.Dodge > 0)
        {
            temp = string.Format("{0}: +{1} {2}", NBStringResource.GetText("medal_Dodge"), medalsuit.Dodge, rowstr);
            propstr.Append(temp);
            rownum++;
        }
        if (rownum % 2 == 0)
            rowstr = "      ";
        else
            rowstr = "\n";

        if (medalsuit.Cri > 0)
        {
            temp = string.Format("{0}: +{1} {2}", NBStringResource.GetText("medal_Cri"), medalsuit.Cri, rowstr);
            propstr.Append(temp);
            rownum++;
        }
        if (rownum % 2 == 0)
            rowstr = "      ";
        else
            rowstr = "\n";

        if (medalsuit.AtkSpeed > 0)
        {
            temp = string.Format("{0}: +{1} {2}", NBStringResource.GetText("medal_AtkSpeed"), medalsuit.AtkSpeed, rowstr);
            propstr.Append(temp);
            rownum++;
        }
        return propstr.ToString();
    }

    public bool CreateSharedMedalData()
    {
        if (ms_pSharedMedalData == null)
        {
            ms_pSharedMedalData = new NBMedalData();
            if (ms_pSharedMedalData != null && ms_pSharedMedalData.Init())
            {
                return true;
            }
            else
            {
                ms_pSharedMedalData = null;
            }
        }
        return false;
    }

    //当前勋章属性计算
    public string GetCurrentmedal(NBMedalDataInfo p_info, NBEquipParametersConfInfo p_medalPara)
    {
        StringBuilder propstr = new StringBuilder();
        string temp;
        NBClassConfInfo pClassInfo = NBGameConfig.SharedConfig().GetClassConfInfo(p_info.ClassID);
        if (p_medalPara != null)
        {
            p_info.Atk_P = (p_medalPara.initAtk_P + p_medalPara.atkAD_P * p_info.Level) * (p_info.AtkAP_P) / 100 * (100 + pClassInfo.AtkAP_P) / 100;
            if (p_info.Atk_P > 0)
            {
                temp = string.Format("{0}: {1} \n", NBStringResource.GetText("medal_Atk_P"), p_info.Atk_P);
                propstr.Append(temp);
            }
            p_info.Atk_M = (p_medalPara.initAtk_M + p_medalPara.atkAD_M * p_info.Level) * (float)(p_info.AtkAP_M) / 100 * (100 + pClassInfo.AtkAP_M) / 100;
            if (p_info.Atk_M > 0)
            {
                temp = string.Format("{0}: {1} \n", NBStringResource.GetText("medal_Atk_M"), p_info.Atk_M);
                propstr.Append(temp);
            }

            p_info.Def_P = (p_medalPara.initDef_P + p_medalPara.defAD_P * p_info.Level) * (p_info.DefAP_P) / 100 * (100 + pClassInfo.DefAP_P) / 100;
            if (p_info.Def_P > 0)
            {
                temp = string.Format("{0}: {1} \n", NBStringResource.GetText("medal_Def_P"), p_info.Def_P);
                propstr.Append(temp);
            }
            p_info.Def_M = (p_medalPara.initDef_M + p_medalPara.defAD_M * p_info.Level) * (p_info.DefAP_M) / 100 * (100 + pClassInfo.DefAP_M) / 100;
            if (p_info.Def_M > 0)
            {
                temp = string.Format("{0}: {1} \n", NBStringResource.GetText("medal_Def_M"), p_info.Def_M);
                propstr.Append(temp);
            }
            p_info.HP = (p_medalPara.initHP + p_medalPara.hPAD * p_info.Level) * (p_info.HPAP) / 100 * (100 + pClassInfo.HPAP) / 100;
            if (p_info.HP > 0)
            {
                temp = string.Format("{0}: {1} \n", NBStringResource.GetText("medal_HP"), p_info.HP);
                propstr.Append(temp);
            }
            p_info.Dodge = (p_medalPara.initDod + p_medalPara.DodgeAD * p_info.Level) * (p_info.DodgeAP) / 100 * (100 + pClassInfo.DodgeAP) / 100;
            if (p_info.Dodge > 0)
            {
                temp = string.Format("{0}: {1} \n", NBStringResource.GetText("medal_Dodge"), p_info.Dodge);
                propstr.Append(temp);
            }
            p_info.Cri = (p_medalPara.initCri + p_medalPara.CriAD * p_info.Level) * (p_info.CriAP) / 100 * (100 + pClassInfo.CritAP) / 100;
            if (p_info.Cri > 0)
            {
                temp = string.Format("{0}: {1} \n", NBStringResource.GetText("medal_Cri"), p_info.Cri);
                propstr.Append(temp);
            }
            p_info.AtkSpeed = (p_medalPara.initAtkSpeed + p_medalPara.AtkSpeedAD * p_info.Level) * (p_info.AtkSpeedAP) / 100 * (100 + pClassInfo.AtkSpeedAp) / 100;
            if (p_info.AtkSpeed > 0)
            {
                temp = string.Format("{0}: {1} \n", NBStringResource.GetText("medal_AtkSpeed"), p_info.AtkSpeed);
                propstr.Append(temp);
            }
        }
        return propstr.ToString();
    }

    public bool IsMedalExist(string medalID)
    {
        for (int i = 0; i < m_medalArray.Count; i++)
        {
            NBMedalDataInfo pMedalInfo = m_medalArray[i] as NBMedalDataInfo;
            if (pMedalInfo.Medalid.CompareTo(medalID) == 0)
            {
                return true;
            }
        }
        return false;
    }
}

public class NBMedalDataInfo
{
    public uint Uid;    // 用户ID
    public string Entityid;    // 勋章实体ID
    public uint Iswear;    // 是否穿戴
    public string Medalid;    // 勋章ID
    public uint Experience;
    public uint Level;    // 等级
    public uint Quality;    // 品质
    public uint Star;    // 星级
    public string Ownerroleid;    // 所属角色ID

    //////////////////////////////////
    public string MedalID;
    public string Name;
    public int InitLevel;
    public int QualityLevel;
    public int ClassID;
    public int SlotID;     //勋章所在位置ID（home2界面6个槽位的ID）
    public string DesCription;
    public int AtkAP_P;
    public int AtkAP_M;
    public int DefAP_P;
    public int DefAP_M;
    public int HPAP;
    public int DodgeAP;
    public int CriAP;
    public int AtkSpeedAP;
    public string ResID;
    public string ResName;
    //////////////////////////////
    public int Atk_P;
    public int Atk_M;
    public int Def_P;
    public int Def_M;
    public int HP;
    public int Dodge;
    public int Cri;
    public int AtkSpeed;
    /////////////////
    public int CoinPrice;
    public int CoinLevelUp;
    public int BaseExp;
    public int LevelUpExp;
    public int Exp;
    ////////////////

    //////////////////////////////
    public string Serverpropstr;
    public string Levelstr;
    public StringBuilder Propstr;
    public string Starstr;
    public Color Medalcolor;

    public NBMedalDataInfo()
    {
    }

    
};
