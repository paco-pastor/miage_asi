using UniversiteDomain.Entities;

namespace UniversiteDomain.Dtos;

public class ParcoursCompletDto
{
    public long Id { get; set; }
    public string NomParcours { get; set; }
    public int AnneeFormation { get; set; }
    public List<UeDto>? Ues { get; set; }
    public List<EtudiantDto>? Inscrits { get; set; }
    
    
    public ParcoursCompletDto ToDto(Parcours parcours)
    {
        Id = parcours.Id;
        NomParcours = parcours.NomParcours;
        AnneeFormation = parcours.AnneeFormation;
        Ues = parcours.UesEnseignees?.Select(ue => new UeDto().ToDto(ue)).ToList();
        Inscrits = parcours.Inscrits?.Select(etudiant => new EtudiantDto().ToDto(etudiant)).ToList();
        return this;
    }
    
    public Parcours ToEntity()
    {
        return new Parcours
        {
            Id = this.Id,
            NomParcours = this.NomParcours,
            AnneeFormation = this.AnneeFormation,
            UesEnseignees = this.Ues?.Select(ue => ue.ToEntity()).ToList(),
            Inscrits = this.Inscrits?.Select(etudiant => etudiant.ToEntity()).ToList()
        };
    }
}