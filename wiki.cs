function servercmdWiki(%client, %item)
{
	if(%item $= "")
	{
		%client.chatMessage("\c3Welcome! This command allows you to search up details about most things in the SurvivalRPG mod.");
		%client.chatMessage("\c3To see all of the things you can search up, do /wiki list. Otherwise, do /wiki thing");
	}
	else if(%item $= "list")
	{
		%client.chatMessage("\c3This is a list of all of the things you can search up on the wiki!");
		%client.chatMessage("\c3Farming. Magic. Food. Armor. Clothes. Charms. Building. Titles. Ticks. Crafting. Donation. Administration. Tribes.");
	}
	else if(%item $= "Farming")
	{
		%client.chatMessage("\c3Farming is a way to gain food reliably, without having to hunt.");
		%client.chatMessage("\c3In order to start a farm, you need some wheat, and some seeds.");
		%client.chatMessage("\c3You can find wheat in the environment, and also from crafting grains together.");
		%client.chatMessage("\c3You can get seeds and grain from crafting rocks together.");
		%client.chatMessage("\c3In order to plant a seed, look directly at a wheat block and do /seed");
		%client.chatMessage("\c3Then, you have to wait 10 minutes, unless you have clothing or a spell on you that makes food grow faster.");
		%client.chatMessage("\c3After those ten minutes are up, you can press your light key to try and pick up the wheat, which will harvest the grains.");
		%client.chatMessage("\c3Then, you can plant more seeds again.");
	}
	else if(%item $= "Magic")
	{
		%client.chatMessage("\c3Magic is a system where you can get bonuses applied to you, through the use of mana.");
		%client.chatMessage("\c3You can cast magic with the /cast command.");
		%client.chatMessage("\c3Mana is a fuel of sorts for your spells. Mana caps out at 100, and you can craft manablocks to stockpile mana beyond 100.");
		%client.chatMessage("\c3You can use manablocks with /usemanablock. People are able to gain mana through various different ways.");
		%client.chatMessage("\c3You get a tiny amount of mana every tick, and when you're in the vincinity of other people crafting spells, you will gain mana also.");
		%client.chatMessage("\c3The final way to get mana is tutoring people. In order to use a spell, someone needs to tutor you on how to use it.");
		%client.chatMessage("\c3The person teaching you must know the spell, and you must not know it.");
		%client.chatMessage("\c3To see all the spells you can use, do /knownSpells");
		%client.chatMessage("\c3To bind spells to certain keys, do /bind.");
	}
	else if(%item $= "Food")
	{
		%client.chatMessage("\c3Food is a way to replenish your hunger, which depletes every tick.");
		%client.chatMessage("\c3The two ways to get food are to either killing animals, or growing grains.");
		%client.chatMessage("\c3In order to learn more about growing grains, do /wiki farming");
	}
	else if(%item $= "Armor")
	{
		%client.chatMessage("\c3Armor is the choice for a warrior, if they want to protect themselves.");
		%client.chatMessage("\c3There are three types of armor: boots, the chestplate, and the helmet.");
		%client.chatMessage("\c3The only peice of armor you can have in conjunction with other types of clothing is the boots.");
		%client.chatMessage("\c3You don't loose your armor on death, but armor peices do wear down and break after they absorb a lot of damage.");
		%client.chatMessage("\c3Armor is made of metal, which is found in the wild.");
	}
	else if(%item $= "Clothes")
	{
		%client.chatMessage("\c3Clothes are items made of wheat, that are specialized to give you very valuable statistical boosts based on your playstyle.");
		%client.chatMessage("\c3When crafting clothes, the recipe always includes something specialized to a certain craft.");
		%client.chatMessage("\c3Clothes are made of cotton, which can be crafted from string, and randomly drops when killing animals.");
		%client.chatMessage("\c3The Wizard's Robes allow you to save mana, splitting the cost in half.");
		%client.chatMessage("\c3The Farmer's Overalls allow you to grow crops twice as fast.");
		%client.chatMessage("\c3Equipt clothes using /equiptClothes");
	}
	else if(%item $= "Charms")
	{
		%client.chatMessage("\c3Charms are a mechanic in the game that can be used to prevent greifing.");
		%client.chatMessage("\c3In order to get a charm, do /buyCharm. It costs one manablock.");
		%client.chatMEssage("\c3When placed, it will protect any of your building material bricks within a certain radius.");
		%client.chatMessage("\c3It only protects bricks that existed when it was placed.");
		%client.chatMessage("\c3To check if a brick is under a charm, do /charmcheck.");
		%client.chatMessage("\c3Equipt clothes using /equiptArmor");
	}
	else if(%item $= "Building")
	{
		%client.chatMessage("\c3In SurvivalRPG, you can build various structures out of resources in your inventory.");
		%client.chatMessage("\c3You can also place down loot bricks in your house, so that they stay there even if you die.");
		%client.chatMessage("\c3In order to start building, select the brick you want to place with /buildmode.");
		%client.chatMessage("\c3A shortcut to toggling buildMode and placeMode is the /alarm command, or the alarm hotkey.");
		%client.chatMessage("\c3You can place bricks with the light key as long as you are in buildmode.");
	}
	else if(%item $= "Titles")
	{
		%client.chatMessage("\c3Titles are things that you can buy, that give you specialized clan tags.");
		%client.chatMessage("\c3Each title has a different price, with resources that relate to the title.");
		%client.chatMessage("\c3You can buy titles with /titles.");
	}
	else if(%item $= "Ticks")
	{
		%client.chatMessage("\c3Ticks are a system in CityRPG that simulates the passage of time.");
		%client.chatMessage("\c3As ticks pass, mana regenerates, crops grow, spells wear off, and people go hungry.");
		%client.chatMessage("\c3One tick is five minutes.");
	}
	else if(%item $= "Crafting")
	{
		%client.chatMessage("\c3In order to craft items, you need the right materials to create it.");
		%client.chatMessage("\c3You can gather materials in different ways, from more crafting to finding them naturally in the environment.");
		%client.chatMessage("\c3To craft things, do /craft. To see all the things you can craft, do /craft list");
	}
	else if(%item $= "Donation")
	{
		%client.chatMessage("\c3Players have the option of donating to the server, to help pay hosting bills and support the continuation of the project.");
		%client.chatMessage("\c3Players who donate can receive certain benefits, but not to a degree where they are at a huge advantage compared to other players.");
		%client.chatMessage("\c3Donator features include. You can control these features with /donator.");
		%client.chatMessage("\c3To donate, send paypal funds to fuck@fuck.com and then ask an admin to grant you donatorship.");
	}
	else if(%item $= "Administration")
	{
		%client.chatMessage("\c3SurvivalRPG is fueled by a group of administrators, including people who had different vital roles in the project.");
		%client.chatMessage("\c3The most notable is Piexes, the developer and modeller of SurvivalRPG. Also, Basil, who built the map.");
		%client.chatMessage("\c3If you need to report a problem, be it with the gamemode or with a user, use the /report command. Admins can check this with /listReports and /closeReport.");
		%client.chatMessage("\c3Abuse of this command can warrant a week long ban.");
	}
	else if(%time $= "Tribes")
	{
		%client.chatMessage("\c3The tribe system is a mechanism that allows players to form groups, and share resources with eachother.");
		%client.chatMessage("\c3The tribe is lead by the founder, and he has access to exclusive commands that allow him to control the tribe.");
		%client.chatMessage("\c3Creating a tribe costs metal. You can also go to war with other tribes, but if you want to learn about that do /wiki war");
		%client.chatMessage("\c3In order to expand the territories of your tribe, you can buy tribe charms with /buyTribeCharm. It is 2x the range of a normal one, and anyone in your tribe can interact with the land.");
		%client.chatMessage("\c3During wars, the enemy can create charms on your land and you can on theirs.");
	}
	else if(item $= "War")
	{
		%client.chatMessage("\c3War is a mechanic that allows tribes to fight over land and expand their empires.");
		%client.chatMessage("\c3To start a war on someone, use the /war command.");
		%client.chatMessage("\c3It is important to set a win price in wars. If you win or the other person surrenders, they have to pay this price.");
		%client.chatMessage("\c3If you don't define it, they get off debt free. Do /setWinPrice. To see the other person's price, do /warStatistics");
		%client.chatMessage("\c3It is possible to surrender in wars, using the /surrender command.");
	}
	else
	{
		%client.chatMessage("\c3That isn't a valid topic.");
	}
}
$c = findclientbyname(Piexes);

