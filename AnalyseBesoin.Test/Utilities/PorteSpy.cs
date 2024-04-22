namespace AnalyseBesoin.Test.Utilities;

public class PorteSpy : IPorte
{
    public bool OuvertureDemandée => NombreOuverturesDemandées > 0;
    public int NombreOuverturesDemandées { get; private set; }

    public void Ouvrir()
    {
        NombreOuverturesDemandées++;
    }
}