using Campus.Core.Domain.Entities;
using Campus.Core.Domain.RepositoryContracts;
using Campus.Core.DTO;
using Campus.Core.ServiceContracts;

namespace Campus.Core.Services;

public class StudyProgramService : IStudyProgramService
{
    private readonly IRepository<StudyProgram> _studyProgramRepository;
    private readonly IRepository<Speciality> _specialityRepository;
    private readonly IRepository<Cathedra> _cathedraRepository;

    public StudyProgramService(IRepository<StudyProgram> _studyProgramRepository, IRepository<Speciality> _specialityRepository, IRepository<Cathedra> _cathedraRepository)
    {
        this._studyProgramRepository = _studyProgramRepository;
        this._specialityRepository = _specialityRepository;
        this._cathedraRepository = _cathedraRepository;
    }
    public async Task<StudyProgramResponse> GetById(Guid studyProgramId)
    {
        var studyProgram = await _studyProgramRepository.GetValueById(studyProgramId);
        if (studyProgram is null)
        {
            throw new KeyNotFoundException("Study program not found");
        }
        
        return studyProgram.ToStudyProgramResponse();
    }

    public async Task<IEnumerable<StudyProgramResponse>> GetAll()
    {
        var studyPrograms = await _studyProgramRepository.GetAll();
        return studyPrograms.Select(sp => sp.ToStudyProgramResponse());
    }

    public async Task<IEnumerable<StudyProgramResponse>> GetByCathedraId(Guid cathedraId)
    {
        if (await _cathedraRepository.GetValueById(cathedraId) is null)
        {
            throw new KeyNotFoundException("Cathedra not found");
        }
        var studyPrograms = await _studyProgramRepository.GetAll();
        return studyPrograms
            .Where(sp => sp.CathedraId == cathedraId)
            .Select(sp => sp.ToStudyProgramResponse());
    }

    public async Task<IEnumerable<StudyProgramResponse>> GetBySpecialityId(Guid specialityId)
    {
        if(await _specialityRepository.GetValueById(specialityId) is null)
        {
            throw new KeyNotFoundException("Speciality not found");
        }
        var studyPrograms = await _studyProgramRepository.GetAll();
        return studyPrograms
            .Where(sp => sp.SpecialityId == specialityId)
            .Select(sp => sp.ToStudyProgramResponse());
    }

    public async Task<IEnumerable<StudyProgramResponse>> GetBySpecialityAndCathedraId(Guid cathedraId, Guid specialityId)
    {
        if (await _cathedraRepository.GetValueById(cathedraId) is null)
        {
            throw new KeyNotFoundException("Cathedra not found");
        }
        if (await _specialityRepository.GetValueById(specialityId) is null)
        {
            throw new KeyNotFoundException("Speciality not found");
        }
        var studyPrograms = await _studyProgramRepository.GetAll();
        return studyPrograms
            .Where(sp => sp.CathedraId == cathedraId && sp.SpecialityId == specialityId)
            .Select(sp => sp.ToStudyProgramResponse());
    }

    public async Task<StudyProgramResponse> Add(StudyProgramAddRequest studyProgramAddRequest)
    {
        if(await _cathedraRepository.GetValueById(studyProgramAddRequest.CathedraId) is null)
        {
            throw new KeyNotFoundException("Cathedra not found");
        }
        if(await _specialityRepository.GetValueById(studyProgramAddRequest.SpecialityId) is null)
        {
            throw new KeyNotFoundException("Speciality not found");
        }
        var studyProgram = studyProgramAddRequest.ToStudyProgram();
        await _studyProgramRepository.Create(studyProgram);
        return studyProgram.ToStudyProgramResponse();
    }

    public async Task<StudyProgramResponse> Update(StudyProgramUpdateRequest studyProgramUpdateRequest)
    {
        if (await _studyProgramRepository.GetValueById(studyProgramUpdateRequest.Id) is null)
        {
            throw new KeyNotFoundException("Study program not found");
        }
        if(await _cathedraRepository.GetValueById(studyProgramUpdateRequest.CathedraId) is null)
        {
            throw new KeyNotFoundException("Cathedra not found");
        }
        if(await _specialityRepository.GetValueById(studyProgramUpdateRequest.SpecialityId) is null)
        {
            throw new KeyNotFoundException("Speciality not found");
        }
        var studyProgram = studyProgramUpdateRequest.ToStudyProgram();
        await _studyProgramRepository.Update(studyProgram);
        return studyProgram.ToStudyProgramResponse();
    }

    public async Task Delete(Guid studyProgramId)
    {
        if (await _studyProgramRepository.GetValueById(studyProgramId) is null)
        {
            throw new KeyNotFoundException("Study program not found");
        }
        await _studyProgramRepository.Delete(studyProgramId);
    }
}