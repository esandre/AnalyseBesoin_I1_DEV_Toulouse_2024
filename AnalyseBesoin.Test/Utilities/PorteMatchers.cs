using Moq;

namespace AnalyseBesoin.Test.Utilities;

internal static class PorteMatchers
{
    public static void VérifierOuvertureDemandée(this IPorte porte, Times times)
    {
        var mock = Mock.Get(porte);
        mock.Verify(m => m.Ouvrir(), times);
    }
}