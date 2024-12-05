using Moq;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCase.UeUseCase.Create;
using UniversiteDomain.UseCases.NoteUseCases.Create;

namespace UniversiteDomainUnitTests;

public class NoteUnitTests
{

    [Test]
    public async Task CreateNoteUseCase()
    {
        long idEtudiant = 1;
        long idUe = 1;
        float valeur = 10;
        Etudiant etudiant = new Etudiant { Id = idEtudiant };
        Ue ue = new Ue { Id = idUe };
        
        Note noteAvant = new Note { IdEtudiant = idEtudiant, IdUe = idUe, Valeur = valeur, Etudiant = etudiant, Ue = ue };
        
        var mockNote = new Mock<INoteRepository>();
        
        mockNote
            .Setup(repo => repo.FindByConditionAsync(n => n.IdEtudiant.Equals(idEtudiant) && n.IdUe.Equals(idUe)))
            .ReturnsAsync((List<Note>)null);
        
        Note noteFinal = new Note { IdEtudiant = idEtudiant, IdUe = idUe, Valeur = valeur, Etudiant = etudiant, Ue = ue };
        mockNote.Setup(repo => repo.CreateAsync(noteAvant)).ReturnsAsync(noteFinal);
        
        var mockFactory = new Mock<IRepositoryFactory>();
        mockFactory.Setup(facto => facto.NoteRepository()).Returns(mockNote.Object);
        
        CreateNoteUseCase useCase = new CreateNoteUseCase(mockFactory.Object);
        
        var noteTeste = await useCase.ExecuteAsync(noteAvant);
        
        Assert.That(noteTeste.IdEtudiant, Is.EqualTo(noteFinal.IdEtudiant));
        Assert.That(noteTeste.IdUe, Is.EqualTo(noteFinal.IdUe));
        Assert.That(noteTeste.Valeur, Is.EqualTo(noteFinal.Valeur));
        Assert.That(noteTeste.Etudiant, Is.EqualTo(noteFinal.Etudiant));
        Assert.That(noteTeste.Ue, Is.EqualTo(noteFinal.Ue));
    }
}