//PACKAGE START	-----------
 
package tribeHook
{
	function gameConnection::autoAdminCheck(%this)
	{
		%p = parent::autoAdminCheck(%this);
		
		if(!tribeContainer.initialized)
		{
			if(discoverFile("config/server/Tribes/TribeContainer/tribeContainer.cs"))
			{
				//Tribe container file exists, let's load it up.
				exec("config/server/Tribes/TribeContainer/tribeContainer.cs");
				echo("Initialized the tribe container.");
			}
			else
			{
				//First time running this script, we need to create and save the new tribecontainer
				new scriptGroup(tribeContainer);
				tribeContainer.save("config/server/Tribes/TribeContainer/tribeContainer.cs");
				echo("First time initializing the tribe container: Success.");
			}
			tribeContainer.initialized = true;
		}
		
		%path = "config/server/Tribes/Users/" @ %this.bl_id @ ".txt";
		if(discoverFile(%path))
		{
			%f = new fileObject();
			%f.openForRead(%path);
			%tribe = %f.readLine();
			//To deal with user name changes or whatever..
			%userName = %f.readLine();
			%f.close();
			%f.delete();
			%this.tribe = tribeContainer.findTribeByName(%tribe);
			//Did we get an invalid tribe
			if(!%this.tribe)
			{
				error("Tribe invalid! Deleting user data associated with that Tribe.");
				%this.tribe = "";
				fileDelete(%path);
			}
			else
			{
				%this.tribe.add(%this);
				echo("Added user " @ %this.name @ " to Tribe " @ %this.getTribeName() @ " successfully.");
			}
		}
		return %p;
	}
}; activatePackage(tribeHook);

//PACKAGE END -----------


//TRIBECONTAINER FUNCTIONS START -----------

//Assuming the host will only be able
//to create new tribes
function tribeContainer::newTribe(%this,%client,%name)
{
	%c = %this.getCount();
	for(%i=0;%i<%c;%i++)
	{
		%tribeObj = %this.getObject(%i);
		if(%tribeObj.name $= %name)
		{
			//Already a tribe with that name.. end function
			return false;
		}
	}
	
	%newTribe = new simSet()
	{
		name = %name;
		owner = %client.bl_id SPC %client.name;
		private = 0;
	};
	
	%this.add(%newTribe);
	%this.save("config/server/Tribes/TribeContainer/tribeContainer.cs");
	echo("New tribe " @ %name @ " added to Tribe container.");
	return %newTribe;
}

//Using this function will throw errors if users are still
//Associated to this tribe, so make sure the tribe is empty.
function tribeContainer::removeTribe(%this,%name)
{
	%c = %this.getCount();
	for(%i=0;%i<%c;%i++)
	{
		%tribeObj = %this.getObject(%i);
		if(%tribeObj.name $= %name)
		{
			%tribeObj.delete();
			echo("Successfully deleted Tribe " @ %name);
			%this.save("config/server/Tribes/TribeContainer/tribeContainer.cs");
			return true;
		}
	}
	error("Could not find a Tribe object with the name " @ %name);
	return false;
}

function tribeContainer::findTribeByName(%this,%tribeName)
{
	%c = %this.getCount();
	for(%i=0;%i<%c;%i++)
	{
		%tribeObj = %this.getObject(%i);
		if(%tribeObj.name $= %tribeName)
		{
			return %tribeObj;
		}
	}
	return false;
}

//TRIBECONTAINER FUNCTIONS END -----------


//GAMECONNECTION FUNCTIONS START -----------

function gameConnection::addToTribe(%this,%name)
{
	//Tribe simset..
	%ss = tribeContainer.findTribeByName(%name);
	//No valid simset object supplied
	if(!isObject(%ss))
	{
		error("We need a valid simset object for Tribe named " @ %name @ ".");
		return false;
	}
	//User has a tribe already..
	if(isObject(%this.getTribe()))
	{
		error("This user is already in a tribe.");
		return false;
	}
	%this.tribe = %ss;
	//Save tribe info for a user to load upon joining the server
	%f = new fileObject();
	%f.openForWrite("config/server/Tribes/Users/" @ %this.bl_id @ ".txt");
	%f.writeLine(%ss.name);
	%f.writeLine(%this.name);
	%f.close();
	%f.delete();
	echo(%this.name @ " added to " @ %name @ " (" @ %ss @ ") successfully.");
	%ss.add(%this);
	return true;
}

function gameConnection::removeFromTribe(%this)
{
	if(isObject(%this.tribe))
	{
		%tribe = %this.getTribe();
		%tribeName = %this.getTribeName();
		%tribe.remove(%this);
		%this.tribe = "";
		%res = fileDelete("config/server/Tribes/Users/" @ %this.bl_id @ ".txt");
		if(!%res)
		{
			error("No tribe file for " @ %this.name);
			return false;
		}
		else
		{
			echo("Removed " @ %this.name @ " from tribe " @ %tribeName);
			return true;
		}
	}
	else
	{
		error("This user is not in a tribe.");
		return false;
	}
}

