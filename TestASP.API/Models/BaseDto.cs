using System;
namespace TestASP.API.Models
{
    public class BaseDto<KeyDT>
    {
        public KeyDT Id { get; set; }
    }

    public class BaseDto: BaseDto<int>
    {

    }

}
