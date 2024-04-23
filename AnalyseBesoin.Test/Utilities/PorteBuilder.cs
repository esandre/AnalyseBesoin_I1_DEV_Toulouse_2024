namespace AnalyseBesoin.Test.Utilities;

internal class PorteBuilder
{
    private bool _bloquée;

    public static PorteBuilder DeTest() => new PorteBuilder();

    public PorteSpy Build()
    {
        var comportement = new PorteFake(_bloquée);
        return new PorteSpy(comportement);
    }

    public PorteBuilder Bloquée()
    {
        _bloquée = true;
        return this;
    }

    public PorteBuilder NonBloquée()
    {
        _bloquée = false;
        return this;
    }
}