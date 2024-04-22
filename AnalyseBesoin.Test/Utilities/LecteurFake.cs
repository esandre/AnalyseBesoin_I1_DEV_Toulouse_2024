namespace AnalyseBesoin.Test.Utilities;

public class LecteurFake : ILecteur
{
    public NuméroBadge? BadgeDétecté
    {
        get
        {
            var returnedValue = _détectionSimulée;
            _détectionSimulée = null;
            return returnedValue;
        }
    }

    private NuméroBadge? _détectionSimulée;

    public void SimulerDétectionBadge(NuméroBadge? badge = null)
    {
        _détectionSimulée = badge ?? new NuméroBadge(0);
    }
}