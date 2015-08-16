function servercmdCast(%client, %spell, %a)
{
	if(%spell $= "")
	{
		%client.chatMessage("\c3This command allows you to cast spells! To see what spells you can cast, do /knownSpells");
		%client.chatMessage("\c3The syntax is /cast spellName");
		return;
	}
	if(%client.m[%spell] $= "")
	{
		%client.chatMessage("\c3That is either an invalid spell, or you have not been taught that spell.");
		%client.chatMessage("\c3In order to learn a spell, you need to be tutored by someone else who knows it.");
		%client.chatMessage("\c3Do /knownSpells to find out what spells you know.");
		return;
	}
	if(%spell $= "Teleport")
	{
		%cost = 60;
		if(%client.statBoost && %client.clothing $= "Mage")
			%cost *= 0.5;
		if(%client.clothing $= "Mage")
			%cost *= 0.5;
		if(%client.mana < %cost)
		{
			%client.chatMessage("\c3You don't have enough mana for this spell. You need 60 mana.");
			return;
		}
		%client.mana -= %cost;
		%client.aurora();
		%client.player.schedule(4500, setTransform, $spawn);
		%client.schedule(4533, aurora);
		%client.schedule(4533, setEmitter, teleportSpellImage, 3000);
		%client.chatMessage("\c5Teleport spell activated!");
		serverPlay3D(teleportSpellSound, %client.player.getPosition());
		%client.refreshHud();
	}
	else if(%spell $= "Surplus") //ST4T BOOST
	{
		%cost = 90;
		if(%client.statBoost && %client.clothing $= "Mage")
			%cost *= 0.5;
		if(%client.clothing $= "Mage")
			%cost *= 0.5;
		if(%client.mana < %cost)
		{
			%client.chatMessage("\c3You don't have enough mana for this spell. You need 90 mana.");
			return;
		}
		if(%client.abilityCD !$= "")
		{
			%Client.chatMessage("\c3You already have one spell active. You must wait for it to expire before using this one.");
			return;
		}
		%client.mana -= %cost;
		%client.statBoost = 1;
		%client.aurora();
		%client.abilityCD = 2 SPC "Surplus";
		%client.chatMessage("\c3Surplus spell used!");
		%client.setEmitter(surplusspellEmitter);
		serverPlay3D(surplusSpellSound, %client.player.getPosition());
		%client.refreshHud();
	}
	else if(%spell $= "Speed")
	{
		%cost = 55;
		if(%client.statBoost && %client.clothing $= "Mage")
			%cost *= 0.5;
		if(%client.clothing $= "Mage")
			%cost *= 0.5;
		if(%client.mana < %cost)
		{
			%client.chatMessage("\c3You don't have enough mana for this spell. You need 90 mana.");
			return;
		}
		if(%client.abilityCD !$= "")
		{
			%Client.chatMessage("\c3You already have one spell active. You must wait for it to expire before using this one.");
			return;
		}
		%client.player.setBaseMovementSpeed(10);
		%client.player.schedule(10000, setBaseMovementSpeed, 13);
		%client.player.schedule(15000, setBaseMovementSpeed, 15);
		%client.player.schedule(20000, setBaseMovementSpeed, 20);
		%client.player.schedule(15000, setEmitter, speedSpellImage);
		%client.abilityCD = 1 SPC "Speed";
		%client.mana -= %cost;
		%client.hassSpeed = 1;
		%client.aurora();
		%client.chatMessage("\c3Speed spell used!");
		%client.play2D(speedSpellSound);
		%client.refreshHud();
	}
	else if(%spell $= "Protection")
	{
		%cost = 66;
		if(%client.statBoost && %client.clothing $= "Mage")
			%cost *= 0.5;
		if(%client.clothing $= "Mage")
			%cost *= 0.5;
		if(%client.mana < %cost)
		{
			%client.chatMessage("\c3You don't have enough mana for this spell. You need 66 mana.");
			return;
		}
		if(%client.abilityCD !$= "")
		{
			%client.chatMessage("\c3You already have one spell active. You must wait for it to expire before using this one.");
			return;
		}
		%client.hassDamageProtection = 1;
		%client.abilityCD = 1 SPC "Protection";
		%client.mana -= %cost;
		%client.aurora();
		%client.chatMessage("\c3Protection spell used!");
		%client.setEmitter(ProtectionSpellImage);
		serverPlay3D(protectionSpellSound, %client.player.getPosition());
		%client.refreshHud();
	}
	else if(%spell $= "Iceball")
	{
		%cost = 34;
		if(%client.statBoost && %client.clothing $= "Mage")
			%cost *= 0.5;
		if(%client.clothing $= "Mage")
			%cost *= 0.5;
		if(%client.mana < %cost)
		{
			%client.chatMessage("\c3You don't have enough mana for this spell. You need 34 mana.");
			return;
		}
		%client.mana -= %cost;
		%pos = vectoradd(%client.player.getEyeVector(), %client.player.getEyePoint());
		%p = new Projectile()
		{
			dataBlock = iceballProjectile;
			initialVelocity = vectorScale(%client.player.getEyeVector(), 100);
			initialPosition = %pos;
			client = %client;
			sourceObject = %client.player;
			sourceClient = %client;
		};
		MissionCleanup.add(%p);
		serverPlay3D(iceFireSound, %client.player.getPosition());
	}
	else if(%spell $= "Fireball")
	{
		%cost = 40;
		if(%client.statBoost && %client.clothing $= "Mage")
			%cost *= 0.5;
		if(%client.clothing $= "Mage")
			%cost *= 0.5;
		if(%client.mana < %cost)
		{
			%client.chatMessage("\c3You don't have enough mana for this spell. You need 40 mana.");
			return;
		}
		%client.mana -= %cost;
		%pos = vectoradd(%client.player.getEyeVector(), %client.player.getEyePoint());
		%p = new Projectile()
		{
			dataBlock = fireballProjectile;
			initialVelocity = vectorScale(%client.player.getEyeVector(), 50);
			initialPosition = %pos;
			client = %client;
			sourceObject = %client.player;
			sourceClient = %client;
		};
		MissionCleanup.add(%p);
		serverPlay3D(fireballFireSound, %client.player.getPosition());
	}
	else if(%spell $= "Location")
	{
		%cost = 80;
		if(%client.statBoost && %client.clothing $= "Mage")
			%cost *= 0.5;
		if(%client.clothing $= "Mage")
			%cost *= 0.5;
		if(%client.mana < %cost)
		{
			%client.chatMessage("\c3You don't have enough mana for this spell. You need 80 mana.");
			return;
		}
		if(%client.abilityCD !$= "")
		{
			%client.chatMessage("\c3You already have one spell active. You must wait for it to expire before using this one.");
			return;
		}
		if(%a $= "")
		{
			%client.chatMessage("\c3This command allows you to start a path to a person on the server. It displays as a number in the HUD.");
			%client.chatMessage("\c3Please say the person you want to start a path to. /cast location personsName");
			return;
		}
		%target = findclientbyname(%a);
		if(!%target)
		{
			%client.chatMEssage("\c3That person couldn't be found.");
			return;
		}
		if(%target == %client)
		{
			%client.chatMessage("\c3You can't start a quest to yourself!");
			return;
		}
		%client.chatMessage("\c3Location spell used! To cancel your quest and refund some mana, do /cancelQuest");
		%client.quest = "Starting...";
		%client.setEmitter(LocationSpellImage);
		%client.refreshHUD();
		%client.questLoop(%target);
		%client.abilityCD = 5 SPC "Location";
		%client.aurora();
		%client.mana -= 80;
		%client.refreshHud();
		serverPlay3D(locationSpellSound, %client.player.getPosition());
	}
}

