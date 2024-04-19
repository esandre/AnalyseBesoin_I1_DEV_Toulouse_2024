namespace AnalyseBesoin.Test.Utilities;

public class LecteurFake : ILecteur
{
    public bool BadgeDétecté
    {
        get
        {
            var returnedValue = _détectionSimulée;
            _détectionSimulée = false;
            return returnedValue;
        }
    }

    private bool _détectionSimulée;

    public void SimulerDétectionBadge()
    {
        _détectionSimulée = true;
    }
}