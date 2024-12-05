namespace UniversiteDomain.Entities;

public class Note
{
    public long IdEtudiant { get; set; }
    public long IdUe { get; set; }
    public float Valeur { get; set; }
    public Etudiant Etudiant { get; set; }
    public Ue Ue { get; set; }
    
    public override string ToString()
    {
        return "Note de "+Valeur+" pour l'etudiant "+IdEtudiant+" en "+IdUe;
    }
}