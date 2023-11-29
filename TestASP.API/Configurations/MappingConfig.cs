using System;
using AutoMapper;
using TestASP.Model;
using TestASP.Data;
using static System.Net.Mime.MediaTypeNames;
using TestASP.API.Models;

namespace TestASP.API.Configurations
{
	public class MappingConfig : Profile
	{
		public MappingConfig()
		{
			CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, PublicProfile>()
                .ForMember(
                    publicProfile => publicProfile.Image,
                    opt => opt.MapFrom((u,pp) => Setting.Current.GetUserFileUrl(u.Image)) )
                .ReverseMap();
            //CreateMap<User, SignInUserRequestDto>().ReverseMap();
            //CreateMap<User, SignUpUserRequestDto>().ReverseMap();
            CreateMap<SignInUserRequestDto, User>();
            CreateMap<SignUpUserRequestDto, User>();

            CreateMap<DataTypeTable, DataTypeDto>().ReverseMap();
        }
	}
}

