//sniff... it's beautiful
function gameConnection::has(%client, %i1, %a1, %i2, %a2, %i3, %a3, %i4, %a4)
{
	if(%a1 $= "" && %a2 $= "") //They don't have two things. Minimum two.
		return;

	if(%client.BP[%i1] >= %a1)
	{
		if(%client.BP[%i2] >= %a2)
		{
			if(%client.BP[%i3] >= %a3 && %a3 !$= "")
			{
				if(%client.BP[%i4] >= %a4 && %a4 !$= "")
				{
					if(%client.BP[%i5] >= %a5 && %a5 !$= "")
					{
						return 1;
					}
					else if(%a5 $= "")
						return 1;
					else
						return 0;
				}
				else if(%a4 $= "")
					return 1;
				else
					return 0;
			}
			else if(%a3 $= "")
				return 1;
			else
				return 0;
		}
		else
			return 0;
	}
	else
		return 0;
}

function gameConnection::BPRemove(%client, %i1, %a1, %i2, %a2, %i3, %a3, %i4, %a4)
{
	%client.BP[%i1] -= %a1;
	%client.BP[%i2] -= %a2;
	%client.BP[%i3] -= %a3;
	%client.BP[%i4] -= %a4;
}

function gameConnection::BPAdd(%client, %i1, %a1, %i2, %a2, %i3, %a3, %i4, %a4)
{
	%client.BP[%i1] += %a1;
	%client.BP[%i2] += %a2;
	%client.BP[%i3] += %a3;
	%client.BP[%i4] += %a4;
}

