using UniversiteDomain.Entities;

namespace UniversiteDomain.Dtos;

public class NoteBodyDto
{
    public float Valeur { get; set; }
    public long IdEtudiant { get; set; }
    public long IdUe { get; set; }
    
    public Note ToEntity()
    {
        return new Note()
        {
            Valeur = Valeur,
            EtudiantId = IdEtudiant,
            UeId = IdUe,
        };
    }
    
    public NoteBodyDto FromEntity(Note note)
    {
        Valeur = note.Valeur;
        IdEtudiant = note.EtudiantId;
        IdUe = note.UeId;
        return this;
    }

}