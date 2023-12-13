using System;
using TestASP.Data;

namespace TestASP.Core.IService
{
	public interface IDataValidationService
	{
        Task<bool> IsDataExist<T>(int id) where T : BaseData;

    }
}