function servercmdCraft(%client, %item)
{
	if(%item $= "")
	{
		%client.chatMessage("\c3This menu allows you to craft items off of things in your backpack.");
		%client.chatMessage("\c3The syntax is /craft itemName. Set itemName to ALL to get a list of items.");
		return;
	}
	if(%item $= "ALL")
	{
		%client.chatMessage("\c3Item List:");
		%client.chatMessage("\c3Sword: \c6A useful short-range weapon.");
		%client.chatMessage("\c3Bow: \c6A powerful long-range weapon.");
		%client.chatMessage("\c3Helmet: \c6A must-have armor peice that protects your head.");
		%client.chatMessage("\c3Chestplate: \c6A necesary armor peice that protects your chest, and blocks the most damage.");
		%client.chatMessage("\c3Boots: \c6An easy to craft armor peice that doesn't require as many materials.");
		%client.chatMessage("\c3Cotton: \c6A material that is very useful for crafting clothing.");
		%client.chatMessage("\c3Farmer's Rags: \c6If you're going to be a dedicated farmer, you need these overalls. They allow you to grow crops twice as fast.");
		%client.chatMessage("\c3Wizard's Robes: \c6An extremely useful clothing peice for mages all around, that allows you to reduce the amount of mana you need to spend.");
		%client.chatMessage("\c3Manablock: \c6This brick allows you to stockpile mana, and is necessary in some crafting recipes.");
		%client.chatMessage("\c3Seeds: \c6For any farmer, in order to grow crops you need seeds.");
		return;
	}
	if(%item $= "Sword")
	{
		if(%client.has(Metal, 2, Wood, 1))
		{
			%client.chatMessage("\c3Your item has been successfully crafted.");
			%client.BPremove(Metal, 2, Wood, 1);
			%client.player.addNewItem("Sword");
		}
		else
		{
			%client.chatMessage("\c3You don't have the right materials! This item requires two metal and one wood.");
		}
		%client.refreshHUD();
		return;
	}
	if(%item $= "Bow")
	{
		if(%client.has(String, 1, Wood, 3))
		{
			%client.chatMessage("\c3Your item has been successfully crafted.");
			%client.BPremove(String, 1, Wood, 3);
			%client.player.addNewItem("Bow");
		}
		else
		{
			%client.chatMessage("\c3You don't have the right materials! This item requires one string and three wood.");
		}
		%client.refreshHUD();
		return;
	}
	if(%item $= "Helmet")
	{
		if(%client.BPMetal >= 4)
		{
			%client.chatMessage("\c3Your item has been successfully crafted. To equipt it, do /equiptArmor.");
			%client.BPremove(Metal, 4);
			%client.ahelmet = 1;
		}
		else
		{
			%client.chatMessage("\c3You don't have the right materials! This item requires four metal.");
		}
		%client.refreshHUD();
		return;
	}
	if(%item $= "Chestplate")
	{
		if(%client.BPMetal >= 6)
		{
			%client.chatMessage("\c3Your item has been successfully crafted. To equipt it, do /equiptArmor.");
			%client.BPremove(Metal, 6);
			%client.achestplate = 1;
		}
		else
		{
			%client.chatMessage("\c3You don't have the right materials! This item requires six metal.");
		}
		%client.refreshHUD();
		return;
	}
	if(%item $= "Boots")
	{
		if(%client.BPMetal >= 2)
		{
			%client.chatMessage("\c3Your item has been successfully crafted. To equipt it, do /equiptArmor.");
			%client.BPremove(Metal, 2);
			%client.aboots = 1;
		}
		else
		{
			%client.chatMessage("\c3You don't have the right materials! This item requires two metal.");
		}
		%client.refreshHUD();
		return;
	}
	if(%item $= "Cotton")
	{
		if(%client.has(String, 1))
		{
			%client.chatMessage("\c3Your item has been successfully crafted.");
			%client.BPremove(String, 1);
			%client.BPAdd(Cotton, 3);
		}
		else
		{
			%client.chatMessage("\c3You don't have the right materials! This item requires one string.");
		}
		%client.refreshHUD();
		return;
	}
	if(%item $= "Metal")
	{
		if(%client.has(Rock, 6))
		{
			%client.chatMessage("\c3Your item has been successfully crafted.");
			%client.BPremove(Rock, 1);
			%client.BPAdd(Metal, 2);
			%d = getRandom(1,10);
			if(%d == 10 || %d == 5)
				%client.BPAdd(%Metal, 2);
		}
		else
		{
			%client.chatMessage("\c3You don't have the right materials! This item requires six rocks. Metal can also be found naturally in the environment.");
		}
		%client.refreshHUD();
		return;
	}
	if(%item $= "Farmer" || %item $= "Farmer's" || %item $= "Farmers" || %item $= "FarmersRags" || %item $= "Farmer'sRags" || %item $= "Rags")
	{
		if(%client.CFarmer > 0)
		{
			%client.chatMessage("\c3You already have this item.");
			return;
		}
		if(%client.has(Cotton, 5, Wheat, 5))
		{
			%client.chatMessage("\c3Your item has been successfully crafted. Do /equiptClothes Farmer");
			%client.BPremove(Cotton, 5, Wheat, 5);
			%client.cFarmer++;
		}
		else
		{
			%client.chatMessage("\c3You don't have the right materials! This item requires five cotton and five wheat.");
		}
		%client.refreshHUD();
		return;
	}
	if(%item $= "Wizard" || %item $= "Wizard's" || %item $= "Wizards" || %item $= "WizardsRobes" || %item $= "Wizard'sRobes" || %item $= "Robes")
	{
		if(%client.CMage > 0)
		{
			%client.chatMessage("\c3You already have this item.");
			return;
		}
		if(%client.has(Cotton, 5, Manablock, 1))
		{
			%client.chatMessage("\c3Your item has been successfully crafted. Do /equiptClothes Wizard");
			%client.cMage++;
			%client.BPremove(Cotton, 5, Manablock, 1);
		}
		else
		{
			%client.chatMessage("\c3You don't have the right materials! This item requires five cotton and one manablock.");
		}
		%client.refreshHUD();
		return;
	}
	if(%item $= "Manablock")
	{
		if(%client.mana == 100)
		{
			%client.chatMessage("\c3A manablock has successfully been made.");
			%client.mana = "";
			%client.BPManaBlock++;
		}
		else
		{
			%client.chatMessage("\c3You need 100 mana to craft a manablock. You only have" SPC %client.mana SPC "mana.");
		}
		%client.refreshHUD();
		return;
	}
	if(%item $= "Seeds" || %item $= "Grain")
	{
		if(%client.has(Rock, 2))
		{
			%client.chatMessage("\c3Your item has been successfully crafted.");
			%client.BPremove(Rock, 2);
			%client.BPAdd(Seeds, 2, Grain, 1);
		}
		else
		{
			%client.chatMessage("\c3You don't have the right materials! This item requires two rocks.");
		}
		%client.refreshHUD();
		return;
	}
	if(%item $= "Wheat")
	{
		if(%client.has(Grain, 2))
		{
			%client.chatMessage("\c3Your item has been successfully crafted.");
			%client.BPremove(Grain, 2);
			%client.BPAdd(Wheat, 1);
		}
		else
		{
			%client.chatMessage("\c3You don't have the right materials! This item requires two grain.");
		}
		%client.refreshHUD();
		return;
	}
	%client.chatMessage("\c3That isn't a valid item.");
}

