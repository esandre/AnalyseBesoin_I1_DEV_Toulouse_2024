namespace AnalyseBesoin.Test.Utilities;

internal class PorteDummy : IPorte
{
    public void Ouvrir()
    {
        throw new NotSupportedException();
    }

    public bool EstBloquée => throw new NotSupportedException();
}