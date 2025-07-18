using AutoMapper;
using Moq;
using MovieApp.Contracts;
using MovieApp.Core.Contracts;
using MovieApp.Core.Dtos.Actor;
using MovieApp.Core.Dtos.Parameters;
using MovieApp.Core.Entities;
using MovieApp.Core.Exceptions;
using MovieApp.Core.Shared;
using MovieApp.Data;
using MovieApp.Data.Profiles;
using MovieApp.Services;
using Services.Tests.Helpers;

namespace Services.Tests;

public class ActorServiceTests
{
    private const int ActorId = 1;
    private const string ActorName = "Actor Name";

    private readonly Mock<IUnitOfWork> uow;
    private readonly IMapper mapper;
    private readonly IActorService service;

    public ActorServiceTests()
    {
        uow = new Mock<IUnitOfWork>();
        mapper = MapperFactory.Create<ActorProfile>();
        service = new ActorService(uow.Object, mapper);
    }

    [Fact]
    public async Task GetActorsAsync_ReturnsPagedResultOfDtos()
    {
        var actorsCount = 5;
        var actors = GenerateActors(actorsCount);
        var parameters = new ActorParameters();
        var pageMeta = new PaginationMeta { PageIndex = 1, PageSize = 10, TotalCount = actorsCount };
        var pagedActors = new PagedResult<Actor>(items: actors, details: pageMeta);

        uow.Setup(u => u.Actors.GetActorsAsync(parameters, false))
            .ReturnsAsync(pagedActors);

        var result = await service.GetActorsAsync(parameters);

        uow.Verify(u => u.Actors.GetActorsAsync(parameters, false), Times.Once);
        Assert.IsType<PagedResult<ActorDto>>(result);
        Assert.Equivalent(pageMeta, result.Details);
        Assert.Equal(actorsCount, result.Items.Count());
        Assert.Equal(actors.First().Id, result.Items.First().Id);
    }

    [Fact]
    public async Task GetActorAsync_ThrowsException_WhenActorDoesNotExist()
    {
        SetupActor(null);

        await Assert.ThrowsAsync<NotFoundException<Actor>>(
            () => service.GetActorAsync(ActorId));
    }

    [Fact]
    public async Task GetActorAsync_ReturnsActorDto_WhenActorExists()
    {
        var actor = GenerateActor();
        SetupActor(actor);

        var result = await service.GetActorAsync(ActorId);

        Assert.IsType<ActorDto>(result);
        AssertDtoMatchesEntity(actor, result);
    }

    [Fact]
    public async Task PutActorAsync_ThrowsBadRequest_WhenIdsDontMatch()
    {
        var updateDto = GenerateUpdateDto();

        await Assert.ThrowsAsync<BadRequestException>(
            () => service.PutActorAsync(ActorId + 1, updateDto));
    }

    [Fact]
    public async Task PutActorAsync_ThrowsNotFound_WhenActorDoesNotExist()
    {
        var updateDto = GenerateUpdateDto();
        SetupActor(null);

        await Assert.ThrowsAsync<NotFoundException<Actor>>(
            () => service.PutActorAsync(ActorId, updateDto));
    }

    [Fact]
    public async Task PutActorAsync_UpdatesAndCompletes_WhenActorExists()
    {
        var updateDto = GenerateUpdateDto();
        SetupActor(GenerateActor(), true);

        await service.PutActorAsync(ActorId, updateDto);

        uow.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task PostActorAsync_CreatesAndCompletes()
    {
        var createDto = GenerateCreateDto();
        Actor? createdActor = null;

        uow.Setup(u => u.Actors.Add(It.IsAny<Actor>()))
            .Callback<Actor>(a => createdActor = a);

        var result = await service.PostActorAsync(createDto);

        uow.Verify(u => u.Actors.Add(It.IsAny<Actor>()), Times.Once);
        uow.Verify(u => u.CompleteAsync(), Times.Once);
        Assert.NotNull(createdActor);
        Assert.IsType<ActorDto>(result);
        Assert.Equal(createDto.Name, result.Name);
        Assert.Equal(createDto.Name, createdActor.Name);
    }

    private void SetupActor(Actor? actor, bool trackChanges = false) =>
        uow.Setup(u => u.Actors.GetByIdAsync(It.IsAny<int>(), trackChanges))
            .ReturnsAsync(actor);

    private static Actor GenerateActor(int id = ActorId, string name = ActorName) =>
        new() { Id = id, Name = name };

    private static IEnumerable<Actor> GenerateActors(int count) =>
        DbInitializer.GenerateActors(count);

    private static ActorUpdateDto GenerateUpdateDto(int id = ActorId, string name = ActorName) =>
        new() { Id = id, Name = name };

    private static ActorCreateDto GenerateCreateDto(string name = ActorName) =>
        new() { Name = name };

    private static void AssertDtoMatchesEntity(Actor actor, ActorDto dto)
    {
        Assert.Equal(actor.Id, dto.Id);
        Assert.Equal(actor.Name, dto.Name);
    }
}