datablock ItemData(MetalItem)
{
	category = "Weapon";
	className = "Weapon";
	
	shapeFile = "base/data/shapes/brickWeapon.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;
	
	doColorShift = true;
	colorShiftColor = "0.8 0.8 0.8 1";
	image = MetalImage;
	candrop = true;
	canPickup = false;
};

datablock ShapeBaseImageData(MetalImage)
{
	shapeFile = "base/data/shapes/brickWeapon.dts";
	emap = true;
	
	doColorShift = true;
	colorShiftColor = metalItem.colorShiftColor;
	canPickup = false;
};
//Wood
datablock ItemData(WoodItem : MetalItem)
{
	colorShiftColor = "0.7 0.5 0 1";
	image = WoodImage;
};
datablock ShapeBaseImageData(WoodImage : MetalImage)
{
	colorShiftColor = woodItem.colorShiftColor;
};

//Cotton
datablock ItemData(CottonItem : MetalItem)
{
	doColorShift = false;
	colorShiftColor = "0.5 0.5 0 1";
	image = CottonImage;
};
datablock ShapeBaseImageData(CottonImage : MetalImage)
{
	doColorShift = false;
	colorShiftColor = CottonItem.colorShiftColor;
};

//Manablock
datablock ItemData(ManaItem : MetalItem)
{
	colorShiftColor = "0.5 0 0.9 1";
	image = ManaImage;
};
datablock ShapeBaseImageData(ManaImage : MetalImage)
{
	colorShiftColor = ManaItem.colorShiftColor;
};

//Wheat
datablock ItemData(WheatItem : MetalItem)
{
	colorShiftColor = "0.7 0.4 0.3 1";
	image = WheatImage;
};
datablock ShapeBaseImageData(WheatImage : MetalImage)
{
	colorShiftColor = WheatItem.colorShiftColor;
};

//Grain
datablock ItemData(GrainItem : MetalItem)
{
	colorShiftColor = "0.7 0.4 0.3 1";
	image = GrainImage;
};
datablock ShapeBaseImageData(GrainImage : MetalImage)
{
	colorShiftColor = GrainItem.colorShiftColor;
};

//Rock
datablock ItemData(RockItem : MetalItem)
{
	colorShiftColor = "0.3 0.3 0.3 1";
	image = RockImage;
};
datablock ShapeBaseImageData(RockImage : MetalImage)
{
	colorShiftColor = RockItem.colorShiftColor;
};

//String
datablock ItemData(StringItem : CottonItem)
{
	image = StringImage;
};
datablock ShapeBaseImageData(StringImage : CottonImage)
{
	colorShiftColor = "0 0 0 1";
};
//Seeds
datablock ItemData(SeedsItem : MetalItem)
{
	colorShiftColor = "0.1 0.9 0.5 1";
	image = SeedsImage;
};
datablock ShapeBaseImageData(SeedsImage : MetalImage)
{
	colorShiftColor = "0.1 0.9 0.1 1";
};

