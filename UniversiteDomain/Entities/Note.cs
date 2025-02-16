namespace UniversiteDomain.Entities;

public class Note
{
    public float Valeur { get; set; }
    public long EtudiantId { get; set; }
    public Etudiant Etudiant { get; set; } = null!;
    public long UeId { get; set; }
    public Ue Ue { get; set; } = null!;
    
    public override string ToString()
    {
        return "Note de "+Valeur +" pour l'étudiant "+Etudiant?.Nom+" "+Etudiant?.Prenom+" en "+Ue?.Intitule;
    }
}