function gameConnection::getTribe(%this)
{
	//return the tribe however you're storing it
	//I'm just gunna assume it'll be tagged to the client
	return %this.tribe;
}

function gameConnection::getTribeName(%this)
{
	return %this.tribe.name;
}

//GAMECONNECTION FUNCTIONS END
//SERVERCOMMANDS BEGIN

function servercmdcreateTribe(%client, %name)
{
	if(%name $= "")
	{
		%client.chatMessage("\c3Please specify a name for your tribe!");
		return;
	}
	if(findTribeByName(%client))
	{
		%client.chatMessage("\c3That name is taken!");
		return;
	}
	if(%client.tribe)
	{
		%client.chatMessage("\c3You're already in a tribe! Leave your current tribe by doing /leaveTribe or /disbandTribe");
		return;
	}
	if(%client.has(Metal, 5))
	{
		TribeContainer.newTribe(%client, %name);
		announce("\c5The tribe" SPC %name SPC "has been created by" SPC %client.name);
	}
	else
	{
		%client.chatMessage("\c3Creating a tribe requires five metal.");
	}
}
function servercmdleaveTribe(%client)
{
	if(%client.tribe $= "")
	{
		%client.chatMessage("\c3You don't have a tribe to leave!");
		return;
	}
	if(getWord(%client.tribe.owner, 0) == %client.bl_id)
	{
		%client.chatMessage("\c3You are the owner of your tribe, you can't leave! You will need to disband your entire tribe if you do that.");
		return;
	}
	%msg = "The user" SPC %client.name SPC "has left the tribe.";
	%client.tribeAnnounce(%msg);
	%client.removeFromTribe();
	%client.chatMessage("\c3You have been removed from your tribe.");
}
function servercmddisbandTribe(%client) //make this delete the tribe charms
{
	if(%client.tribe $= "")
	{
		%client.chatMessage("\c3You don't have a tribe to disband.");
	}
	if(getWord(%client.tribe.owner, 0) != %client.bl_id)
	{
		%client.chatMessage("\c3You aren't the owner of this tribe!");
		return;
	}
	%client.chatMessage("\c3Tribe removed!");
	%msg = "The tribe has been disbanded by" SPC %client.name;
	%client.tribeAnnounce(%msg);
	for(%i=0;%i<TribeContainer.getCount();%i++)
	{
		%tribe = TribeContainer.getObject(%i);
		if(%tribe.enemy1 == %client.tribe)
			%tribe.enemey1 = "";
		else if(%tribe.enemy2 == %client.tribe)
			%tribe.enemey2 = "";
		else if(%tribe.enemy3 == %client.tribe)
			%tribe.enemey3 = "";
		else if(%tribe.enemy4 == %client.tribe)
			%tribe.enemey4 = "";
		else if(%tribe.enemy5 == %client.tribe)
			%tribe.enemey5 = "";
		if(getWord(%tribe.debt1, 2) == %client.tribe)
			%tribe.debt1 = "";
		if(getWord(%tribe.debt2, 2) == %client.tribe)
			%tribe.debt2 = "";
	}
	TribeContainer.removeTribe(%client.tribe.name);

}
function servercmdjoinTribe(%client, %name)
{
	if(%client.requestCooldown > $Sim::Time)
	{
		%client.chatMessage("\c3You need to wait before you can request to join a tribe again.");
		return;
	}
	if(%client.tribe)
	{
		%client.chatMessage("\c3You are already in a tribe. Do /disbandTribe or /leaveTribe to lose your tribe.");
		return;
	}
	if(%name $= "")
	{
		%client.chatMessage("\c3Please enter the name of the tribe you want to join.");
		return;
	}
	%tribe = TribeContainer.findTribeByName();
	if(!%tribe)
	{
		%client.chatMessage("\c3That tribe wasn't found.");
		return;
	}
	if(%tribe.private)
	{
		%client.chatMessage("\c3This tribe is private, meaning no one can request to join.");
		%client.chatMessage("\c3Do undo this, the owner has to do /makeTribePublic.");
		return;
	}
	%client.requestCooldown = $Sim::Time + 30;
	%c = ClientGroup.getCount();
	%blid = getWord(%tribe.owner, 0);
	%tclient = findclientbyblid(%blid);
	if(%tclient)
	{
		if(%tclient.tribeRequest)
		{
			%client.chatMessage("\c3The owner of this tribe already has a pending join request.");
			%client.chatMessage("\c3They have been notified of this, and urged that they accept or deny the pending request.");
			%tclient.chatMessage("\c3The user" SPC %client.name SPC "has tried to join your clan but you already have a pending request.");
			%tclient.chatMessage("\c3Please use /tribeAccept or /tribeDeny to accept or deny the request by ID" SPC %tclient.tribeRequest SPC ".");
			%client.requestCooldown = $Sim::Time + 10;
			return;
		}
		%client.chatMessage("\c3A message has been sent to" SPC %tclient.name SPC "that you are trying to join.");
		%client.chatMessage("\c3He needs to accept your request before you can join.");
		%tclient.tribeRequest = %client.bl_id;
		%tclient.chatMessage("\c3The player" SPC %client.name SPC "is trying to join your tribe.");
		%client.chatMessage("\c3To accept or deny his attempt, do /tribeAcccept or /tribeDeny");
	}
	else
	{
		%client.chatMessage("\c3The tribe owner isn't online. He needs to be on the server in order to accept new members.");
	}

}
function servercmdtribeAccept(%client)
{
	if(getWord(%client.tribe.owner, 0) != %client.bl_id)
	{
		%client.chatMessage("\c3You aren't the owner of the clan, so you can't accept or deny any requests.");
		return;
	}
	if(!%client.tribeRequest)
	{
		%client.chatMessage("\c3Currently, there are no requests you can accept or deny.");
		return;
	}
	for(%i=0;%i<%c;%i++)
	{
		%blid = getWord(%tribe.owner, 0);
		%tclient = ClientGroup.getObject(%i);
		if(%tclient.bl_id == %client.tribeRequest)
		{
			%found++;
			break;
		}
	}
	if(%found)
	{
		if(%tclient.tribe)
		{
			%client.chatMessage("\c3Since the user has sent this request, he joined a tribe!");
			%tclient.chatMessage("\c3The tribe" SPC %tclient.tribe.name SPC "has accepted your request to join their tribe, but you have since then joined another tribe.");
			return;
		}
		%tclient.chatMessage("\c3Your request to join the" SPC %client.tribe.name SPC "tribe has been accepted!");
		%msg = "The user" SPC %tclient.name SPC "has been accepted into the tribe!";
		%client.tribeAnnounce(%msg);
		%client.tribeRequest = "";
		%tclient.addToTribe(%client.tribe.name);
	}
	else
	{
		%client.chatMessage("\c3The person that sent the request has since left the server.");
		%client.chatMessage("\c3You can either deny this request, or do /clearRequests");
	}
}
function servercmdtribeDeny(%client)
{
	if(getWord(%client.tribe.owner, 0) != %client.bl_id)
	{
		%client.chatMessage("\c3You aren't the owner of the clan, so you can't accept or deny any requests.");
		return;
	}
	if(!%client.tribeRequest)
	{
		%client.chatMessage("\c3Currently, there are no requests you can accept or deny.");
		return;
	}
	for(%i=0;%i<%c;%i++)
	{
		%blid = getWord(%tribe.owner, 0);
		%tclient = ClientGroup.getObject(%i);
		if(%tclient.bl_id == %client.tribeRequest)
		{
			%found++;
			break;
		}
	}
	if(%found)
	{
		if(%tclient.tribe)
		{
			%client.chatMessage("\c3Since the user has sent this request, he joined a tribe.");
			%tclient.chatMessage("\c3The tribe" SPC %tclient.tribe.name SPC "has denied your request to join their tribe.");
			return;
		}
		%tclient.chatMessage("\c3Your request to join the" SPC %client.tribe.name SPC "tribe has been denied.");
		%client.chatMessage("\c3You have denied" SPC %tclient.name @ "'s request.");
		%client.tribeRequest = "";
	}
	else
	{
		%client.chatMessage("\c3Request denied! Note that the person won't know they got denied since they aren't on the server right now.");
	}
}
function servercmdclearRequests(%client)
{
	if(getWord(%client.tribe.owner, 0) != %client.bl_id)
	{
		%client.chatMessage("\c3You aren't the owner of the clan, so you can't accept or deny any requests.");
		return;
	}
	if(!%client.tribeRequest)
	{
		%client.chatMessage("\c3Currently, there are no requests you can accept or deny.");
		return;
	}
	%client.chatMessage("\c3Requests cleared.");
	%client.tribeRequest = "";
}
function servercmdkickFromTribe(%client, %target)
{
	if(getWord(%client.tribe.owner, 0) != %client.bl_id)
	{
		%client.chatMessage("\c3You aren't the owner of the clan, so you cannot kick this person.");
		return;
	}
	if(%target $= "")
	{
		%Client.chatMessage("\c3You need to specify a person to kick.");
		return;
	}
	%target = findclientbyname(%target);
	if(!%target)
	{
		%client.chatMessage("\c3That client couldn't be found.");
		return;
	}
	if(%target.tribe != %client.tribe)
	{
		%client.chatMessage("\c3That person isn't in your tribe.");
		return;
	}
	%msg = "The user" SPC %client.name SPC "has kicked" SPC %target.name SPC "from the tribe.";
	%client.tribeAnnounce(%msg);
	%target.removeFromTribe();
}
function gameConnection::tribeAnnounce(%client, %msg)
{
	%tribe = %client.tribe;
	%t = ClientGroup.getCount();
	for(%i=0;%i<%t;%i++)
	{
		%cl = ClientGroup.getObject(%i);
		if(%cl.tribe == %client.tribe)
		{
			%cl.chatMessage(trim("\c5Tribe\c6:" SPC %msg));
		}
	}
}
function servercmdmakeTribePrivate(%client)
{
	if(!%client.tribe)
	{
		%client.chatMessage("\c3You don't have a tribe!");
		return;
	}
	if(getWord(%client.tribe.owner, 0) != %client.bl_id)
	{
		%client.chatMessage("\c3You need to own the tribe in order to do that.");
		return;
	}
	%client.tribe.private = 1;
	%client.chatMessage("\c3Your tribe has been set to private.");
	%client.chatMessage("\c3This means that people can't send you requests to join your tribe.");
	%client.chatMessage("\c3To undo this, do /makeTribePublic.");
}
function servercmdmakeTribePublic(%client)
{
	if(!%client.tribe)
	{
		%client.chatMessage("\c3You don't have a tribe!");
		return;
	}
	if(getWord(%client.tribe.owner, 0) != %client.bl_id)
	{
		%client.chatMessage("\c3You need to own the tribe in order to do that.");
		return;
	}
	%client.tribe.private = 0;
	%client.chatMessage("\c3Your tribe has been set to public.");
	%client.chatMessage("\c3This means that people can freely send you requests to join your tribe.");
	%client.chatMessage("\c3To undo this, do /makeTribePrivate.");
}