function dropItems(%client)
{
	%loc = %client.player.getHackPosition();
	%vec = %client.player.getVelocity();
	if(%client.BPMetal > 0)
	{
		%metal = new Item()
		{
			datablock = MetalItem;
			canPickup = false;
			value = %client.BPMetal;
			position = %loc;
		};
		%metal.schedule(10000, delete);
		%v = %vec;
		%v = vectorAdd(%v,getRandom(-8,8) SPC getRandom(-8,8) SPC 10);
		%metal.setVelocity(%v);
	}
	if(%client.BPWood > 0)
	{
		%wood = new Item()
		{
			datablock = WoodItem;
			canPickup = false;
			value = %client.BPWood;
			position = %loc;
		};
		%Wood.schedule(10000, delete);
		%v = %vec;
		%v = vectorAdd(%v,getRandom(-8,8) SPC getRandom(-8,8) SPC 10);
		%wood.setVelocity(%v);
	}
	if(%client.BPWheat > 0)
	{
		%Wheat = new Item()
		{
			datablock = WheatItem;
			canPickup = false;
			value = %client.BPWheat;
			position = %loc;
		};
		%Wheat.schedule(10000, delete);
		%v = %vec;
		%v = vectorAdd(%v,getRandom(-8,8) SPC getRandom(-8,8) SPC 10);
		%Wheat.setVelocity(%v);
	}
	if(%client.BPString > 0)
	{
		%String = new Item()
		{
			datablock = StringItem;
			canPickup = false;
			value = %client.BPString;
			position = %loc;
		};
		%String.schedule(10000, delete);
		%v = %vec;
		%v = vectorAdd(%v,getRandom(-8,8) SPC getRandom(-8,8) SPC 10);
		%String.setVelocity(%v);
	}
	if(%client.BPManablock > 0)
	{
		%Mana = new Item()
		{
			datablock = ManaItem;
			canPickup = false;
			value = %client.BPMana;
			position = %loc;
		};
		%Mana.schedule(10000, delete);
		%v = %vec;
		%v = vectorAdd(%v,getRandom(-8,8) SPC getRandom(-8,8) SPC 10);
		%Mana.setVelocity(%v);
	}
	if(%client.BPRock > 0)
	{
		%Rock = new Item()
		{
			datablock = RockItem;
			canPickup = false;
			value = %client.BPRock;
			position = %loc;
		};
		%Rock.schedule(10000, delete);
		%v = %vec;
		%v = vectorAdd(%v,getRandom(-8,8) SPC getRandom(-8,8) SPC 10);
		%Rock.setVelocity(%v);
	}
	if(%client.BPCotton > 0)
	{
		%Cotton = new Item()
		{
			datablock = CottonItem;
			canPickup = false;
			value = %client.BPCotton;
			position = %loc;
		};
		%Cotton.schedule(10000, delete);
		%v = %vec;
		%v = vectorAdd(%v,getRandom(-8,8) SPC getRandom(-8,8) SPC 10);
		%Cotton.setVelocity(%v);
	}
	if(%client.BPGrain > 0)
	{
		%Grain = new Item()
		{
			datablock = GrainItem;
			canPickup = false;
			value = %client.BPGrain;
			position = %loc;
		};
		%Grain.schedule(10000, delete);
		%v = %vec;
		%v = vectorAdd(%v,getRandom(-8,8) SPC getRandom(-8,8) SPC 10);
		%Grain.setVelocity(%v);
	}
	if(%client.BPSeeds > 0)
	{
		%Seeds = new Item()
		{
			datablock = SeedsItem;
			canPickup = false;
			value = %client.BPSeeds;
			position = %loc;
		};
		%Seeds.schedule(10000, delete);
		%v = %vec;
		%v = vectorAdd(%v,getRandom(-8,8) SPC getRandom(-8,8) SPC 10);
		%Seeds.setVelocity(%v);
	}
}

