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
            CreateMap<User, PublicProfile>().ReverseMap();
            //CreateMap<User, SignInUserRequestDto>().ReverseMap();
            //CreateMap<User, SignUpUserRequestDto>().ReverseMap();
            CreateMap<SignInUserRequestDto, User>();
            CreateMap<SignUpUserRequestDto, User>();

            CreateMap<DataTypeTable, DataTypeDto>().ReverseMap();
        }
	}
}

