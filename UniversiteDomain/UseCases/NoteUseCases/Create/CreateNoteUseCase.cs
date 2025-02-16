using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.NoteExceptions;

namespace UniversiteDomain.UseCases.NoteUseCases.Create;

public class CreateNoteUseCase(IRepositoryFactory repositoryFactory)
{
    public async Task<Note> ExecuteAsync(long idEtudiant, long idUe, float valeur)
    {
        var note = new Note{Valeur = valeur, EtudiantId = idEtudiant, UeId = idUe};
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
        ArgumentNullException.ThrowIfNull(note.Valeur);
        
        // La note doit être comprise entre 0 et 20
        if (note.Valeur < 0 || note.Valeur > 20) throw new ValeurNoteException("La note doit être comprise entre 0 et 20");

        ArgumentNullException.ThrowIfNull(note.EtudiantId);
        ArgumentNullException.ThrowIfNull(note.UeId);
        
        Etudiant etudiant = await repositoryFactory.EtudiantRepository().FindAsync(note.EtudiantId) ?? throw new InvalidOperationException("L'étudiant n'existe pas");
        Ue ue = await repositoryFactory.UeRepository().FindAsync(note.UeId) ?? throw new InvalidOperationException("L'UE n'existe pas");

        
        if(!etudiant.ParcoursSuivi.UesEnseignees.Contains(ue)) throw new UeNonInscriteException("L'étudiant n'est pas inscrit à cette UE");

        // Un étudiant n'a qu'une note par Ue
        List<Note> existe = await repositoryFactory.NoteRepository().FindByConditionAsync
            (e=>e.EtudiantId.Equals(note.EtudiantId) && e.UeId.Equals(note.UeId));
        
        // Si une note pour cet étudiant et cette UE existe déjà, on lève une exception personnalisée
        if (existe is {Count:>0}) throw new DuplicateNoteException("Une note pour cet étudiant et cette UE existe déjà");
        
    }
    
    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite);
    }
}