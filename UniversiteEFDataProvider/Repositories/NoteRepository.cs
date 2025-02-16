using Microsoft.EntityFrameworkCore;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteEFDataProvider.Data;

namespace UniversiteEFDataProvider.Repositories;

public class NoteRepository(UniversiteDbContext context) : Repository<Note>(context), INoteRepository
{
    public async Task AffecterNoteAsync(long idEtudiant, long idUe, float note)
    {
        ArgumentNullException.ThrowIfNull(Context.Etudiants);
        ArgumentNullException.ThrowIfNull(Context.Ues);
        Etudiant e = (await Context.Etudiants.FindAsync(idEtudiant))!;
        Ue ue = (await Context.Ues.FindAsync(idUe))!;
        Note n = new Note { Etudiant = e, Ue = ue, Valeur = note };
        Context.Notes.Add(n);
        await Context.SaveChangesAsync();
    }
    
    public async Task<Etudiant> AffecterNoteAsync(Etudiant etudiant, Ue ue, float note)
    {
        await AffecterNoteAsync(etudiant.Id, ue.Id, note);
        return etudiant;
    }
    
    public async Task<double> GetNoteAsync(long idEtudiant, long idUe)
    {
        ArgumentNullException.ThrowIfNull(Context.Notes);
        return (await Context.Notes.FirstOrDefaultAsync(n => n.Etudiant.Id == idEtudiant && n.Ue.Id == idUe))?.Valeur ?? 0;
    }
    
    public async Task<double> GetNoteAsync(Etudiant etudiant, Ue ue)
    {
        return await GetNoteAsync(etudiant.Id, ue.Id);
    }
    public async Task<IEnumerable<Note>> GetNotesAsync(Etudiant etudiant)
    {
        return await GetNotesAsync(etudiant.Id);
    }
    
    public async Task<IEnumerable<Note>> GetNotesAsync(long idUe)
    {
        ArgumentNullException.ThrowIfNull(Context.Notes);
        return await Context.Notes.Where(n => n.Ue.Id == idUe).ToListAsync();
    }
    
    public async Task<IEnumerable<Note>> GetNotesAsync(Ue ue)
    {
        return await GetNotesAsync(ue.Id);
    }
    
    public async Task<IEnumerable<Note>> GetNotesAsync(long idEtudiant, long idUe)
    {
        ArgumentNullException.ThrowIfNull(Context.Notes);
        return await Context.Notes.Where(n => n.Etudiant.Id == idEtudiant && n.Ue.Id == idUe).ToListAsync();
    }
    
    public async Task<IEnumerable<Note>> GetNotesAsync(Etudiant etudiant, Ue ue)
    {
        return await GetNotesAsync(etudiant.Id, ue.Id);
    }
}