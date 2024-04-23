using Moq;

namespace AnalyseBesoin.Test.Utilities;

internal class PorteBuilder
{
    private bool _bloquée;

    public static PorteBuilder DeTest() => new ();

    public PorteSpy Build()
    {
        var comportement = Mock.Of<IPorte>(m => m.EstBloquée == _bloquée);
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

    public static IPorte Défaillante()
    {
        return new Mock<IPorte>(MockBehavior.Strict).Object;
    }
}