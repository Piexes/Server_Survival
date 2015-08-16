package SurvivalBuilding
{
	function servercmdLight(%client)
	{
		%start = %client.player.getEyePoint();
		%mid = vectorScale(%client.player.getEyeVector(), 100);
		%end = vectorAdd(%start, %mid);
		%ray = containerRaycast(%start, %end, $Typemasks::FXBrickObjectType);
		%brick = getWord(%ray, 0);
		if(%brick)
		{
			if(%client.buildmode != 1)
			{
				if(%brick.charm !$= "" && %brick.charm != %client.bl_id && %brick.tribe != %client.tribe && %datablock.getName() !$= "MetalBrickData" && %datablock.getName() !$= "ManaBrickData")
				{
					%client.chatMessage("\c3That brick is under a charm, so you can't pick it up.");
					return;
				}
				%datablock = %brick.getDatablock();
				if(%datablock.getName() $= "WoodBrickData")
					%client.BPWood++;
				else if(%datablock.getName() $= "StringBrickData")
					%client.BPString++;
				else if(%datablock.getName() $= "MetalBrickData")
					%client.BPMetal++;
				else if(%datablock.getName() $= "CottonBrickData")
					%client.BPCotton++;
				else if(%datablock.getName() $= "ManaBrickData")
					%client.BPManablock++;
				else if(%datablock.getName() $= "RockBrickData")
					%client.BPRock++;
				else if(%datablock.getName() $= "GrainBrickData")
					%client.BPGrain++;
				else if(%datablock.getName() $= "SeedBrickData")
					%client.BPSeeds++;
				else if(%datablock.getName() $= "Brick2x2x5Data" || %datablock.getName() $= "brick1x2x5Data" || %datablock.getName() $= "brick1x2x5RampData")
				{
					initContainerRadiusSearch(%brick.getTransform(), 5, $Typemasks:FXBrickObjectType);
					while(%foundBrick = containerSearchNext())
					{
						%found++;
						%n = %foundBrick.getName();
						if(%n $= "_" @ %brick.getName())
							%foundBrick.delete();
					}
					if(%found)
						%client.BPWood += 3;
				}
				else if(%datablock.getName() $= "WheatBrickData")
				{
					if(%brick.isGrown)
					{
						%client.chatMessage("\c3Brick harvested!");
						%brick.isSeeded = "";
						%brick.isGrown = "";
						%client.BPGrain += 5;
						%client.BPSeeds += 2;
					}
					else
						%client.BPWheat++;
				}
				else
					return;
				%brick.delete();
				%client.refreshHud();
				%client.chatMessage("\c3Resource acquired!");
			}
			else
			{
				%p = posFromRaycast(%ray);
				if(%client.buildblock $= "")
				{
					%client.chatMessage("\c3You tried to place a block, but you haven't selected a block type to place!");
					%client.chatMessage("\c3Do /builderMode blockName to choose one.");
					return;
				}
				if(%p $= "")
					return;
				if(%client.buildblock $= "Wood")
				{
					%t = WoodBrickData;
					%color = 47;
					%fx = 0;
				}
				else if(%client.buildblock $= "String")
				{
					%t = StringBrickData;
					%color = 1;
					%fx = 0;
				}
				else if(%client.buildblock $= "Metal")
				{
					%t = MetalBrickData;
					%color = 3;
					%fx = 0;
				}
				else if(%client.buildblock $= "Cotton")
				{
					%t = CottonBrickData;
					%color = %client.currentColor;
					%fx = 0;
				}
				else if(%client.buildblock $= "Manablock")
				{
					%t = ManaBrickData;
					%color = 41;
					%fx = 6;
				}
				else if(%client.buildblock $= "Rock")
				{
					%t = RockBrickData;
					%color = 4;
					%fx = 0;
				}
				else if(%client.buildblock $= "Wheat")
				{
					%t = WheatBrickData;
					%color = 45;
					%fx = 0;
				}
				else if(%client.buildblock $= "Grain")
				{
					%t = GrainBrickData;
					%color = 57;
					%fx = 0;
				}
				else if(%client.buildblock $= "Seeds")
				{
					%t = SeedBrickData;
					%color = 31;
					%fx = 0;
				}
				else
				{
					%client.chatMessage("\c3That's an invalid block type!");
					%client.buildblock = "";
				}
				if(%client.bp[%client.buildblock] < 1)
				{
					%client.chatMessage("\c3You don't have any of that brick to place!");
					return;
				}
 		        %newbrick = createResource(%client, %t, %p, %fx, %color);
 		        if(!isObject(%newBrick))
 		        {
 		        	%p = vectorAdd(%p, "0 0 0.2");
 		        	%newbrick = createResource(%client, %t, %p, %fx, %color);
 		        	if(!isObject(%newBrick))
 		        	{
 		        		%client.chatMessage("\c3Plant error x2!");
 		        	}
 		        }
 		        %client.refreshHud();
			}
		}

	}
};
activatePackage(SurvivalBuilding);

