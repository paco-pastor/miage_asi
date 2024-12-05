using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.ParcoursExceptions;
using UniversiteDomain.Exceptions.UeExceptions;

namespace UniversiteDomain.UseCases.NoteUseCases.Create;

public class CreateNoteUseCase(IRepositoryFactory repositoryFactory)
{
    public async Task<Note> ExecuteAsync(long idEtudiant, long idUe, float valeur)
    {
        var note = new Note { IdEtudiant = idEtudiant, IdUe = idUe, Valeur = valeur };
        return await ExecuteAsync(note);
    }
    public async Task<Note> ExecuteAsync(Note note)
    {
        await CheckBusinessRules(note);
        Note n = await repositoryFactory.NoteRepository().CreateAsync(note);
        repositoryFactory.NoteRepository().SaveChangesAsync().Wait();
        return n;
    }

    private async Task CheckBusinessRules(Note note)
    {
        ArgumentNullException.ThrowIfNull(note);
        ArgumentNullException.ThrowIfNull(note.IdEtudiant);
        ArgumentNullException.ThrowIfNull(note.IdUe);
        ArgumentNullException.ThrowIfNull(note.Valeur);
        
        List<Note> existe = await repositoryFactory.NoteRepository().FindByConditionAsync(n => n.IdEtudiant.Equals(note.IdEtudiant) && n.IdUe.Equals(note.IdUe));
        if (existe is { Count: > 0}) throw new DuplicateNoteException("La note de l'étudiant " + note.IdEtudiant + " pour l'Ue " + note.IdUe + " existe déjà");
        
        if (note.Valeur < 0 || note.Valeur > 20) throw new InvalidNoteException("La note doit être comprise entre 0 et 20");

        List<Parcours> listeParcours = await repositoryFactory.ParcoursRepository().FindByConditionAsync(p =>
            p.Inscrits.Any(e => e.Id == note.IdEtudiant) && p.UesEnseignees.Any(u => u.Id == note.IdUe));
        if (listeParcours is { Count: > 0 }) throw new ParcoursNotFoundException();
    }
}