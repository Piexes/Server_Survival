function servercmdbuyCharm(%client)
{
	if(%client.BPManablock >= 1)
	{
		%client.chatMessage("\c3The charm has been placed!");
		%client.chatMessage("\c3Do /charmCheck to see if a block is protected by a charm.");
		%client.chatMessage("\c3A mana block has been spent.");
		%client.BPManablock -= 1;
		initContainerRadiusSearch(%client.player.getHackPosition(), 15, $Typemasks::FxBrickObjectType);
		while(%brick = containerSearchNext())
		{
			if(%brick.charm !$= "" && %brick.charm != %client.bl_id)
			{
				%encounters++;
				%client.bottomPrint("You have tried to put a brick under a charm, but it is already under one by ID" SPC %brick.charm @ ". It has been ignored. (" @ %encounters @ ")", 5, 1);
				return;
			}
			if(%brick.tribe)
			{
				%client.bottomPrint("\c3You unsuccesfully attempted to settle on a brick owned by the" SPC %brick.tribe SPC "tribe.", 5, 1);
				return;
			}
			%brick.charm = %client.BL_ID;
			%brick.addEvent(true, 0, "onActivate", "Self", "saveVariable", "client" SPC %client.bl_id);
		}
	}
	else
	{
		%client.chatMessage("\c3A charm is an item that prevents the destruction of building materials within its reach.");
		%client.chatMessage("\c3It only secures blocks that existed when it was first placed. They have to be planted by you.");
		%client.chatMessage("\c3It costs one manablock.");
	}
	%client.refreshHud();
}
function servercmdcharmCheck(%client)
{
	%e = %client.player.getEyePoint();
	%m = vectorScale(%client.player.getEyeVector(), 100);
	%r = containerRaycast(%e, vectorAdd(%e, %m), $Typemasks::FxBrickObjectType);
	%brick = getWord(%r, 0);
	if(%brick)
	{
		%blid = %brick.charm;
		if(%blid $= "")
		{
			%client.chatMessage("\c3That brick doesn't have any charms on it.");
			return;
		}
		%c = ClientGroup.getCount();
		for(%i=0;%i<%c;%i++)
		{
			%cl = ClientGroup.getObject(%i);
			if(%cl.BL_ID == %blid)
			{
				%name = %cl.name;
			}
		}
		if(%name $= "")
		{
			%client.chatMessage("\c3This brick is protected by a charm by ID" SPC %blid @ ".");
		}
		else
		{
			%client.chatMessage("\c3This brick is protected by a charm by" SPC %cl.name @ ".");
		}
	}
	else
	{
		%client.chatMessage("\c3A brick hasn't been found.");
	}
}

function servercmdbuyTribeCharm(%client)
{
	if(%client.BPManablock >= 3)
	{
		%client.chatMessage("\c3The tribe charm has been placed!");
		%client.chatMessage("\c3Do /charmCheck to see if a block is protected by a charm.");
		%client.chatMessage("\c3Three mana blocks has been spent.");
		%client.chatMessage("\c3Since this is a tribe charm, anyone that is in your tribe can modify the land. It also has a higher range than normal charms.");
		%client.BPManablock -= 3;
		initContainerRadiusSearch(%client.player.getHackPosition(), 30, $Typemasks::FxBrickObjectType);
		while(%brick = containerSearchNext())
		{
			if(%brick.charm !$= "" && %brick.charm != %client.bl_id && %brick.tribe $= "")
			{
				%encounters++;
				%client.bottomPrint("You have tried to put a brick under a tribe charm, but it is already under one by ID" SPC %brick.charm @ ". It has been ignored. (" @ %encounters @ ")", 5, 1);
				return;
			}
			if(%brick.tribe)
			{
				if(!TribeContainer.findTribeByName(%brick.tribe.name))
				{
					%client.bottomPrint("\c3This land used to belong to a tribe, but they have since disbanded.", 5, 1);
					%brick.tribe = "";
					%brick.charm = "";
				}
				else if(%client.isAtWarWith(%team))
				{
					%client.chatMessage("\c3Settled on enemy territory.",5,1);
				}
				else
				{
					%client.bottomPrint("\c3This brick is covered by a charm from an opposing tribe. If you want to settle on it, go to war with them. (" @ %brick.tribe.name @ ")",5,1);
					return;
				}
			}
			%brick.tribe = %client.tribe;
			%brick.addEvent(true, 0, "onActivate", "Self", "saveVariable", "tribe" SPC %client.tribe);
		}
	}
	else
	{
		%client.chatMessage("\c3A charm is an item that prevents the destruction of building materials within its reach.");
		%client.chatMessage("\c3It only secures blocks that existed when it was first placed. They have to be planted by your tribe.");
		%client.chatMessage("\c3It costs three manablocks.");
	}
	%client.refreshHud();
}

package CharmLoading
{
	function fxDtsBrick::onLoadPlant(%brick)
	{
		parent::onLoadPlant(%brick);
		if(%brick.getDatablock().category $= "SurvivalRPG")
		{
			Brickgroup_Resources.add(%brick);
			//Getting ownership
			schedule(500, 0, correctCharm, %brick);
		}
	}
	function fxDtsBrick::onPlant(%brick)
	{
		parent::onPlant(%brick);
		if(%brick.getDatablock().category $= "SurvivalRPG")
		{
			if(%brick.getDatablock().getName() $= "WoodBrickData")
				%color = 47;
			else if(%brick.getDatablock().getName() $= "StringBrickData")
				%color = 1;
			else if(%brick.getDatablock().getName() $= "MetalBrickData")
				%color = 3;
			else if(brick.getDatablock().getName() $= "RockBrickData")
				%color = 4;
			else if(%brick.getDatablock().getName() $= "WheatBrickData")
				%color = 45;
			else if(%brick.getDatablock().getName() $= "GrainBrickData")
				%color = 57;
			else if(%brick.getDatablock().getName() $= "SeedBrickData")
				%color = 31;
			else if(%brick.getDatablock().getName() $= "ManaBrickData")
			{
				%fx = 6;
				%color = 41;
			}
			if(%color)
				%brick.setColor(%color);
			if(%fx)
				%brick.setColorFX(%fx);
			Brickgroup_Resources.add(%brick);
		}
	}
	function gameConnection::autoAdminCheck(%client)
	{
		parent::autoAdminCheck(%client);
		if(!isObject(Brickgroup_Resources))
		{
			echo("Creating Resource Brickgroup.");
			%group = new simGroup(Brickgroup_Resources)
			{
			    name = "Resoures";
			    blid = 1337;
			};
			mainBrickGroup.add(%group);
		}
	}
};
activatePackage(CharmLoading);
registerOutputEvent("fxDTSBrick", "saveVariable", "string 100 300");

function correctCharm(%brick)
{
	%ownerOrTribe = getWord(%brick.eventOutputParameter0_1, 0);
	%id = getWord(%brick.eventOutputParameter0_1, 1);
	if(%ownerOrTribe $= "Tribe")
	{
		%brick.tribe = %id;
	}
	else if(%ownerOrTribe $= "Client")
	{
		%brick.charm = %id;
	}
}