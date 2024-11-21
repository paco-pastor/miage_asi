using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.UeUseCases.Create;

public class CreateUeUseCases(IRepositoryFactory repositoryFactory)
{
        public async Task<Ue> ExecuteAsync(string numeroUe, string intitule)
    {
        var ue = new Ue { NumeroUe = numeroUe, Intitule = intitule };
        return await ExecuteAsync(ue);
    }
    public async Task<Ue> ExecuteAsync (Ue ue)
    {
        // TODO Check business rules
        Ue u = await repositoryFactory.UeRepository().CreateAsync(ue);
        repositoryFactory.UeRepository().SaveChangesAsync().Wait();
        return u;
    }
    
}