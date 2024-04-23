namespace AnalyseBesoin.Test.Utilities;

internal class PorteFake : IPorte
{
    public PorteFake(bool estBloquée)
    {
        EstBloquée = estBloquée;
    }

    public void Ouvrir()
    {
    }

    public bool EstBloquée { get; }
}