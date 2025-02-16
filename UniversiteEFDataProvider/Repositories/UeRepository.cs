using Microsoft.EntityFrameworkCore;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteEFDataProvider.Data;

namespace UniversiteEFDataProvider.Repositories;

public class UeRepository(UniversiteDbContext context) : Repository<Ue>(context), IUeRepository
{
    public async Task<Ue> AffecterUeAsync(long idUe, long idParcours)
    {
        ArgumentNullException.ThrowIfNull(Context.Ues);
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        Ue ue = (await Context.Ues.FindAsync(idUe))!;
        Parcours parcours = (await Context.Parcours.FindAsync(idParcours))!;
        ue.EnseigneeDans.Add(parcours);
        await Context.SaveChangesAsync();
        return ue;
    }
    
    public async Task<Ue> AffecterUeAsync(Ue ue, Parcours parcours)
    {
        await AffecterUeAsync(ue.Id, parcours.Id); 
        return ue;
    }
    
}