function servercmdWar(%client, %enemy, %resource1, %amt1, %resource2, %amt2)
{
	if(getWord(%client.tribe.owner, 0) != %client.bl_id)
	{
		%client.chatMessage("\c3You need to be the owner of the tribe in order to start a war.");
		return;
	}
	if(!%enemy)
	{
		%client.chatMessage("\c3Declare the tribe that you want to wage war against.");
		return;
	}
	%enemyTribe = TribeContainer.findTribeByName(%enemy);
	if(!%enemyTribe)
	{
		%client.chatMessage("\c3That tribe could not be found.");
		return;
	}
	if(%client.isAtWarWith(%enemyTribe))
	{
		%client.chatMessage("\c3You're already at war with this tribe!");
		return;
	}
	if(%client.tribe.lastWarredTribe == %enemyTribe)
	{
		%client.chatMessage("\c3The last tribe your guild warred was this one. You can't war them two times in a row.");
		return;
	}
	if(!%resource1)
	{
		%client.chatMessage("\c3You must set the items that they have to pay if they surrender. The maximum amount is 3 for each.");
		%client.chatMessage("\c3The syntax: /war enemyTribeName Resource1 AmountOfThatResource Resource2 AmountOfThatResource");
		return;
	}
	if(%amt1 > 6 || %amt2 > 6)
	{
		%client.chatMessage("\c3You can't demand higher than six of each item.");
		return;
	}
	if(%resource1 $= %resource2)
	{
		%client.chatMessage("\c3You can't demand the same resource twice.");
		return;
	}
	if(%resource1 $= "Seeds" || %resource1 $= "Rocks" || %resource1 $= "Cotton" || %resource1 $= "String" || %resource1 $= "Wood" || %resource1 $= "Metal")
	{
		if(%resource2 !$= "")
		{
			if(%resource2 !$= "Seeds" || %resource2 !$= "Rocks" || %resource2 !$= "Cotton" || %resource2 !$= "String" || %resource2 !$= "Wood" || %resource2 !$= "Metal")
			{
				%client.chatMessage("\c3That either isn't an actual resource, or it is an illegal one.");
				%client.chatMessage("\c3The full list of illegal resources are manablocks, grains, and wheat.");
				return;
			}
		}
		%c = ClientGroup.getCount();
		for(%i=0;%i<%c;%i++)
		{
			%cl = ClientGroup.getObject(%i);
			if(%cl.tribe == %enemyTribe)
				%enemiesOnline++;
		}
		if(%enemiesOnline == 0 || %enemiesOnline $= "")
		{
			%client.chatMessage("\c3None of the members of that tribe are online, so you cannot declare war.");
			return;
		}
		if(%client.tribe.getCount() / 2 > %enemyTribe.getCount())
		{
			%client.chatMessage("\c3You are more than double their size! You cannot go to war with this tribe.");
			return;
		}
		for(%i=0;%i<5;%i++)
		{
			if(%enemyTribe.enemy[%i] $= "")
			{
				%found = %i;
				break;
			}
		}
		if(%found)
				%enemyTribe.enemy[%found] = %client.tribe;
		else
		{
			%client.chatMessage("\c3You can't war this tribe, since they are already at war with five other tribes.");
			return;
		}
		
		for(%i=0;%i<5;%i++)
		{
			if(%client.tribe.enemy[%i] $= "")
			{
				%found = %i;
				break;
			}
		}
		if(%found)
				%client.tribe.enemy[%found] = %enemyTribe;
		else
		{
			%client.chatMessage("\c3You can't war this tribe, since your tribe is already at war with five other tribes.");
			return;
		}
		announce("\c0The tribe\c6" SPC %client.tribe.name SPC "\c0has declared war on\c6" SPC %enemyTribe.name @ "\c0!");
		%client.tribe.surrenderPrice = %resource1 SPC %amt1 SPC %resource2 SPC %amt2;
		%cl.tribeAnnounce("\c3Please set your win price with /setWinPrice. This is the amount of resources that the enemy will have to pay your tribe if they lose. If you don't define it and they lose, you don't get anything.");
		%cl.tribeAnnounce("\c3To see their win price, do /warStatistics.");
	}
	else
	{
		%client.chatMessage("\c3That either isn't an actual resource, or it is an illegal one.");
		%client.chatMessage("\c3The full list of illegal resources are manablocks, grains, and wheat.");
	}
}
function servercmdSurrender(%client, %enemyname)
{
	if(getWord(%client.tribe.owner, 0) != %client.bl_id)
	{
		%client.chatMessage("\c3You need to be the leader of your tribe to do that!");
		return;
	}
	if(hasMultipleEnemies(%client) && %enemyname $= "")
	{
		%client.chatMessage("\c3You have multiple enemies that you're at war with! Please specify the name of the one you want.");
		return;
	}
	else if(hasMultipleEnemies(%client) && %enemyname !$= "")
	{
		%enemy = TribeContainer::findTribeByName(%enemyname);
		if(%enemy $= "")
		{
			%client.chatMessage("\c3That tribe couldn't be found.");
			return;
		}
		else if(!%client.isAtWarWith(%enemy))
		{
			%client.chatMessage("\c3You aren't at war with the tribe specified.");
			return;
		}
	}
	else
	{
		%enemy = %client.tribe.enemy1;
		if(%enemy $= "")
		{
			%client.chatMessage("\c3You aren't at war with anyone!");
			return;
		}
	}
	for(%i=0;%i<ClientGroup.getCount();%i++)
	{
		%c = ClientGroup.getObject(%i);
		if(%c.tribe == %enemy)
			%gotchaNigger++;
	}
	if(!%gotchaNigger)
	{
		%client.chatMessage("\c3There needs to be atleast one person from the enemy tribe who is online in order to surrender.");
		return;
	}
	//OK now heres where we do it. %enemy is the enemy.
	announce("\c0The\c6" SPC %client.tribe.name SPC "\c0tribe has surrendered to the\c6" SPC %enemy.name SPC "\c0tribe!");
	if(%client.tribe.enemy1 == %enemy)
		%client.tribe.enemy1 = "";
	else if(%client.tribe.enemy2 == %enemy)
		%client.tribe.enemy2 = "";
	else if(%client.tribe.enemy3 == %enemy)
		%client.tribe.enemy3 = "";
	else if(%client.tribe.enemy4 == %enemy)
		%client.tribe.enemy4 = "";
	else if(%client.tribe.enemy5 == %enemy)
		%client.tribe.enemy5 = "";

	if(%enemy.enemy1 == %client.tribe)
		%enemy.enemy1 = "";
	else if(%enemy.enemy2 == %client.tribe)
		%enemy.enemy2 = "";
	else if(%enemy.enemy3 == %client.tribe)
		%enemy.enemy3 = "";
	else if(%enemy.enemy4 == %client.tribe)
		%enemy.enemy4 = "";
	else if(%enemy.enemy5 == %client.tribe)
		%enemy.enemy5 = "";

	%enemy.lastWarredTribe = %client.tribe;
	%price = %enemy.surrenderPrice;
	if(!%price)
	{
		%client.chatMessage("\c3It turns out that they don't actually have a surrender price set. You got off easy!");
		return;
	}
	%r1 = getWord(%price, 0);
	%a1 = getWord(%price, 1);
	%r2 = getWord(%price, 2);
	%a2 = getWord(%price, 3);

	if(%r1 && %a1)
		%fnd1 = takeTribeItem(%client.tribe, %r1, %a1);
		%debt1 = %a1 - %fnd1;
	if(%r2 && %a2)
		%fnd2 = takeTribeItem(%client.tribe, %r2, %a2);
		%debt2 = %a2 - %fnd2;
	if(%debt1 < %a1)
	{
		%client.chatMessage("\c3You are now in the debt of" SPC %debt1 SPC %r1 @ ".");
		%client.tribe.Debt1 = %r1 SPC %debt1 SPC %enemyTribe;
		%d++;
	}
	if(%debt2 < %a2)
	{
		%client.chatMessage("\c3You are now in the debt of" SPC %debt2 SPC %r2 @ ".");
		%client.tribe.Debt2 = %r2 SPC %debt2 SPC %enemyTribe;
		%d++;
	}
	if(!%d)
		%client.chatMessage("\c3You have successfully surrendered. Their surrender price has been charged to the tribemembers.");
	for(%i=0;%i<ClientGroup.getCount();%i++)
	{
		%cl = ClienTGroup.getObject(%i);
		if(%cl.bl_id == getWord(%enemy.owner,0))
		{
			%found = %cl;
		}
	}
	if(%found)
	{
		%found.BP[%r1] += %fnd11;
		%found.BP[%r2] += %fnd2;
		%found.refreshHUD();
		announce("\c0The war spoils have been awarded to" SPC %found.name @ "!");
	}
	if(%enemy.enemy1 == %client.tribe)
		%warNum = 1;
	else if(%enemy.enemy2 == %client.tribe)
		%warNum = 2;
	else if(%enemy.enemy3 == %client.tribe)
		%warNum = 3;
	else if(%enemy.enemy4 == %client.tribe)
		%warNum = 4;
	else if(%enemy.enemy5 == %client.tribe)
		%warNum = 5;
	else
	{
		for(%i=0;%i<ClientGroup.getCount;%i++)
		{
			%cl = ClientGroup.getObject(%i);
			if(%cl.tribe == %enemy)
			{
				if(%cl.warKills[%warNum] > %highestKillsYet)
				{
					%highestKillsYet = %cl.warKills[%warnum];
					%highestKiller = %cl;
					%cl.warKills[%warnum] = "";
				}
			}
		}
		announce("\c0The player\c6" SPC %highestKiller.name SPC "\c0has been selected to recieve the spoils of war.");
		%highestKiller.BP[%r1] += %debt1;
		%highestKiller.BP[%r2] += %debt2;
		%highestKiller.refreshHUD();
	}
	for(%i=0;%i<ClientGroup.getCount;%i++)
	{
		%cl = ClientGroup.getObject(%i);
		if(%cl.tribe == %client.tribe)
		{
			%juden = getWarNum(%client.tribe, %enemy);
			%cl.warKills[%juden] = "";
		}
	}
}
function servercmdsetWinPrice(%client, %resource1, %amt1, %resource2, %amt2)//only one for code simplicity, pl0x
{
	if(!%resource1)
	{
		%client.chatMessage("\c3This command allows you to set what the enemy has to pay if they surrender. The maximum amount is 3 for each.");
		%client.chatMessage("\c3The syntax: /setWinPrice Resource1 AmountOfThatResource Resource2 AmountOfThatResource");
		return;
	}
	if(%amt1 > 6 || %amt2 > 6)
	{
		%client.chatMessage("\c3You can't demand higher than six of each item.");
		return;
	}
	if(%resource1 $= %resource2)
	{
		%client.chatMessage("\c3You can't demand the same resource twice.");
		return;
	}
	if(%resource1 $= "Seeds" || %resource1 $= "Rocks" || %resource1 $= "Cotton" || %resource1 $= "String" || %resource1 $= "Wood" || %resource1 $= "Metal")
	{
		if(%resource2 !$= "")
		{
			if(%resource2 !$= "Seeds" || %resource2 !$= "Rocks" || %resource2 !$= "Cotton" || %resource2 !$= "String" || %resource2 !$= "Wood" || %resource2 !$= "Metal")
			{
				%client.chatMessage("\c3That either isn't an actual resource, or it is an illegal one.");
				%client.chatMessage("\c3The full list of illegal resources are manablocks, grains, and wheat.");
				return;
			}
		}
		%client.chatMessage("\c3Win price set!");
		%client.tribe.surrenderPrice = %resource1 SPC %amt1 SPC %resource2 SPC %amt2;
	}
	else
	{
		%client.chatMessage("\c3That either isn't an actual resource, or it is an illegal one.");
		%client.chatMessage("\c3The full list of illegal resources are manablocks, grains, and wheat.");
	}
}
function servercmdWarStatistics(%client, %enemyname)
{
	if(%enemyName !$= "")
	{
		if(hasMultipleEnemies(%client))
		{
			%client.chatMessage("\c3Your tribe is at war with multiple other tribes.");
			%client.chatMessage("\c3Please specify the tribe you want statics about.");
			return;
		}
		if(%client.tribe.enemy1)
		{
			getStats(%client.tribe.enemy1, %client);
			return;
		}

	}
	if(%enemyName)
	{
		if(%client.tribe.enemy1.name $= %enemyName)
			getStats(%client.tribe.enemy1, %client);
		else if(%client.tribe.enemy2.name $= %enemyName)
			getStats(%client.tribe.enemy2, %client);
		else if(%client.tribe.enemy3.name $= %enemyName)
			getStats(%client.tribe.enemy3, %client);
		else if(%client.tribe.enemy4.name $= %enemyName)
			getStats(%client.tribe.enemy4, %client);
		else if(%client.tribe.enemy5.name $= %enemyName)
			getStats(%client.tribe.enemy5, %client);
	}
}
function getStats(%tribe, %client)
{
	%client.chatMessage("\c0War Statistics");
	%client.chatMessage("\c0Their Win Price\c6:" SPC %tribe.surrenderPrice);
	%client.chatMessage("\c0Your Win Price\c6:" SPC %client.tribe.surrenderPrice);
	%client.chatMessage("\c3Their Member Count\c6:" SPC %tribe.getCount());
	%client.chatMessage("\c3Your Member Count\c6:" SPC %tribe.getCount());
}
function gameConnection::isAtWarWith(%client, %enemy)
{
	if(%client.tribe.enemy1 == %enemy.tribe)
		return true;
	else if(%client.tribe.enemy2 == %enemy.tribe)
		return true;
	else if(%client.tribe.enemy3 == %enemy.tribe)
		return true;
	else if(%client.tribe.enemy4 == %enemy.tribe)
		return true;
	else if(%client.tribe.enemy5 == %enemy.tribe)
		return true;
	else
		return false;
}
function gameConnection::isAtWar(%client)
{
	if(%client.tribe.enemy1)
		return true;
	else if(%client.tribe.enemy2)
		return true;
	else if(%client.tribe.enemy3)
		return true;
	else if(%client.tribe.enemy4)
		return true;
	else if(%client.tribe.enemy5)
		return true;
	else
		return false;
}

