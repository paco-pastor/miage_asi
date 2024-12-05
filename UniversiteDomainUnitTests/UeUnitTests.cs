using Moq;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCase.UeUseCase.Create;

namespace UniversiteDomainUnitTests;

public class UeUnitTests
{

    [Test]
    public async Task CreateUeUseCase()
    {
        long idUe = 1;
        string numeroUe = "C1-VRM";
        string intitule = "VR Menuiserie";

        Ue ueAvant = new Ue { NumeroUe = numeroUe, Intitule = intitule };

        var mockUe = new Mock<IUeRepository>();

        mockUe
            .Setup(repo => repo.FindByConditionAsync(u => u.Id.Equals(idUe)))
            .ReturnsAsync((List<Ue>)null);

        Ue ueFinal = new Ue { Id = idUe, NumeroUe = numeroUe, Intitule = intitule };
        mockUe.Setup(repo => repo.CreateAsync(ueAvant)).ReturnsAsync(ueFinal);

        var mockFactory = new Mock<IRepositoryFactory>();
        mockFactory.Setup(facto => facto.UeRepository()).Returns(mockUe.Object);

        CreateUeUseCase useCase = new CreateUeUseCase(mockFactory.Object);

        var ueTeste = await useCase.ExecuteAsync(ueAvant);

        Assert.That(ueTeste.Id, Is.EqualTo(ueFinal.Id));
        Assert.That(ueTeste.NumeroUe, Is.EqualTo(ueFinal.NumeroUe));
        Assert.That(ueTeste.Intitule, Is.EqualTo(ueFinal.Intitule));
    }
}