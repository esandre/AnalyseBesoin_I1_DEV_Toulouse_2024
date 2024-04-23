namespace AnalyseBesoin;

public class MoteurOuverture
{
    private readonly AssociationsLecteurPorte _associations = new ();
    private readonly HashSet<NuméroBadge> _listeNoire = new();

    public void Interroger()
    {
        var lecteurs = _associations.LecteursAyantDétectéUnBadge;
        var portesAOuvrir = new HashSet<IPorte>();

        foreach (var lecteur in lecteurs)
        {
            var badge = lecteur.BadgeDétecté;
            if(_listeNoire.Contains(badge)) 
                continue;

            foreach (var porte in lecteur.Portes)
                portesAOuvrir.Add(porte);
        }

        portesAOuvrir.Where(porte => !porte.EstBloquée)
            .AsParallel()
            .ForAll(porte => porte.Ouvrir());
    }

    public void Associer(ILecteur lecteur, IPorte porte)
    {
        _associations.Enregistrer(lecteur, porte);
    }

    public void Bloquer(NuméroBadge badge)
    {
        _listeNoire.Add(badge);
    }

    public void Débloquer(NuméroBadge badge)
    {
        _listeNoire.Remove(badge);
    }
}