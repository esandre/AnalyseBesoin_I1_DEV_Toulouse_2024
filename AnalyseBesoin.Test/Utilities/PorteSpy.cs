namespace AnalyseBesoin.Test.Utilities;

public class PorteSpy : IPorte
{
    private readonly IPorte _comportement;
    public bool OuvertureDemandée => NombreOuverturesDemandées > 0;
    public int NombreOuverturesDemandées { get; private set; }

    public void Ouvrir()
    {
        NombreOuverturesDemandées++;
        _comportement.Ouvrir();
    }

    public bool EstBloquée => _comportement.EstBloquée;

    public PorteSpy(IPorte comportement)
    {
        _comportement = comportement;
    }

    public PorteSpy() : this(new PorteStub())
    {
    }
}