namespace AnalyseBesoin.Test.Utilities;

internal class PorteStub : IPorte
{
    public void Ouvrir()
    {
    }

    public bool EstBloquée => false;
}