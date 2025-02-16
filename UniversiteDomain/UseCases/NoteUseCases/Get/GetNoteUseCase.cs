using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.NoteUseCases.Get;

public class GetNoteUseCase(IRepositoryFactory repositoryFactory)
{
    public async Task<Note> ExecuteAsync(long id)
    {
        Note? note = await repositoryFactory.NoteRepository().FindAsync(id);
        await CheckBusinessRules(note);
        return note;
    }

    private async Task CheckBusinessRules(Note notes)
    {
        
    }
    
    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite);
    }
}