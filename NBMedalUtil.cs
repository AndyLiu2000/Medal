//
//  NBMedalUtil.cs
//
//  Created on 7/15/14.
//
//
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class NBMedalUtil
{
    public void initMedal(NBMedal pMedal, BOH.MsgUserMedalInfo info)
    {
	    if (pMedal == null) return;

        pMedal.Init(info.Medalid);
        pMedal.Iswear = info.Iswear;
        pMedal.Ownerroleid = info.Ownerroleid;
    }
}
