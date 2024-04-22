namespace AnalyseBesoin;

internal class AssociationsLecteurPorte
{
    private readonly HashSet<(ILecteur Lecteur, IPorte Porte)> _associations = [];

    public IEnumerable<DemandeOuverture> LecteursAyantDétectéUnBadge
    {
        get
        {
            var groupes = _associations.GroupBy(t => t.Lecteur);

            foreach (var groupe in groupes)
            {
                var badgeDétecté = groupe.Key.BadgeDétecté;
                if(badgeDétecté == default) continue;

                yield return new DemandeOuverture(groupe.Select(t => t.Porte), badgeDétecté);
            }
        }
    }

    public void Enregistrer(ILecteur lecteur, IPorte porte)
    {
        _associations.Add((lecteur, porte));
    }

    internal class DemandeOuverture
    {
        public DemandeOuverture(IEnumerable<IPorte> portes, NuméroBadge badgeDetecté)
        {
            Portes = portes;
            BadgeDétecté = badgeDetecté;
        }

        public IEnumerable<IPorte> Portes { get; }
        public NuméroBadge BadgeDétecté { get; }
    }
}