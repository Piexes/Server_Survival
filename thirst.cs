package thirstSystem
{
	function armor::onEnterWater(%data, %obj, %coverage, %type)
	{
		if(%client.hydration != 10)
			%client.chatMessage("\c3Water supply replenished!");
		%client.hydration = 10;
		parent::onEnterWater(%data, %obj, %coverage, %type);
	}
};
activatePackage(thirstSystem);