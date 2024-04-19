namespace AnalyseBesoin.Test.Utilities;

public class PorteSpy : IPorte
{
    public bool OuvertureDemandée { get; private set; }

    public void Ouvrir()
    {
        OuvertureDemandée = true;
    }
}