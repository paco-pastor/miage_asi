using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.UeExceptions;

namespace UniversiteDomain.UseCase.UeUseCase.Create;

public class CreateUeUseCase(IRepositoryFactory repositoryFactory)
{
        public async Task<Ue> ExecuteAsync(string numeroUe, string intitule)
    {
        var ue = new Ue { NumeroUe = numeroUe, Intitule = intitule };
        return await ExecuteAsync(ue);
    }
    public async Task<Ue> ExecuteAsync (Ue ue)
    {
        await CheckBusinessRules(ue);
        Ue u = await repositoryFactory.UeRepository().CreateAsync(ue);
        repositoryFactory.UeRepository().SaveChangesAsync().Wait();
        return u;
    }

    private async Task CheckBusinessRules(Ue ue)
    {
        ArgumentNullException.ThrowIfNull(ue);
        ArgumentNullException.ThrowIfNull(ue.NumeroUe);
        ArgumentNullException.ThrowIfNull(ue.Intitule);
        
        List<Ue> existe = await repositoryFactory.UeRepository().FindByConditionAsync(u => u.NumeroUe.Equals(ue.NumeroUe));
        if (existe is { Count: > 0}) throw new DuplicateUeException("L'Ue " + ue.NumeroUe + " existe déjà");

        if (ue.Intitule.Length < 3) throw new InvalidIntituleException("L'intitulé de l'Ue doit contenir plus de 3 caractères");
    }
    
}