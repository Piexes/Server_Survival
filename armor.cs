//Rewrite this with model armors.
function servercmdEquiptArmor(%client, %a)
{
	if(%a $= "")
	{
		%client.chatMessage("\c3This command allows you to equipt armor that you have crafted.");
		%client.chatMessage("\c3The syntax for this command is /equiptArmor armorType.");
		%client.chatMessage("\c3The different armor types are the helmet, the chestplate, and boots.");
		return;
	}
	if(%client.A[%a] >= 1)
	{
		%client.chatMessage("\c3Armor successfully equipt!");
		if(%a $= "Helmet")
		{
			%client.helmet = 3000;
			%client.player.unhideNode("Helmet");
			%client.player.hideNode("copHat");
			%client.player.hideNode("knitHat");
			%client.player.hideNode("scoutHat");
			%client.player.hideNode("bicorn");
			%client.player.hideNode("pointyHelmet");
			%client.player.hideNode("flareHelmet");
			%client.player.hideNode("plume");
			%client.player.hideNode("triplume");
			%client.player.hideNode("septplume");
			%client.player.hideNode("visor");
			%client.player.setNodeColor("Helmet", "71 71 71 1");
			%client.clothing = "";
			%client.player.unmountImage(1);
		}
		else if(%a $= "Chestplate")
		{
			%client.chestplate = 4000;
			%client.player.unhideNode("Armor");
			%client.player.hideNode("Pack");
			%client.player.hideNode("Quiver");
			%client.player.hideNode("Tank");
			%client.player.hideNode("Bucket");
			%client.player.hideNode("Cape");
			%client.player.setNodeColor("Armor", "71 71 71 1");
			%client.clothing = "";
			%client.player.unmountImage(1);
		}
		else if(%a $= "Boots")
		{
			%client.boots = 2000;
			%client.player.hideNode("RPeg");
			%client.player.hideNode("LPeg");
			%client.player.unhideNode("RShoe");
			%client.player.unhideNode("LShoe");
			%client.player.setNodeColor("LShoe", "113 111 111 1");
			%client.player.setNodeColor("RShoe", "113 111 111 1");
		}
		%client.play2D(armorEquiptSound);
	}
	else
	{
		%client.chatMessage("\c3That armortype either doesn't exist, or you don't have it on you!");
		%client.chatMessage("\c3The different armor types are the helmet, the chestplate, and boots.");
	}
}
function servercmdEquiptClothes(%client, %c)
{
	if(%c $= "")
	{
		%client.chatMessage("\c3This command allows you to equipt clothes that you have crafted.");
		%client.chatMessage("\c3The syntax for this command is /equiptClothes clothing name.");
		%client.chatMessage("\c3The different clothing choices are Farmer's Rags and Mage's Robes.");
		return;
	}
	if(%c $= "Farmer's" || %c $= "Farmers" || %c $= "Overalls")
		%c = "Farmer";
	else if(%c $= "Mage's" || %c $= "Mages" || %c $= "Wizard" || %c $= "Wizards" || %c $= "Robes")
		%c = "Mage";

	if(%client.c[%c] >= 1)
	{
		%client.chatMessage("\c3Clothing equipt.");
		if(%c $= "Farmer")
			%client.clothing = "Farmer's Overalls";
		else if(%c $= "Mage")
			%client.clothing = "Wizard's Robes";
		if(%c $= "Mage" && %client.skins != 1)
			%client.equiptMage();
		else if(%c $= "Farmer" && %client.skins != 1)
			%client.equiptFarmer();
		if(%c $= "Mage" && %client.skins == 1)
			%client.equiptMageDonor();
		//else if(%c $= "Farmer" && %client.skins != 1)
		//	%client.equiptFarmerDonor();

		//Unequipt armor
		if(%client.helmet || %client.chestplate || %client.boots)
			%client.chatMessage("\c3Your armor has been unequipt.");
		%client.helmet = "";
		%client.chestplate = "";
		%client.boots = "";
		%client.player.hideNode("Armor");
		%client.player.hideNode("helmet");
		%client.player.hideNode("pack");
		%client.player.hideNode("quiver");
		%client.player.hideNode("tank");
		%client.player.hideNode("cape");
		%client.player.hideNode("bucket");
		%client.player.hideNode("EpauletsRankA");
		%client.player.hideNode("EpauletsRankB");
		%client.player.hideNode("EpauletsRankC");
		%client.player.hideNode("EpauletsRankD");
		%client.player.hideNode("shoulderPads");
		%client.player.hideNode("epaulets");
		%client.player.setNodeColor("LShoe", "0.2 0.1 0 1");
		%client.player.setNodeColor("RShoe", "0.2 0.1 0 1");
	}
	else
	{
		%client.chatMessage("\c3You don't have that type of clothing!");
	}
	%client.refreshHUD();
}
package Armors
{
	function gameConnection::spawnPlayer(%client)
	{
		parent::spawnPlayer(%client);
		if(!$spawn)
			schedule(100, 0, setSpawnVar, %client.player);
		%client.canChangeAvatar = 1;
		%client.applyBodyParts();
		%client.applyBodyColors();
		%client.canChangeAvatar = "";
		if(%client.clothing $= "Wizard's Robes")
		{
			%client.player.hideNode("Pack");
			%client.player.hideNode("Quiver");
			%client.player.hideNode("Tank");
			%client.player.hideNode("Cape");
			%client.player.hideNode("Bucket");
			%client.player.hideNode("EpauletsRankA");
			%client.player.hideNode("EpauletsRankB");
			%client.player.hideNode("EpauletsRankC");
			%client.player.hideNode("EpauletsRankD");
			%client.player.hideNode("shoulderPads");
			%client.player.hideNode("epaulets");
			if(%client.skins)
				%client.equiptmageDonor();
			else
				%client.equiptMage();
		}
		else if(%client.clothing $= "Farmer's Overalls")
		{
			%client.player.hideNode("pack");
			%client.player.hideNode("quiver");
			%client.player.hideNode("tank");
			%client.player.hideNode("cape");
			%client.player.hideNode("bucket");
			%client.player.hideNode("EpauletsRankA");
			%client.player.hideNode("EpauletsRankB");
			%client.player.hideNode("EpauletsRankC");
			%client.player.hideNode("EpauletsRankD");
			%client.player.hideNode("shoulderPads");
			%client.player.hideNode("epaulets");
			//if(%client.skins)
			//	%client.equiptFarmerDonor();
			//else
				%client.equiptFarmer();
		}
		%client.player.setNodeColor("LShoe", "0.2 0.1 0 1");
		%client.player.setNodeColor("RShoe", "0.2 0.1 0 1");
		%client.player.hideNode("Armor");
		%client.player.hideNode("Helmet");
		if(%client.chestplate)
		{
			%client.player.unhidenode("Armor");
			%client.player.setNodeColor("Armor", "71 71 71 1");
		}
		if(%client.helmet)
		{
			%client.player.unhideNode("Helmet");
			%client.player.setNodeColor("Helmet", "71 71 71 1");
		}
		if(%client.boots)
		{
			%client.player.setNodeColor("RShoe", "71 71 71 1");
			%client.player.setNodeColor("LShoe", "71 71 71 1");
		}
		if(%client.abilityCD)
			%client.abilityCD = "";
		%client.schedule(66, refreshHUD);
	}
	function armor::Damage(%this, %obj, %sourceObject, %position, %dmg, %damageType)
	{
		%client = %obj.client;
		%player = %obj;
		%player.lastDamager = %sourceObject.client;
		if(%client.chestplate >= 1 && %client.isSuiciding == 0)
		{
			%dmg *= 0.7;
			%client.chestplate -= %dmg * 0.3;
			if(%client.chestplate < 0)
			{
				%client.chatMessage("\c3Your metal chestplate has broken!");
				%client.chestplate = "";
			}
		}
		if(%client.helmet >= 1 && %client.isSuiciding == 0)
		{
			%dmg *= 0.8;
			%client.helmet -= %dmg * 0.2;
			if(%client.helmet < 0)
			{
				%client.chatMessage("\c3Your metal helmet has broken!");
				%client.helmet = "";
			}
		}
		if(%client.boots >= 1 && %client.isSuiciding == 0)
		{
			%dmg *= 0.9;
			%client.boots -= %dmg * 0.1;
			if(%client.boots < 0)
			{
				%client.chatMessage("\c3Your metal boots have broken!");
				%client.boots = "";
			}
		}
		if(%client.hassDamageProtection)
		{
			%dmg *= 0.7;
		}
		if(isObject(%sourceObject.client) && %client.canPvP)
		{
			%dmg = 0;
		}
		parent::Damage(%this, %obj, %sourceObject, %position, %dmg, %damageType);
		%data = %obj.getDatablock();
		if(%player.getDamageLevel() >= %data.maxDamage && %player.getClassName() $= "AiPlayer" && isObject(%player.lastDamager))
		{
			animalDrop(%player);
			%player.lastDamager.refreshHUD();
		}
		if(%client.clothing $= "Wizard's Robes" && %client.skins != 1)
			%client.equiptMage();
		else if(%client.clothing $= "Farmer's Overalls" && %client.skins != 1)
			%client.equiptFarmer();
		else if(%client.clothing $= "Wizard's Robes" && %client.skins == 1)
			%client.equiptMageDonor();
		//else if(%client.clothing $= "Farmer's Overalls" && %client.skins == 1)
		//	%client.equiptFarmerDonor();
	}
	function gameConnection::applyBodyParts(%c){ if(%c.canChangeAvatar == 1){ parent::applyBodyParts(%c);}}
	function gameConnection::applyBodyColors(%c){ if(%c.canChangeAvatar == 1){ parent::applyBodyColors(%c);}}
};
activatePackage(Armors);

