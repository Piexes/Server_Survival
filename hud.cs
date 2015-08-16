function gameConnection::RefreshHud(%client)
{
	%c = "\c5";
	if(%client.food $= "")
		%client.food = 0;
	if(%client.hydration $= "")
		%client.hydration = 0;
	if(%client.BPWood $= "")
		%client.BPWood = 0;
	if(%client.BPMetal $= "")
		%client.BPMetal = 0;
	if(%client.BPCotton $= "")
		%client.BPCotton = 0;
	if(%client.BPString $= "")
		%client.BPString = 0;
	if(%client.BPRock $= "")
		%client.BPRock = 0;
	if(%client.BPManablock $= "")
		%client.BPManablock = 0;
	if(%client.BPWheat $= "")
		%client.BPWheat = 0;
	if(%client.Mana $= "")
		%client.Mana = 100;
	if(%client.BPGrain $= "")
		%client.BPGrain = 0;
	if(%client.BPSeeds $= "")
		%client.BPSeeds = 0;
	if(%client.food > 10)
		%client.food = 10;
	if(%client.hydration > 10)
		%client.hydration = 10;
	if(%client.buildmode $= "")
		%client.buildmode = "Off";
	if(%client.Mana > 100)
		%client.mana = 100;
	if(%client.quest)
		%doesQuest = "Quest\c6:";
	else if(%client.buildmode == 1)
		%client.buildmode = "On";
	%client.bottomPrint("<just:center>" @ %c @ "Food\c6:" SPC %client.food @ "/10" SPC %c @ "Water\c6:" SPC %client.hydration SPC %c @ "Mana\c6:" SPC %client.mana SPC %c @ "Buildmode\c6:" SPC %client.buildmode SPC %c @ "Clothes\c6:" SPC %client.clothing SPC "<br>" SPC %c @ "Wood\c6:" SPC %client.BPWood SPC %c @ "Metal\c6:" SPC %client.BPMetal SPC %c @ "Rock\c6:" SPC %client.BPRock SPC %c @ "Wheat\c6:" SPC %client.BPWheat SPC %c @ "Manablock\c6:" SPC %client.BPManablock SPC %c @ "Cotton\c6:" SPC %client.BPCotton SPC %c @ "String\c6:" SPC %client.BPString SPC %c @ "Grain\c6:" SPC %client.BPGrain SPC %c @ "Seeds\c6:" SPC %client.BPSeeds SPC "<br>" @ %c @ %doesQuest SPC %client.quest, 0, 1);
	if(%client.buildmode $= "Off")
		%client.buildmode = "";
	else if(%client.buildmode $= "On")
		%client.buildmode = 1;
}
function servercmdRefreshHud(%client)
{
	%client.refreshHud();
}
function servercmdecancelQuest(%client)
{
	if(%client.quest)
	{
		%client.chatMessage("\c3Quest cancelled.");
		%client.quest = "";
		%client.refreshHud();
	}
	else
	{
		%client.chatMessage("\c3There isn't a quest to end!");
	}
}
function gameConnection::questLoop(%client, %target)
{
	if(%client.quest $= "")
		return;
	%a = %target.getTransform();
	%b = %client.player.getTransform();
	%client.quest = vectorDist(%a, %b);
	if(%client.quest <= 7)
	{
		%client.chatMessage("\c3Quest ended!");
		%client.quest = "";
		%client.abilityCD = "";
		%client.unmountImage(3);
	}
	else
	{
		%client.schedule(350, questLoop, %target);
	}
	%client.refreshHUD();
}