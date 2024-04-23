using AnalyseBesoin.Test.Utilities;
using Moq;
using Porte = AnalyseBesoin.Test.Utilities.PorteBuilder;
using LecteurTest = AnalyseBesoin.Test.Utilities.LecteurFake;

namespace AnalyseBesoin.Test;

public class ControleAccesTest
{
    [Fact]
    public void CasNominal()
    {
        // ETANT DONNE une Porte reliée à un Lecteur, ayant détecté un Badge
        var porte = Porte.DeTest().Build();
        var lecteur = new LecteurTest();

        lecteur.SimulerDétectionBadge();

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteur, porte);

        // QUAND le Moteur d'Ouverture effectue une interrogation des lecteurs
        moteurOuverture.Interroger();

        // ALORS le signal d'ouverture est envoyé à la porte
        porte.VérifierOuvertureDemandée(Times.Once());
    }

    [Fact]
    public void Cas2Portes()
    {
        // ETANT DONNE deux Portes reliée à un Lecteur, ayant détecté un Badge
        var porte1 = Porte.DeTest().Build();
        var porte2 = Porte.DeTest().Build();
        var lecteur = new LecteurTest();

        lecteur.SimulerDétectionBadge();

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteur, porte1);
        moteurOuverture.Associer(lecteur, porte2);

        // QUAND le Moteur d'Ouverture effectue une interrogation des lecteurs
        moteurOuverture.Interroger();

        // ALORS le signal d'ouverture est envoyé aux deux portes
        porte1.VérifierOuvertureDemandée(Times.Once());
        porte2.VérifierOuvertureDemandée(Times.Once());
    }

    [Fact]
    public void CasAlambiqué()
    {
        // ETANT DONNE deux Portes, dont une Bloquée, reliées à un Lecteur, ayant détecté un Badge
        var porteBloquée = Porte.DeTest().Bloquée().Build();
        var porteNonBloquée = Porte.DeTest().NonBloquée().Build();
            
        var lecteur = new LecteurTest();

        lecteur.SimulerDétectionBadge();

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteur, porteBloquée);
        moteurOuverture.Associer(lecteur, porteNonBloquée);

        // QUAND le Moteur d'Ouverture effectue une interrogation des lecteurs
        moteurOuverture.Interroger();

        // ALORS le signal d'ouverture n'est envoyé qu'à la porte non-bloquée
        porteBloquée.VérifierOuvertureDemandée(Times.Never());
        porteNonBloquée.VérifierOuvertureDemandée(Times.Once());
    }

    [Fact]
    public void CasUnePorteDéfaillante()
    {
        // ETANT DONNE deux Portes, dont une défaillante, reliées à un Lecteur, ayant détecté un Badge
        var porteNormale = Porte.DeTest().Build();
        var porteDéfaillante = Porte.Défaillante();
            
        var lecteur = new LecteurTest();

        lecteur.SimulerDétectionBadge();

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteur, porteDéfaillante);
        moteurOuverture.Associer(lecteur, porteNormale);

        // QUAND le Moteur d'Ouverture effectue une interrogation des lecteurs
        void Act() => moteurOuverture.Interroger();

        // ALORS le signal d'ouverture est envoyé aux deux portes
        Assert.ThrowsAny<Exception>(Act);

        porteNormale.VérifierOuvertureDemandée(Times.Once());
        porteDéfaillante.VérifierOuvertureDemandée(Times.Once());
    }

    [Fact]
    public void Cas2Lecteurs()
    {
        // ETANT DONNE une Porte reliée à deux Lecteurs, ayant tous les deux détecté un Badge
        var porte = Porte.DeTest().Build();

        var lecteur1 = new LecteurTest();
        lecteur1.SimulerDétectionBadge();

        var lecteur2 = new LecteurTest();
        lecteur2.SimulerDétectionBadge();

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteur1, porte);
        moteurOuverture.Associer(lecteur2, porte);

        // QUAND le Moteur d'Ouverture effectue une interrogation des lecteurs
        moteurOuverture.Interroger();

        // ALORS un seul signal d'ouverture est envoyé à la Porte
        porte.VérifierOuvertureDemandée(Times.Once());
    }

    [Fact]
    public void CasBadgeBloqué()
    {
        // ETANT DONNE une Porte reliée à un Lecteur, ayant détecté un Badge bloqué
        var porte = Porte.DeTest().Build();
        var lecteur = new LecteurTest();
        var badge = new NuméroBadge(1);

        lecteur.SimulerDétectionBadge(badge);

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteur, porte);
        moteurOuverture.Bloquer(badge);

        // QUAND le Moteur d'Ouverture effectue une interrogation des lecteurs
        moteurOuverture.Interroger();

        // ALORS le signal d'ouverture n'est pas envoyé à la porte
        porte.VérifierOuvertureDemandée(Times.Never());
    }

    [Fact]
    public void CasBadgeBloquéPuisDébloqué()
    {
        // ETANT DONNE une Porte reliée à un Lecteur, ayant détecté un Badge bloqué, puis débloqué
        var porte = Porte.DeTest().Build();
        var lecteur = new LecteurTest();
        var badge = new NuméroBadge(1);

        lecteur.SimulerDétectionBadge(badge);

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteur, porte);
        moteurOuverture.Bloquer(badge);
        moteurOuverture.Débloquer(badge);

        // QUAND le Moteur d'Ouverture effectue une interrogation des lecteurs
        moteurOuverture.Interroger();

        // ALORS le signal d'ouverture est envoyé à la porte
        porte.VérifierOuvertureDemandée(Times.Once());
    }

    [Fact]
    public void CasAucuneInterrogation()
    {
        // ETANT DONNE une Porte reliée à un Lecteur, ayant détecté un Badge
        var porte = Porte.DeTest().Build();
        var lecteur = new LecteurTest();

        lecteur.SimulerDétectionBadge();

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteur, porte);

        // ALORS le signal d'ouverture n'est pas envoyé à la porte
        porte.VérifierOuvertureDemandée(Times.Never());
    }

    [Fact]
    public void CasNonBadgé()
    {
        // ETANT DONNE une Porte reliée à un Lecteur, n'ayant pas détecté un Badge
        var porte = Porte.DeTest().Build();
        var lecteur = new LecteurTest();

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteur, porte);

        // QUAND le Moteur d'Ouverture effectue une interrogation des lecteurs
        moteurOuverture.Interroger();

        // ALORS le signal d'ouverture n'est pas envoyé à la porte
        porte.VérifierOuvertureDemandée(Times.Never());
    }

    [Fact]
    public void DeuxPortes()
    {
        // ETANT DONNE un Lecteur ayant détecté un Badge
        // ET un autre Lecteur n'ayant rien détecté
        // ET une Porte reliée chacune à un Lecteur
        var porteDevantSOuvrir = Porte.DeTest().Build();
        var porteDevantResterFermée = Porte.DeTest().Build();

        var lecteurAyantDétecté = new LecteurTest();
        lecteurAyantDétecté.SimulerDétectionBadge();

        var lecteurNAyantPasDétecté = new LecteurTest();

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteurAyantDétecté, porteDevantSOuvrir);
        moteurOuverture.Associer(lecteurNAyantPasDétecté, porteDevantResterFermée);

        // QUAND le Moteur d'Ouverture effectue une interrogation des lecteurs
        moteurOuverture.Interroger();

        // ALORS seule la Porte reliée au Lecteur reçoit le signal d'ouverture
        porteDevantResterFermée.VérifierOuvertureDemandée(Times.Never());
        porteDevantSOuvrir.VérifierOuvertureDemandée(Times.Once());
    }

    [Fact]
    public void DeuxPortesMaisLinverse()
    {
        // ETANT DONNE un Lecteur ayant détecté un Badge
        // ET un autre Lecteur n'ayant rien détecté
        // ET une Porte reliée chacune à un Lecteur
        var porteDevantSOuvrir = Porte.DeTest().Build();
        var porteDevantResterFermée = Porte.DeTest().Build();

        var lecteurAyantDétecté = new LecteurTest();
        lecteurAyantDétecté.SimulerDétectionBadge();

        var lecteurNAyantPasDétecté = new LecteurTest();

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteurNAyantPasDétecté, porteDevantResterFermée);
        moteurOuverture.Associer(lecteurAyantDétecté, porteDevantSOuvrir);

        // QUAND le Moteur d'Ouverture effectue une interrogation des lecteurs
        moteurOuverture.Interroger();

        // ALORS seule la Porte reliée au Lecteur reçoit le signal d'ouverture
        porteDevantResterFermée.VérifierOuvertureDemandée(Times.Never());
        porteDevantSOuvrir.VérifierOuvertureDemandée(Times.Once());
    }

    [Fact]
    public void PorteDefaillante()
    {
        // ETANT DONNE une Porte défaillante reliée à un Lecteur, ayant détecté un Badge
        var porte = Porte.Défaillante();
        var lecteur = new LecteurTest();

        lecteur.SimulerDétectionBadge();

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteur, porte);

        // QUAND le Moteur d'Ouverture effectue une interrogation des lecteurs
        void Act() => moteurOuverture.Interroger();

        // ALORS l'exception n'est pas avalée
        Assert.ThrowsAny<Exception>(Act);
    }
}