function gameConnection::equiptMage(%client)
{
	if(!isObject(%client.player))
		return false;
	%client.player.mountImage(MageRobesImage, 1);
}
function gameConnection::equiptMageDonor(%client)
{
	if(!isObject(%client.player))
		return false;
	%client.player.mountImage(MageRobesDonorImage, 1);
}
datablock shapeBaseImageData(MageRobesImage)
{
	shapeFile = "./models/mageRobes.dts";
	mountPoint = $backSlot;
	emap = false;
	rotation = eulerToMatrix("0 0 0");
 	offset = "-0.06 0.68 -0.5";
 	eyeOffset = "0 0 50";
 	scale = "1 1 1";
  	doColorShift = false;
  	colorShiftColor = "50.000 50.000 50.000 255.000";
};
datablock shapeBaseImageData(MageRobesDonorImage)
{
	shapeFile = "./models/mageRobesDonor.dts";
	mountPoint = $backSlot;
	emap = false;
	rotation = eulerToMatrix("0 0 0");
 	offset = "0 0 -0.65";
 	eyeOffset = "0 0 50";
 	scale = "1 1 1";
  	doColorShift = false;
  	colorShiftColor = "50.000 50.000 50.000 255.000";
};

function gameConnection::equiptFarmer(%client)
{
	if(!isObject(%client.player))
		return false;
	%client.player.mountImage(OverallsImage, 1);
}
function gameConnection::equiptFarmerDonor(%client)
{
	if(!isObject(%client.player))
		return false;
	%client.player.mountImage(OverallsDonorImage, 1);
}
datablock shapeBaseImageData(OverallsDonorImage) //Update the datablock data when you add it
{
	shapeFile = "./models/preoveralls.dts";
	mountPoint = $backSlot;
	emap = false;
	rotation = eulerToMatrix("0 0 0");
 	offset = "0.02 0.07 -0.43";
 	eyeOffset = "0 0 50";
 	firstPerson = false;
 	scale = "1 1 1";
  	doColorShift = false;
  	colorShiftColor = "50.000 50.000 50.000 255.000";
};
datablock shapeBaseImageData(OverallsImage)
{
	shapeFile = "./models/overalls.dts";
	mountPoint = $backSlot;
	emap = false;
	rotation = eulerToMatrix("0 0 0");
 	offset = "0 0 -0.65";
 	eyeOffset = "0 0 50";
 	firstPerson = false;
 	scale = "1 1 1";
  	doColorShift = false;
  	colorShiftColor = "50.000 50.000 50.000 255.000";
};

