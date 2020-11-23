using System;
public class OnBlockMinedEventArgs : EventArgs
{
    public int posX, posY;
    public int blockTypeId;
    public BreakerSources breakerSource;
}
