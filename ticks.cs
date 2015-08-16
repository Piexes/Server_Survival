function tick(%tock)
{
	%tock++;
	announce("\c5Another day has passed in the land of the ancients.");
	%c = ClientGroup.getCount();
	for(%i=0;%i<%c;%i++)
	{
		%client = ClientGroup.getObject(%i);
		%client.food -= 3;
		%client.hydration -= 2;
		if(%client.hunger <= 0)
		{
			%client.player.kill();
			%client.food = 4;
			%client.hydration = 8;
			%client.chatMessage("\c3You have starved to death.");
		}
		if(%client.hydration <= 0)
		{
			%client.player.kill();
			%client.food = 4;
			%client.hydration = 8;
			%client.chatMessage("\c3You have died of dehydration.");
		}
		if(%client.AbilityCD !$= "")
		{
			%Ability = getWord(%client.abilityCD, 1);
			%CD = getWord(%client.abilityCD, 0);
			%CD -= 1;
			if(%CD <= 0)
			{
				%client.AbilityCD = "";
				if(%ability $= "Surplus")
				{
					%client.statBoost = "";
				}
				else if(%ability $= "Speed")
				{
					if(%client.hassSpeed)
					{
						%client.hassSpeed = "";
						%client.player.resetMovementSpeed();
					}
				}
				else if(%ability $= "Protection")
				{
					%client.hassDamageProtection = "";
				}
				else if(%ability $= "Location")
				{
					%client.quest = "";
				}
				%client.abilityCD = "";
				%client.unmountImage(3);
			}
		}
		%client.mana += 15;
		if(%client.mana > 100)
			%client.mana = 100;
		%client.refreshHud();
	}
	for(%i=0;%i<TribeContainer.getCount();%i++)
	{
		%tribe = TribeContainer.geTObject(%i);
		if(%tribe.debt1)
		{
			%r = getWord(%tribe.debt1,0);
			%debt = getWord(%tribe.debt1,1);
			%enemy = getWord(%tribe.debt1,2);
			%owner = setWord(%enemy.owner,0,"");
			%owner = findclientbyname(%owner);
			%found = takeTribeItem(%r, %debt);
			if(%owner)
			{
				%owner.BP[%r] += %found;
			}
		}
		if(%tribe.debt2)
		{
			%r = getWord(%tribe.debt2,0);
			%debt = getWord(%tribe.debt2,1);
			%enemy = getWord(%tribe.debt2,2);
			%owner = setWord(%enemy.owner,0,"");
			%owner = findclientbyname(%owner);
			%found = takeTribeItem(%r, %debt);
			if(%owner)
			{
				%owner.BP[%r] += %found;
			}
		}
	}
	$tickschedule = schedule(300000, 0, tick, %tock);
}