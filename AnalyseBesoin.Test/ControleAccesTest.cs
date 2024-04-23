using Moq;
using PorteTest = AnalyseBesoin.Test.Utilities.PorteSpy;
using Porte = AnalyseBesoin.Test.Utilities.PorteBuilder;
using LecteurTest = AnalyseBesoin.Test.Utilities.LecteurFake;

namespace AnalyseBesoin.Test;

public class ControleAccesTest
{
    [Fact]
    public void CasNominal()
    {
        // ETANT DONNE une Porte reliée à un Lecteur, ayant détecté un Badge
        var porte = new PorteTest();
        var lecteur = new LecteurTest();

        lecteur.SimulerDétectionBadge();

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteur, porte);

        // QUAND le Moteur d'Ouverture effectue une interrogation des lecteurs
        moteurOuverture.Interroger();

        // ALORS le signal d'ouverture est envoyé à la porte
        Assert.True(porte.OuvertureDemandée);
    }

    [Fact]
    public void Cas2Portes()
    {
        // ETANT DONNE deux Portes reliée à un Lecteur, ayant détecté un Badge
        var porte1 = new PorteTest();
        var porte2 = new PorteTest();
        var lecteur = new LecteurTest();

        lecteur.SimulerDétectionBadge();

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteur, porte1);
        moteurOuverture.Associer(lecteur, porte2);

        // QUAND le Moteur d'Ouverture effectue une interrogation des lecteurs
        moteurOuverture.Interroger();

        // ALORS le signal d'ouverture est envoyé aux deux portes
        Assert.True(porte1.OuvertureDemandée);
        Assert.True(porte2.OuvertureDemandée);
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
        Assert.False(porteBloquée.OuvertureDemandée);
        Assert.True(porteNonBloquée.OuvertureDemandée);
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
        Assert.True(porteNormale.OuvertureDemandée);
        Mock.Get(porteDéfaillante).Verify(m => m.Ouvrir(), Times.Once);
    }

    [Fact]
    public void Cas2Lecteurs()
    {
        // ETANT DONNE une Porte reliée à deux Lecteurs, ayant tous les deux détecté un Badge
        var porte = new PorteTest();

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
        Assert.Equal(1, porte.NombreOuverturesDemandées);
    }

    [Fact]
    public void CasBadgeBloqué()
    {
        // ETANT DONNE une Porte reliée à un Lecteur, ayant détecté un Badge bloqué
        var porte = new PorteTest();
        var lecteur = new LecteurTest();
        var badge = new NuméroBadge(1);

        lecteur.SimulerDétectionBadge(badge);

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteur, porte);
        moteurOuverture.Bloquer(badge);

        // QUAND le Moteur d'Ouverture effectue une interrogation des lecteurs
        moteurOuverture.Interroger();

        // ALORS le signal d'ouverture n'est pas envoyé à la porte
        Assert.False(porte.OuvertureDemandée);
    }

    [Fact]
    public void CasBadgeBloquéPuisDébloqué()
    {
        // ETANT DONNE une Porte reliée à un Lecteur, ayant détecté un Badge bloqué, puis débloqué
        var porte = new PorteTest();
        var lecteur = new LecteurTest();
        var badge = new NuméroBadge(1);

        lecteur.SimulerDétectionBadge(badge);

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteur, porte);
        moteurOuverture.Bloquer(badge);
        moteurOuverture.Débloquer(badge);

        // QUAND le Moteur d'Ouverture effectue une interrogation des lecteurs
        moteurOuverture.Interroger();

        // ALORS le signal d'ouverture n'est pas envoyé à la porte
        Assert.True(porte.OuvertureDemandée);
    }

    [Fact]
    public void CasAucuneInterrogation()
    {
        // ETANT DONNE une Porte reliée à un Lecteur, ayant détecté un Badge
        var porte = new PorteTest();
        var lecteur = new LecteurTest();

        lecteur.SimulerDétectionBadge();

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteur, porte);

        // ALORS le signal d'ouverture n'est pas envoyé à la porte
        Assert.False(porte.OuvertureDemandée);
    }

    [Fact]
    public void CasNonBadgé()
    {
        // ETANT DONNE une Porte reliée à un Lecteur, n'ayant pas détecté un Badge
        var porte = new PorteTest();
        var lecteur = new LecteurTest();

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteur, porte);

        // QUAND le Moteur d'Ouverture effectue une interrogation des lecteurs
        moteurOuverture.Interroger();

        // ALORS le signal d'ouverture n'est pas envoyé à la porte
        Assert.False(porte.OuvertureDemandée);
    }

    [Fact]
    public void DeuxPortes()
    {
        // ETANT DONNE un Lecteur ayant détecté un Badge
        // ET un autre Lecteur n'ayant rien détecté
        // ET une Porte reliée chacune à un Lecteur
        var porteDevantSOuvrir = new PorteTest();
        var porteDevantResterFermée = new PorteTest();

        var lecteurAyantDétecté = new LecteurTest();
        lecteurAyantDétecté.SimulerDétectionBadge();

        var lecteurNAyantPasDétecté = new LecteurTest();

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteurAyantDétecté, porteDevantSOuvrir);
        moteurOuverture.Associer(lecteurNAyantPasDétecté, porteDevantResterFermée);

        // QUAND le Moteur d'Ouverture effectue une interrogation des lecteurs
        moteurOuverture.Interroger();

        // ALORS seule la Porte reliée au Lecteur reçoit le signal d'ouverture
        Assert.False(porteDevantResterFermée.OuvertureDemandée);
        Assert.True(porteDevantSOuvrir.OuvertureDemandée);
    }

    [Fact]
    public void DeuxPortesMaisLinverse()
    {
        // ETANT DONNE un Lecteur ayant détecté un Badge
        // ET un autre Lecteur n'ayant rien détecté
        // ET une Porte reliée chacune à un Lecteur
        var porteDevantSOuvrir = new PorteTest();
        var porteDevantResterFermée = new PorteTest();

        var lecteurAyantDétecté = new LecteurTest();
        lecteurAyantDétecté.SimulerDétectionBadge();

        var lecteurNAyantPasDétecté = new LecteurTest();

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteurNAyantPasDétecté, porteDevantResterFermée);
        moteurOuverture.Associer(lecteurAyantDétecté, porteDevantSOuvrir);

        // QUAND le Moteur d'Ouverture effectue une interrogation des lecteurs
        moteurOuverture.Interroger();

        // ALORS seule la Porte reliée au Lecteur reçoit le signal d'ouverture
        Assert.False(porteDevantResterFermée.OuvertureDemandée);
        Assert.True(porteDevantSOuvrir.OuvertureDemandée);
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