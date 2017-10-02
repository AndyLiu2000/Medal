//
//  NBMedalConfInfo.cs
//
//  Created on 3/24/14.
//
//

public class NBMedalConfInfo
{
    public string  MedalID;
	public string  Name;
    public int ClassID;
    public int QualityLevel;
    public int StarLevel;
    public int AttriLevel;
    public int SlotID;
    public int AtkAP_P;
    public int AtkAP_M;
    public int DefAP_P;
    public int DefAP_M;
    public int HPAP;
    public int DodgeAP;
    public int CriAP;
    public int AtkSpeedAP;
    public int SoldPrice;
    public string LootMission_1;
    public string LootMission_2;
    public string LootMission_3;
    public string ResID;

	public NBMedalConfInfo() {
		MedalID = "";
		Name = "";
		ClassID = 0;
		QualityLevel = 0;
		StarLevel = 0;
		AttriLevel = 0;
		SlotID = 0;
		AtkAP_P = 0;
		AtkAP_M = 0;
		DefAP_P = 0;
		DefAP_M = 0;
		HPAP = 0;
		DodgeAP = 0;
		CriAP = 0;
		AtkSpeedAP = 0;
		SoldPrice = 0;
		LootMission_1 = "";
		LootMission_2 = "";
		LootMission_3 = "";
        ResID = "";
	}
};
