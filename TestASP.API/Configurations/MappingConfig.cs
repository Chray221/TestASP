using System;
using AutoMapper;
using TestASP.API.Models;
using TestASP.Data;

namespace TestASP.API.Configurations
{
	public class MappingConfig : Profile
	{
		public MappingConfig()
		{
			CreateMap<User, UserDto>().ReverseMap();
		}
	}
}