function servercmduseManaBlock(%client)
{
	if(%client.BPmanablock < 1)
	{
		%client.chatMessage("\c3You don't have a manablock to use!");
		return;
	}
	%client.chatMessage("\c3A manablock has been used!");
	%client.mana = 100;
	%client.BPmanablock -= 1;
	%client.aurora();
	%client.refreshHud();
}

function servercmdKnownSpells(%client)
{
	%client.chatMessage("\c3You know the following spells:");
	if(%client.mTeleport)
		%client.chaTMessage("\c6Teleport");
	if(%client.mSurplus)
		%client.chatMessage("\c6Surplus");
	if(%client.mProtection)
		%client.chatMessage("\c6Protection");
	if(%client.mSpeed)
		%client.chatMessage("\c6Speed");
	if(%client.mLocation)
		%client.chatMessage("\c6Location");
	if(%client.mIceball)
		%client.chatMessage("\c3Iceball");
	if(%client.mFireball)
		%client.cahtMessage("\c3Fireball");
	%client.chatMessage("\c3To learn more spells, someone who already knows it needs to tutor you.");
	%client.chatMessage("\c3In order to tutor someone, do /tutor.");
	%client.chatMessage("\c3The current usable spells are Teleport, Surplus, Protection, and Speed.");
}

function servercmdTutor(%client, %target, %spell)
{
	if(%target $= "")
	{
		%client.chatMessage("\c3This command allows you to teach other people spells that you know.");
		%client.chatMessage("\c3To find out the spells you know, do knownSpells.");
		%client.chatMessage("\c3The syntax is /tutor PersonsNameToTutor spellToTutor");
		%client.chatMessage("\c3You need to be physically near this person. Tutoring someone drains 80 mana from you.");
		return;
	}
	if(%spell $= "")
	{
		%client.chatMessage("\c3Please verify a spell to tutor.");
		return;
	}
	if(%client.m[%spell] $= "")
	{
		%client.chatMessage("\c3You either don't know that spell, or that is an invalid spell.");
		%client.chatMessage("To see spells that you know, do /knownSpells");
		return;
	}
	if(%client.mana < 80)
	{
		%client.chatMessage("\c3You don't have enough mana to tutor that person. You need at least 80 mana.");
		return;
	}
	if(!isObject(%client.player))
	{
		%client.chatMessage("\c3You need to be alive to use this command.");
	}
	%target = findclientbyname(%target);
	if(%target $= "")
	{
		%Client.chatMessage("\c3Client not found.");
		return;
	}
	if(%target.m[%spell] > 0)
	{
		%client.chatMessage("\c3That person already knows that spell!");
	}
	initContainerRadiusSearch(%client.player.getPosition(), 10, $Typemasks::PlayerObjectType, %client.player);
	while(%player = containerSearchNext())
	{
		if(%player.client == %target)
		{
			%found++;
			break;
		}
	}
	if(%found $= "")
	{
		%client.chatMessage("\c3You aren't in range of" SPC %target.name @ ".");
		return;
	}
	%client.mana -= 80;
	%client.aurora();
	%client.refreshHud();
	%client.chatMessage("\c3You successfully taught the spell\c5" SPC %spell @ "\c3!");
	%target.chatMessage("\c3The player" SPC %client.name SPC "has taught you the spell\c5" SPC %spell @ "\c3!");
	%target.m[%spell] = 1;
	%target.aurora();
	%target.refreshHud();
}

