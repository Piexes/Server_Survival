function servercmdTitles(%client, %title, %buy)
{
	if(%client.t[%title] > 0)
	{
		%client.chatMessage("\c3You have equipt the" SPC %title SPC "title.");
		if(%title $= "Holy")
			%client.title = "\c5The Holy";
		else if(%title $= "Warmongler")
			%client.title = "\c0The Warmongler";
		else if(%title $= "Farmer")
			%client.title = "\c2The Farmer";
		%client.ClanPrefix = %client.title;
	}
	else if(%title $= "Warmongler" || %title $= "Holy" || %title $= "Farmer")
	{
		if(%buy $= "")
		{
			if(%client.BPmanablock >= 2 && %title $= "Holy")
				%client.chatMessage("\c3You do not own this title, but you can afford it! Do '/Titles Holy Buy' to get this title and spend 2 Mana Blocks.");
			else if(%client.BPmetal >= 10 && %title $= "Warmongler")
				%client.chatMessage("\c3You do not own this title, but you can afford it! Do '/Titles Warmongler Buy' to get this title and spend 10 Metal.");
			else if(%client.BPwheat >= 10 && %title $= "Farmer")
				%client.chatMessage("\c3You do not own this title, but you can afford it! Do '/Titles Farmer Buy' to get this title and spend 10 Wheat.");
		}
		else if(%buy == 1 || %buy $= "Yes" || %buy $= "Buy")
		{
			if(%client.BPmanablock >= 2 && %title $= "Holy")
			{
				%client.chatMessage("\c3You have bought the Holy title for 2 Manablocks.");
				%client.chatMessage("\c3Do /titles Holy to equipt it.");
				%client.BPmanablock -= 2;
				%client.t[%title] = 1;
				return;
			}
			else
			{
				%client.chatMessage("\c3You don't have the right resources for this title.");
				%client.chatMessage("\c3You need 2 Manablocks.");
			}
			if(%client.BPmetal >= 10 && %title $= "Warmongler")
			{
				%client.chatMessage("\c3You have bought the Warmongler title for 10 Metal.");
				%client.chatMessage("\c3Do /titles Warmongler to equipt it.");
				%client.t[%title] = 1;
				%client.BPMetal -= 10;
			}
			else
			{
				%client.chatMessage("\c3You don't have the right resources for this title.");
				%client.chatMessage("\c3You need 10 Metal.");
			}
			if(%client.BPwheat >= 10 && %title $= "Farmer")
			{
				%client.chatMessage("\c3You have bought the Farmer title for 10 Wheat.");
				%client.chatMessage("\c3Do /titles Farmer to equipt it.");
				%client.t[%title] = 1;
				%client.BPwheat -= 10;
			}
			else
			{
				%client.chatMessage("\c3You don't have the right resources for this title.");
				%client.chatMessage("\c3You need 10 Wheat.");
			}
		}
		else
		{
			%client.chatMessage("\c3You have to do /titles titleName buy, not /titles titleName" SPC %buy @ "...");
		}
	}
	else
	{
		%client.chatMessage("\c3That title doesn't exist!");
	}
}
package SurvivalTitles
{
	function gameConnection::spawnPlayer(%client) //I'm setting it to spawnPlayer for safety. IDK if %title will be loaded by the time onEnterGame triggers.
	{
		%client.clanPrefix = %client.title;
		parent::spawnPlayer(%client);
	}
};
activatePackage(SurvivalTitles);