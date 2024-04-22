namespace AnalyseBesoin;

internal class AssociationsLecteurPorte
{
    private readonly HashSet<(ILecteur Lecteur, IPorte Porte)> _associations = [];

    public IEnumerable<Lecteur> LecteursAyantDétectéUnBadge
    {
        get
        {
            var groupes = _associations.GroupBy(t => t.Lecteur);

            foreach (var groupe in groupes)
            {
                var badgeDétecté = groupe.Key.BadgeDétecté;
                if(!badgeDétecté) continue;

                yield return new Lecteur(groupe.Select(t => t.Porte));
            }
        }
    }

    public void Enregistrer(ILecteur lecteur, IPorte porte)
    {
        _associations.Add((lecteur, porte));
    }

    internal class Lecteur
    {
        public Lecteur(IEnumerable<IPorte> portes)
        {
            Portes = portes;
        }

        public IEnumerable<IPorte> Portes { get; }
    }
}