function getWarNum(%tribe1, %tribe2)
{
	if(%tribe.enemy1 == %tribe2)
		return 1;
	else if(%tribe.enemy2 == %tribe2)
		return 2;
	else if(%tribe.enemy3 == %tribe3)
		return 3;
	else if(%tribe.enemy4 == %tribe4)
		return 4;
	else if(%tribe.enemy5 == %tribe5)
		return 5;
	else
		return false;
}

function hasMultipleEnemies(%client)
{
	if(%client.tribe.enemy1)
		%enemy++;
	else if(%client.tribe.enemy2)
		%enemy++;
	else if(%client.tribe.enemy3)
		%enemy++;
	else if(%client.tribe.enemy4)
		%enemy++;
	else if(%client.tribe.enemy5)
		%enemy++;
	if(%enemy)
		return true;
	else
		return false;
}

function takeTribeItem(%tribe, %item, %amount)
{
	%c = %tribe.getCount();
	for(%i=0;%i<%c;%i++)
	{
		%cl = %tribe.getObject(%i);
		if(%cl.BP[%item] != 0 || %cl.BP[%item] !$= "")
		{
			if(%cl.BP[%item] >= %amount)
			{
				%cl.BP[%item] -= %amount;
				return %amount;
			}
			%found += %cl.BP[%item];
			%cl.BP[%item] = 0;
			if(%found == %amount)
			{
				return %found;
			}
		}
	}
	return %found;
}
package TribeWars
{
	function gameConnection::onDeath(%client, %killerPlayer, %killer, %damageType, %damageLoc)
	{
		if(%client.isAtWarWith(%killer.tribe))
		{
			if(%client.tribe.enemy1 == %killer.tribe)
				%warNum = 1;
			else if(%client.tribe.enemy2 == %killer.tribe)
				%warNum = 2;
			else if(%client.tribe.enemy3 == %killer.tribe)
				%warNum = 3;
			else if(%client.tribe.enemy4 == %killer.tribe)
				%warNum = 4;
			else if(%client.tribe.enemy5 == %killer.tribe)
				%warNum = 5;
			%killer.warKills[%warNum]++;
			%killer.tribe.EnemyKills[%warNum]++;
			if(%killer.tribe.enemyKills[%warNum] >= 50)
			{
				winWar(%killer.tribe, %client.tribe, %warNum);
			}
		}
		parent::onDeath(%client, %killerPlayer, %killer, %damageType, %damageLoc);
	}
	function servercmdteamMessageSent(%client, %msg)
	{
		%cl = %client;
		if(%cl.isShadowMuted)
		{
			//Credit for the following snippet goes to the creator of the colored names add-on.
			%msg  = stripMLControlChars(trim(%msg));
			%cnt  = getWordCount(%msg);
			%spam = %cl.isSpamming;
			%time = getSimTime();
			// Remaking chat so we need to block words
			if($Pref::Server::ETardFilter)
			{
				%filter = strReplace($Pref::Server::ETardList,",","\t");
				%count  = getFieldCount(%filter);
				
				for(%i = 0; %i < %count; %i++)
				{
					%word = getField(%filter,%i);
					
					// This will check space-requiring filters
					if(strPos(" " @ %msg @ " ",%word) != -1)
					{
						messageClient(%cl,'',"\c5This is a civilized game. Please use full words.");
						return;
					}
				}
			}
			
			if(%msg $= %cl.lastChatText && %time - %cl.lastChatTime < 15000)
			{
				// An admin gets sent this warning message
				messageClient(%cl,'',"\c5Do not repeat yourself.");
				
				if(!%cl.isAdmin && !%spam)
				{
					// They get flood protected for 10 seconds
					%cl.isSpamming       = 1;
					%cl.spamProtectStart = %time;
					
					schedule(10000,0,eval,%cl @ ".isSpamming = 0;");
				}
			}
			
			if(!%cl.isAdmin && !%spam)
			{
				if(%cl.spamMessageCount == 4)
				{
					// They get flood protected for 10 seconds
					%cl.isSpamming       = 1;
					%cl.spamProtectStart = %time;
					
					schedule(10000,0,eval,%cl @ ".isSpamming = 0;");
				}
				else
				{
					// They can only send four lines every 10s
					%cl.spamMessageCount++;
					schedule(10000,0,eval,%cl @ ".spamMessageCount--;");
				}
			}
			
			// Tells them that they're spamming
			if(%cl.isSpamming)
			{
				spamAlert(%cl);
				return;
			}
			
			// Remaking chat so we need to parse links
			for(%i = 0; %i < %cnt; %i++)
			{
				%word = getWord(%msg,%i);
				%end  = getSubStr(%word,7,strLen(%word));
				
				// Added in check for ":" which broke tags
				if(getSubStr(%word,0,7) $= "https://" && strPos(%end,":") == -1)
					%word = "<a:" @ %end @ ">" @ %end @ "</a>\c6";
				
				if(%i) %new = %new SPC %word;
				else   %new = %word;
				%msg = setWord(%msg, %i, %word);
			}
			for(%i=0;%i<ClientGroup.getCount();%i++)
			{
				%cl = ClientGroup.getObject(%i);
				if(%cl.tribe == %client.tribe)
				%client.chatMessage("\c7[Tribe] \c0" @ %client.name @ "\c7" @ %client.clanSuffix @ "\c4:" SPC %msg);
			}
			%client.chatMessage("\c7" @ %client.ClanPrefix @ "\c3" @ %client.name @ "\c7" @ %client.clanSuffix @ "\c4:" SPC %msg);
			echo("(" @ %client.tribe.name @ %client.name @ ":" SPC %msg);
			return;
		}
		parent::servercmdteamMessageSent(%client, %msg);
	}
};
activatePackage(TribeWars);
function winWar(%tribe, %enemy, %warNum)
{
	announce("\c0The\c6" SPC %tribe.name SPC "\c0tribe has won the war agaisnt the\c6" SPC %enemy.name SPC "\c0tribe!");
	if(%tribe.enemy1 == %enemy)
		%tribe.enemy1 = "";
	else if(%tribe.enemy2 == %enemy)
		%tribe.enemy2 = "";
	else if(%tribe.enemy3 == %enemy)
		%tribe.enemy3 = "";
	else if(%tribe.enemy4 == %enemy)
		%tribe.enemy4 = "";
	else if(%tribe.enemy5 == %enemy)
		%tribe.enemy5 = "";

	if(%enemy.enemy1 == %tribe)
		%enemy.enemy1 = "";
	else if(%enemy.enemy2 == %tribe)
		%enemy.enemy2 = "";
	else if(%enemy.enemy3 == %tribe)
		%enemy.enemy3 = "";
	else if(%enemy.enemy4 == %tribe)
		%enemy.enemy4 = "";
	else if(%enemy.enemy5 == %tribe)
		%enemy.enemy5 = "";

	%tribe.lastWarredTribe = %enemy;
	%price = %tribe.surrenderPrice;
	%r1 = getWord(%price, 0);
	%a1 = getWord(%price, 1);
	%r2 = getWord(%price, 2);
	%a2 = getWord(%price, 3);

	if(%r1 && %a1)
		%fnd1 = takeTribeItem(%enemy, %r1, %a1);
		%debt1 = %a1 - %fnd1;
	if(%r2 && %a2)
		%fnd2 = takeTribeItem(%enemy, %r2, %a2);
		%debt2 = %a2 - %fnd2;
	if(%debt1 < %a1)
	{
		%enemy.Debt1 = %r1 SPC %debt1 SPC %enemyTribe;
		%d++;
	}
	if(%debt2 < %a2)
	{
		%enemy.Debt2 = %r2 SPC %debt2 SPC %enemyTribe;
		%d++;
	}
	if(%d)
		announce("\c0The\c6" SPC %enemy.name SPC "tribe is now in debt!");
	for(%i=0;%i<ClientGroup.getCount();%i++)
	{
		%cl = ClienTGroup.getObject(%i);
		if(%cl.bl_id == getWord(%tribe.owner,0))
		{
			%found = %cl;
		}
	}
	if(%found)
	{
		%found.BP[%r1] += %fnd11;
		%found.BP[%r2] += %fnd2;
		%found.refreshHUD();
		announce("\c0The war spoils have been awarded to" SPC %found.name @ "!");
	}
	else
	{
		for(%i=0;%i<ClientGroup.getCount;%i++)
		{
			%cl = ClientGroup.getObject(%i);
			if(%cl.tribe == %tribe)
			{
				if(%cl.warKills[%warNum] > %highestKillsYet)
				{
					%highestKillsYet = %cl.warKills[%warnum];
					%highestKiller = %cl;
					%cl.warKills[%warnum] = "";
				}
			}
		}
		announce("\c0The player\c6" SPC %highestKiller.name SPC "\c0has been selected to recieve the spoils of war.");
		%highestKiller.BP[%r1] += %debt1;
		%highestKiller.BP[%r2] += %debt2;
		%highestKiller.refreshHUD();
	}
	for(%i=0;%i<ClientGroup.getCount;%i++)
	{
		%cl = ClientGroup.getObject(%i);
		if(%cl.tribe == %enemy)
		{
			%juden = getWarNum(%client.tribe, %enemy);
			%cl.warKills[%juden] = "";
		}
	}
}