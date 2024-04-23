using Moq;
using Dummy = Moq.Mock<AnalyseBesoin.IPorte>;
using Fake = Moq.Mock;

namespace AnalyseBesoin.Test.Utilities;

internal class PorteBuilder
{
    private bool _bloquée;

    public static PorteBuilder DeTest() => new ();

    public IPorte Build()
    {
        var comportement = Fake.Of<IPorte>(m => m.EstBloquée == _bloquée);
        return comportement;
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
        return new Dummy(MockBehavior.Strict).Object;
    }
}