//Credit: Visolator
//Reset the speed to their datablock's max speed
function AIPlayer::resetMovementSpeed(%this)
{
	%data = %this.getDatablock();
	%this.setMaxForwardSpeed(%data.MaxForwardSpeed);
	%this.setMaxBackwardSpeed(%data.MaxBackwardSpeed);
	%this.setMaxSideSpeed(%data.MaxSideSpeed);
	%this.setMaxCrouchForwardSpeed(%data.MaxCrouchForwardSpeed);
	%this.setMaxCrouchBackwardSpeed(%data.MaxCrouchBackwardSpeed);
	%this.setMaxCrouchSideSpeed(%data.MaxCrouchSideSpeed);
}

//Reset the speed to their datablock's max speed
function Player::resetMovementSpeed(%this)
{
	%data = %this.getDatablock();
	%this.setMaxForwardSpeed(%data.MaxForwardSpeed);
	%this.setMaxBackwardSpeed(%data.MaxBackwardSpeed);
	%this.setMaxSideSpeed(%data.MaxSideSpeed);
	%this.setMaxCrouchForwardSpeed(%data.MaxCrouchForwardSpeed);
	%this.setMaxCrouchBackwardSpeed(%data.MaxCrouchBackwardSpeed);
	%this.setMaxCrouchSideSpeed(%data.MaxCrouchSideSpeed);
}

//Set the player's total movement speed
function AIPlayer::setBaseMovementSpeed(%this,%value)
{
	if(%value < 0) %value = 0;
	if(%value > 200) %value = 200;
	%this.setMaxForwardSpeed(%value);
	%this.setMaxBackwardSpeed(%value);
	%this.setMaxSideSpeed(%value);
	%this.setMaxCrouchForwardSpeed(%value);
	%this.setMaxCrouchBackwardSpeed(%value);
	%this.setMaxCrouchSideSpeed(%value);
}

//Set the player's total movement speed
function Player::setBaseMovementSpeed(%this,%value)
{
	if(%value < 0) %value = 0;
	if(%value > 200) %value = 200;
	%this.setMaxForwardSpeed(%value);
	%this.setMaxBackwardSpeed(%value);
	%this.setMaxSideSpeed(%value);
	%this.setMaxCrouchForwardSpeed(%value);
	%this.setMaxCrouchBackwardSpeed(%value);
	%this.setMaxCrouchSideSpeed(%value);
}

function Player::addRelativeVelocity(%player,%xyz)
{
	%x = getWord(%xyz,0);
	%y = getWord(%xyz,1);
	%z = getWord(%xyz,2);
	%forwardVector = %player.getForwardVector();
	%forwardX = getWord(%forwardVector,0);
	%forwardY = getWord(%forwardVector,1);
	%player.addVelocity((%x * %forwardY + %y * %forwardX) SPC (%y * %forwardY + %x * -%forwardX) SPC %z);
}