package ResourcePickup
{
	//Credit to iBan, lol. Modified a tad by me. By a tad, I mean how hard I fucked your mom last night.
	function Armor::onCollision(%this, %obj, %col, %thing, %other)
	{
		%name = %col.getDatablock().getName();
		%client = %obj.client;
		if(%name $= "MetalItem" || %name $= "WoodItem" || %name $= "CottonItem" || %name $= "ManablockItem" || %name $= "StringItem" || %name $= "WheatItem" || %name $= "RockItem" || %name $= "GrainItem" || %name $= "SeedsItem" || %name $= "MeatItem")
		{
			if(isObject(%obj.client))
			{
				if(isObject(%col))
				{
					if(%obj.client.minigame)
						%col.minigame = %obj.client.minigame;
					%value = %col.value;
					%type = strReplace(%name, "Item", "");
					if(%name $= "MeatItem")
						%client.food += %value;
					else
						%client.BP[%type] += %value;
					%client.chatMessage("\c3You picked up" SPC %value SPC %type @ ".");
					%client.refreshHUD();
					%col.canPickup = false;
					%col.delete();
				}
				else
				{
					%col.delete();
					MissionCleanup.remove(%col);
				}
			}
		}

		if(isObject(%col))
			parent::onCollision(%this, %obj, %col, %thing, %other);
	}
	function gameConnection::onDeath(%client, %killerPlayer, %killer, %damageType, %damageLoc)
	{
		dropItems(%client);
		%client.BPCotton = 0;
		%client.BPWheat = 0;
		%client.BPRock = 0;
		%client.BPWood = 0;
		%client.BPMetal = 0;
		%client.BPManablock = 0;
		%client.BPString = 0;
		%client.BPGrain = 0;
		%client.BPSeeds = 0;
		%client.refreshHud();
		%client.isDead = 1;
		parent::onDeath(%client, %killerPlayer, %killer, %damageType, %damageLoc);
	}
};
activatePackage(ResourcePickup);

function Player::addNewItem(%player,%item)
{
	%client = %player.client;
	if(isObject(%item))
	{
		if(%item.getClassName() !$= "ItemData") return false;
		%item = %item.getName();
	}
	else
		%item = findItemByName(%item);
	if(!isObject(%item)) return false;
	%item = nameToID(%item);
	for(%i = 0; %i < %player.getDatablock().maxTools; %i++)
	{
		%tool = %player.tool[%i];
		if(%tool == 0)
		{
			%player.tool[%i] = %item;
			%player.weaponCount++;
			messageClient(%client,'MsgItemPickup','',%i,%item);
			return true;
		}
	}
	return false; //We didn't find a slot :(
}
function findItemByName(%item,%val)
{
	if(isObject(%item)) return %item.getName();
	if($lastDatablockCount != DatablockGroup.getCount() || %val) //We don't need to cause lag everytime we try to find an item
	{
		//talk("findItemByName - Resetting cached tables");
		$itemTableLookUp_Count = 0;
		for(%i=0;%i<DatablockGroup.getCount();%i++)
		{
			%obj = DatablockGroup.getObject(%i);
			if(%obj.getClassName() $= "ItemData" && strLen(%obj.uiName) > 0)
			{
				$itemTableLookup[$itemTableLookUp_Count] = %obj;
				$itemTableLookUp_Count++;
			}
		}
		//talk("findItemByName - Cached tables set to " @ $itemTableLookUp_Count);
	}
	for(%a=0;%a<$itemTableLookUp_Count;%a++)
	{
		%objA = $itemTableLookup[%a];
		if(%objA.getClassName() $= "ItemData")
			if(strPos(%objA.uiName,%item) >= 0)
			{
				//talk("Found item, position detection: " @ strPos(%objA.uiName,%item));
				return %objA.getName();
			}
	}
	$lastDatablockCount = DatablockGroup.getCount();
	return -1;
}
forceRequiredAddOn("Script_Player_Persistence");
registerPersistenceVar("BP", true, "");
registerPersistenceVar("clothing", false, "");
registerPersistenceVar("food", false, "");
registerPersistenceVar("mana", false, "");
registerPersistenceVar("chestplate", false, "");
registerPersistenceVar("helmet", false, "");
registerPersistenceVar("boots", false, "");
registerPersistenceVar("cFarmer", false, "");
registerPersistenceVar("cMage", false, "");
registerPersistenceVar("mProtection", false, "");
registerPersistenceVar("mSpeed", false, "");
registerPersistenceVar("mSurplus", false, "");
registerPersistenceVar("loveHotkey", false, "");
registerPersistenceVar("hateHotkey", false, "");
registerPersistenceVar("confusionHotkey", false, "");
registerPersistenceVar("mTeleport", false, "");
registerPersistenceVar("title", false, "");
registerPersistenceVar("tWarmongler", false, "");
registerPersistenceVar("tHoly", false, "");
registerPersistenceVar("tFarmer", false, "");
registerPersistenceVar("aHelmet", false, "");
registerPersistenceVar("aChestplate", false, "");
registerPersistenceVar("aBoots", false, "");
registerPersistenceVar("donator", false, "");