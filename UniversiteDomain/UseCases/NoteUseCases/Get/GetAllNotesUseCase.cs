using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.NoteUseCases.Get;

public class GetAllNotesUseCase(IRepositoryFactory repositoryFactory)
{
    public async Task<List<Note>> ExecuteAsync()
    {
        List<Note> notes = await repositoryFactory.NoteRepository().FindAllAsync();
        await CheckBusinessRules(notes);
        return notes;
    }

    private async Task CheckBusinessRules(List<Note> notes)
    {
        
    }
    
    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite);
    }
}