//Bricks
datablock fxDTSBrickData(WoodBrickData : brick2x2Data)
{
	category = "SurvivalRPG";
	subCategory = "Wood";
	uiName = "Wood";
};
datablock fxDTSBrickData(StringBrickData : brick1x4fData)
{
	category = "SurvivalRPG";
	subCategory = "Resource";
	uiName = "String";
};
datablock fxDTSBrickData(MetalBrickData : brick2x2fData)
{
	category = "SurvivalRPG";
	subCategory = "Resource";
	uiName = "Metal";
};
datablock fxDTSBrickData(CottonBrickData : brick2x2fData)
{
	category = "SurvivalRPG";
	subCategory = "Resource";
	uiName = "Cotton";
};
datablock fxDTSBrickData(ManaBrickData : brick2x2fData)
{
	category = "SurvivalRPG";
	subCategory = "Magic";
	uiName = "Mana Block";
};
datablock fxDTSBrickData(RockBrickData : brick2x2Data)
{
	category = "SurvivalRPG";
	subCategory = "Resource";
	uiName = "Rock";
};
datablock fxDTSBrickData(WheatBrickData : brick6x6fData)
{
	category = "SurvivalRPG";
	subCategory = "Crops";
	uiName = "Wheat";
};
datablock fxDTSBrickData(SeedBrickData : brick1x1fData)
{
	category = "SurvivalRPG";
	subCategory = "Resource";
	uiName = "Seeds";
};
datablock fxDTSBrickData(GrainBrickData : brick2x2fData)
{
	category = "SurvivalRPG";
	subCategory = "Crops";
	uiName = "Grain";
};

package buildKeybind
{
	function servercmdAlarm(%client)
	{
		if(%client.buildmode $= "")
		{
			%client.buildmode = 1;
			%client.chatMessage("\c3Buildmode has been turned on.");
		}
		else if(%client.buildmode == 1)
		{
			%client.buildmode = "";
			%client.chatMessage("\c3Buildmode has been turned off.");
		}
	}
};
activatePackage(buildKeybind);

function servercmdbuildmode(%client, %blocktype)
{
	if(%blocktype $= "")
	{
		if(%client.buildmode $= "")
		{
			%client.buildmode = 1;
			%client.chatMessage("\c3Buildmode has been turned on.");
		}
		else if(%client.buildmode == 1)
		{
			%client.buildmode = "";
			%client.chatMessage("\c3Buildmode has been turned off.");
		}
		if(%client.buildblock $= "")
		{
			%client.chatMessage("\c3Even though buildmode is turned on, you still haven't said what block type you want to place.");
			%client.chatMEssage("\c3Do /buildmode blocktype to select one.");
		}
		return;
	}
	%client.buildblock = %blocktype;
	%client.chatMessage("\c3Blocktype" SPC %blocktype SPC "set.");
}
function createResource(%client, %d, %p, %fx, %color)
{
	%newbrick = new fxDtsBrick(%t)
	{
		datablock = %d;
		isPlanted = false;
		position = %p;
		client = %client;
		colorID = %color;
		brickgroup = 888888;
		colorFXID = %fx;
	};
	%error = %newbrick.plant();
	if(%error)
	{
		%newbrick.schedule(33, delete);
		return false;
	}
	if(!isObject(%newbrick))
		return false;
	%newBrick.setTrusted(1);
	%data = %newBrick.getDatablock();
	%data.onTrustCheckFinished(%newBrick);
	Brickgroup_888888.add(%newbrick);
	%client.BP[%client.buildblock] -= 1;
	return %newbrick;
}