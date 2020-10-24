using System;
public class OnBlockMinedEventArgs : EventArgs
{
    public int posX, posY;
    public int blockTypeId;
}
