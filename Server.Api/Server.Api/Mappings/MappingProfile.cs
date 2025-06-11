using AutoMapper;
using GovServices.Server.Entities;
using GovServices.Server.DTOs;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System.Linq;
using System.Collections.Generic;

namespace GovServices.Server.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Application, ApplicationDto>()
                .ForMember(d => d.ServiceName, o => o.MapFrom(s => s.Service != null ? s.Service.Name : null))
                .ForMember(d => d.CurrentStepName, o => o.MapFrom(s => s.CurrentStep != null ? s.CurrentStep.Name : null))
                .ForMember(d => d.AssignedToUserName, o => o.MapFrom(s => s.AssignedTo != null ? s.AssignedTo.FullName : null))
                .ReverseMap();

            CreateMap<ApplicationLog, ApplicationLogDto>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.User != null ? s.User.FullName : null))
                .ReverseMap();

            CreateMap<AuditLog, AuditLogDto>().ReverseMap();

            CreateMap<Document, DocumentDto>()
                .ForMember(d => d.UploadedByUserName, o => o.MapFrom(s => s.UploadedBy != null ? s.UploadedBy.FullName : null))
                .ForMember(d => d.MetadataJson, o => o.MapFrom(s => s.Metadata != null ? s.Metadata.MetadataJson : null))
                .ReverseMap()
                .ForPath(s => s.Metadata.MetadataJson, o => o.MapFrom(d => d.MetadataJson));

            CreateMap<GeoObject, GeoObjectDto>()
                .ForMember(d => d.GeoJson, o => o.MapFrom(s => s.Geometry != null ? new GeoJsonWriter().Write(s.Geometry) : null))
                .ReverseMap()
                .ForMember(d => d.Geometry, o => o.MapFrom(s => !string.IsNullOrEmpty(s.GeoJson) ? new GeoJsonReader().Read<Geometry>(s.GeoJson) : null));

            CreateMap<Order, OrderDto>()
                .ForMember(d => d.SignerUserName, o => o.MapFrom(s => s.Signer != null ? s.Signer.FullName : null))
                .ReverseMap();

            CreateMap<OutgoingDocument, OutgoingDocumentDto>()
                .ForMember(d => d.AttachmentFileNames, o => o.MapFrom(s => s.Attachments != null ? s.Attachments.Select(a => a.FileName).ToList() : new List<string>()))
                .ReverseMap()
                .ForMember(d => d.Attachments, o => o.Ignore());

            CreateMap<RosreestrRequest, RosreestrRequestDto>().ReverseMap();
            CreateMap<SedDocumentLog, SedDocumentLogDto>().ReverseMap();
            CreateMap<Service, ServiceDto>().ReverseMap();
            CreateMap<Template, TemplateDto>().ReverseMap();
            CreateMap<Workflow, WorkflowDto>().ReverseMap();
            CreateMap<WorkflowStep, WorkflowStepDto>().ReverseMap();
            CreateMap<WorkflowTransition, WorkflowTransitionDto>().ReverseMap();

            CreateMap<ApplicationUser, UserDto>()
                .ForMember(d => d.DepartmentName, o => o.MapFrom(s => s.Department != null ? s.Department.Name : null))
                .ForMember(d => d.Roles, o => o.Ignore())
                .ReverseMap();
        }
    }
}
