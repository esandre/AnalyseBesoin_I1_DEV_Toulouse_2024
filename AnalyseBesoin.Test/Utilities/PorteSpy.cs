namespace AnalyseBesoin.Test.Utilities;

public class PorteSpy : IPorte
{
    public bool OuvertureDemandée => NombreOuverturesDemandées > 0;
    public int NombreOuverturesDemandées { get; private set; }

    private List<(bool R, bool V, bool B)> _invocations = new ();

    public void Ouvrir()
    {
        NombreOuverturesDemandées++;
    }

    public void Light(bool r, bool v, bool b)
    {
        _invocations.Add((r, v, b));
    }

    public int NombreFlashsViolets => _invocations.Count(invocation => invocation is { R: true, B: true, V: false });
}