function gameConnection::Aurora(%client, %manual)
{
	%di = getRandom(5,35);
	initContainerRadiusSearch(%client.player.getPosition(), 10, $Typemasks::PlayerObjectType);
	while(%player = containerSearchNext())
	{
		if(%player == %client.player)
			return;
		if(%manual $= "")
			%player.client.mana += %di;
		else
			%player.client.mana += %manual;
		%player.client.refreshHUD();
	}
}

function servercmdBind(%client, %key, %spell)
{
	if(%spell $= "")
	{
		%client.chatMessage("\c3This command allows you to bind spells to a key.");
		%client.chatMessage("\c3You can choose to bind a spell to the love, hate, or confusion keys.");
		%client.chatMessage("\c3The syntax for this commmand is /bind key spell");
		return;
	}
	if(%key $= "Love" || %key $= "Hate" || %key $= "Confusion")
	{
		if(%key $= "Love")
			%client.loveHotkey = %spell;
		else if(%key $= "Hate")
			%client.hateHotkey = %spell;
		else if(%key $= "Confusion")
			%client.confusionHotkey = %spell;
		else
			talk("\c3This shouldn't have happened. Tell Piexes that the emote binds are fucked up."); //lol
		%client.chatMessage("\c3Key bound!");
	}
	else
	{
		%client.chatMessage("\c3That isn't a valid key.");
	}
}

