using UniversiteDomain.Entities;

namespace UniversiteDomain.Dtos;

public class NoteDto
{
    public float Valeur { get; set; }
    public EtudiantDto Etudiant { get; set; }
    public UeDto Ue { get; set; }
    
    public NoteDto ToDto(Note note)
    {
        Valeur = note.Valeur;
        Etudiant = new EtudiantDto().ToDto(note.Etudiant);
        Ue = new UeDto().ToDto(note.Ue);
        return this;
    }
    
    public Note ToEntity()
    {
        return new Note {Valeur = this.Valeur, Etudiant = this.Etudiant.ToEntity(), Ue = this.Ue.ToEntity()};
    }

}