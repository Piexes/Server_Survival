datablock TriggerData(PvPTriggerData)
{
	tickPeriodMS = 150;
};

function fxDtsBrick::createPvPTrigger(%this,%data)
{
	//credits to Space Guy for showing how to create triggers

	%t = new Trigger()
	{
		datablock = PvPTriggerData;
		polyhedron = "0 0 0 200 0 0 0 -200 0 0 0 200"; //this determines the shape of the trigger
	};
	missionCleanup.add(%t);
	
	%boxMax = getWords(%this.getWorldBox(), 3, 5);
	%boxMin = getWords(%this.getWorldBox(), 0, 2);
	%boxDiff = vectorSub(%boxMax,%boxMin);
	%boxDiff = vectorAdd(%boxDiff,"0 0 0.2"); 
	%t.setScale(%boxDiff);
	%posA = %this.getWorldBoxCenter();
	%posB = %t.getWorldBoxCenter();
	%posDiff = vectorSub(%posA, %posB);
	%posDiff = vectorAdd(%posDiff, "0 0 0.1");
	%t.setTransform(%posDiff);

	%this.trigger = %t;
	%t.brick = %this;

	return %t;
}
package brickTriggers
{
	function fxDtsBrick::onRemove(%this) //make sure to clean up your triggers
	{
		if(isObject(%this.trigger))
			%this.trigger.delete();

		parent::onRemove(%this);
	}
	function fxDtsBrick::onPlant(%brick)
	{
		parent::onPlant(%brick);
		if(%brick.getDatablock().getName() $= "NoPvPZoneBrickData")
		{
			%brick.createPvPTrigger();
		}
	}
	function fxDtsBrick::onLoadPlant(%brick)
	{
		parent::onLoadPlant(%brick);
		if(%brick.getDatablock().getName() $= "NoPvPZoneBrickData")
		{
			%brick.createPvPTrigger();
		}
	}
	function servercmdSuicide(%client)
	{
		%client.canPvP = "";
		%client.isSuiciding = true;
		parent::servercmdSuicide(%client);
	}
	function gameConnection::spawnPlayer(%client)
	{
		%client.isSuiciding = "";
		parent::spawnPlayer(%client);
	}
};
activatePackage(brickTriggers);

function PvPTriggerData::onEnterTrigger(%this,%trigger,%obj)
{
	%client = %obj.client;
	%client.canPvP = 1;
	%client.chatMessage("\c3You've entered the Peaceful Plains.");
	serverPlay3D(townEnterSound, %client.player.getPosition());
}

function PvPTriggerData::onLeaveTrigger(%this,%trigger,%obj)
{
	%client = %obj.client;
	%client.canPvP = "";
	%client.chatMessage("\c3You've left the Peaceful Plains.");
}

//function PvPTriggerData::onTickTrigger(%this,%trigger,%obj){}

datablock fxDTSBrickData(NoPvPZoneBrickData : brick2x2Data)
{
	category = "SurvivalRPG";
	subCategory = "Zones";
	uiName = "No PvP Zone";
};