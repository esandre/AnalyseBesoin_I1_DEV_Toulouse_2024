using AnalyseBesoin.Test.Utilities;
using PorteTest = AnalyseBesoin.Test.Utilities.PorteSpy;
using Porte = AnalyseBesoin.Test.Utilities.PorteBuilder;
using PorteD�faillante = AnalyseBesoin.Test.Utilities.PorteDummy;
using LecteurTest = AnalyseBesoin.Test.Utilities.LecteurFake;

namespace AnalyseBesoin.Test;

public class ControleAccesTest
{
    [Fact]
    public void CasNominal()
    {
        // ETANT DONNE une Porte reli�e � un Lecteur, ayant d�tect� un Badge
        var porte = new PorteTest();
        var lecteur = new LecteurTest();

        lecteur.SimulerD�tectionBadge();

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteur, porte);

        // QUAND le Moteur d'Ouverture effectue une interrogation des lecteurs
        moteurOuverture.Interroger();

        // ALORS le signal d'ouverture est envoy� � la porte
        Assert.True(porte.OuvertureDemand�e);
    }

    [Fact]
    public void Cas2Portes()
    {
        // ETANT DONNE deux Portes reli�e � un Lecteur, ayant d�tect� un Badge
        var porte1 = new PorteTest();
        var porte2 = new PorteTest();
        var lecteur = new LecteurTest();

        lecteur.SimulerD�tectionBadge();

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteur, porte1);
        moteurOuverture.Associer(lecteur, porte2);

        // QUAND le Moteur d'Ouverture effectue une interrogation des lecteurs
        moteurOuverture.Interroger();

        // ALORS le signal d'ouverture est envoy� aux deux portes
        Assert.True(porte1.OuvertureDemand�e);
        Assert.True(porte2.OuvertureDemand�e);
    }

    [Fact]
    public void CasAlambiqu�()
    {
        // ETANT DONNE deux Portes, dont une Bloqu�e, reli�es � un Lecteur, ayant d�tect� un Badge
        var porteBloqu�e = Porte.DeTest().Bloqu�e().Build();
        var porteNonBloqu�e = Porte.DeTest().NonBloqu�e().Build();
            
        var lecteur = new LecteurTest();

        lecteur.SimulerD�tectionBadge();

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteur, porteBloqu�e);
        moteurOuverture.Associer(lecteur, porteNonBloqu�e);

        // QUAND le Moteur d'Ouverture effectue une interrogation des lecteurs
        moteurOuverture.Interroger();

        // ALORS le signal d'ouverture n'est envoy� qu'� la porte non-bloqu�e
        Assert.False(porteBloqu�e.OuvertureDemand�e);
        Assert.True(porteNonBloqu�e.OuvertureDemand�e);
    }

    [Fact]
    public void CasUnePorteD�faillante()
    {
        // ETANT DONNE deux Portes, dont une d�faillante, reli�es � un Lecteur, ayant d�tect� un Badge
        var porteNormale = Porte.DeTest().Build();
        var porteD�faillante = Porte.D�faillante();
            
        var lecteur = new LecteurTest();

        lecteur.SimulerD�tectionBadge();

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteur, porteD�faillante);
        moteurOuverture.Associer(lecteur, porteNormale);

        // QUAND le Moteur d'Ouverture effectue une interrogation des lecteurs
        void Act() => moteurOuverture.Interroger();

        // ALORS le signal d'ouverture est envoy� aux deux portes
        Assert.ThrowsAny<Exception>(Act);
        Assert.True(porteNormale.OuvertureDemand�e);
        Assert.True(porteD�faillante.OuvertureDemand�e);
    }

    [Fact]
    public void Cas2Lecteurs()
    {
        // ETANT DONNE une Porte reli�e � deux Lecteurs, ayant tous les deux d�tect� un Badge
        var porte = new PorteTest();

        var lecteur1 = new LecteurTest();
        lecteur1.SimulerD�tectionBadge();

        var lecteur2 = new LecteurTest();
        lecteur2.SimulerD�tectionBadge();

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteur1, porte);
        moteurOuverture.Associer(lecteur2, porte);

        // QUAND le Moteur d'Ouverture effectue une interrogation des lecteurs
        moteurOuverture.Interroger();

        // ALORS un seul signal d'ouverture est envoy� � la Porte
        Assert.Equal(1, porte.NombreOuverturesDemand�es);
    }

    [Fact]
    public void CasBadgeBloqu�()
    {
        // ETANT DONNE une Porte reli�e � un Lecteur, ayant d�tect� un Badge bloqu�
        var porte = new PorteTest();
        var lecteur = new LecteurTest();
        var badge = new Num�roBadge(1);

        lecteur.SimulerD�tectionBadge(badge);

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteur, porte);
        moteurOuverture.Bloquer(badge);

        // QUAND le Moteur d'Ouverture effectue une interrogation des lecteurs
        moteurOuverture.Interroger();

        // ALORS le signal d'ouverture n'est pas envoy� � la porte
        Assert.False(porte.OuvertureDemand�e);
    }

    [Fact]
    public void CasBadgeBloqu�PuisD�bloqu�()
    {
        // ETANT DONNE une Porte reli�e � un Lecteur, ayant d�tect� un Badge bloqu�, puis d�bloqu�
        var porte = new PorteTest();
        var lecteur = new LecteurTest();
        var badge = new Num�roBadge(1);

        lecteur.SimulerD�tectionBadge(badge);

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteur, porte);
        moteurOuverture.Bloquer(badge);
        moteurOuverture.D�bloquer(badge);

        // QUAND le Moteur d'Ouverture effectue une interrogation des lecteurs
        moteurOuverture.Interroger();

        // ALORS le signal d'ouverture n'est pas envoy� � la porte
        Assert.True(porte.OuvertureDemand�e);
    }

    [Fact]
    public void CasAucuneInterrogation()
    {
        // ETANT DONNE une Porte reli�e � un Lecteur, ayant d�tect� un Badge
        var porte = new PorteTest();
        var lecteur = new LecteurTest();

        lecteur.SimulerD�tectionBadge();

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteur, porte);

        // ALORS le signal d'ouverture n'est pas envoy� � la porte
        Assert.False(porte.OuvertureDemand�e);
    }

    [Fact]
    public void CasNonBadg�()
    {
        // ETANT DONNE une Porte reli�e � un Lecteur, n'ayant pas d�tect� un Badge
        var porte = new PorteTest();
        var lecteur = new LecteurTest();

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteur, porte);

        // QUAND le Moteur d'Ouverture effectue une interrogation des lecteurs
        moteurOuverture.Interroger();

        // ALORS le signal d'ouverture n'est pas envoy� � la porte
        Assert.False(porte.OuvertureDemand�e);
    }

    [Fact]
    public void DeuxPortes()
    {
        // ETANT DONNE un Lecteur ayant d�tect� un Badge
        // ET un autre Lecteur n'ayant rien d�tect�
        // ET une Porte reli�e chacune � un Lecteur
        var porteDevantSOuvrir = new PorteTest();
        var porteDevantResterFerm�e = new PorteTest();

        var lecteurAyantD�tect� = new LecteurTest();
        lecteurAyantD�tect�.SimulerD�tectionBadge();

        var lecteurNAyantPasD�tect� = new LecteurTest();

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteurAyantD�tect�, porteDevantSOuvrir);
        moteurOuverture.Associer(lecteurNAyantPasD�tect�, porteDevantResterFerm�e);

        // QUAND le Moteur d'Ouverture effectue une interrogation des lecteurs
        moteurOuverture.Interroger();

        // ALORS seule la Porte reli�e au Lecteur re�oit le signal d'ouverture
        Assert.False(porteDevantResterFerm�e.OuvertureDemand�e);
        Assert.True(porteDevantSOuvrir.OuvertureDemand�e);
    }

    [Fact]
    public void DeuxPortesMaisLinverse()
    {
        // ETANT DONNE un Lecteur ayant d�tect� un Badge
        // ET un autre Lecteur n'ayant rien d�tect�
        // ET une Porte reli�e chacune � un Lecteur
        var porteDevantSOuvrir = new PorteTest();
        var porteDevantResterFerm�e = new PorteTest();

        var lecteurAyantD�tect� = new LecteurTest();
        lecteurAyantD�tect�.SimulerD�tectionBadge();

        var lecteurNAyantPasD�tect� = new LecteurTest();

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteurNAyantPasD�tect�, porteDevantResterFerm�e);
        moteurOuverture.Associer(lecteurAyantD�tect�, porteDevantSOuvrir);

        // QUAND le Moteur d'Ouverture effectue une interrogation des lecteurs
        moteurOuverture.Interroger();

        // ALORS seule la Porte reli�e au Lecteur re�oit le signal d'ouverture
        Assert.False(porteDevantResterFerm�e.OuvertureDemand�e);
        Assert.True(porteDevantSOuvrir.OuvertureDemand�e);
    }

    [Fact]
    public void PorteDefaillante()
    {
        // ETANT DONNE une Porte d�faillante reli�e � un Lecteur, ayant d�tect� un Badge
        var porte = new PorteD�faillante();
        var lecteur = new LecteurTest();

        lecteur.SimulerD�tectionBadge();

        var moteurOuverture = new MoteurOuverture();
        moteurOuverture.Associer(lecteur, porte);

        // QUAND le Moteur d'Ouverture effectue une interrogation des lecteurs
        void Act() => moteurOuverture.Interroger();

        // ALORS l'exception n'est pas aval�e
        Assert.ThrowsAny<Exception>(Act);
    }
}