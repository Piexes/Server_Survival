exec("./hud.cs");
exec("./wiki.cs");
exec("./tags.cs");
exec("./build.cs");
exec("./armor.cs");
exec("./ticks.cs");
exec("./magic.cs");
exec("./sound.cs");
exec("./tribes.cs");
exec("./thirst.cs");
exec("./farming.cs");
exec("./support.cs");
exec("./greifing.cs");
exec("./crafting.cs");
exec("./emitters.cs");
exec("./triggers.cs");
function servercmdexec(%client)
{
	if(!%client.isSuperAdmin)
	{
		%client.chatMessage("\c3This command is SUPER ADMIN only.");
		return;
	}
	announce("\c0S\c1e\c2r\c3v\c4e\c5r\c6.\c7c\c8s \c0E\c1x\c2e\c3c\c4u\c4t\c6e\c7d\c8!");
	exec("Add-Ons/Server_Survival/server.cs");
}