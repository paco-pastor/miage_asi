using Microsoft.EntityFrameworkCore;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteEFDataProvider.Data;

namespace UniversiteEFDataProvider.Repositories;

public class ParcoursRepository(UniversiteDbContext context) : Repository<Parcours>(context), IParcoursRepository
{
    public async Task<Parcours> AddUeAsync(long idParcours, long idUe)
    {
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        ArgumentNullException.ThrowIfNull(Context.Ues);
        Parcours p = (await Context.Parcours.FindAsync(idParcours))!;
        Ue ue = (await Context.Ues.FindAsync(idUe))!;
        p.UesEnseignees.Add(ue);
        await Context.SaveChangesAsync();
        return p;
    }
    
    public async Task<Parcours> AddUeAsync(Parcours parcours, Ue ue)
    {
        Parcours p = await AddUeAsync(parcours.Id, ue.Id);
        await Context.SaveChangesAsync();
        return p;
    }

    public async Task<Parcours> AddUeAsync(Parcours? idParcours, List<Ue> ue)
    {
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        ArgumentNullException.ThrowIfNull(Context.Ues);
        Parcours p = (await Context.Parcours.FindAsync(idParcours))!;
        foreach (Ue u in ue)
        {
            p.UesEnseignees.Add(u);
        }
        await Context.SaveChangesAsync();
        return p;
    }
    
    public async Task<Parcours> AddUeAsync(long idParcours, long[] idUes)
    {
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        ArgumentNullException.ThrowIfNull(Context.Ues);
        Parcours p = (await Context.Parcours.FindAsync(idParcours))!;
        foreach (long idUe in idUes)
        {
            Ue ue = (await Context.Ues.FindAsync(idUe))!;
            p.UesEnseignees.Add(ue);
        }
        await Context.SaveChangesAsync();
        return p;
    }
    
    public async Task<Parcours> AddEtudiantAsync(long idParcours, long idEtudiant)
    {
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        ArgumentNullException.ThrowIfNull(Context.Etudiants);
        Parcours p = (await Context.Parcours.FindAsync(idParcours))!;
        Etudiant e = (await Context.Etudiants.FindAsync(idEtudiant))!;
        p.Inscrits.Add(e);
        await Context.SaveChangesAsync();
        return p;
    }
    
    public async Task<Parcours> AddEtudiantAsync(Parcours parcours, Etudiant etudiant)
    {
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        ArgumentNullException.ThrowIfNull(Context.Etudiants);
        Parcours p = await AddEtudiantAsync(parcours.Id, etudiant.Id);
        await Context.SaveChangesAsync();
        return p;
    }
    
    public async Task<Parcours> AddEtudiantAsync(Parcours ? parcours, List<Etudiant> etudiants)
    {
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        ArgumentNullException.ThrowIfNull(Context.Etudiants);
        Parcours p = (await Context.Parcours.FindAsync(parcours))!;
        foreach (Etudiant e in etudiants)
        {
            p.Inscrits.Add(e);
        }
        await Context.SaveChangesAsync();
        return p;
    }
    
    public async Task<Parcours> AddEtudiantAsync(long idParcours, long[] idEtudiants)
    {
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        ArgumentNullException.ThrowIfNull(Context.Etudiants);
        Parcours p = (await Context.Parcours.FindAsync(idParcours))!;
        foreach (long idEtudiant in idEtudiants)
        {
            Etudiant e = (await Context.Etudiants.FindAsync(idEtudiant))!;
            p.Inscrits.Add(e);
        }
        await Context.SaveChangesAsync();
        return p;
    }
}