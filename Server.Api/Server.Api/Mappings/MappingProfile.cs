using AutoMapper;
using GovServices.Server.Entities;
using GovServices.Server.DTOs;
using System;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;

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

            CreateMap<CreateApplicationDto, Application>();
            CreateMap<UpdateApplicationDto, Application>();

            CreateMap<ApplicationLog, ApplicationLogDto>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.User != null ? s.User.FullName : null))
                .ReverseMap();

            CreateMap<AuditLog, AuditLogDto>().ReverseMap();

            CreateMap<Document, DocumentDto>().ReverseMap();

            CreateMap<GeoObject, GeoObjectDto>()
                .ForMember(d => d.GeoJson, o => o.MapFrom(s => s.Geometry != null ? new GeoJsonWriter().Write(s.Geometry) : null))
                .ReverseMap()
                .ForMember(d => d.Geometry, o => o.MapFrom(s => !string.IsNullOrEmpty(s.GeoJson) ? new GeoJsonReader().Read<Geometry>(s.GeoJson) : null));

            CreateMap<CreateGeoObjectDto, GeoObject>();

            CreateMap<Order, OrderDto>()
                .ForMember(d => d.SignerUserName, o => o.MapFrom(s => s.Signer != null ? s.Signer.FullName : null))
                .ReverseMap();

            CreateMap<CreateOrderDto, Order>();
            CreateMap<UpdateOrderDto, Order>();

            CreateMap<OutgoingDocument, OutgoingDocumentDto>()
                .ForMember(d => d.AttachmentFileNames, o => o.MapFrom(s => s.Attachments != null ? s.Attachments.Select(a => a.FileName).ToList() : new List<string>()))
                .ReverseMap()
                .ForMember(d => d.Attachments, o => o.Ignore());

            CreateMap<CreateOutgoingDocumentDto, OutgoingDocument>();

            CreateMap<ZagsRequest, ZagsRequestDto>()
                .ForMember(d => d.Attachments, o => o.MapFrom(s => s.Attachments.Select(a => new AttachmentDto
                {
                    FileName = a.FileName,
                    ContentBase64 = Convert.ToBase64String(a.Content)
                }).ToList()))
                .ReverseMap()
                .ForMember(d => d.Attachments, o => o.Ignore());
            CreateMap<CreateZagsRequestDto, ZagsRequest>();

            CreateMap<RosreestrRequest, RosreestrRequestDto>()
                .ForMember(d => d.Attachments, o => o.MapFrom(s => s.Attachments.Select(a => new AttachmentDto
                {
                    FileName = a.FileName,
                    ContentBase64 = Convert.ToBase64String(a.Content)
                }).ToList()))
                .ReverseMap()
                .ForMember(d => d.Attachments, o => o.Ignore());
            CreateMap<CreateRosreestrRequestDto, RosreestrRequest>();
            CreateMap<SedDocumentLog, SedDocumentLogDto>().ReverseMap();
            CreateMap<Service, ServiceDto>()
                .ForMember(d => d.ExecutionStages, o => o.MapFrom(s => DeserializeStages(s.ExecutionStagesJson)))
                .ReverseMap()
                .ForMember(d => d.ExecutionStagesJson, o => o.MapFrom(s => SerializeStages(s.ExecutionStages)));

            CreateMap<CreateServiceDto, Service>()
                .ForMember(d => d.ExecutionStagesJson, o => o.MapFrom(s => SerializeStages(s.ExecutionStages)));

            CreateMap<UpdateServiceDto, Service>()
                .ForMember(d => d.ExecutionStagesJson, o => o.MapFrom(s => SerializeStages(s.ExecutionStages)));
            CreateMap<Template, TemplateDto>().ReverseMap();
            CreateMap<CreateTemplateDto, Template>();
            CreateMap<UpdateTemplateDto, Template>();
            CreateMap<NumberTemplate, NumberTemplateDto>().ReverseMap();
            CreateMap<CreateNumberTemplateDto, NumberTemplate>();
            CreateMap<UpdateNumberTemplateDto, NumberTemplate>();
            CreateMap<Workflow, WorkflowDto>().ReverseMap();
            CreateMap<WorkflowStep, WorkflowStepDto>().ReverseMap();
            CreateMap<WorkflowTransition, WorkflowTransitionDto>().ReverseMap();

            CreateMap<ApplicationUser, UserDto>()
                .ForMember(d => d.DepartmentName, o => o.MapFrom(s => s.Department != null ? s.Department.Name : null))
                .ForMember(d => d.Roles, o => o.Ignore())
                .ReverseMap();

            CreateMap<CreateUserDto, ApplicationUser>();
            CreateMap<UpdateUserDto, ApplicationUser>();

            CreateMap<ApplicationResult, ApplicationResultDto>().ReverseMap();
            CreateMap<CreateApplicationResultDto, ApplicationResult>();

            CreateMap<ApplicationRevision, ApplicationRevisionDto>().ReverseMap();
            CreateMap<CreateApplicationRevisionDto, ApplicationRevision>();

            CreateMap<ServiceTemplate, ServiceTemplateDto>()
                .ForMember(d => d.ServiceName, o => o.MapFrom(s => s.Service != null ? s.Service.Name : null))
                .ForMember(d => d.UpdatedByName, o => o.MapFrom(s => s.UpdatedBy != null ? s.UpdatedBy.FullName : null))
                .ReverseMap();
            CreateMap<CreateServiceTemplateDto, ServiceTemplate>();
            CreateMap<UpdateServiceTemplateDto, ServiceTemplate>();

            CreateMap<Dictionary, DictionaryDto>();
        }

        private static List<ExecutionStage>? DeserializeStages(string? json)
        {
            return string.IsNullOrEmpty(json)
                ? null
                : JsonSerializer.Deserialize<List<ExecutionStage>>(json!);
        }

        private static string? SerializeStages(List<ExecutionStage>? stages)
        {
            return stages != null ? JsonSerializer.Serialize(stages) : null;
        }
    }
}
