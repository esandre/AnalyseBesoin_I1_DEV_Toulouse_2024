using PorteTest = AnalyseBesoin.Test.Utilities.PorteSpy;
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
}