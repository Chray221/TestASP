using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestASP.Data;

public abstract class BaseData
{
    [Key]
    public int Id { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public BaseData()
    {
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }
}


//public class BaseModel : IBaseModel
//{
//    //[Key, BsonId]
//    [Key]
//    public Guid Id { get; set; }
//    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//    public DateTime CreatedAt { get; set; }
//    public DateTime UpdatedAt { get; set; }
//    public bool IsDeleted { get; set; }

//    public BaseModel()
//    {
//        Id = Guid.NewGuid();
//        CreatedAt = DateTime.Now;
//        UpdatedAt = DateTime.Now;
//    }

//    public virtual BaseDto ToDto()
//    {
//        return new BaseDto(this);
//    }
//}

//public interface IBaseModel<TDto> where TDto : BaseDto
//{
//    Guid Id { get; set; }
//    DateTime CreatedAt { get; set; }
//    DateTime UpdatedAt { get; set; }
//    bool IsDeleted { get; set; }
//    TDto ToDto();
//}

//public interface IBaseModel : IBaseModel<BaseDto>
//{

//}