function animalDrop(%player)
{
	%d = getRandom(1,10);
	%loc = %player.getPosition();
	%vec = %player.getVelocity();
	if(%d == 10 || %d == 1)
	{
		%Cotton = new Item()
		{
			datablock = CottonItem;
			canPickup = false;
			value = 2;
			position = %loc;
		};
		%Cotton.schedule(10000, delete);
		%v = %vec;
		%v = vectorAdd(%v,getRandom(-8,8) SPC getRandom(-8,8) SPC 10);
		%Cotton.setVelocity(%v);
	}
	if(%player.getdatablock.getName() !$= "SkeletonArcherHoleBot" || %player.getdatablock().getName() !$= "SkeletonArmoredHoleBot" || %player.getdatablock().getName() !$= "SkeletonArmoredHoleBot")
	{
		%Meat = new Item()
		{
			datablock = MeatItem;
			canPickup = false;
			value = %d;
			position = %loc;
		};
		%Meat.schedule(10000, delete);
		%v = %vec;
		%v = vectorAdd(%v,getRandom(-8,8) SPC getRandom(-8,8) SPC 10);
		%Meat.setVelocity(%v);
	}
	if(%player.getDatablock().getName() $= "RaptorHoleBot" || %player.getdatablock().getName() $= "SkeletonArmoredHoleBot")
	{
		%metal = new Item()
		{
			datablock = metalItem;
			canPickup = false;
			value = 1;
			position = %loc;
		};
		%metal.schedule(10000, delete);
		%v = %vec;
		%v = vectorAdd(%v,getRandom(-8,8) SPC getRandom(-8,8) SPC 10);
		%metal.setVelocity(%v);
	}
	if(%player.getDatablock().getName() $= "BearHoleBot")
	{
		%cotton = new Item()
		{
			datablock = cottonItem;
			canPickup = false;
			value = 1;
			position = %loc;
		};
		%cotton.schedule(10000, delete);
		%v = %vec;
		%v = vectorAdd(%v,getRandom(-8,8) SPC getRandom(-8,8) SPC 10);
		%cotton.setVelocity(%v);
	}
	if(%player.getdatablock().getName() $= "SkeletonArcherHoleBot" || %player.getdatablock().getName() $= "SkeletonArmoredHoleBot" || %player.getdatablock().getName() $= "SkeletonArmoredHoleBot")
	{
		%seeds = new Item()
		{
			datablock = seedItem;
			canPickup = false;
			value = 1;
			position = %loc;
		};
		%seeds.schedule(10000, delete);
		%v = %vec;
		%v = vectorAdd(%v,getRandom(-8,8) SPC getRandom(-8,8) SPC 10);
		%seeds.setVelocity(%v);
	}
}
//Grain
datablock ItemData(MeatItem : MetalItem)
{
	colorShiftColor = "0.7 0 0.1 1";
	image = MeatImage;
};
datablock ShapeBaseImageData(MeatImage : MetalImage)
{
	colorShiftColor = MeatItem.colorShiftColor;
};
function setSpawnVar(%p)
{
	$spawn = %p.getPosition();
}