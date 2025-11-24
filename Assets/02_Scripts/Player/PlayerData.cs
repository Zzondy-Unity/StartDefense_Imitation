using System;

[Serializable]
public class PlayerData
{
    public int maxProbes = 20;
    public int curRound;
    public int startMineral = 10;
    public int startGold = 30;
    public int startProbe;

    public int[] ownedTrescend = new[]
    {
        15000001,
    };
}