package CastingHotkeys
{
	function servercmdLove(%client)
	{
		if(%client.loveHotkey $= "")
		{
			parent::servercmdLove(%client);
			return;
		}
		servercmdCast(%client, %client.loveHotkey);
	}
	function servercmdHate(%client)
	{
		if(%client.hateHotkey $= "")
		{
			parent::servercmdHate(%client);
			return;
		}
		servercmdCast(%client, %client.hateHotkey);
	}
	function servercmdConfusion(%client)
	{
		if(%client.confusionHotkey $= "")
		{
			parent::servercmdConfusion(%client);
			return;
		}
		servercmdCast(%client, %client.confusionHotkey);
	}
};
activatePackage(CastingHotkeys);

datablock AudioProfile(iceFireSound)
{
   filename    = "./Models/iceFire.wav";
   description = AudioClose3d;
   preload = true;
};
datablock AudioProfile(iceHitSound)
{
   filename    = "./Models/iceHit.wav";
   description = AudioClose3d;
   preload = true;
};
datablock ProjectileData(iceballProjectile : gunProjectile)
{
   projectileShapeName = "./Models/iceBall.dts";
   directDamage        = 34;
   directDamageType    = $DamageType::Gun;
   radiusDamageType    = $DamageType::Gun;

   impactImpulse	     = 1000;
   verticalImpulse	  = 1000;
   explosion           = iceballExplosion;
   particleEmitter     = "snowCloudEmitter"; //bulletTrailEmitter;

   muzzleVelocity      = 40;
   velInheritFactor    = 1;

   bounceElasticity    = 0.7;
   bounceFriction      = 0.2;

   explodeOnDeath = true;
   gravitymod = 0.5;
   isBallistic = true;

   uiName = "Iceball";
};
datablock ExplosionData(iceballExplosion : gunExplosion)
{
   //explosionShape = "";
   soundProfile = iceHitSound;
   lifeTimeMS = 150;
   particleEmitter = gunExplosionEmitter;
   emitter[0] = gunExplosionEmitter;
};

//Fireball
datablock AudioProfile(fireballFireSound)
{
   filename    = "./Sounds/fireballFire.wav";
   description = AudioClose3d;
   preload = true;
};
datablock AudioProfile(fireballHitSound)
{
   filename    = "./Sounds/fireballHit.wav";
   description = AudioClose3d;
   preload = true;
};
datablock ProjectileData(fireballProjectile : iceballProjectile)
{
   projectileShapeName = "./Models/rockBall.dts";
   directDamage        = 50;
   explosion           = fireballExplosion;
   particleEmitter     = "fireballEmitter"; //bulletTrailEmitter;
   muzzleVelocity      = 40;
   bounceElasticity    = 0.7;
   bounceFriction      = 0.2;
   explodeOnDeath = true;
   gravitymod = 0.5;
   isBallistic = true;
   uiName = "Fireball";
};
datablock ExplosionData(fireballExplosion : iceballExplosion)
{
   //explosionShape = "";
   soundProfile = fireballHitSound;
   lifeTimeMS = 150;
   particleEmitter = fireballExplosionEmitter; //Replace this
   emitter[0] = fireballExplosionEmitter; //Idiot
};
package MagicProjectiles
{
	function iceballProjectile::damage(%this,%obj,%col,%fade,%pos,%normal)
	{
		%col.setBaseMovementSpeed(2);
		%col.schedule(7000, resetMovementSpeed);
		parent::damage(%this,%obj,%col,%fade,%pos,%normal);
	}
	function fireballProjectile::damage(%this,%obj,%col,%fade,%pos,%normal)
	{
		%col.addRelativeVelocity("0 -10 5");
		parent::damage(%this,%obj,%col,%fade,%pos,%normal);
	}
};
activatePackage(MagicProjectiles);