
PUBLIC:Hunting_OnGameModeInit()	return SetTimer("StartHunting", random(300000), false);

PUBLIC:Hunting_OnPlayerDisconnect(playerid)
{
	if(HuntingId == playerid){
		HuntingId = -1;
		SetTimer("StartHunting", random(300000), false);
	}return 1;
}

PUBLIC:Hunting_OnPlayerDeath(playerid, killerid)
{
	if(HuntingId == playerid){
	    HuntingId = -1;
	    SetTimer("StartHunting", random(300000), false);
	    if(IsPlayerConnected(killerid))GivePlayerMoney(killerid, 15000);
	}return 1;
}

PUBLIC:StartHunting()
{
	if(HuntingId == -1 && S[ServerPlayers]>1){
	    new str[128];
	    HuntingId = GetRandomPlayer();
	    SetPlayerColor(HuntingId, COLOR_HUNTING_PLAYER, true);
	    format(str, 128, "Server: Elimina al jugador %s(%i) por $15.000", I[HuntingId][pName], HuntingId);
	    SendClientMessageToAllEx(HuntingId, COLOR_HUNTING_MSG, str);
	    SendClientMessage(HuntingId, COLOR_WHITE, "Server: Eres la nueva presa del servidor, corre por tu vida.");
	}
}