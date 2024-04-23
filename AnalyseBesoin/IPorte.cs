namespace AnalyseBesoin;

public interface IPorte
{
    void Ouvrir();
    bool EstBloquée { get; }
}