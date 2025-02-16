using UniversiteDomain.Entities;

namespace UniversiteDomain.Dtos;

public class UeCompletDto
{
    public long Id { get; set; }
    public string NumeroUe { get; set; }
    public string Intitule { get; set; }
    public List<ParcoursDto>? EnseigneeDans { get; set; }
    
    public UeCompletDto ToDto(Ue ue)
    {
        Id = ue.Id;
        NumeroUe = ue.NumeroUe;
        Intitule = ue.Intitule;
        EnseigneeDans = ue.EnseigneeDans?.Select(p => new ParcoursDto().ToDto(p)).ToList();
        return this;
    }
    
    public Ue ToEntity()
    {
        return new Ue
        {
            Id = this.Id,
            NumeroUe = this.NumeroUe,
            Intitule = this.Intitule,
            EnseigneeDans = this.EnseigneeDans?.Select(p => p.ToEntity()).ToList()
        };
    }
}