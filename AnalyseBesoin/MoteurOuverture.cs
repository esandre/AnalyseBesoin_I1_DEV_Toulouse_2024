namespace AnalyseBesoin;

public class MoteurOuverture
{
    private readonly AssociationsLecteurPorte _associations = new ();

    public void Interroger()
    {
        var lecteurs = _associations.LecteursAyantDétectéUnBadge;
        var portesAOuvrir = new HashSet<IPorte>();

        foreach (var lecteur in lecteurs)
        foreach (var porte in lecteur.Portes)
            portesAOuvrir.Add(porte);

        foreach (var porte in portesAOuvrir)
            porte.Ouvrir();
    }

    public void Associer(ILecteur lecteur, IPorte porte)
    {
        _associations.Enregistrer(lecteur, porte);
    }
}