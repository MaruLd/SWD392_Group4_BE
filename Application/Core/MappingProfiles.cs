using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Events.DTOs;
using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CreateEventDTO, Event>();
            CreateMap<Event, CreateEventDTO>();

            CreateMap<Ticket, Ticket>();
        }
        
    }
}