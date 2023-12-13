using System;
namespace TestASP.Model
{
    public class BaseDto<KeyDT>
    {
        public KeyDT Id { get; set; }
    }

    public class BaseDto: BaseDto<int?>
    {

    }

    public class BaseRequestDto : BaseDto<int?>
    {

    }

}
