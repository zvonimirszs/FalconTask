using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using DataService.Data;
using System.Text.Json;

namespace DataService.SyncDataServices.Grpc;
// gRPC Server
public class GrpcDataService : GrpcData.GrpcDataBase
{
    private readonly IDataRepo _repository;
    private readonly IMapper _mapper;

    public GrpcDataService(IDataRepo repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public override Task<ZanrResponse>  GetAllZanrovi(GetAllRequest request, ServerCallContext context)
    {        
        var response = new ZanrResponse();
        var zanrovi = _repository.GetAllZanrovi();

        foreach(var z in zanrovi)
        {                
            response.Zanr.Add(_mapper.Map<GrpcZanrModel>(z));
        }
        Console.WriteLine("--> Sending Žanrovi For other services...");
        return Task.FromResult(response);
    }

    public override Task<DirektorResponse> GetAllDirektori(GetAllRequest request, ServerCallContext context)
    {            
        var response = new DirektorResponse();
        var direktori = _repository.GetAllDirektori();

        foreach(var d in direktori)
        {
            Console.WriteLine($"--> Send to Client GetAllDirektori {JsonSerializer.Serialize(d)}");
            Console.WriteLine($"--> Send to Client GetAllDirektori {JsonSerializer.Serialize(_mapper.Map<GrpcDirektorModel>(d))}");
            response.Direktor.Add(_mapper.Map<GrpcDirektorModel>(d));
        }
        Console.WriteLine("--> Sending Direktori For other services...");
        return Task.FromResult(response);
    }
}