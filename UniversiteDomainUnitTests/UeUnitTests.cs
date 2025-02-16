using Moq;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.UeUseCases.Create;

namespace UniversiteDomainUnitTests;

public class UeUnitTests
{
    [SetUp]
    public void Setup()
    {
    }
    
    [Test]
    public async Task CreateUeUseCase()
    {
        int idUe = 1;
        String numeroUe = "UE1";
        String intitule = "Intitulé de l'UE1";
        
        Ue ueNoSave = new Ue{NumeroUe = numeroUe, Intitule = intitule};
        
        var mockUe = new Mock<IUeRepository>();
        
        mockUe
            .Setup(repo=>repo.FindByConditionAsync(u=>u.NumeroUe.Equals(numeroUe)))
            .ReturnsAsync((List<Ue>)null);
        
        Ue ueFinal = new Ue{Id=idUe,NumeroUe= numeroUe, Intitule = intitule};
        
        mockUe.Setup(repo=>repo.CreateAsync(ueNoSave)).ReturnsAsync(ueFinal);
        
        var mockFactory = new Mock<IRepositoryFactory>();
        mockFactory.Setup(facto=>facto.UeRepository()).Returns(mockUe.Object);
        
        CreateUeUseCase useCase = new CreateUeUseCase(mockFactory.Object);
        
        var ueTeste = await useCase.ExecuteAsync(ueNoSave);
        
        Assert.That(ueTeste.Id, Is.EqualTo(ueFinal.Id));
        Assert.That(ueTeste.NumeroUe, Is.EqualTo(ueFinal.NumeroUe));
        Assert.That(ueTeste.Intitule, Is.EqualTo(ueFinal.Intitule));
    }
}