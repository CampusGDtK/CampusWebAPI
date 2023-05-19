using Campus.Core.Domain.Entities;
using Campus.Core.Domain.RepositoryContracts;
using Campus.Core.DTO;
using Campus.Core.ServiceContracts;

namespace Campus.Core.Services;

public class DisciplineService : IDisciplineService
{
    private readonly IRepository<Discipline> _disciplineRepository;
    private readonly IRepository<Cathedra> _cathedraRepository;


    public DisciplineService(IRepository<Discipline> disciplineRepository, IRepository<Cathedra> cathedraRepository)
    {
        _disciplineRepository = disciplineRepository;
        _cathedraRepository = cathedraRepository;
    }

    public async Task<DisciplineResponse> GetDisciplineById(Guid id)
    {
        var discipline = await _disciplineRepository.GetValueById(id);
        if (discipline is null)
        {
            throw new KeyNotFoundException("Id of discipline not found");
        }

        return discipline.ToDisciplineResponse();
    }

    public async Task<IEnumerable<DisciplineResponse>> GetAll()
    {
        var disciplines = await _disciplineRepository.GetAll();
        return disciplines.Select(d => d.ToDisciplineResponse());
    }
    
    public async Task<IEnumerable<DisciplineResponse>> GetByCathedraId(Guid cathedraId)
    {
        if (await _cathedraRepository.GetValueById(cathedraId) is null)
        {
            throw new KeyNotFoundException("Id of cathedra not found");
        }
        
        var disciplines = await _disciplineRepository.GetAll();
        
        return disciplines
            .Where(d => d.CathedraId == cathedraId)
            .Select(d => d.ToDisciplineResponse());
    }
    
    public async Task<DisciplineResponse> Add(DisciplineAddRequest? disciplineRequest)
    {
        if (disciplineRequest is null)
        {
            throw new ArgumentNullException("DisciplineAddRequest is null");
        }
        
        if (await _cathedraRepository.GetValueById(disciplineRequest.CathedraId) is null)
        {
            throw new KeyNotFoundException("Cathedra not found");
        }

        var discipline = disciplineRequest.ToDiscipline();
        await _disciplineRepository.Create(discipline);
        return discipline.ToDisciplineResponse();
    }
    
    public async Task<DisciplineResponse> Update(DisciplineUpdateRequest? disciplineRequest)
    {
        if (disciplineRequest is null)
        {
            throw new ArgumentNullException("DisciplineUpdateRequest is null");
        }

        var cathedra = await _cathedraRepository.GetValueById(disciplineRequest.CathedraId);

        if (cathedra is null)
        {
            throw new KeyNotFoundException("Id of cathedra not found");
        }

        var discipline = disciplineRequest.ToDiscipline();
        discipline.Cathedra = cathedra;

        var result = await _disciplineRepository.Update(discipline);

        if (result is null)
        {
            throw new KeyNotFoundException("Id of discipline not found");
        }
        
        return result.ToDisciplineResponse();
    }
    
    public async Task Remove(Guid id)
    {
        if (await _disciplineRepository.GetValueById(id) is null)
        {
            throw new KeyNotFoundException("Id of discipline not found");
        }
        await _disciplineRepository.Delete(id);
    }
    
}