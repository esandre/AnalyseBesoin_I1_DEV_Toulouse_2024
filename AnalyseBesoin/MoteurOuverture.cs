namespace AnalyseBesoin;

public class MoteurOuverture
{
    private readonly IDictionary<ILecteur, IPorte> _associations 
        = new Dictionary<ILecteur, IPorte>();

    public void Interroger()
    {
        foreach (var (lecteur, porte) in _associations)
            if(lecteur.BadgeDétecté)
                porte.Ouvrir();
    }

    public void Associer(ILecteur lecteur, IPorte porte)
    {
        _associations.Add(lecteur, porte);
    }
}