function servercmdSeed(%client)
{
	%e = %client.player.getEyePoint();
	%ray = containerRaycast(%e, vectorAdd(vectorScale(%client.player.getEyeVector(),5), %e), $Typemasks::FxBrickObjectType);
	%brick = getWord(%ray, 0);
	if(!%brick)
	{
		%client.chatMessage("\c3You're not aiming at a brick.");
		return;
	}
	if(%brick.getDatablock().getName() $= "WheatBrickData")
	{
		if(%brick.isSeeded || %brick.isGrown)
		{
			%client.chatMessage("\c3That crop brick is already seeded!");
			return;
		}
		if(%client.BPSeeds < 1)
		{
			%client.chatMessage("\c3You need to craft seeds in order to plant on this brick!");
			return;
		}
		%client.BPSeeds--;
		%brick.isSeeded = 1;
		if(%client.clothing $= "Farmer" && %client.statBoost)
			%brick.growth = schedule(150000, 0, completeGrowth, %brick);
		else if(%client.clothing $= "Farmer")
			%brick.growth = schedule(300000, 0, completeGrowth, %brick);
		else
			%brick.growth = schedule(600000, 0, completeGrowth, %brick);
		%client.chatMessage("\c3You've successfully seeded that brick! When it's grown, it's color will change. You can then pick up the brick to harvest it.");
	}
	else
	{
		%client.chatMessage("\c3That brick isn't a crop brick. (wheat)");
	}
}

function completeGrowth(%brick)
{
	if(!isObject(%brick))
		return;
	%brick.setColor(56);
	%brick.isGrown = 1;
}

function servercmdeatGrain(%client, %n)
{
	if(%n $= "")
	{
		if(%client.bpgrain $= "" || %client.bpgrain == 0)
		{
			%client.chatMessage("\c3You don't have any grains to eat!");
			return;
		}
		%client.chatMessage("\c3You ate" SPC %client.BPGrain SPC "grain.");
		%client.food += %client.BPGrain * 0.5; // 1 grain = 0.5 food
		%client.BPGrain = 0;
		return;
	}
	if(%n < 1)
	{
		if(%client.BPGrain < %n)
		{
			%client.chatMessage("\c3You don't have that much grain, you only have" SPC %client.BPGrain @ ".");
			return;
		}
		%client.BPGrain -= %n;
		%client.food += %n * 0.5;
		%client.chatMessage("\c3You ate" SPC %n SPC "grain.");
	}
}