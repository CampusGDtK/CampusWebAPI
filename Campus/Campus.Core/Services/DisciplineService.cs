using Campus.Core.Domain.Entities;
using Campus.Core.Domain.RepositoryContracts;
using Campus.Core.DTO;

namespace Campus.Core.Services;

public class DisciplineService
{
    private readonly IRepository<Discipline> _disciplineRepository;
    private readonly IRepository<Cathedra> _cathedraRepository;


    public DisciplineService(IRepository<Discipline> disciplineRepository, IRepository<Cathedra> cathedraRepository)
    {
        _disciplineRepository = disciplineRepository;
        _cathedraRepository = cathedraRepository;
    }

    public async Task<DisciplineResponse> GetById(Guid id)
    {
        var discipline = await _disciplineRepository.GetValueById(id);
        if (discipline is null)
        {
            throw new KeyNotFoundException("Discipline not found");
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
            throw new KeyNotFoundException("Cathedra not found");
        }
        
        var disciplines = await _disciplineRepository.GetAll();
        
        return disciplines
            .Where(d => d.CathedralId == cathedraId)
            .Select(d => d.ToDisciplineResponse());
    }
    
    public async Task<DisciplineResponse> Create(DisciplineAddRequest? disciplineRequest)
    {
        if (disciplineRequest is null)
        {
            throw new ArgumentNullException(nameof(disciplineRequest), "Discipline request is null");
        }

        var discipline = disciplineRequest.ToDiscipline();
        await _disciplineRepository.Create(discipline);
        return discipline.ToDisciplineResponse();
    }
    
    public async Task<DisciplineResponse> Update(DisciplineUpdateRequest? disciplineRequest)
    {
        if (disciplineRequest is null)
        {
            throw new ArgumentNullException(nameof(disciplineRequest), "Discipline request is null");
        }

        var result = await _disciplineRepository.Update(disciplineRequest.ToDiscipline());
        if (result is null)
        {
            throw new KeyNotFoundException("Discipline not found");
        }
        
        return result.ToDisciplineResponse();
    }
    
    public async Task<bool> Delete(Guid id)
    {
        if (await _disciplineRepository.GetValueById(id) is null)
        {
            throw new KeyNotFoundException("Discipline not found");
        }
        return await _disciplineRepository.Delete(id);
    }
    
}