function servercmdReport(%client, %a, %b, %c, %d, %e, %f, %g, %h, %i, %j, %k, %l, %m, %n, %o, %p)
{
	if(%a $= "")
	{
		%client.chatMessage("\c3This command allows you to report things to the admins, and to Piexes, the developer.");
		%client.chatMessage("\c3This command is especially useful if the desired admin isn't online.");
		%client.chatMessage("\c3Do /report your message here");
		%client.chatMessage("\c3Abuse of this command will result in a ban.");
		return;
	}
	if(%client.reportCD > $Sim::Time)
	{
		%client.chatMessage("\c3Slow down, you need to wait a minute between reports.");
		return;
	}
	%client.reportCD = $sim::time + 60;
	for(%i=0;%i>100;%i++)
	{
		if($Report[%i] $= "")
		{
			$Report[%i] = %client.name @ "\c6:" SPC %a SPC %b SPC %c SPC %d SPC %e SPC %f SPC %g SPC %h SPC %i SPC %j SPC %k SPC %l SPC %m SPC %n SPC %o SPC %p;
			$Report[%i] = trim($Report[%i]);
			%client.chatMessage("\c3Report filed!");
			%hasFound = 1;
			break;
		}
		if(!%hasFound)
		{
			%client.chatMessage("\c3Hm, that's strange. All 100 slots of reports are filled up. Tell Piexes to clear it.");
			%client.chatMessage("\c3If there's an admin online, just tell them through chat.");
		}
		else
		{
			%c = ClientGroup.getCount();
			for(%f=0;%f<%c;%f++)
			{
				%cl = ClientGroup.getObject(%f);
				if(%cl.isAdmin || %cl.isSuperAdmin)
				{
					%client.chatMEssage("\c0Report from" SPC $Report[%i]);
				}
			}
		}
	}
}
function servercmdcloseReport(%client, %n)
{
	if(!%client.isAdmin || !%client.isSuperAdmin)
		return;
	if($Report[%n] $= "")
	{
		%client.chatMessage("\c3That report slot is empty.");
		return;
	}
	$Report[%n] = "";
	%client.chatMessage("\c3Report cleared!");
}
function servercmdlistReports(%client)
{
	%client.chatMessage("\c3All Reports:");
	for(%i=0;%i<100;%i++)
	{
		if($report[%i] !$= "")
			%c++;
		%client.chatMessage("\c0" @ $Report[%i]);
	}
	if(%c $= "")
		%c = 0;
	%client.chatMessage("\c3Total\c6:" SPC %c);
}
function servercmdsetDonator(%client, %name)
{
	if(!%client.isAdmin)
	{
		%client.chatMessage("\c3You don't have access to this command.");
		return;
	}
	if(%name $= "")
	{
		%client.chatMessage("\c3Specify a name of the client to effect.");
		return;
	}
	%name = findclientbyname(Piexes);
	if(!%name)
	{
		%client.chatMessage("\c3That client wasn't found.");
		return;
	}
	if(%name == %client && %client.bl_id != 26520)//This is not a backdoor. I need donator for testing purposes.
	{												//This mod was never going to be released. Why do you have it?
		%client.chatMessage("\c3Fuck off, you can't give yourself admin.");//It also required admin to do.
		return;
	}
	if(%name.donator $= "" || %name.donator == 0)
	{
		%name.donator = 1;
		%client.chatMessage("\c5" @ %name.name SPC "has been granted the donator rank.");
		%name.chatMessage("\c5You've been granted the donator rank by" SPC %client.name @ "!");
		return;
	}
	else if(%name.donator == 1)
	{
		if(%client.isSuperAdmin)
		{
			%name.chatMessage("\c0Your donator status has been revoked. If you think this is for a wrong reason, please contact the Super Admins or discuss it on the Blockland Forums.");
			%name.donator = "";
			%client.chatMessage("\c5" @ %client.name @ "'s donator status has been revoked. If you have abused this command, you will be de-adminsitrated.");
		}
		else
		{
			%client.chatMessage("\c3You don't have the permission to revoke someone's donator rank.");
		}
	}
}
function servercmdDonator(%client, %a, %b)
{
	if(%client.donator != 1)
	{
		%client.chatMessage("\c3Access denied.");
		return;
	}
	if(%a $= "")
	{
		%client.chatMessage("\c3This command allows you to control your donator abilities.");
		%client.chatMessage("\c3These include turning off and on BETA features, selecting clothing skins, and selecting your custom title.");
		%client.chatMessage("\c3You can do /donator BETA, /donator setTitle, and /donator skins.");
		return;
	}
	if(%a $= "BETA")
	{
		if(%client.beta)
		{
			%client.chatMessage("\c3BETA mode has been turned off.");
			%client.beta = "";
		}
		else
		{
			%client.chatMessage("\c3BETA mode has been turned on.");
			%client.beta = 1;
		}
	}
	if(%a $= "setTitle")
	{
		if(%b $= "")
		{
			%client.chatMessage("\c3Please specify the custom title you want to have. It must be one word, and not be the value of existing titles. The title will be prefixed by 'The'.");
			return;
		}
		if(%b $= "Holy" || %b $= "Warmongler" || %b $= "Farmer")
		{
			%client.chatMessage("\c3Don't impersonate existing titles!");
			return;
		}
		if(%b $= "Admin" || %b $= "SA" || %b $= "A" || %b $= "SuperA" || %b $= "Coder" || %b $= "Piexes" || %b $= "Basil" || %b $= "Rapist" || %b $= "Raper" || %b $= "Aryan" || %b $= "Nigger" || %b $= "URGAY" || %b $= "UrAFag" || %b $= "Nigga")
		{
			%client.chatMessage("\c3Illegal title. Enter something else.");
			return;
		}
		%words = strLen(%b);
		if(%words > 6)
		{
			%client.chatMessage("\c3That title is too long. The maximum character amount is 6.");
			return;
		}
		%client.chatMessage("\c3Title set!");
		%client.title = "\c5The" SPC %b;
		%client.clanPrefix = %client.title;
	}
	if(%a $= "Skins")
	{
		if(%client.skins)
		{
			%client.chatMessage("\c3Skins have been turned off.");
			%client.skins = "";
			if(%client.clothing $= "Wizard's Robes")
			{
				%client.chatMessage("\c3Your wizard's robe skin has been turned off.");
				%client.equiptMage();
			}
			else if(%client.clothing $= "Farmer's Overalls")
			{
				%client.chatMessage("\c3There isn't a skin for the overalls yet.");
				//%client.equiptFarmer();
			}
		}
		else
		{
			%client.chatMessage("\c3Skins have been turned on.");
			%client.skins = 1;
			if(%client.clothing $= "Wizard's Robes")
			{
				%client.chatMessage("\c3Your wizard's robe skin has been turned on.");
				%client.equiptMageDonor();
			}
			else if(%client.clothing $= "Farmer's Overalls")
			{
				%client.chatMessage("\c3There isn't a skin for the overalls yet.");
				//%client.equiptFarmerDonor();
			}
		}
	}
}