namespace UniversiteDomain.Entities;

public class Parcours
{
    public long Id { get; set; }
    public string NomParcours { get; set; } = string.Empty;
    public int AnneeFormation { get; set; }
    public List<Etudiant>? Inscrits { get; set; } = new();
    public List<Ue>? UesEnseignees { get; set; } = new();

    public override string ToString()
    {
        return "ID "+Id +" : "+NomParcours+" - Master "+AnneeFormation;
    }
}