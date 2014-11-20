public static class GameEventManager
{
	public delegate void GameEvent();
	public static event GameEvent GameStart, GameOver, KillPlayer, RevivePlayer;
	
	public static void TriggerGameStart()
	{
		if (GameStart != null)
			GameStart();
	}
	
	public static void TriggerGameOver()
	{
		if (GameOver != null)
			GameOver();
	}
	
	public static void TriggerPlayerDeath()
	{
		if (KillPlayer != null)
			KillPlayer();
	}
	
	public static void TriggerPlayerLife()
	{
		if (RevivePlayer != null)
			RevivePlayer();
	}
}